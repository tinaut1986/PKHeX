namespace PKHeX.WinForms
{
    partial class LivingDex
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
            this.L_FilterGame = new System.Windows.Forms.Label();
            this.L_SelectedGamesCount = new System.Windows.Forms.Label();
            this.B_SelectGames = new System.Windows.Forms.Button();
            this.L_FilterLevel = new System.Windows.Forms.Label();
            this.CB_LevelOp = new System.Windows.Forms.ComboBox();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.B_ApplyFilter = new System.Windows.Forms.Button();
            this.P_Toolbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            this.SuspendLayout();
            // 
            // P_Toolbox
            // 
            this.P_Toolbox.Controls.Add(this.B_ApplyFilter);
            this.P_Toolbox.Controls.Add(this.NUD_Level);
            this.P_Toolbox.Controls.Add(this.CB_LevelOp);
            this.P_Toolbox.Controls.Add(this.L_FilterLevel);
            this.P_Toolbox.Controls.Add(this.B_SelectGames);
            this.P_Toolbox.Controls.Add(this.L_SelectedGamesCount);
            this.P_Toolbox.Controls.Add(this.L_FilterGame);
            this.P_Toolbox.Controls.Add(this.PB_Progress);
            this.P_Toolbox.Controls.Add(this.L_Status);
            this.P_Toolbox.Controls.Add(this.L_Folder);
            this.P_Toolbox.Controls.Add(this.B_SelectFolder);
            this.P_Toolbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_Toolbox.Location = new System.Drawing.Point(0, 0);
            this.P_Toolbox.Name = "P_Toolbox";
            this.P_Toolbox.Size = new System.Drawing.Size(800, 90);
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
            this.L_Status.Location = new System.Drawing.Point(160, 68);
            this.L_Status.Name = "L_Status";
            this.L_Status.Size = new System.Drawing.Size(0, 15);
            this.L_Status.TabIndex = 2;
            // 
            // PB_Progress
            // 
            this.PB_Progress.Location = new System.Drawing.Point(12, 72);
            this.PB_Progress.Name = "PB_Progress";
            this.PB_Progress.Size = new System.Drawing.Size(135, 10);
            this.PB_Progress.TabIndex = 3;
            this.PB_Progress.Visible = false;
            // 
            // FLP_Pokedex
            // 
            this.FLP_Pokedex.AutoScroll = true;
            this.FLP_Pokedex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FLP_Pokedex.Location = new System.Drawing.Point(0, 90);
            this.FLP_Pokedex.Name = "FLP_Pokedex";
            this.FLP_Pokedex.Size = new System.Drawing.Size(800, 360);
            this.FLP_Pokedex.TabIndex = 1;
            // 
            // L_FilterGame
            // 
            this.L_FilterGame.AutoSize = true;
            this.L_FilterGame.Location = new System.Drawing.Point(12, 45);
            this.L_FilterGame.Name = "L_FilterGame";
            this.L_FilterGame.Size = new System.Drawing.Size(46, 15);
            this.L_FilterGame.TabIndex = 4;
            this.L_FilterGame.Text = "Games:";
            // 
            // L_SelectedGamesCount
            // 
            this.L_SelectedGamesCount.AutoSize = true;
            this.L_SelectedGamesCount.Location = new System.Drawing.Point(64, 45);
            this.L_SelectedGamesCount.Name = "L_SelectedGamesCount";
            this.L_SelectedGamesCount.Size = new System.Drawing.Size(43, 15);
            this.L_SelectedGamesCount.TabIndex = 10;
            this.L_SelectedGamesCount.Text = "All selected";
            // 
            // B_SelectGames
            // 
            this.B_SelectGames.Location = new System.Drawing.Point(150, 42);
            this.B_SelectGames.Name = "B_SelectGames";
            this.B_SelectGames.Size = new System.Drawing.Size(110, 23);
            this.B_SelectGames.TabIndex = 5;
            this.B_SelectGames.Text = "Change Games...";
            this.B_SelectGames.UseVisualStyleBackColor = true;
            this.B_SelectGames.Click += new System.EventHandler(this.B_SelectGames_Click);
            // 
            // L_FilterLevel
            // 
            this.L_FilterLevel.AutoSize = true;
            this.L_FilterLevel.Location = new System.Drawing.Point(280, 45);
            this.L_FilterLevel.Name = "L_FilterLevel";
            this.L_FilterLevel.Size = new System.Drawing.Size(37, 15);
            this.L_FilterLevel.TabIndex = 6;
            this.L_FilterLevel.Text = "Level:";
            // 
            // CB_LevelOp
            // 
            this.CB_LevelOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_LevelOp.FormattingEnabled = true;
            this.CB_LevelOp.Items.AddRange(new object[] {
            "Any",
            "=",
            ">",
            "<",
            ">=",
            "<="});
            this.CB_LevelOp.Location = new System.Drawing.Point(323, 42);
            this.CB_LevelOp.Name = "CB_LevelOp";
            this.CB_LevelOp.Size = new System.Drawing.Size(50, 23);
            this.CB_LevelOp.TabIndex = 7;
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(379, 42);
            this.NUD_Level.Name = "NUD_Level";
            this.NUD_Level.Size = new System.Drawing.Size(45, 23);
            this.NUD_Level.TabIndex = 8;
            this.NUD_Level.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // B_ApplyFilter
            // 
            this.B_ApplyFilter.Location = new System.Drawing.Point(440, 42);
            this.B_ApplyFilter.Name = "B_ApplyFilter";
            this.B_ApplyFilter.Size = new System.Drawing.Size(100, 23);
            this.B_ApplyFilter.TabIndex = 9;
            this.B_ApplyFilter.Text = "Apply Filters";
            this.B_ApplyFilter.UseVisualStyleBackColor = true;
            this.B_ApplyFilter.Click += new System.EventHandler(this.B_ApplyFilter_Click);
            // 
            // SAV_LivingDex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.FLP_Pokedex);
            this.Controls.Add(this.P_Toolbox);
            this.Name = "LivingDex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LivingDex";
            this.P_Toolbox.ResumeLayout(false);
            this.P_Toolbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel P_Toolbox;
        private System.Windows.Forms.Button B_SelectFolder;
        private System.Windows.Forms.Label L_Folder;
        private System.Windows.Forms.Label L_Status;
        private System.Windows.Forms.ProgressBar PB_Progress;
        private System.Windows.Forms.FlowLayoutPanel FLP_Pokedex;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label L_FilterGame;
        private System.Windows.Forms.Label L_SelectedGamesCount;
        private System.Windows.Forms.Button B_SelectGames;
        private System.Windows.Forms.Label L_FilterLevel;
        private System.Windows.Forms.ComboBox CB_LevelOp;
        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.Button B_ApplyFilter;
    }
}
