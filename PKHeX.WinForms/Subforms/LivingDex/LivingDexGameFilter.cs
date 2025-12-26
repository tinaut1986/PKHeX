using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PKHeX.Core;

namespace PKHeX.WinForms
{
    public partial class LivingDexGameFilter : Form
    {
        private readonly CheckedListBox CLB_Games;

        public HashSet<GameVersion> SelectedGames { get; private set; }

        public LivingDexGameFilter(HashSet<GameVersion> currentSelection)
        {
            InitializeComponent();
            Icon = Properties.Resources.Icon;
            Text = "Select Games";
            Size = new System.Drawing.Size(650, 450);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var P_Bottom = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            var B_OK = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new System.Drawing.Point(540, 12), Size = new System.Drawing.Size(80, 26) };
            var B_Cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new System.Drawing.Point(450, 12), Size = new System.Drawing.Size(80, 26) };
            var B_All = new Button { Text = "Select All", Location = new System.Drawing.Point(12, 12), Size = new System.Drawing.Size(80, 26) };
            var B_None = new Button { Text = "None", Location = new System.Drawing.Point(100, 12), Size = new System.Drawing.Size(80, 26) };

            B_All.Click += (s, e) => SetAll(true);
            B_None.Click += (s, e) => SetAll(false);

            P_Bottom.Controls.AddRange(new Control[] { B_OK, B_Cancel, B_All, B_None });
            Controls.Add(P_Bottom);

            CLB_Games = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                MultiColumn = true,
                ColumnWidth = 150,
                FormattingEnabled = true,
                DisplayMember = "Text",
                ValueMember = "Value"
            };

            var games = GameInfo.Strings.gamelist;
            for (int i = 0; i < games.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(games[i]) || games[i] == "---") continue;
                var version = (GameVersion)i;
                var item = new ComboItem(games[i], i);
                CLB_Games.Items.Add(item, currentSelection.Contains(version));
            }

            Controls.Add(CLB_Games);
            
            SelectedGames = currentSelection;
            FormClosing += (s, e) =>
            {
                if (DialogResult == DialogResult.OK)
                {
                    SelectedGames = CLB_Games.CheckedItems.Cast<ComboItem>().Select(z => (GameVersion)z.Value).ToHashSet();
                }
            };

            WinFormsUtil.TranslateInterface(this, Main.CurrentLanguage);
        }

        private void SetAll(bool state)
        {
            for (int i = 0; i < CLB_Games.Items.Count; i++)
                CLB_Games.SetItemChecked(i, state);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(634, 411);
            this.Name = "LivingDexGameFilter";
            this.ResumeLayout(false);
        }
    }
}
