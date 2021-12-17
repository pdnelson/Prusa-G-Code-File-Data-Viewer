using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PrusaGCodeFileDataViewer
{
    public partial class frmGCodeViewer : Form
    {
        private List<GCodeFile> GCodeFiles;
        private FrmProgressForm progressForm;
        private static Semaphore progressSem;

        public frmGCodeViewer()
        {
            InitializeComponent();
            GCodeFiles = new List<GCodeFile>();
            progressForm = new FrmProgressForm();
            progressSem = new Semaphore(0, 2);
            progressSem.Release();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            GCodeFiles.Clear();
            dgvFiles.DataSource = null;
            dgvFiles.Rows.Clear();
            lblTotalUsed.Text = "Total: 0g";
            lblTotalCost.Text = "Total: $0.00";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // First, isolate any *.gcode directories
                    List<String> gCodeDirectories = GetGCodeDirectoriesFromDirectoryList(Directory.GetFiles(fbd.SelectedPath));

                    // If files are found, run the processes
                    if(gCodeDirectories.Count > 0)
                    {
                        progressForm.ProgressBar.Maximum = gCodeDirectories.Count;
                        progressForm.ProgressBar.Value = 0;
                        progressForm.CancellationRequested = false;
                        
                        Thread newFormThread = new Thread(() => { progressForm.ShowDialog(); });
                        newFormThread.Start();

                        // This process WILL freeze up the screen.
                        LoadGCodeFilesFromDirectoryList(gCodeDirectories);
                        
                        Utilities.WriteToThreadFromThread(progressForm, () =>
                        {
                            progressForm.Close();
                        });

                        newFormThread.Join();

                        lblTotalUsed.Text = "Total: " + GetTotalFilamentUsed() + "g";
                        lblTotalCost.Text = string.Format("Total: {0:C}", GetTotalFilamentUsedCost());
                    }

                    // If no files are found, pop up an error message
                    else
                    {
                        MessageBox.Show("No *.gcode files were found in that directory. :(", "No Files Found");
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvFiles.Rows.Count > 0)
            {
                //Stream stream;
                SaveFileDialog saveFile = new SaveFileDialog();

                saveFile.Filter = "csv files (*.csv)|*.csv";
                saveFile.FilterIndex = 2;
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = saveFile.OpenFile())
                    {
                        if (stream != null)
                        {
                            StringBuilder csv = new StringBuilder();

                            // First write the header
                            csv.AppendLine("Item Name,Filament Used,Total Cost");

                            // Write each G-Code file to the CSV
                            foreach (GCodeFile g in GCodeFiles)
                            {
                                if (g.FileName != null) csv.AppendLine($"{g.FileName},{g.FilamentUsed},{g.FilamentUsedCost}");
                            }

                            // Append totals for each column
                            csv.AppendLine($"Total:,{GetTotalFilamentUsed()},{GetTotalFilamentUsedCost()}");

                            // Convert StringBuilder to bytes and close the stream
                            byte[] bytes = Encoding.ASCII.GetBytes(csv.ToString());
                            stream.Write(bytes, 0, bytes.Length);
                            stream.Flush();
                        }
                    }
                }
            }
            else MessageBox.Show("Uh-oh, there isn't any data to export!", "No G-Code Files");
        }

        private void dgvFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = (int)dgvFiles.Rows[dgvFiles.CurrentCell.RowIndex].Cells[0].Value;

                string output = $"File name: {GCodeFiles[index].FileName}\n" +
                    $"Filament Spool Cost: {string.Format("{0:C}", GCodeFiles[index].FilamentSpoolCost)}\n" +
                    $"Filament Used: {GCodeFiles[index].FilamentUsed}g\n" +
                    $"Total cost: {string.Format("{0:C}", GCodeFiles[index].FilamentUsedCost)}";

                MessageBox.Show(output, "G-Code File Details");
            }

            // If this exception occurs, the user didn't click an existing cell
            catch(NullReferenceException) { }
        }

        private void dgvFiles_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            foreach(DataGridViewRow row in dgvFiles.SelectedRows)
            {
                try
                {
                    int index = (int)row.Cells[0].Value;

                    // Taking away the index makes it not match up with the DGV, so we just make it blank...
                    GCodeFiles[index] = new GCodeFile();
                }

                // If this exception occurs, the user didn't delete an existing cell
                catch (NullReferenceException) { /* Ignored */ }
            }

            lblTotalUsed.Text = "Total: " + GetTotalFilamentUsed() + "g";
            lblTotalCost.Text = string.Format("Total: {0:C}", GetTotalFilamentUsedCost());
        }

        /// <summary>
        /// Takes a list of directories and keeps only the ones that end in *.gcode.
        /// </summary>
        /// <param name="directories">List of subdirectories within a directory.</param>
        /// <returns></returns>
        private List<String> GetGCodeDirectoriesFromDirectoryList(string[] directories)
        {
            List<string> gCodeDirectories = new List<string>();
            for (int i = 0; i < directories.Length; i++)
            {
                if (directories[i].EndsWith(".gcode")) gCodeDirectories.Add(directories[i]);
            }
            return gCodeDirectories;
        }

        /// <summary>
        /// Loads .gcode files from a directory.
        /// </summary>
        /// <param name="directories">A list of GCode file directories.</param>
        private void LoadGCodeFilesFromDirectoryList(List<String> directories)
        {
            GCodeFile[] d1 = new GCodeFile[0];
            GCodeFile[] d2 = new GCodeFile[0];
            int threadTaskDivider = 0;

            // If more than one file, calculate how much each thread will process
            if(directories.Count > 1)
            {
                threadTaskDivider = directories.Count / 2;
            }

            // If only one file, one thread can take care of it all
            else
            {
                threadTaskDivider = directories.Count - 1;
            }

            // Parse and add the files to the main list
            Thread p1 = new Thread(() => {
                d1 = GCodeFileParser(0, threadTaskDivider, directories);
            });
            p1.Start();

            // If more than one file is present, split up the tasks between two threads
            if (directories.Count > 1)
            {
                Thread p2 = new Thread(() =>
                {
                    d2 = GCodeFileParser(threadTaskDivider + 1, directories.Count - 1, directories);
                });
                p2.Start();
                p2.Join();
            }

            p1.Join();

            if (!progressForm.CancellationRequested)
            {
                // Display files on the UI
                AddGCodeToList(d1);
                AddGCodeToList(d2);
            }
        }

        private GCodeFile[] GCodeFileParser(int startIndex, int endIndex, List<string> directories)
        {
            GCodeFile[] gCodes = new GCodeFile[endIndex - startIndex + 1];
            
            for(int i = 0; i < gCodes.Length && !progressForm.CancellationRequested; i++)
            {
                gCodes[i] = new GCodeFile();

                // File name

                string oneDir = directories[i + startIndex];

                // If this fails, it means no "\\" was found, and we are in a single-letter directory
                try
                {
                    string[] oneDirSplit = oneDir.Split(new string[] { "\\" }, StringSplitOptions.None);
                    oneDir = oneDirSplit[oneDirSplit.Length - 1];
                }
                catch { }

                gCodes[i].FileName = oneDir;


                using (StreamReader stream = File.OpenText(directories[i + startIndex]))
                {
                    string[] currLine = new string[0];

                    // Keep searching the file until all GCodeFile fields have been populated, 
                    // or the end of the file is reached
                    while (!gCodes[i].AllFieldsPopulated() && stream.Peek() > 0 && !progressForm.CancellationRequested)
                    {
                        // This represents one line of the .gcode file. This function moves the file pointer to the next
                        // line after each call
                        currLine = stream.ReadLine().Split(new string[] { " = " }, StringSplitOptions.None);

                        // Filament spool cost
                        if (currLine[0].Contains("filament_cost"))
                        {
                            double value = double.Parse(currLine[1]);
                            gCodes[i].FilamentSpoolCost = value;
                        }

                        // Filament used
                        else if (currLine[0].Contains("total filament used [g]"))
                        {
                            double value = double.Parse(currLine[1]);
                            gCodes[i].FilamentUsed = value;
                        }

                        // Filament used cost
                        else if (currLine[0].Contains("total filament cost"))
                        {
                            double value = double.Parse(currLine[1]);
                            gCodes[i].FilamentUsedCost = value;
                        }
                    }
                }

                // Increment the loading bar by 1
                progressSem.WaitOne();
                Utilities.WriteToThreadFromThread(progressForm, () =>
                {
                    progressForm.ProgressBar.PerformStep();
                });
                progressSem.Release();
            }

            return gCodes;
        }

        private void AddGCodeToList(GCodeFile[] file)
        {
            for (int i = 0; i < file.Length; i++)
            {
                dgvFiles.Rows.Add(GCodeFiles.Count, file[i].FileName, file[i].FilamentSpoolCost, file[i].FilamentUsed, string.Format("{0:C}", file[i].FilamentUsedCost));
                GCodeFiles.Add(file[i]);
            }
        }

        private double GetTotalFilamentUsed()
        {
            double total = 0;
            for(int i = 0; i < GCodeFiles.Count; i++)
            {
                total += GCodeFiles[i].FilamentUsed;
            }

            return total;
        }

        private double GetTotalFilamentUsedCost()
        {
            double total = 0;
            for (int i = 0; i < GCodeFiles.Count; i++)
            {
                total += GCodeFiles[i].FilamentUsedCost;
            }

            return total;
        }
    }
}
