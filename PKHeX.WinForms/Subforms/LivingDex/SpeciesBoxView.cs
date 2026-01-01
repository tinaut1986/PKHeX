using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.Drawing.PokeSprite;
using PKHeX.WinForms.Controls;

namespace PKHeX.WinForms
{
    public class SpeciesBoxView : Form
    {
        private readonly PKMEditor PKME;
        private readonly SaveFile SAV;
        private readonly PokeGrid Grid;
        private readonly VScrollBar ScrollBar;
        private List<SlotCache> SpeciesList = [];
        private readonly SummaryPreviewer ShowSet = new();
        private readonly ToolTip hover = new();
        
        private const int GridWidth = 6;
        private const int GridHeight = 5;
        private const int RES_MIN = GridWidth * 1;
        private const int RES_MAX = GridWidth * GridHeight;

        public SpeciesBoxView(PKMEditor pkme, SaveFile sav)
        {
            PKME = pkme;
            SAV = sav;
            Text = "Species Detail View";
            Size = new Size(400, 450);
            Icon = Properties.Resources.Icon;
            StartPosition = FormStartPosition.CenterParent;
            
            ScrollBar = new VScrollBar 
            { 
                Dock = DockStyle.Right, 
                LargeChange = 1,
                Width = 20
            };
            ScrollBar.Scroll += (s, e) => FillGrid(e.NewValue);

            Grid = new PokeGrid 
            { 
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Controls.Add(Grid);
            Controls.Add(ScrollBar);

            Grid.InitializeGrid(GridWidth, GridHeight, SpriteUtil.Spriter);
            Grid.SetBackground(Properties.Resources.box_wp_clean);

            foreach (var pb in Grid.Entries)
            {
                pb.MouseClick += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
                    {
                        int speciesIndex = Grid.Entries.IndexOf(pb);
                        int index = speciesIndex + (ScrollBar.Value * GridWidth);
                        if (index < SpeciesList.Count)
                            PKME.PopulateFields(SpeciesList[index].Entity);
                    }
                };
                
                pb.MouseMove += (_, args) => ShowSet.UpdatePreviewPosition(args.Location);
                pb.MouseEnter += (s, e) =>
                {
                    int speciesIndex = Grid.Entries.IndexOf(pb);
                    int index = speciesIndex + (ScrollBar.Value * GridWidth);
                    if (index < SpeciesList.Count)
                    {
                        var entry = SpeciesList[index];
                        ShowSet.Show(pb, entry.Entity, entry.Source.Type);
                    }
                };
                pb.MouseLeave += (_, _) => ShowSet.Clear();
            }
        }

        public void UpdateSpecies(ushort species, List<SlotCache> allResults)
        {
            SpeciesList = allResults.Where(z => z.Entity.Species == species).ToList();
            Text = $"#{species:000} {(Species)species} Box View";
            
            int maxScroll = (int)Math.Ceiling((decimal)SpeciesList.Count / GridWidth) - GridHeight;
            ScrollBar.Maximum = Math.Max(0, maxScroll + ScrollBar.LargeChange - 1);
            ScrollBar.Value = 0;
            
            FillGrid(0);
        }

        private void FillGrid(int start)
        {
            int begin = start * GridWidth;
            int end = Math.Min(RES_MAX, SpeciesList.Count - begin);
            
            for (int i = 0; i < end; i++)
            {
                var entry = SpeciesList[i + begin];
                Grid.Entries[i].Image = entry.Entity.Sprite(SAV, flagIllegal: true, storage: entry.Source.Type);
            }
            for (int i = Math.Max(0, end); i < RES_MAX; i++)
            {
                Grid.Entries[i].Image = null;
            }
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
