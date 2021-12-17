using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrusaGCodeFileDataViewer
{
    public partial class FrmProgressForm : Form
    {
        public ProgressBar ProgressBar;
        public bool CancellationRequested;

        public FrmProgressForm()
        {
            InitializeComponent();

            ProgressBar = new ProgressBar();
            Controls.Add(ProgressBar);
            ProgressBar.Location = new Point(37, 30);
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(326, 23);
            ProgressBar.TabIndex = 1;
            ProgressBar.Value = 0;
            ProgressBar.Minimum = 0;
            ProgressBar.Step = 1;

            CancellationRequested = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancellationRequested = true;
        }
    }
}
