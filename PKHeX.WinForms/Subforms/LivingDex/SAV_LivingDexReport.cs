using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.WinForms.Controls;
using PKHeX.Drawing.PokeSprite;

namespace PKHeX.WinForms
{
    public partial class SAV_LivingDexReport : ReportGrid
    {
        private readonly SplitContainer SC_Main;
        private readonly Panel P_Side;
        private readonly PictureBox PB_Sprite;
        private readonly Label L_Nickname;
        private readonly Label L_Species;
        private readonly Label L_Level;
        private readonly Label L_Nature;
        private readonly Label L_Ability;
        private readonly Label L_Item;
        private readonly StatRadarChart Chart;

        public SAV_LivingDexReport()
        {
            InitializeComponent();
            Icon = Properties.Resources.Icon;
            BackColor = Color.White;
            Width = 1100;
            Height = 750;
            MinimumSize = new Size(800, 600);

            // Set up the SplitContainer
            SC_Main = new SplitContainer
            {
                Dock = DockStyle.Fill,
                FixedPanel = FixedPanel.Panel2,
                TabIndex = 1
            };

            // Important: move the grid from the form to Panel1
            Controls.Remove(dgData);
            SC_Main.Panel1.Controls.Add(dgData);
            dgData.Dock = DockStyle.Fill;
            dgData.SelectionChanged += Data_SelectionChanged;

            // Side Panel Container
            P_Side = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                TabIndex = 0
            };

            var P_Header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 170,
                Padding = new Padding(10),
                TabIndex = 0
            };

            PB_Sprite = new PictureBox
            {
                Dock = DockStyle.Left,
                Size = new Size(110, 110),
                SizeMode = PictureBoxSizeMode.CenterImage,
                TabIndex = 0,
                TabStop = false
            };

            var TLP_Info = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                TabIndex = 1
            };
            TLP_Info.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            TLP_Info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 6; i++)
                TLP_Info.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));

            L_Nickname = new Label { AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0) };
            L_Species = new Label { AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0) };
            L_Level = new Label { AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0) };
            L_Nature = new Label { AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0) };
            L_Ability = new Label { AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0) };
            L_Item = new Label { AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0) };

            TLP_Info.Controls.Add(new Label { Text = "Nick:", AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 0);
            TLP_Info.Controls.Add(L_Nickname, 1, 0);
            TLP_Info.Controls.Add(new Label { Text = "Species:", AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 1);
            TLP_Info.Controls.Add(L_Species, 1, 1);
            TLP_Info.Controls.Add(new Label { Text = "Level:", AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 2);
            TLP_Info.Controls.Add(L_Level, 1, 2);
            TLP_Info.Controls.Add(new Label { Text = "Nature:", AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 3);
            TLP_Info.Controls.Add(L_Nature, 1, 3);
            TLP_Info.Controls.Add(new Label { Text = "Ability:", AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 4);
            TLP_Info.Controls.Add(L_Ability, 1, 4);
            TLP_Info.Controls.Add(new Label { Text = "Item:", AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 5);
            TLP_Info.Controls.Add(L_Item, 1, 5);

            P_Header.Controls.Add(TLP_Info);
            P_Header.Controls.Add(PB_Sprite);

            Chart = new StatRadarChart
            {
                Dock = DockStyle.Fill,
                TabIndex = 2,
                Visible = true
            };

            P_Side.Controls.Add(Chart);
            P_Side.Controls.Add(P_Header);

            SC_Main.Panel2.Controls.Add(P_Side);
            Controls.Add(SC_Main);

            // Set size properties after adding to ensure the control has a valid Width
            SC_Main.Panel2MinSize = 350;
            SC_Main.SplitterDistance = Math.Max(SC_Main.Panel1MinSize, SC_Main.Width - SC_Main.Panel2MinSize);

            WinFormsUtil.TranslateInterface(this, Main.CurrentLanguage);
        }

        private void Data_SelectionChanged(object sender, EventArgs e)
        {
            if (dgData.SelectedRows.Count == 0)
            {
                UpdateSidePanel(null);
                return;
            }

            var row = dgData.SelectedRows[0];
            if (row.DataBoundItem is EntitySummaryImage summary)
            {
                UpdateSidePanel(summary.Entity);
            }
            else
            {
                UpdateSidePanel(null);
            }
        }

        private void UpdateSidePanel(PKM? pk)
        {
            Chart.SelectedPKM = pk;
            if (pk == null)
            {
                PB_Sprite.Image = null;
                L_Nickname.Text = L_Species.Text = L_Level.Text = L_Nature.Text = L_Ability.Text = L_Item.Text = string.Empty;
                return;
            }

            var strings = GameInfo.Strings;
            PB_Sprite.Image = pk.Sprite();

            L_Nickname.Text = pk.Nickname;
            L_Species.Text = strings.specieslist[pk.Species];
            L_Level.Text = pk.CurrentLevel.ToString();
            L_Nature.Text = strings.natures[(int)pk.Nature];
            L_Ability.Text = strings.abilitylist[pk.Ability];

            var items = strings.GetItemStrings(pk.Context);
            L_Item.Text = pk.HeldItem < items.Length ? items[pk.HeldItem] : "None";
        }
    }
}
