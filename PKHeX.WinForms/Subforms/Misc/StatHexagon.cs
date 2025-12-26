using System;
using System.Drawing;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.WinForms.Controls;

namespace PKHeX.WinForms
{
    public partial class StatHexagon : Form
    {
        private readonly StatRadarChart Chart;

        public StatRadarChart GetChart() => Chart;

        public StatHexagon()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Text = "Stat Hexagon";
            Size = new Size(300, 300);
            TopMost = true;
            ShowInTaskbar = false;
            MinimumSize = new Size(200, 200);
            
            Chart = new StatRadarChart { Dock = DockStyle.Fill };
            Controls.Add(Chart);
        }

        public void UpdatePKM(PKM pk)
        {
            if (Chart.SelectedPKM != pk)
            {
                Chart.SelectedPKM = pk;
            }
            else
            {
                Chart.Invalidate();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "StatHexagon";
            this.StartPosition = FormStartPosition.Manual;
            this.ResumeLayout(false);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            base.OnFormClosing(e);
        }
    }
}
