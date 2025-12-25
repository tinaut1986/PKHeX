namespace PKHeX.WinForms
{
    partial class SAV_LivingDex
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.P_Toolbox = new System.Windows.Forms.Panel();
            this.B_SelectFolder = new System.Windows.Forms.Button();
            this.L_Folder = new System.Windows.Forms.Label();
            this.FLP_Pokedex = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.L_Status = new System.Windows.Forms.Label();
            this.PB_Progress = new System.Windows.Forms.ProgressBar();
            this.P_Toolbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_Toolbox
            // 
            this.P_Toolbox.Controls.Add(this.PB_Progress);
            this.P_Toolbox.Controls.Add(this.L_Status);
            this.P_Toolbox.Controls.Add(this.L_Folder);
            this.P_Toolbox.Controls.Add(this.B_SelectFolder);
            this.P_Toolbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_Toolbox.Location = new System.Drawing.Point(0, 0);
            this.P_Toolbox.Name = "P_Toolbox";
            this.P_Toolbox.Size = new System.Drawing.Size(800, 60);
            this.P_Toolbox.TabIndex = 0;
            // 
            // B_SelectFolder
            // 
            this.B_SelectFolder.Location = new System.Drawing.Point(12, 11);
            this.B_SelectFolder.Name = "B_SelectFolder";
            this.B_SelectFolder.Size = new System.Drawing.Size(135, 26);
            this.B_SelectFolder.TabIndex = 0;
            this.B_SelectFolder.Text = "Select Folder...";
            this.B_SelectFolder.UseVisualStyleBackColor = true;
            this.B_SelectFolder.Click += new System.EventHandler(this.B_SelectFolder_Click);
            // 
            // L_Folder
            // 
            this.L_Folder.AutoSize = true;
            this.L_Folder.Location = new System.Drawing.Point(160, 15);
            this.L_Folder.Name = "L_Folder";
            this.L_Folder.Size = new System.Drawing.Size(111, 15);
            this.L_Folder.TabIndex = 1;
            this.L_Folder.Text = "No folder selected.";
            // 
            // L_Status
            // 
            this.L_Status.AutoSize = true;
            this.L_Status.Location = new System.Drawing.Point(160, 35);
            this.L_Status.Name = "L_Status";
            this.L_Status.Size = new System.Drawing.Size(0, 15);
            this.L_Status.TabIndex = 2;
            // 
            // PB_Progress
            // 
            this.PB_Progress.Location = new System.Drawing.Point(12, 38);
            this.PB_Progress.Name = "PB_Progress";
            this.PB_Progress.Size = new System.Drawing.Size(120, 10);
            this.PB_Progress.TabIndex = 3;
            this.PB_Progress.Visible = false;
            // 
            // FLP_Pokedex
            // 
            this.FLP_Pokedex.AutoScroll = true;
            this.FLP_Pokedex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FLP_Pokedex.Location = new System.Drawing.Point(0, 60);
            this.FLP_Pokedex.Name = "FLP_Pokedex";
            this.FLP_Pokedex.Size = new System.Drawing.Size(800, 390);
            this.FLP_Pokedex.TabIndex = 1;
            // 
            // SAV_LivingDex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.FLP_Pokedex);
            this.Controls.Add(this.P_Toolbox);
            this.Name = "SAV_LivingDex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LivingDex";
            this.P_Toolbox.ResumeLayout(false);
            this.P_Toolbox.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel P_Toolbox;
        private System.Windows.Forms.Button B_SelectFolder;
        private System.Windows.Forms.Label L_Folder;
        private System.Windows.Forms.Label L_Status;
        private System.Windows.Forms.ProgressBar PB_Progress;
        private System.Windows.Forms.FlowLayoutPanel FLP_Pokedex;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
