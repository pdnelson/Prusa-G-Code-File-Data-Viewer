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
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.size,
            this.spoolCost,
            this.used,
            this.filamentUsedCost});
            this.dgvFiles.Location = new System.Drawing.Point(12, 41);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.Size = new System.Drawing.Size(622, 238);
            this.dgvFiles.TabIndex = 2;
            // 
            // name
            // 
            this.name.HeaderText = "File Name";
            this.name.Name = "name";
            this.name.Width = 200;
            // 
            // size
            // 
            this.size.HeaderText = "File Size";
            this.size.Name = "size";
            this.size.Width = 75;
            // 
            // spoolCost
            // 
            this.spoolCost.HeaderText = "Spool Cost";
            this.spoolCost.Name = "spoolCost";
            this.spoolCost.Width = 90;
            // 
            // used
            // 
            this.used.HeaderText = "Filament Used";
            this.used.Name = "used";
            // 
            // filamentUsedCost
            // 
            this.filamentUsedCost.HeaderText = "Filament Used Cost";
            this.filamentUsedCost.Name = "filamentUsedCost";
            // 
            // frmGCodeViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 291);
            this.Controls.Add(this.dgvFiles);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnLoad);
            this.Name = "frmGCodeViewer";
            this.Text = "Prusa G-Code File Data Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn spoolCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn used;
        private System.Windows.Forms.DataGridViewTextBoxColumn filamentUsedCost;
    }
}

