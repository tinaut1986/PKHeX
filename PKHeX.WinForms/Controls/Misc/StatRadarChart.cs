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

            // Ensure party stats are present so we don't display 0s or Level 1 stats for box PKMs
            _pkm.ForcePartyData();

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = ClientRectangle;
            var center = new PointF(rect.Width / 2f, rect.Height / 2f);
            var radius = Math.Min(rect.Width, rect.Height) * 0.35f;

            // Stats order: HP, Atk, Def, SpD, SpA, Spe
            var p = _pkm.PersonalInfo;
            int[] baseStats = [p.HP, p.ATK, p.DEF, p.SPD, p.SPA, p.SPE];
            
            // Recalculate stats to ensure we have data even if the PKM is freshly loaded/cloned
            // PKM.GetStats returns H/A/B/S/C/D order (3=Spe, 4=SpA, 5=SpD)
            var stats = _pkm.GetStats(p);
            int[] liveStats = [stats[0], stats[1], stats[2], stats[5], stats[4], stats[3]];
            int[] ivs = [_pkm.IV_HP, _pkm.IV_ATK, _pkm.IV_DEF, _pkm.IV_SPD, _pkm.IV_SPA, _pkm.IV_SPE];
            string[] labels = ["HP", "Atk", "Def", "SpD", "SpA", "Spe"];

            // A scale of 100-160 usually looks best for individual Pokémon stats display.
            const float maxScale = 160f;

            // Draw background rings
            for (int r = 1; r <= 4; r++)
            {
                DrawPolygon(g, center, radius * (r / 4f), 6, Pens.LightGray);
            }

            var pointsBase = new PointF[6];
            var pointsLive = new PointF[6];

            for (int i = 0; i < 6; i++)
            {
                float angle = (float)(i * Math.PI * 2 / 6 - Math.PI / 2);

                float rBase = radius * (Math.Min(baseStats[i], maxScale) / maxScale);
                pointsBase[i] = new PointF(center.X + (float)Math.Cos(angle) * rBase, center.Y + (float)Math.Sin(angle) * rBase);

                float rLive = radius * (Math.Min(liveStats[i], maxScale) / maxScale);
                pointsLive[i] = new PointF(center.X + (float)Math.Cos(angle) * rLive, center.Y + (float)Math.Sin(angle) * rLive);

                float labelR = radius + 25;
                var lp = new PointF(center.X + (float)Math.Cos(angle) * labelR, center.Y + (float)Math.Sin(angle) * labelR);

                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using var font = new Font(Font.FontFamily, 7.5f, FontStyle.Regular);
                g.DrawString($"{labels[i]}\n{liveStats[i]} ({ivs[i]})", font, Brushes.DimGray, lp, sf);
            }

            // Draw Base Stats Polygon (Standard PKHeX Orange/Yellow)
            using (var penBase = new Pen(Color.FromArgb(180, Color.Orange), 1f) { DashStyle = DashStyle.Dash })
                g.DrawPolygon(penBase, pointsBase);

            // Draw Live Stats Polygon (Standard PKHeX Blue)
            using (var brush = new SolidBrush(Color.FromArgb(100, Color.CornflowerBlue)))
                g.FillPolygon(brush, pointsLive);
            using (var penLive = new Pen(Color.CornflowerBlue, 2f))
                g.DrawPolygon(penLive, pointsLive);
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
