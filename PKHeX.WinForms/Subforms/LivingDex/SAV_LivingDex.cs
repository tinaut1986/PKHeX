using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;
using PKHeX.WinForms.Controls;

namespace PKHeX.WinForms
{
    public partial class SAV_LivingDex : Form
    {
        private readonly SaveFile SAV;
        private readonly PKMEditor PKME;
        private readonly Dictionary<ushort, List<SlotCache>> SpeciesFound = new();
        private readonly Dictionary<(ushort, bool), Image> SpriteCache = new();
        private string CurrentPath = string.Empty;
        private bool IsPopulating = false;

        public SAV_LivingDex(PKMEditor pkme, SaveFile sav)
        {
            InitializeComponent();
            Icon = Properties.Resources.Icon;
            BackColor = Color.White;
            SAV = sav;
            PKME = pkme;
            SpriteUtil.Initialize(SAV);

            B_SelectFolder.Image = Properties.Resources.folder;
            B_SelectFolder.TextImageRelation = TextImageRelation.ImageBeforeText;
            B_SelectFolder.ImageAlign = ContentAlignment.MiddleLeft;
            
            // Enable DoubleBuffer for smoother scrolling
            var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(FLP_Pokedex, true);

            WinFormsUtil.TranslateInterface(this, Main.CurrentLanguage);
            Load += async (_, _) => await PopulatePokedex();
        }

        private async void B_SelectFolder_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            CurrentPath = fbd.SelectedPath;
            L_Folder.Text = CurrentPath;
            await ScanFolder(CurrentPath);
        }

        private async Task ScanFolder(string path)
        {
            SpeciesFound.Clear();
            FLP_Pokedex.Controls.Clear();
            
            var extensions = new HashSet<string>(EntityFileExtension.GetExtensionsAll().Select(z => $".{z}"));
            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .ToList();

            await Task.Run(() =>
            {
                foreach (var file in files)
                {
                    var data = File.ReadAllBytes(file);
                    var pk = EntityFormat.GetFromBytes(data, SAV.Context);
                    if (pk == null) continue;

                    var species = (ushort)pk.Species;
                    if (!SpeciesFound.TryGetValue(species, out var list))
                        SpeciesFound[species] = list = new List<SlotCache>();
                    
                    list.Add(new SlotCache(new SlotInfoFileSingle(file), pk));
                }
            });

            await PopulatePokedex();
        }

        private async Task PopulatePokedex()
        {
            if (IsPopulating) return;
            IsPopulating = true;

            FLP_Pokedex.SuspendLayout();
            FLP_Pokedex.Controls.Clear();
            
            int maxSpecies = (int)PKHeX.Core.Species.Pecharunt; 
            PB_Progress.Maximum = maxSpecies;
            PB_Progress.Value = 0;
            PB_Progress.Visible = true;
            L_Status.Text = "Populating LivingDex...";

            for (int i = 1; i <= maxSpecies; i++)
            {
                var species = (ushort)i;
                bool owned = SpeciesFound.ContainsKey(species);
                
                var pb = new PictureBox
                {
                    Width = 68,
                    Height = 56,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = GetCachedSprite(species, owned),
                    Cursor = owned ? Cursors.Hand : Cursors.Default,
                    Tag = species
                };

                if (owned)
                {
                    pb.Click += Species_Click;
                    toolTip.SetToolTip(pb, $"{((Species)species).ToString()} ({SpeciesFound[species].Count})");
                }
                else
                {
                    toolTip.SetToolTip(pb, ((Species)species).ToString());
                }

                FLP_Pokedex.Controls.Add(pb);

                if (i % 100 == 0)
                {
                    PB_Progress.Value = i;
                    L_Status.Text = $"Populating... {i}/{maxSpecies}";
                    await Task.Yield();
                }
            }

            PB_Progress.Visible = false;
            L_Status.Text = $"Scan complete. {SpeciesFound.Count} species found.";
            FLP_Pokedex.ResumeLayout();
            IsPopulating = false;
        }

        private Image GetCachedSprite(ushort species, bool owned)
        {
            if (SpriteCache.TryGetValue((species, owned), out var img))
                return img;

            var baseSprite = SpriteUtil.GetSprite(species, (byte)0, (byte)0, 0u, 0, false, Shiny.Never, EntityContext.None);
            if (!owned)
            {
                baseSprite = ImageUtil.ChangeAllColorTo(baseSprite, Color.Black);
            }

            SpriteCache[(species, owned)] = baseSprite;
            return baseSprite;
        }

        private void Species_Click(object sender, EventArgs e)
        {
            var pb = (PictureBox)sender;
            var species = (ushort)pb.Tag;

            if (SpeciesFound.TryGetValue(species, out var results))
            {
                var report = new SAV_LivingDexReport();
                report.Text = $"{((Species)species).ToString()} - {CurrentPath}";
                report.PopulateData(results);
                report.Show();
            }
        }
    }
}
