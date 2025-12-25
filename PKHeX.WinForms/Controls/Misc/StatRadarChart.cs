using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PKHeX.Core;

namespace PKHeX.WinForms.Controls
{
    public partial class StatRadarChart : UserControl
    {
        private PKM? _pkm;
        public PKM? SelectedPKM
        {
            get => _pkm;
            set
            {
                _pkm = value;
                Invalidate();
            }
        }

        public StatRadarChart()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "StatRadarChart";
            this.Size = new System.Drawing.Size(200, 200);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Chart_Paint);
            this.ResumeLayout(false);
        }

        private void Chart_Paint(object sender, PaintEventArgs e)
        {
            if (_pkm == null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = ClientRectangle;
            var center = new PointF(rect.Width / 2f, rect.Height / 2f);
            var radius = Math.Min(rect.Width, rect.Height) * 0.32f;

            // Stats: HP, Atk, Def, Spe, SpAtk, SpDef
            var stats = _pkm.GetStats(_pkm.PersonalInfo);
            int[] drawStats = [stats[0], stats[1], stats[2], stats[5], stats[4], stats[3]];
            int[] drawIVs = [_pkm.IV_HP, _pkm.IV_ATK, _pkm.IV_DEF, _pkm.IV_SPD, _pkm.IV_SPA, _pkm.IV_SPE];
            string[] labels = ["HP", "Atk", "Def", "SpD", "SpA", "Spe"];

            const float maxStat = 250f;
            const float maxIV = 31f;

            // Draw background rings
            for (int r = 1; r <= 4; r++)
            {
                DrawPolygon(g, center, radius * (r / 4f), 6, Pens.LightGray);
            }

            var pointsStat = new PointF[6];
            var pointsIV = new PointF[6];

            for (int i = 0; i < 6; i++)
            {
                float angle = (float)(i * Math.PI * 2 / 6 - Math.PI / 2);

                float rStat = radius * (Math.Min(drawStats[i], maxStat) / maxStat);
                pointsStat[i] = new PointF(center.X + (float)Math.Cos(angle) * rStat, center.Y + (float)Math.Sin(angle) * rStat);

                float rIV = radius * (drawIVs[i] / maxIV);
                pointsIV[i] = new PointF(center.X + (float)Math.Cos(angle) * rIV, center.Y + (float)Math.Sin(angle) * rIV);

                float labelR = radius + 30;
                var lp = new PointF(center.X + (float)Math.Cos(angle) * labelR, center.Y + (float)Math.Sin(angle) * labelR);

                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using var font = new Font(Font, FontStyle.Bold);
                g.DrawString($"{labels[i]}\n{drawStats[i]} ({drawIVs[i]})", font, Brushes.DimGray, lp, sf);
            }

            using (var penIV = new Pen(Color.FromArgb(150, Color.Orange), 1.5f) { DashStyle = DashStyle.Dash })
                g.DrawPolygon(penIV, pointsIV);

            using (var brush = new SolidBrush(Color.FromArgb(120, Color.FromArgb(65, 105, 225))))
                g.FillPolygon(brush, pointsStat);

            using (var penStat = new Pen(Color.FromArgb(65, 105, 225), 2f))
                g.DrawPolygon(penStat, pointsStat);
        }

        private static void DrawPolygon(Graphics g, PointF center, float radius, int sides, Pen pen)
        {
            var points = new PointF[sides];
            for (int i = 0; i < sides; i++)
            {
                float angle = (float)(i * Math.PI * 2 / sides - Math.PI / 2);
                points[i] = new PointF(center.X + (float)Math.Cos(angle) * radius, center.Y + (float)Math.Sin(angle) * radius);
            }
            g.DrawPolygon(pen, points);
        }
    }
}
