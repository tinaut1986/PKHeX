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
    public partial class LivingDex : Form
    {
        private readonly SaveFile SAV;
        private readonly PKMEditor PKME;
        private readonly Dictionary<ushort, List<SlotCache>> SpeciesFound = new();
        private readonly Dictionary<(ushort, bool), Image> SpriteCache = new();
        private HashSet<GameVersion> SelectedGames = new();
        private string CurrentPath = string.Empty;
        private bool IsPopulating = false;
        private bool LoadingFilters = true;

        public LivingDex(PKMEditor pkme, SaveFile sav)
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

            InitializeFilters();
        }

        private void InitializeFilters()
        {
            var games = GameInfo.Strings.gamelist;
            SelectedGames.Clear();
            for (int i = 0; i < games.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(games[i]) || games[i] == "---") continue;
                SelectedGames.Add((GameVersion)i);
            }
            UpdateSelectedGamesLabel();
            CB_LevelOp.SelectedIndex = 0;
            NUD_Level.Value = 100;
            LoadingFilters = false;
        }

        private void UpdateSelectedGamesLabel()
        {
            var total = GameInfo.Strings.gamelist.Count(z => !string.IsNullOrWhiteSpace(z) && z != "---");
            if (SelectedGames.Count == total)
                L_SelectedGamesCount.Text = "All selected";
            else if (SelectedGames.Count == 0)
                L_SelectedGamesCount.Text = "None selected";
            else
                L_SelectedGamesCount.Text = $"{SelectedGames.Count} selected";
        }

        private void B_SelectGames_Click(object? sender, EventArgs e)
        {
            using var filter = new LivingDexGameFilter(new HashSet<GameVersion>(SelectedGames));
            if (filter.ShowDialog() == DialogResult.OK)
            {
                SelectedGames = filter.SelectedGames;
                UpdateSelectedGamesLabel();
            }
        }

        private async void B_ApplyFilter_Click(object? sender, EventArgs e)
        {
            await PopulatePokedex();
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
                    var pk = EntityFormat.GetFromBytes(data, EntityContext.None); // Auto-detect format
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

            var filteredFound = GetFilteredResults();

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
                bool owned = filteredFound.ContainsKey(species);
                
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
                    pb.Tag = filteredFound[species]; // Store the filtered list of SlotCache for the report
                    toolTip.SetToolTip(pb, $"{((Species)species).ToString()} ({filteredFound[species].Count})");
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
            L_Status.Text = $"Scan complete. {filteredFound.Count} species found matching filters.";
            FLP_Pokedex.ResumeLayout();
            IsPopulating = false;
        }

        private Dictionary<ushort, List<SlotCache>> GetFilteredResults()
        {
            var filtered = new Dictionary<ushort, List<SlotCache>>();
            var op = CB_LevelOp.Text;
            var level = (int)NUD_Level.Value;

            foreach (var kvp in SpeciesFound)
            {
                var list = kvp.Value.Where(sc => 
                {
                    var pk = sc.Entity;
                    if (!SelectedGames.Contains(pk.Version)) return false;

                    if (op == "Any") return true;
                    if (op == "=" && pk.CurrentLevel != level) return false;
                    if (op == ">" && pk.CurrentLevel <= level) return false;
                    if (op == "<" && pk.CurrentLevel >= level) return false;
                    if (op == ">=" && pk.CurrentLevel < level) return false;
                    if (op == "<=" && pk.CurrentLevel > level) return false;

                    return true;
                }).ToList();

                if (list.Count > 0)
                    filtered[kvp.Key] = list;
            }
            return filtered;
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
            var results = (List<SlotCache>)pb.Tag;
            var species = (ushort)results[0].Entity.Species;

            var report = new LivingDexReport(PKME);
            report.Text = $"{((Species)species).ToString()} - {CurrentPath}";
            report.PopulateData(results);
            report.Show();
        }
    }
}
