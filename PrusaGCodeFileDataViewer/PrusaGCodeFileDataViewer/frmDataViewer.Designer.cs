namespace PrusaGCodeFileDataViewer
{
    partial class frmGCodeViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.lblTotalUsed = new System.Windows.Forms.Label();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spoolCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.used = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filamentUsedCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(90, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load Directory";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(108, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToOrderColumns = true;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.name,
            this.spoolCost,
            this.used,
            this.filamentUsedCost});
            this.dgvFiles.Location = new System.Drawing.Point(12, 41);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.ReadOnly = true;
            this.dgvFiles.Size = new System.Drawing.Size(639, 386);
            this.dgvFiles.TabIndex = 2;
            this.dgvFiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFiles_CellDoubleClick);
            this.dgvFiles.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvFiles_UserDeletingRow);
            // 
            // lblTotalUsed
            // 
            this.lblTotalUsed.AutoSize = true;
            this.lblTotalUsed.Location = new System.Drawing.Point(419, 25);
            this.lblTotalUsed.Name = "lblTotalUsed";
            this.lblTotalUsed.Size = new System.Drawing.Size(49, 13);
            this.lblTotalUsed.TabIndex = 3;
            this.lblTotalUsed.Text = "Total: 0g";
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.AutoSize = true;
            this.lblTotalCost.Location = new System.Drawing.Point(519, 25);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(64, 13);
            this.lblTotalCost.TabIndex = 4;
            this.lblTotalCost.Text = "Total: $0.00";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(189, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // index
            // 
            this.index.HeaderText = "index";
            this.index.Name = "index";
            this.index.ReadOnly = true;
            this.index.Visible = false;
            // 
            // name
            // 
            this.name.HeaderText = "File Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 280;
            // 
            // spoolCost
            // 
            this.spoolCost.HeaderText = "Spool Cost";
            this.spoolCost.Name = "spoolCost";
            this.spoolCost.ReadOnly = true;
            this.spoolCost.Width = 90;
            // 
            // used
            // 
            this.used.HeaderText = "Filament Used";
            this.used.Name = "used";
            this.used.ReadOnly = true;
            // 
            // filamentUsedCost
            // 
            this.filamentUsedCost.HeaderText = "Filament Used Cost";
            this.filamentUsedCost.Name = "filamentUsedCost";
            this.filamentUsedCost.ReadOnly = true;
            // 
            // frmGCodeViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 439);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblTotalCost);
            this.Controls.Add(this.lblTotalUsed);
            this.Controls.Add(this.dgvFiles);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmGCodeViewer";
            this.Text = "Prusa G-Code File Data Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.Label lblTotalUsed;
        private System.Windows.Forms.Label lblTotalCost;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn spoolCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn used;
        private System.Windows.Forms.DataGridViewTextBoxColumn filamentUsedCost;
    }
}

