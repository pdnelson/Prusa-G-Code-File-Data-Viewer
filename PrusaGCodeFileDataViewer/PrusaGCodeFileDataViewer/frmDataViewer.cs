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

        public frmGCodeViewer()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
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
                string file = File.ReadAllText(directories[i + startIndex]);

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

        private void AddGCodeToList(GCodeFile[] file)
        {
            for (int i = 0; i < file.Length; i++)
            {
                dgvFiles.Rows.Add(file[i].FileName, file[i].FileSize, file[i].FilamentSpoolCost, file[i].FilamentUsed, file[i].FilamentUsedCost);
            }
        }
    }
}
