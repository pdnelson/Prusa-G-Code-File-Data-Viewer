using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrusaGCodeFileDataViewer
{
    public partial class frmGCodeViewer : Form
    {
        List<GCodeFile> GCodeFiles;

        public frmGCodeViewer()
        {
            InitializeComponent();
            GCodeFiles = new List<GCodeFile>();
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
                    LoadGCodeFilesFromDirectoryList(Directory.GetFiles(fbd.SelectedPath));
                }
            }

            lblTotalUsed.Text = "Total: " + GetTotalFilamentUsed() + "g";
            lblTotalCost.Text = string.Format("Total: {0:C}", GetTotalFilamentUsedCost());
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (GCodeFiles.Count > 0)
            {
                Stream stream;
                SaveFileDialog saveFile = new SaveFileDialog();

                saveFile.Filter = "csv files (*.csv)|*.csv";
                saveFile.FilterIndex = 2;
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {

                    if ((stream = saveFile.OpenFile()) != null)
                    {
                        StringBuilder csv = new StringBuilder();

                        // First write the header
                        csv.AppendLine("Item Name,Filament Used,Total Cost");

                        // Write each G-Code file to the CSV
                        foreach(GCodeFile g in GCodeFiles)
                        {
                            csv.AppendLine($"{g.FileName},{g.FilamentUsed},{g.FilamentUsedCost}");
                        }

                        // Convert StringBuilder to bytes and close the stream
                        byte[] bytes = Encoding.ASCII.GetBytes(csv.ToString());
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Close();
                    }
                }
            }
            else MessageBox.Show("Uh-oh, there isn't any data to export!", "No G-Code Files");
        }

        private void LoadGCodeFilesFromDirectoryList(string[] directories)
        {
            // Isolate G-Code files
            List<string> gCodeDirectories = new List<string>();
            for(int i = 0; i < directories.Length; i++)
            {
                if (directories[i].EndsWith(".gcode")) gCodeDirectories.Add(directories[i]);
            }

            GCodeFile[] d1 = new GCodeFile[0];
            GCodeFile[] d2 = new GCodeFile[0];
            int threadTaskDivider = 0;

            // If more than one file, calculate how much each thread will process
            if(directories.Length > 1)
            {
                threadTaskDivider = directories.Length / 2;
            }

            // If only one file, one thread can take care of it all
            else
            {
                threadTaskDivider = directories.Length - 1;
            }

            // Parse and add the files to the main list
            Thread p1 = new Thread(() => {
                d1 = GCodeFileParser(0, threadTaskDivider, gCodeDirectories);
            });
            p1.Start();

            // If more than one file is present, split up the tasks between two threads
            if (directories.Length > 1)
            {
                Thread p2 = new Thread(() =>
                {
                    d2 = GCodeFileParser(threadTaskDivider + 1, directories.Length - 1, gCodeDirectories);
                });
                p2.Start();
                p2.Join();
            }

            p1.Join();

            // Display files on the UI
            AddGCodeToList(d1);
            AddGCodeToList(d2);
        }

        private GCodeFile[] GCodeFileParser(int startIndex, int endIndex, List<string> directories)
        {
            GCodeFile[] gCodes = new GCodeFile[endIndex - startIndex + 1];

            for(int i = 0; i < gCodes.Length; i++)
            {
                gCodes[i] = new GCodeFile();

                // File name
                string[] oneDir = directories[i + startIndex].Split(new string[] { "\\" }, StringSplitOptions.None);
                gCodes[i].FileName = oneDir[oneDir.Length - 1];


                using (StreamReader stream = File.OpenText(directories[i + startIndex]))
                {
                    string[] currLine = new string[0];

                    while (gCodes[i].FilamentUsed == 0 || gCodes[i].FilamentUsedCost == 0 || gCodes[i].FilamentSpoolCost == 0)
                    {
                        if (stream.Peek() > 0)
                        {
                            currLine = stream.ReadLine().Split(new string[] { " = " }, StringSplitOptions.None);
                        }
                        else
                        {
                            break;
                        }

                        // Filament spool cost
                        if (currLine[0].Contains("filament_cost"))
                        {
                            double value = double.Parse(currLine[1]);
                            gCodes[i].FilamentSpoolCost = value;
                        }

                        // Filament used
                        if (currLine[0].Contains("total filament used [g]"))
                        {
                            double value = double.Parse(currLine[1]);
                            gCodes[i].FilamentUsed = value;
                        }

                        // Filament used cost
                        if (currLine[0].Contains("total filament cost"))
                        {
                            double value = double.Parse(currLine[1]);
                            gCodes[i].FilamentUsedCost = value;
                        }

                    }
                }
            }

            return gCodes;
        }

        private void AddGCodeToList(GCodeFile[] file)
        {
            for (int i = 0; i < file.Length; i++)
            {
                dgvFiles.Rows.Add(file[i].FileName, file[i].FilamentSpoolCost, file[i].FilamentUsed, file[i].FilamentUsedCost);
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
