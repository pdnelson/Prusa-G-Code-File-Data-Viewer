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
        //private static Semaphore sem;
        List<GCodeFile> Files;

        public frmGCodeViewer()
        {
            InitializeComponent();
            //sem = new Semaphore(0, 4);
            Files = new List<GCodeFile>();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Files.Clear();
            dgvFiles.DataSource = null;
            dgvFiles.Rows.Clear();
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
        }

        private void LoadGCodeFilesFromDirectoryList(string[] directories)
        {
            // Isolate G-Code files
            List<string> gCodeDirectories = new List<string>();
            for(int i = 0; i < directories.Length; i++)
            {
                if (directories[i].EndsWith(".gcode")) gCodeDirectories.Add(directories[i]);
            }

            // Parse and add the files to the main list
            Files.AddRange(GCodeFileParser(0, gCodeDirectories.Count - 1, gCodeDirectories));

            // Display files on the UI
            AddGCodeToList(Files);
        }

        private GCodeFile[] GCodeFileParser(int startIndex, int endIndex, List<string> directories)
        {
            GCodeFile[] gCodes = new GCodeFile[endIndex - startIndex + 1];

            for(int i = startIndex; i <= endIndex; i++)
            {
                gCodes[i] = new GCodeFile();
                string file = File.ReadAllText(directories[i]);

                // File name
                string[] oneDir = directories[i].Split(new string[] { "\\" }, StringSplitOptions.None);
                gCodes[i].FileName = oneDir[oneDir.Length - 1];

                // File size
                gCodes[i].FileSize = file.Length;

                // Filament spool cost
                gCodes[i].FilamentSpoolCost = 0; // TODO: Implement

                // Filament used
                gCodes[i].FilamentUsed = 0; // TODO: Implement

                // Filament used cost
                gCodes[i].FilamentUsedCost = 0; // TODO: Implement
            }

            return gCodes;
        }

        private void AddGCodeToList(List<GCodeFile> file)
        {
            for (int i = 0; i < file.Count; i++)
            {
                dgvFiles.Rows.Add(file[i].FileName, file[i].FileSize, file[i].FilamentSpoolCost, file[i].FilamentUsed, file[i].FilamentUsedCost);
            }
        }
    }
}
