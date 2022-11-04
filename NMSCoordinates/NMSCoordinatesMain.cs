
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using Octokit;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NMSCoordinates.SaveData;
using NMSCoordinates.LocationData;
using static NMSCoordinates.Coordinates;
//using libNOM.map;
using NMSSaveManager;

/**********************************************************\
|                                                          |
| NMSCoordinates 2019  -- NMSCoordinatesMain.cs            |
|                                                          | 
| A fast travel application for No Man's Sky               |
|                                                          |
| Developed by:                                            |
|   Code Author: Kevin Lozano / Kevin0M16                  |
|   Email: <kevin@k0m16.net>                               |
|   Website: https://kevin0m16.github.io/NMSCoordinates/   |
|                                                          |
\**********************************************************/


namespace NMSCoordinates
{
    public partial class NMSCoordinatesMain : Form
    {
        #region NMSC Variables

        private CoordinateCalculator f5;
        public SaveDirectorySelector f8;

        private List<string> _changedFiles = new List<string>();

        public string NMSCVersion;
        public string GamePath;
        public string nmsPath;
        public string nmscConfig;
        public string oldsavePath;
        public string hgFilePath;
        public string hgFileDir;
        public string currentKey;
        public string stmPath;
        public string ssdPath;
        public string ssPath;

        //private GameSave _gs;
        //private GameSaveManager _gsm;
        //private uint _gameSlot;

        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();
        //Dictionary<string, string> sjsonDict = new Dictionary<string, string>();

        private string Save;
        private string modSave;

        //private string rawSave;
        //private string ufSave;        
        //private string ufmodSave;

        public int SelectedSaveSlot;
        public string json;
        //public string ujson;
        public string locjson;
        public int locVersion;

        //public IDictionary<string, string> gameMode;
        public Dictionary<char, Bitmap> glyphDict = new Dictionary<char, Bitmap>();

        public Dictionary<int, string> sn1;
        public Dictionary<int, string> sn2;
        public Dictionary<int, string> sn3;
        public Dictionary<int, string> sn4;
        public Dictionary<int, string> sn5;
        public Dictionary<int, string> sn6;
        public Dictionary<int, string> sn7;
        public Dictionary<int, string> sn8;
        public Dictionary<int, string> sn9;
        public Dictionary<int, string> sn10;
        public Dictionary<int, string> sn11;
        public Dictionary<int, string> sn12;
        public Dictionary<int, string> sn13;
        public Dictionary<int, string> sn14;
        public Dictionary<int, string> sn15;

        public List<DirectoryInfo> SaveDirs = new List<DirectoryInfo>();

        //public List<string> DiscList { get; private set; }
        public List<string> SSlist = new List<string>();
        public List<string> PrevSSlist = new List<string>();

        public List<string> BaseList { get; private set; }

        private char _gl1;
        private char _gl2;
        private char _gl3;
        private char _gl4;
        private char _gl5;
        private char _gl6;
        private char _gl7;
        private char _gl8;
        private char _gl9;
        private char _gl10;
        private char _gl11;
        private char _gl12;

        public int _ScreenWidth { get; private set; }
        public int _ScreenHeight { get; private set; }
        #endregion

        public NMSCoordinatesMain()
        {
            InitializeComponent();
            
            //Set Version here
            NMSCVersion = "2.2"; //"v2.2";
            label29.Text = "Version " + NMSCVersion;
            
            glyphDict = Globals.Glyphs();

            //Default Paths
            nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");            
            nmscConfig = @".\nmsc\config.nmsc";
            oldsavePath = System.Windows.Forms.Application.CommonAppDataPath + "\\save.nmsc";

            Save = @".\json\save.json";
            modSave = @".\debug\saveedit.json";
            locVersion = 2;

            //rawSave = @".\debug\rawsave.json";
            //ufSave = @".\debug\ufsave.json";
            //ufmodSave = @".\debug\ufsaveedit.json";
        }

        // This method is called when the display settings change.
        private async void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);
            CheckRes();
        }      
        private void Form1_Load(object sender, EventArgs e)
        {
            //Trigger if Display resolution changes
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);

            //Check resolution
            CheckRes();

            //Make sure backup dir exists or create it
            CreateBackupDir();

            //Save preference file
            BuildSaveFile();
            ReloadSave();

            //loads the saved locbackup and playerloc files
            LoadTxt();

            //DiscList = new List<string>();
            BaseList = new List<string>();
        }
        private async void Form1_Shown(object sender, EventArgs e)
        {
            //Check to see if there is more than on dir in nmspath
            CheckGoG();

            //Set SS path
            SetGoGSShot();

            //load all the save files in cmbx for later
            LoadCmbx();

            //give time for form to show
            await Task.Delay(300);
            RunBackupAll(hgFileDir);

            //Check Github releases for a newer version
            CheckForUpdates();
        }
        private void CheckRes()
        {
            _ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            _ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            
            if (groupBox2.Visible == false && _ScreenHeight >= 768)
            {
                groupBox2.Visible = true;
                groupBox2.Location = new Point(266, 6);
                groupBox6.Location = new Point(266, 194);
                groupBox17.Location = new Point(266, 289);
                groupBox7.Location = new Point(266, 337);
            }
            if (_ScreenHeight > 768)
            {
                this.MinimumSize = new Size(750, 730);
                this.Size = new Size(750, 768);                
            }
            if (_ScreenHeight <= 768 && _ScreenHeight > 720)
            {
                this.MinimumSize = new Size(750, 730);
                this.Size = new Size(750, 735);
            }
            if (_ScreenHeight <= 720)
            {
                this.MinimumSize = new Size(750, 660);
                this.Size = new Size(750, 685);                

                groupBox2.Visible = false;
                groupBox6.Location = new Point(266, 6);
                groupBox17.Location = new Point(266, 105);
                groupBox7.Location = new Point(266, 157);
            }
        }
        private async void CheckForUpdates()
        {
            //Check Github releases for a newer version method
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("NMSCoordinates"));
                var releases = await client.Repository.Release.GetAll("Kevin0M16", "NMSCoordinates");
                var latest = releases[0];

                string latestversion = latest.Name.Replace("v", "");

                if (Version.Parse(NMSCVersion) < Version.Parse(latestversion)) //(NMSCVersion != latest.Name)
                {
                    //IsUpdated(false, first, latest.Name);

                    linkLabel4.Text = "Version " + latest.Name + " Available";
                    linkLabel4.Visible = true;
                    Globals.AppendLine(textBox17, "This Version: " + NMSCVersion + " Latest Version: " + latestversion); // latest.Name);
                    MessageBox.Show("A newer version of NMSCoordinates is available\r\n\nLatest Version: " + latest.Name + "  Now available for download.\r\n\n", "Update Available", MessageBoxButtons.OK);

                    /*
                    DialogResult dialogResult = MessageBox.Show("A newer version of NMSCoordinates is available\r\n\nLatest Version: " + latest.Name + "  Update Now?\r\n\n", "Update Available", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        UpdateApp();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }*/
                }

                if (Version.Parse(NMSCVersion) > Version.Parse(latestversion))
                {
                    Globals.AppendLine(textBox17, "This Version: " + NMSCVersion + " is a pre-release or experimental version, Version " + latestversion + " is the lastest release.");
                }

                if (NMSCVersion == latestversion) //latest.Name)
                {
                    //IsUpdated(true, first, latest.Name);

                    Globals.AppendLine(textBox17, "This Version: " + latest.Name + " is the latest version");
                }

                // Update with \nmsc\mapping.json file
                // Mapping.Settings = new MappingSettings { Download = @"nmsc\" };
                // Mapping.Update();
                // Mapping.UpdateAsync();
            }
            catch
            {
                Globals.AppendLine(textBox17, "Github Server not available. Could not check version");
            }
        }
        private void CheckGoG()
        {
            //Main Save.hg file and directory Finder
            if (Directory.Exists(nmsPath))
            {
                DirectoryInfo dinfo = new DirectoryInfo(nmsPath);

                if (dinfo.GetDirectories().Count() > 1 && dinfo.Name == "NMS")
                {
                    //If NMS dir is found and there is more than one dir
                    //Get all these dirs and send them to form8
                    foreach (DirectoryInfo dir in dinfo.GetDirectories())
                    {
                        if (dir.GetFiles("save*.hg", SearchOption.AllDirectories).Length > 0)
                            SaveDirs.Add(dir);
                    }
                    if (SaveDirs.Count() > 1)
                    {
                        f8 = new SaveDirectorySelector(SaveDirs);
                        f8.ShowDialog();

                        //After the path to nmsPath is selected on form8, set nmsPath
                        nmsPath = f8.GoGPath;

                        if (Directory.Exists(nmsPath))
                        {
                            WriteTxt("nmsPath", nmsPath, nmscConfig);
                            return;
                        }
                    }
                    else if (SaveDirs.Count() == 1)
                    {
                        WriteTxt("nmsPath", nmsPath, nmscConfig);
                        return;
                    }
                }
                else if (dinfo.GetFiles("save*.hg", SearchOption.AllDirectories).Length > 0)
                {
                    //If only one dir is found, do the following checks
                    if (dinfo.GetFiles("save*.hg", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check if hg files are here if so set nmsPath
                        nmsPath = dinfo.FullName;
                        WriteTxt("nmsPath", nmsPath, nmscConfig);
                        //Globals.AppendLine(textBox17, "Set Dir: " + nmsPath);
                        return;
                    }

                    if (dinfo.GetDirectories("st_*", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check for Steam Folder, if found set nmsPath
                        DirectoryInfo[] dirname1 = dinfo.GetDirectories("st_*", SearchOption.TopDirectoryOnly);
                        nmsPath = dirname1[0].FullName;
                        WriteTxt("nmsPath", nmsPath, nmscConfig);
                        //Globals.AppendLine(textBox17, "Set Dir: " + nmsPath);
                        return;
                    }

                    if (dinfo.GetDirectories("DefaultUser", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check for GoG Folder, if found set nmsPath
                        DirectoryInfo[] dirname2 = dinfo.GetDirectories("DefaultUser", SearchOption.TopDirectoryOnly);
                        nmsPath = dirname2[0].FullName;
                        WriteTxt("nmsPath", nmsPath, nmscConfig);
                        //Globals.AppendLine(textBox17, "Set Dir: " + nmsPath);
                        return;
                    }
                }
            }
        }
        public void LoadCmbx()
        {
            //Load save file names in combobox1 

            //If nmsPath is not found, show message and return
            if (!Directory.Exists(nmsPath))
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);
                return;
            }

            //Search for hg files in the current dir
            DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
            FileInfo[] Files = dinfo.GetFiles("save*.hg", SearchOption.TopDirectoryOnly);// SearchOption.AllDirectories);

            //if hg files are found, start adding them to dictionaries
            if (Files.Length != 0)
            {
                Dictionary<int, string> sl1 = new Dictionary<int, string>();
                sn1 = new Dictionary<int, string>();
                sn2 = new Dictionary<int, string>();
                sn3 = new Dictionary<int, string>();
                sn4 = new Dictionary<int, string>();
                sn5 = new Dictionary<int, string>();
                sn6 = new Dictionary<int, string>();
                sn7 = new Dictionary<int, string>();
                sn8 = new Dictionary<int, string>();
                sn9 = new Dictionary<int, string>();
                sn10 = new Dictionary<int, string>();
                sn11 = new Dictionary<int, string>();
                sn12 = new Dictionary<int, string>();
                sn13 = new Dictionary<int, string>();
                sn14 = new Dictionary<int, string>();
                sn15 = new Dictionary<int, string>();

                foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                {
                    if (file.Name == "save.hg" | file.Name == "save2.hg")
                    {
                        if (!sn1.ContainsKey(1))
                            sn1.Add(1, file.Name);
                        else sn1.Add(2, file.Name);

                        if (!sl1.ContainsValue("Slot 1"))
                            sl1.Add(1, "Slot 1");
                    }
                    if (file.Name == "save3.hg" | file.Name == "save4.hg")
                    {
                        if (!sn2.ContainsKey(3))
                            sn2.Add(3, file.Name);
                        else sn2.Add(4, file.Name);

                        if (!sl1.ContainsValue("Slot 2"))
                            sl1.Add(2, "Slot 2");
                    }
                    if (file.Name == "save5.hg" | file.Name == "save6.hg")
                    {
                        if (!sn3.ContainsKey(5))
                            sn3.Add(5, file.Name);
                        else sn3.Add(6, file.Name);

                        if (!sl1.ContainsValue("Slot 3"))
                            sl1.Add(3, "Slot 3");
                    }
                    if (file.Name == "save7.hg" | file.Name == "save8.hg")
                    {
                        if (!sn4.ContainsKey(7))
                            sn4.Add(7, file.Name);
                        else sn4.Add(8, file.Name);

                        if (!sl1.ContainsValue("Slot 4"))
                            sl1.Add(4, "Slot 4");
                    }
                    if (file.Name == "save9.hg" | file.Name == "save10.hg")
                    {
                        if (!sn5.ContainsKey(9))
                            sn5.Add(9, file.Name);
                        else sn5.Add(10, file.Name);

                        if (!sl1.ContainsValue("Slot 5"))
                            sl1.Add(5, "Slot 5");
                    }
                    if (file.Name == "save11.hg" | file.Name == "save12.hg")
                    {
                        if (!sn6.ContainsKey(11))
                            sn6.Add(11, file.Name);
                        else sn6.Add(12, file.Name);

                        if (!sl1.ContainsValue("Slot 6"))
                            sl1.Add(6, "Slot 6");
                    }
                    if (file.Name == "save13.hg" | file.Name == "save14.hg")
                    {
                        if (!sn7.ContainsKey(13))
                            sn7.Add(13, file.Name);
                        else sn7.Add(14, file.Name);

                        if (!sl1.ContainsValue("Slot 7"))
                            sl1.Add(7, "Slot 7");
                    }
                    if (file.Name == "save15.hg" | file.Name == "save16.hg")
                    {
                        if (!sn8.ContainsKey(15))
                            sn8.Add(15, file.Name);
                        else sn8.Add(16, file.Name);

                        if (!sl1.ContainsValue("Slot 8"))
                            sl1.Add(8, "Slot 8");
                    }
                    if (file.Name == "save17.hg" | file.Name == "save18.hg")
                    {
                        if (!sn9.ContainsKey(17))
                            sn9.Add(17, file.Name);
                        else sn9.Add(18, file.Name);

                        if (!sl1.ContainsValue("Slot 9"))
                            sl1.Add(9, "Slot 9");
                    }
                    if (file.Name == "save19.hg" | file.Name == "save20.hg")
                    {
                        if (!sn10.ContainsKey(19))
                            sn10.Add(19, file.Name);
                        else sn10.Add(20, file.Name);

                        if (!sl1.ContainsValue("Slot 10"))
                            sl1.Add(10, "Slot 10");
                    }
                    if (file.Name == "save21.hg" | file.Name == "save22.hg")
                    {
                        if (!sn11.ContainsKey(21))
                            sn11.Add(21, file.Name);
                        else sn11.Add(22, file.Name);

                        if (!sl1.ContainsValue("Slot 11"))
                            sl1.Add(11, "Slot 11");
                    }
                    if (file.Name == "save23.hg" | file.Name == "save24.hg")
                    {
                        if (!sn12.ContainsKey(23))
                            sn12.Add(23, file.Name);
                        else sn12.Add(24, file.Name);

                        if (!sl1.ContainsValue("Slot 12"))
                            sl1.Add(12, "Slot 12");
                    }
                    if (file.Name == "save25.hg" | file.Name == "save26.hg")
                    {
                        if (!sn13.ContainsKey(25))
                            sn13.Add(25, file.Name);
                        else sn13.Add(26, file.Name);

                        if (!sl1.ContainsValue("Slot 13"))
                            sl1.Add(13, "Slot 13");
                    }
                    if (file.Name == "save27.hg" | file.Name == "save28.hg")
                    {
                        if (!sn14.ContainsKey(27))
                            sn14.Add(27, file.Name);
                        else sn14.Add(28, file.Name);

                        if (!sl1.ContainsValue("Slot 14"))
                            sl1.Add(14, "Slot 14");
                    }
                    if (file.Name == "save29.hg" | file.Name == "save30.hg")
                    {
                        if (!sn15.ContainsKey(29))
                            sn15.Add(29, file.Name);
                        else sn15.Add(30, file.Name);

                        if (!sl1.ContainsValue("Slot 15"))
                            sl1.Add(15, "Slot 15");
                    }
                }

                sl1.Add(0, "(Select Save Slot)");
                comboBox2.DataSource = sl1.ToArray();
                comboBox2.DisplayMember = "VALUE";
                comboBox2.ValueMember = "KEY";

                hgFileDir = Path.GetDirectoryName(Files[0].FullName);
                fileSystemWatcher1.Path = hgFileDir;

                textBox16.Clear();
                Globals.AppendLine(textBox16, hgFileDir);
            }
            else
            {
                Globals.AppendLine(textBox16, "No save files found!");
                return;
            }
        }
        private void ComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selected = this.comboBox2.GetItemText(this.comboBox2.SelectedItem);

            //Gets the dictionaries set in loadcmbbx and sets the data source for save dropdown
            if (selected == "Slot 1")
            {
                SelectedSaveSlot = 1;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn1.ToArray();
                return;
            }
            if (selected == "Slot 2")
            {
                SelectedSaveSlot = 2;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn2.ToArray();
                return;
            }
            if (selected == "Slot 3")
            {
                SelectedSaveSlot = 3;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn3.ToArray();
                return;
            }
            if (selected == "Slot 4")
            {
                SelectedSaveSlot = 4;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn4.ToArray();
                return;
            }
            if (selected == "Slot 5")
            {
                SelectedSaveSlot = 5;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn5.ToArray();
                return;
            }
            if (selected == "Slot 6")
            {
                SelectedSaveSlot = 6;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn6.ToArray();
                return;
            }
            if (selected == "Slot 7")
            {
                SelectedSaveSlot = 7;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn7.ToArray();
                return;
            }
            if (selected == "Slot 8")
            {
                SelectedSaveSlot = 8;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn8.ToArray();
                return;
            }
            if (selected == "Slot 9")
            {
                SelectedSaveSlot = 9;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn9.ToArray();
                return;
            }
            if (selected == "Slot 10")
            {
                SelectedSaveSlot = 10;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn10.ToArray();
                return;
            }
            if (selected == "Slot 11")
            {
                SelectedSaveSlot = 11;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn11.ToArray();
                return;
            }
            if (selected == "Slot 12")
            {
                SelectedSaveSlot = 12;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn12.ToArray();
                return;
            }
            if (selected == "Slot 13")
            {
                SelectedSaveSlot = 13;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn13.ToArray();
                return;
            }
            if (selected == "Slot 14")
            {
                SelectedSaveSlot = 14;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn14.ToArray();
                return;
            }
            if (selected == "Slot 15")
            {
                SelectedSaveSlot = 15;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn15.ToArray();
                return;
            }
            if (selected == "(Select Save Slot)")
            {
                SelectedSaveSlot = -1;
                comboBox1.DataSource = null;
                ClearAll();
                LoadCmbx(); //insert here?
                return;
            }
        }      
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //After selecting a SelectedSaveSlot, this triggers + after selecting a different save   
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "")
            {
                ClearAll();
                GetSaveFile(selected);
                LoadTeleportEndpoints();
                //LoadPersistentPlayerBases();
                GetPlayerCoord();
            }            
        }
        /*
        private void GetRawSave(string savefile, string destfile)
        {
            fileSystemWatcher1.EnableRaisingEvents = false;
            SaveCompression.DecompressSave(savefile, destfile);
            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        */
        /*
        private void GetUnformattedSave(out string ufjson, string inputfilepath, string outputfilepath, bool indented)
        {
            //Sets json from the selected save file
            string rawjson = File.ReadAllText(inputfilepath);
            if (indented)
            {
                ufjson = JsonConvert.SerializeObject(JObject.Parse(rawjson), Formatting.Indented).TrimEnd('\0');
            }
            else
            {
                ufjson = JsonConvert.SerializeObject(JObject.Parse(rawjson), Formatting.None).TrimEnd('\0');
            }            
            File.WriteAllText(outputfilepath, ufjson);
        }
        private void CreateNewSave(out string newjson, string inputfilepath, string outputfilepath, bool reverse)
        {
            string injson = File.ReadAllText(inputfilepath);
            if (reverse)
            {
                // Sets json from input and reverses all key names back to original                
                newjson = injson;
                JObject jObject = JsonConvert.DeserializeObject(injson) as JObject;
                Mapping.Obfuscate(jObject);
                injson = jObject.ToString();
            }
            else
            {
                // Sets json after modifying original values to key names                
                JObject jObject = JsonConvert.DeserializeObject(injson) as JObject;
                Mapping.Deobfuscate(jObject);
                injson = jObject.ToString();
                newjson = injson;
            } 
            File.WriteAllText(outputfilepath, injson);
        }
        */
        /*
        private void GetJsonDict(string keyfilepath, out Dictionary<string,string> outDict)
        {
            //Sets json_map dictionary from the specified file
            string[] j_map = File.ReadAllLines(keyfilepath);
            Dictionary<string, string> inDict = new Dictionary<string, string>();

            foreach (string keyline in j_map)
            {
                string[] values = keyline.Split('\t');
                string key = values[1];
                string value = values[0];
                inDict.Add("\"" + key + "\"", "\"" + value + "\"");
            }

            outDict = inDict;
        }
        */
        private void GetSaveFile(string selected)
        {
            //Main save file loader
            if (Directory.Exists(hgFileDir) && selected != "")
            {
                progressBar2.Visible = true;
                progressBar2.Invoke((System.Action)(() => progressBar2.Value = 20));

                DirectoryInfo dinfo = new DirectoryInfo(hgFileDir);
                FileInfo[] Files = dinfo.GetFiles(selected, SearchOption.TopDirectoryOnly); //AllDirectories);

                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        //sets the file path to work with
                        hgFilePath = file.FullName;
                    }
                }
                else
                {
                    Globals.AppendLine(textBox17, "** Code 3 ** " + selected);
                    return;
                }
                progressBar2.Invoke((System.Action)(() => progressBar2.Value = 40));

                // shows the file path in the path textbox
                textBox16.Clear();
                Globals.AppendLine(textBox16, hgFilePath);

                // displays the last write time
                FileInfo hgfile = new FileInfo(hgFilePath);
                textBox26.Clear();
                Globals.AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                json = GameSave.DecryptSave(hgFilePath, Save).ToString();

                progressBar2.Invoke((System.Action)(() => progressBar2.Value = 60));
                /*
                // Read save file and get rawSave
                //GetRawSave(hgFilePath, rawSave);
                SaveCompression.DecompressSave(hgFilePath, rawSave);
                progressBar2.Invoke((System.Action)(() => progressBar2.Value = 40));

                // Read rawSave and get ujson
                GetUnformattedSave(out string ujson, rawSave, ufSave, true);
                progressBar2.Invoke((System.Action)(() => progressBar2.Value = 60));

                // Read ufSave and get Save and json
                CreateNewSave(out json, ufSave, Save, false);
                progressBar2.Invoke((System.Action)(() => progressBar2.Value = 80));
                */
                try
                {
                    // looks up and then displays the game mode
                    var nms = GameSaveData.FromJson(json);

                    if (nms.PlayerStateData.DifficultyState != null)
                    {
                        label28.Text = nms.PlayerStateData.DifficultyState.Preset.DifficultyPresetType;
                        label31.Text = nms.PlayerStateData.SaveName;
                        label41.Text = nms.PlayerStateData.SaveSummary;
                    }
                    else
                    {
                        int gamemodeint = Convert.ToInt32(nms.Version);
                        label28.Text = Globals.GameModeLookupInt(gamemodeint);
                    }
                }
                catch
                {
                    Globals.AppendLine(textBox17, "Problem loading json! ERROR [6]" + selected);
                    return;
                }
                progressBar2.Invoke((System.Action)(() => progressBar2.Value = 100));
                progressBar2.Visible = false;
            }
        }
        private async Task BackupLoc(string path)
        {
            //Backup all locations to a new locbackup file
            GameSaveData nms = GameSaveData.FromJson(json);
            int teleportArrayLength = nms.PlayerStateData.TeleportEndpoints.Length;

            if (teleportArrayLength > 0)
            {
                tabControl1.SelectedTab = tabPage1;
                await Task.Delay(300);
                // Set Minimum to 1 to represent the first file being copied.
                progressBar2.Minimum = 1;
                // Set Maximum to the total number of files to copy.
                progressBar2.Maximum = teleportArrayLength;
                // Set the initial value of the ProgressBar.
                progressBar2.Value = 1;
                // Set the Step property to a value of 1 to represent each file being copied.
                progressBar2.Step = 1;
                // Display the ProgressBar control.
                progressBar2.Visible = true;

                SavedLocationData sdata = Globals.CreateNewLocationJson(locVersion, teleportArrayLength, 0);
                List<LocationArray> basis = new List<LocationArray>();

                for (int i = 0; i < teleportArrayLength; i++)
                {
                    Destination dest = TeleportEndpoints(i, json);

                    basis.Add(new LocationArray()
                    {
                        Name = GetTeleportEndPointName(i, nms),
                        Details = new Details()
                        {
                            DateTime = DateTime.Now.ToString("MM-dd-yyyy HH:mm"),
                            SaveSlot = SelectedSaveSlot,
                            LongHex = dest.LongHex,
                            Galaxy = dest.iGalaxy,
                            PortalCode = dest.PortalCode,
                            GalacticCoords = dest.GalacticCoordinate,
                            Notes = ""
                        }
                    });

                    progressBar2.PerformStep();
                }

                progressBar2.Visible = false;
                progressBar2.Maximum = 100;

                sdata.Locations.TeleportEndpoints = basis.ToArray();

                //Make a unique path name for the locbackup file and create file
                string path2 = Globals.MakeUniqueLoc(path, SelectedSaveSlot);
                var backuplist = LocationData.Serialize.ToJson(sdata);
                File.WriteAllText(path2, backuplist);

                MessageBox.Show("ALL Locations Backed up \n\n\r Open in Coordinate Share Tab", "Confirmation", MessageBoxButtons.OK);
                LoadTxt();

                tabControl1.SelectedTab = tabPage3;
                if (File.Exists(path2))
                {
                    listBox4.SelectedItem = Path.GetFileName(path2);
                }
            }
            else
            {
                exportLocationRecordToolStripMenuItem.Enabled = false;
                MessageBox.Show("No Locations found! ", "Message");
            }
        }
        private void GetPlayerCoord()
        {
            //Gets the player position off the save file and prints the info on tab1
            textBox22.Clear();
            textBox21.Clear();
            textBox23.Clear();
            textBox29.Clear();

            Player player = PlayerLocation(json);

            //var nms = GameSaveData.FromJson(json);
            //pgalaxy = nms.PlayerStateData.UniverseAddress.RealityIndex.ToString();//.ToString();
            //var pX = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelX;//.ToString();
            //var pY = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelY;//.ToString();
            //var pZ = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelZ;//.ToString();
            //var pSSI = nms.PlayerStateData.UniverseAddress.GalacticAddress.SolarSystemIndex;//.ToString();
                        
            //GalacticCoord = CoordCalculations.VoxelToGalacticCoord(Convert.ToInt32(pX), Convert.ToInt32(pY), Convert.ToInt32(pZ), Convert.ToInt32(pSSI), textBox3);
            Globals.AppendLine(textBox22, player.GalacticCoordinate);
            //PortalCode = CoordCalculations.VoxelToPortal(Convert.ToInt32(pX), Convert.ToInt32(pY), Convert.ToInt32(pZ), Convert.ToInt32(pSSI));
            ShowPGlyphs(player.PortalCode);
            Globals.AppendLine(textBox21, player.PortalCode);
            textBox23.Text = player.GalaxyName;
            Globals.AppendLine(textBox29, player.DistanceToCenter);
        }
        private void Clearforsearch()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox30.Clear();

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;
            pictureBox11.Image = null;
            pictureBox12.Image = null;
        }
        private void ClearAll()
        {
            label28.ResetText();

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox12.Clear();
            textBox14.Clear();
            textBox19.Clear();
            textBox20.Clear();
            textBox22.Clear();
            textBox21.Clear();
            textBox23.Clear();
            textBox26.Clear();
            textBox29.Clear();
            textBox30.Clear();

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;
            pictureBox11.Image = null;
            pictureBox12.Image = null;
            pictureBox13.Image = null;
            pictureBox14.Image = null;
            pictureBox15.Image = null;
            pictureBox16.Image = null;
            pictureBox17.Image = null;
            pictureBox18.Image = null;
            pictureBox19.Image = null;
            pictureBox20.Image = null;
            pictureBox21.Image = null;
            pictureBox22.Image = null;
            pictureBox23.Image = null;
            pictureBox24.Image = null;

            //DiscList.Clear();
            BaseList.Clear();
            //Backuplist.Clear();
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            SaveDirs.Clear();

            //json = ""; Insert here 1.1.8??

            comboBox3.Items.Clear();
            
            label35.Text = "Galaxy Name";
            label36.Text = "TYPE";

            //pgalaxy = "";
            //galaxy = "";
            //X = "";
            //Y = "";
            //Z = "";
            //SSI = "";
            //PI = "";

            //PortalCode = "";
            //GalacticCoord = "";
            //GalacticCoord2 = "";

            textBox16.Text = nmsPath;
        }
        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();

            //textBox4.Text = galaxy;
            //textBox5.Text = X;
            //textBox6.Text = Y;
            //textBox7.Text = Z;
            //textBox8.Text = SSI;
            //textBox9.Text = PI;
        }
        /*
        private void JsonMapPersistentBases(int i)
        {
            //lookup info from the Json hg file
            try
            {
                var nms = GameSaveData.FromJson(json);
                string ga = nms.PlayerStateData.PersistentPlayerBases[i].GalacticAddress;
                CoordCalculations.CalculateLongHex(ga, out string galacticCoord2, out iPI, out igalaxy);

                string[] value = galacticCoord2.Replace(" ", "").Split(':');
                string A = value[0].Trim();
                string B = value[1].Trim();
                string C = value[2].Trim();
                string D = value[3].Trim();

                CoordCalculations.GalacticToVoxel(A, B, C, D, out iX, out iY, out iZ, out iSSI);

                galaxy = igalaxy.ToString();
                X = iX.ToString();
                Y = iY.ToString();
                Z = iZ.ToString();
                SSI = iSSI.ToString();
                PI = iPI.ToString();
            }
            catch
            {
                Globals.AppendLine(textBox17, "** Code 22 **");
                return;
            }
        }
        */
        private static string GetTeleportEndPointName(int i, GameSaveData nms)
        {
            List<string> nameList = new List<string>();
            string discd = nms.PlayerStateData.TeleportEndpoints[i].Name;

            if (string.IsNullOrEmpty(nms.PlayerStateData.TeleportEndpoints[i].Name))
            {
                string bl = discd + "None";
                nameList.Add(bl);
            }
            if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "Spacestation")
            {
                string ss = discd + " (SS)";

                if (!nameList.Contains(ss))
                    nameList.Add(ss);
            }
            if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "Base")
            {
                string bl = discd + " (B)";

                if (!nameList.Contains(bl))
                    nameList.Add(bl);
            }
            if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "ExternalBase")
            {
                string bl = discd + " (EB)";

                if (!nameList.Contains(bl))
                    nameList.Add(bl);
            }
            if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "SpacestationFixPosition" || nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "SpacestationFixwMC")
            {
                string ss = discd + " (SS)";

                if (!nameList.Contains(ss))
                    nameList.Add(ss);
            }            
            if (nameList.Count == 1)
            {
                return nameList[0];
            }
            else
            {
                return "";
            }            
        }
        private void LoadTeleportEndpoints()
        {
            //Method to load all location discovered in listbox1
            //DiscList.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            ClearTextBoxes();

            var nms = GameSaveData.FromJson(json);
            try
            {
                for (int i = 0; i < nms.PlayerStateData.TeleportEndpoints.Length; i++)
                {
                    string name = GetTeleportEndPointName(i, nms);
                    
                    if (name.Contains("(SS)"))
                        listBox2.Items.Add(name);

                    if (name.Contains("(B)") || name.Contains("(EB)"))
                        listBox1.Items.Add(name);
                }

                /*
                for (int i = 0; i < nms.PlayerStateData.TeleportEndpoints.Length; i++)
                {
                    string discd = nms.PlayerStateData.TeleportEndpoints[i].Name;

                    if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "Spacestation")
                    {
                        string ss = discd + " (SS)";
                        DiscList.Add(ss);
                        listBox2.Items.Add(ss);
                    }
                    if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "Base") // v1.1.16
                    {
                        string bl = discd + " (B)";
                        DiscList.Add(bl);
                        listBox1.Items.Add(bl);
                    }
                    if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "ExternalBase") // v1.1.16
                    {
                        string bl = discd + " (EB)";
                        DiscList.Add(bl);
                        listBox1.Items.Add(bl);
                    }
                }
                for (int i = 0; i < nms.PlayerStateData.TeleportEndpoints.Length; i++)
                {
                    string discd = nms.PlayerStateData.TeleportEndpoints[i].Name;

                    if (nms.PlayerStateData.TeleportEndpoints[i].TeleporterType == "SpacestationFixPosition") // 1.1.16
                    {
                        string ss = discd + " (SS)";
                        if (!DiscList.Contains(ss))
                        {
                            DiscList.Add(ss);
                            listBox2.Items.Add(ss);
                        }                       
                    }
                }*/

            }
            catch
            {
                Globals.AppendLine(textBox17, "** Code 111 **");
                return;
            }
            
            //listBox1.DataSource = DiscList; //Removed v1.1.11
            textBox19.Text = listBox1.Items.Count.ToString();
            textBox20.Text = listBox2.Items.Count.ToString();
            listBox1.SelectedIndex = -1;

            if (nms.PlayerStateData.OnOtherSideOfPortal == true)
            {
                textBox12.Text = "True";
            }
            else
            {
                textBox12.Text = "False";
            }
        }
        private void ListBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListBox1_MouseClick(this, new EventArgs());
            }
        }
        private void ListBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListBox2_MouseClick(this, new EventArgs());
            }
        }
        /*
        private void LoadPersistentPlayerBases()
        {
            //Future use to add Persistent Bases to a Listbox
            var nms = GameSaveData.FromJson(json);
            try
            {
                for (int i = 0; i < nms.PlayerStateData.PersistentPlayerBases.Length; i++)
                {
                    string baseN = nms.PlayerStateData.PersistentPlayerBases[i].Name;
                    if (baseN != "")
                    {
                        string pb = baseN + " (PB)";
                        BaseList.Add(pb);
                        listBox2.Items.Add(pb);
                    }
                    else
                    {
                        string pb = "Not Named (PB)";
                        BaseList.Add(pb);
                        listBox2.Items.Add(pb);
                    }
                }
                textBox20.Text = listBox2.Items.Count.ToString();
            }
            catch
            {
                Globals.AppendLine(textBox17, "** Code 11 **");
                return;
            }
        }
        */
        private void ListBox1_MouseClick(object sender, EventArgs e)
        {
            //When a location is clicked on listbox1, get all the info
            listBox2.SelectedIndex = -1;
            try
            {
                string si = listBox1.GetItemText(listBox1.SelectedItem);

                if (!string.IsNullOrEmpty(si))
                {
                    //object selecteditem = listBox1.SelectedItem;
                    //string si = selecteditem.ToString();
                    si = si.Replace(" (B)", "");
                    si = si.Replace(" (EB)", ""); //v1.1.16
                    var nms = GameSaveData.FromJson(json);
                    try
                    {
                        for (int i = 0; i < nms.PlayerStateData.TeleportEndpoints.Length; i++)
                        {
                            if (nms.PlayerStateData.TeleportEndpoints[i].Name.ToString() == si)
                            {
                                //JsonMapTeleportEndpoints(i);
                                Destination dest = TeleportEndpoints(i, json, textBox3);

                                ClearTextBoxes();

                                textBox4.Text = dest.Galaxy;
                                textBox5.Text = dest.X;
                                textBox6.Text = dest.Y;
                                textBox7.Text = dest.Z;
                                textBox8.Text = dest.SSI;
                                textBox9.Text = dest.PI;
                                textBox10.Text = dest.GalaxyName;
                                textBox30.Text = dest.DistanceToCenter;

                                ShowGlyphs(dest.PortalCode);

                                Globals.AppendLine(textBox1, dest.GalacticCoordinate);
                                Globals.AppendLine(textBox2, dest.PortalCode);
                            }
                        }
                    }
                    catch
                    {
                        Globals.AppendLine(textBox17, "** Code 51l1 **");
                        return;
                    }
                }
            }
            catch
            {
                Globals.AppendLine(textBox17, "** Code 5l1 **");
                return;
            }
        }
        private void ListBox2_MouseClick(object sender, EventArgs e)
        {
            //When a location is clicked on listbox2, get all the info
            listBox1.SelectedIndex = -1;
            try
            {
                string si = listBox2.GetItemText(listBox2.SelectedItem);

                if (!string.IsNullOrEmpty(si))
                {
                    //object selecteditem = listBox2.SelectedItem;
                    //string si = selecteditem.ToString();
                    si = si.Replace(" (SS)", "");
                    var nms = GameSaveData.FromJson(json);
                    try
                    {
                        for (int i = 0; i < nms.PlayerStateData.TeleportEndpoints.Length; i++)
                        {
                            if (nms.PlayerStateData.TeleportEndpoints[i].Name.ToString() == si)
                            {
                                //JsonMapTeleportEndpoints(i);
                                Destination dest = TeleportEndpoints(i, json, textBox3);

                                ClearTextBoxes();

                                textBox4.Text = dest.Galaxy;
                                textBox5.Text = dest.X;
                                textBox6.Text = dest.Y;
                                textBox7.Text = dest.Z;
                                textBox8.Text = dest.SSI;
                                textBox9.Text = dest.PI;
                                textBox10.Text = dest.GalaxyName;
                                textBox30.Text = dest.DistanceToCenter;

                                ShowGlyphs(dest.PortalCode);

                                Globals.AppendLine(textBox1, dest.GalacticCoordinate);
                                Globals.AppendLine(textBox2, dest.PortalCode);
                            }
                        }
                    }
                    catch
                    {
                        Globals.AppendLine(textBox17, "** Code 51l2 **");
                        return;
                    }
                }
            }
            catch
            {
                Globals.AppendLine(textBox17, "** Code 5l2 **");
                return;
            }
            /*
            listBox1.SelectedIndex = -1;
            try
            {
                string si = listBox2.GetItemText(listBox2.SelectedItem);
                if (!string.IsNullOrEmpty(si))
                {
                    //object selecteditem = listBox2.SelectedItem;
                    //string si = selecteditem.ToString();                    
                    //si = si.Replace(" (SS)", "");
                    si = si.Replace("Not Named (PB)", "");
                    si = si.Replace(" (PB)", "");
                    var nms = GameSaveData.FromJson(json);
                    try
                    {
                        for (int i = 0; i < nms.PlayerStateData.PersistentPlayerBases.Length; i++)
                        {
                            if (nms.PlayerStateData.PersistentPlayerBases[i].Name.ToString() == si)
                            {
                                Destination dest = PersistentBases(i, json);

                                ClearTextBoxes();

                                textBox4.Text = dest.Galaxy;
                                textBox5.Text = dest.X;
                                textBox6.Text = dest.Y;
                                textBox7.Text = dest.Z;
                                textBox8.Text = dest.SSI;
                                textBox9.Text = dest.PI;
                                textBox10.Text = dest.GalaxyName;
                                textBox30.Text = dest.DistanceToCenter;

                                ShowGlyphs(dest.PortalCode);
                                Globals.AppendLine(textBox1, dest.GalacticCoordinate);
                                Globals.AppendLine(textBox2, dest.PortalCode);
                            }
                        }
                    }
                    catch
                    {
                        Globals.AppendLine(textBox17, "** Code 51l2 **");
                        return;
                    }
                }
            }
            catch
            {
                Globals.AppendLine(textBox17, "** Code 5l2 **");
                return;
            }
            */
        }
        private void ShowPGlyphs(string pc)
        {
            //Index chars in PortalCode
            _gl1 = pc[0];
            _gl2 = pc[1];
            _gl3 = pc[2];
            _gl4 = pc[3];
            _gl5 = pc[4];
            _gl6 = pc[5];
            _gl7 = pc[6];
            _gl8 = pc[7];
            _gl9 = pc[8];
            _gl10 = pc[9];
            _gl11 = pc[10];
            _gl12 = pc[11];

            //Display player glyph images
            pictureBox13.Image = glyphDict[_gl1];
            pictureBox13.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox14.Image = glyphDict[_gl2];
            pictureBox14.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.Image = glyphDict[_gl3];
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox16.Image = glyphDict[_gl4];
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox17.Image = glyphDict[_gl5];
            pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox18.Image = glyphDict[_gl6];
            pictureBox18.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox19.Image = glyphDict[_gl7];
            pictureBox19.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox20.Image = glyphDict[_gl8];
            pictureBox20.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox21.Image = glyphDict[_gl9];
            pictureBox21.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox22.Image = glyphDict[_gl10];
            pictureBox22.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox23.Image = glyphDict[_gl11];
            pictureBox23.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox24.Image = glyphDict[_gl12];
            pictureBox24.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void ShowGlyphs(string pc)
        {
            //Index chars in PortalCode
            _gl1 = pc[0];
            _gl2 = pc[1];
            _gl3 = pc[2];
            _gl4 = pc[3];
            _gl5 = pc[4];
            _gl6 = pc[5];
            _gl7 = pc[6];
            _gl8 = pc[7];
            _gl9 = pc[8];
            _gl10 = pc[9];
            _gl11 = pc[10];
            _gl12 = pc[11];

            //Display selected location glyph images
            pictureBox1.Image = glyphDict[_gl1];
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = glyphDict[_gl2];
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.Image = glyphDict[_gl3];
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = glyphDict[_gl4];
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.Image = glyphDict[_gl5];
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Image = glyphDict[_gl6];
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Image = glyphDict[_gl7];
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.Image = glyphDict[_gl8];
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox9.Image = glyphDict[_gl9];
            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox10.Image = glyphDict[_gl10];
            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox11.Image = glyphDict[_gl11];
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox12.Image = glyphDict[_gl12];
            pictureBox12.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            //Reload save button
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "")
            {
                ClearAll();
                Globals.AppendLine(textBox17, "Loading Save File...");
                GetSaveFile(selected);
                LoadTeleportEndpoints();
                //LoadPersistentPlayerBases();
                GetPlayerCoord();
                LoadTxt();
                Globals.AppendLine(textBox17, "Save File Reloaded.");
            }
            else
            {
                MessageBox.Show("No Save Slot Selected!", "Alert");
            }
        }
        private void SetSavePath()
        {
            //Manually set save path method
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        //Checks for hg files in the selected dir
                        string[] files = Directory.GetFiles(fbd.SelectedPath, "save*.hg");

                        if (files.Length != 0)
                        {
                            nmsPath = fbd.SelectedPath;
                            CheckGoG();
                            //Globals.AppendLine(textBox16, fbd.SelectedPath + "save.hg");
                            MessageBox.Show("Path to save files set.", "Confirmation");
                        }
                        else
                        {
                            MessageBox.Show("No Save files found! ", "Message");
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Cancelled no path set!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n Save path problem!");
                return;
            }
        }
        private void CreateBackupDir()
        {
            //Checks for the required directories and creates if don't exist
            if (!Directory.Exists(@".\nmsc"))
            {
                Directory.CreateDirectory(@".\nmsc");
            }

            if (!Directory.Exists(@".\backup"))
            {
                Directory.CreateDirectory(@".\backup");
                Directory.CreateDirectory(@".\backup\saves");
                Directory.CreateDirectory(@".\backup\locations");
            }

            if (!Directory.Exists(@".\backup\saves"))
            {
                Directory.CreateDirectory(@".\backup\saves");
            }

            if (!Directory.Exists(@".\backup\locations"))
            {
                Directory.CreateDirectory(@".\backup\locations");
            }

            if (!Directory.Exists(@".\debug"))
            {
                Directory.CreateDirectory(@".\debug");
            }

            if (!Directory.Exists(@".\json"))
            {
                Directory.CreateDirectory(@".\json");
            }
        }
        public void BuildSaveFile()
        {
            //if the old save.txt exists delete it
            if (File.Exists(oldsavePath))
            {
                File.Delete(oldsavePath);
            }
            
            //if save.nmsc doesn't exist, create it
            if (!File.Exists(nmscConfig))
            {
                File.Create(nmscConfig).Close();
                TextWriter tw = new StreamWriter(nmscConfig);
                tw.WriteLine("nmsPath=" + nmsPath);
                tw.WriteLine("ssdPath=" + ssdPath);
                tw.Close();
            }
            else if (File.Exists(nmscConfig))
            {
                return;
            }
        }
        private void Read(string key, string path)
        {
            //Read the save.nmsc file and get the value from key
            try
            {
                if (File.Exists(path))
                {

                    string[] array = File.ReadAllLines(path).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        string a = array[i].Substring(0, array[i].IndexOf("="));
                        string s = array[i].Substring(array[i].IndexOf("=") + 1);

                        if (a == key)
                        {
                            currentKey = s;
                        }

                    }
                    return;
                }
            }
            catch
            {
                //Globals.AppendLine(textBox1, "ini File Corrupted! See Log!");
                return;
            }
        }
        public void ReloadSave()
        {            
            Read("nmsPath", nmscConfig);
            nmsPath = currentKey;

            //If save.nmsc gets corrupt, back to defaults on nmsPath
            if (Directory.Exists(nmsPath))
            {
                DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
                if (dinfo.GetFiles("save*.hg", SearchOption.TopDirectoryOnly).Length <= 0)
                {
                    nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
                }
            }
            else if(!Directory.Exists(nmsPath))
            {
                nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            }               
            
            Read("ssdPath", nmscConfig);
            ssdPath = currentKey;
        }
        public void WriteTxt(string key, string newKey, string path)
        {
            //Write to save.nmsc method
            try
            {
                if (File.Exists(path))
                {
                    string[] array = File.ReadAllLines(path).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();               

                    for (int i = 0; i < array.Length; i++)
                    {
                        string a = array[i].Substring(0, array[i].IndexOf("="));
                        string s = array[i].Substring(array[i].IndexOf("=") + 1);
                        if (a == key)
                        {
                            string text = File.ReadAllText(path);
                            text = text.Replace(key + "=" + s, key + "=" + newKey);
                            File.WriteAllText(path, text);
                        }

                    }
                    return;
                }
                else
                {
                    //Globals.AppendLine(textBox1, "Error File not found!");
                    return;
                }
            }
            catch
            {
                //Globals.AppendLine(textBox1, "ini File Corrupted! See Log!");
                return;
            }
        }
        private void AppDataDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set back to default AppData\HelloGames\NMS
            //Check if Travel Mode ON
            if (checkBox1.Checked == false)
            {
                nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
                CheckGoG();

                SetGoGSShot();

                comboBox1.DataSource = null;
                comboBox2.DataSource = null;
                comboBox1.SelectedIndex = -1;
                ClearAll();
                LoadCmbx();
                tabControl1.SelectedTab = tabPage1;
            }
            else
            {
                MessageBox.Show("Turn off Travel Mode!", "Alert");
            }

        }
        private void ManuallySelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manually select a save hg file path
            //Check if Travel Mode is ON
            if (checkBox1.Checked == false)
            {
                SetSavePath();

                SetGoGSShot();

                comboBox1.DataSource = null;
                comboBox2.DataSource = null;
                comboBox1.SelectedIndex = -1;
                ClearAll();
                LoadCmbx();
                tabControl1.SelectedTab = tabPage1;
            }
            else
            {
                MessageBox.Show("Turn off Travel Mode!", "Alert");
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //Left search button
            Clearforsearch();
            if (listBox1.Items.Count >= 1)
                listBox1.SelectedIndex = Globals.Find(listBox1, textBox24.Text, listBox1.SelectedIndex + 1);
            if (listBox1.SelectedIndex != -1)
                ListBox1_MouseClick(this, new EventArgs());
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            //Right search button
            Clearforsearch();
            if (listBox2.Items.Count >= 1)
                listBox2.SelectedIndex = Globals.Find(listBox2, textBox25.Text, listBox2.SelectedIndex + 1);
            if (listBox2.SelectedIndex != -1)
                ListBox2_MouseClick(this, new EventArgs());
        }
        private void TextBox24_KeyUp(object sender, KeyEventArgs e)
        {
            //Pressing Enter searches listboxes
            if (e.KeyCode == Keys.Enter)
            {
                Button1_Click(this, new EventArgs());
            }
        }
        private void TextBox25_KeyUp(object sender, KeyEventArgs e)
        {
            //Pressing Enter searches listboxes
            if (e.KeyCode == Keys.Enter)
            {
                Button2_Click(this, new EventArgs());
            }
        }
        public void BackUpSaveSlot(TextBox tb, int slot, bool msg)
        {
            //Backup a single save slot Method
            if (SelectedSaveSlot >= 1 && SelectedSaveSlot <= 15)
            {
                string hgFileName = Path.GetFileNameWithoutExtension(hgFilePath);

                string mf_hgFilePath = hgFilePath;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                string mf_hgFileName = Path.GetFileNameWithoutExtension(mf_hgFilePath);

                if (Directory.Exists(@".\temp"))
                {
                    Directory.Delete(@".\temp", true);
                }
                
                Directory.CreateDirectory(@".\temp");

                File.Copy(hgFilePath, @".\temp\" + hgFileName + Path.GetExtension(hgFilePath));
                File.Copy(mf_hgFilePath, @".\temp\" + mf_hgFileName + Path.GetExtension(mf_hgFilePath));

                string datetime = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                ZipFile.CreateFromDirectory(@".\temp", @".\backup\saves\savebackup_" + slot + "_" + datetime + ".zip");

                Directory.Delete(@".\temp", true);

                if (File.Exists(@".\backup\saves\savebackup_" + slot + "_" + datetime + ".zip"))
                {
                    Globals.AppendLine(tb, "Save file Slot (" + slot + ") backed up to \\backup\\saves folder...");

                    if (msg == true)
                    {
                        MessageBox.Show("Save slot backup up to: savebackup_" + slot + "_" + datetime + ".zip", "Save Backup", MessageBoxButtons.OK);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (msg == true)
                    {
                        MessageBox.Show("No File backed up!", "Alert");
                    }
                    else
                    {
                        Globals.AppendLine(tb, "Something went wrong, no file backed up Error BU:5567");
                        return;
                    }
                }
            }
            else
            {
                if (msg == true)
                {
                    MessageBox.Show("No File Found / Select Save Slot!", "Alert");
                }
                else
                {
                    return;
                }
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            //backup a single save slot
            if (comboBox1.GetItemText(comboBox1.SelectedItem) != "")
                BackUpSaveSlot(textBox17, SelectedSaveSlot, true);
            else
                MessageBox.Show("Please select a save slot!", "Alert");
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            //Clear Interference Button
            if (SelectedSaveSlot >= 1 && SelectedSaveSlot <= 15 && textBox12.Text != "")
            {
                if (textBox12.Text == "False" || textBox12.Text == "false")
                {
                    MessageBox.Show("No Portal Interference Found!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Clear Portal Interference ? ", "Portal Interference", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //Read - Edit - Write Json save file for portal
                        WriteSavePortal(progressBar1, textBox27, SelectedSaveSlot);

                        ////Check save file edits
                        var nms = GameSaveData.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.PlayerStateData.OnOtherSideOfPortal.ToString();

                        if (textBox12.Text == "False" || textBox12.Text == "false")
                        {
                            progressBar1.Invoke((System.Action)(() => progressBar1.Value = 100));
                            progressBar1.Visible = false;

                            MessageBox.Show("Portal Interference removal successful!", "Confirmation", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Portal Interference Problem!", "Error");
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Save slot not selected!", "Alert");
            }
        }
        private void Button14_Click(object sender, EventArgs e)
        {
            //Freighter Battle Button
            if (SelectedSaveSlot >= 1 && SelectedSaveSlot <= 15)
            {
                DialogResult dialogResult = MessageBox.Show("Trigger a Freighter Battle ? ", "Freighter Battle", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //Read - Edit - Write Json save file for portal
                    WriteSaveFB(progressBar4, textBox15, SelectedSaveSlot);

                    var nms = GameSaveData.FromJson(json);
                    bool O5J = nms.PlayerStateData.TimeLastSpaceBattle == 0;
                    bool Ebr = nms.PlayerStateData.WarpsLastSpaceBattle == 0;
                    bool Exx = nms.PlayerStateData.ActiveSpaceBattleUA == 0;

                    if (O5J && Ebr && Exx)
                    {
                        progressBar4.Invoke((System.Action)(() => progressBar4.Value = 100));
                        progressBar4.Visible = false;

                        Globals.AppendLine(textBox15, "Freighter Battle Triggered, Reload save in game and warp.");
                        MessageBox.Show("Freighter Battle triggered successfully! \r\n\r\n Reload Save in game and warp.", "Confirmation", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Freighter Battle Problem!", "Error");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Save slot not selected!", "Alert");
            }
        }
        private async void DiscoveriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //backup all discoveries to json file
            //Backuplist.Clear();
            await BackupLoc(@".\backup\locations\locbackup.json");
        }
        private bool CheckForSameLoc(Destination dest)
        {
            //looks up the players current location
            Player player = PlayerLocation(json);

            bool same = dest.X == player.X && dest.Y == player.Y && dest.Z == player.Z && dest.SSI == player.SSI && dest.Galaxy == player.Galaxy;

            return same;

            //var nms = GameSaveData.FromJson(json);
            //var pgalaxy = nms.PlayerStateData.UniverseAddress.RealityIndex.ToString();
            //var pX = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelX.ToString();
            //var pY = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelY.ToString();
            //var pZ = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelZ.ToString();
            //var pSSI = nms.PlayerStateData.UniverseAddress.GalacticAddress.SolarSystemIndex.ToString();

            //checks to see if the current location is the same as stored (no move if same)
            //bool b = X == pX && Y == pY && Z == pZ && SSI == pSSI && pgalaxy == galaxy;
            //return b;
        }
        private string ListBoxSelected()
        {
            if (listBox1.SelectedIndex < 0 && listBox2.SelectedIndex != -1)
            {
                return listBox2.GetItemText(listBox2.SelectedItem);
            }
            else if (listBox2.SelectedIndex < 0 && listBox1.SelectedIndex != -1)
            {
                return listBox1.GetItemText(listBox1.SelectedItem);
            }
            else
            {
                return "";
            }
        }
        private void Button5_ClickAsync(object sender, EventArgs e)
        {
            //Move Player button on Base and Space Station tab
            try
            {
                if (SelectedSaveSlot < 1 || SelectedSaveSlot > 15)
                {
                    MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                    return;
                }
                if (listBox1.SelectedIndex < 0 && listBox2.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a location!", "Confirmation", MessageBoxButtons.OK);
                    return;
                }

                string si = ListBoxSelected();

                if (string.IsNullOrEmpty(si))
                {
                    MessageBox.Show("Something went wrong! ERROR [50]", "Error", MessageBoxButtons.OK);
                    return;
                }

                Destination dest = new Destination();

                //object selecteditem = listBox1.SelectedItem;
                //string si = selecteditem.ToString();
                si = si.Replace(" (B)", "");
                si = si.Replace(" (EB)", ""); //v1.1.16
                si = si.Replace(" (SS)", "");
                var nms = GameSaveData.FromJson(json);
                for (int i = 0; i < nms.PlayerStateData.TeleportEndpoints.Length; i++)
                {
                    if (nms.PlayerStateData.TeleportEndpoints[i].Name.ToString() == si)
                    {
                        dest = TeleportEndpoints(i, json);
                    }
                }

                if (dest.GalaxyName == "")
                {
                    MessageBox.Show("Something went wrong! ERROR [51]", "Error", MessageBoxButtons.OK);
                    return;
                }

                if (!string.IsNullOrEmpty(listBox1.GetItemText(listBox1.SelectedItem)) || !string.IsNullOrEmpty(listBox2.GetItemText(listBox2.SelectedItem)))
                {
                    DialogResult dialogResult = MessageBox.Show("Move Player to: " + listBox1.GetItemText(listBox1.SelectedItem) + listBox2.GetItemText(listBox2.SelectedItem) + " ? ", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (dest.Galaxy != "" && dest.X != "" && dest.Y != "" && dest.Z != "" && dest.SSI != "")
                        {
                            //Check if location is the same as cuurent
                            if (CheckForSameLoc(dest))
                            {
                                MessageBox.Show("Same as Current location!", "Alert");
                                return;
                            }
                            Globals.AppendLine(textBox27, "Move Player to: Galaxy (" + dest.Galaxy + ") X:" + dest.X + " Y:" + dest.Y + " Z:" + dest.Z + " SSI:" + dest.SSI);
                        
                            //Read - Edit - Write Json save file for move player
                            WriteSaveMove(SelectedSaveSlot, dest, progressBar1, textBox27);

                            //Read the new json and check portal interference state
                            var nms2 = GameSaveData.FromJson(json);
                            textBox12.Clear();
                            textBox12.Text = nms2.PlayerStateData.OnOtherSideOfPortal.ToString();
                            GetPlayerCoord();

                            progressBar1.Invoke((System.Action)(() => progressBar1.Value = 100));
                            progressBar1.Visible = false;

                            //set the last write time box
                            textBox26.Clear();
                            FileInfo hgfile = new FileInfo(hgFilePath);
                            Globals.AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                            MessageBox.Show("Player moved successfully! \r\n\r\n Reload Save in game.", "Confirmation", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Please click a location!", "Confirmation", MessageBoxButtons.OK);
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please click a location!", "Confirmation", MessageBoxButtons.OK);
                }
            }
            catch
            {
                Globals.AppendLine(textBox27, "Invalid Coordinates!");
            }
        }
        private void BackupALLSaveFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manually backup all save files in nmspath dir
            RunBackupAll(hgFileDir);
            MessageBox.Show("Save Backup Completed!", "Confirmation", MessageBoxButtons.OK);
        }     
        private void SetGoGSShot()
        {
            //Set the screenshot paths
            try
            {
                //Check for hg paths
                if (!Directory.Exists(nmsPath))
                {
                    return;
                }
                DirectoryInfo dname = new DirectoryInfo(nmsPath);

                //If nmsPath is GoG game save path, set to Pictures\No Mans Sky
                if (dname.Name == "DefaultUser")
                {
                    ssdPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\No Mans Sky";

                    if (!Directory.Exists(ssdPath))
                        ssdPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GOG Galaxy\Screenshots\No Man's Sky";
                }
                else
                {
                    //If not GoG, must be Steam, so set the screenshot dir path
                    stmPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Steam\userdata\"; // 307405899\";
                    if (Directory.Exists(stmPath))
                    {
                        List<string> list2 = new List<string>();
                        DirectoryInfo dinfo1 = new DirectoryInfo(stmPath);
                        DirectoryInfo[] dinfoss = dinfo1.GetDirectories("760", SearchOption.AllDirectories);

                        foreach (DirectoryInfo di in dinfoss)
                        {
                            string spath = di.FullName + @"\remote\275850\screenshots";
                            if (Directory.Exists(spath))
                            {
                                DirectoryInfo d3 = new DirectoryInfo(spath);
                                //Globals.AppendLine(textBox17, d3.FullName);

                                if (d3.GetFiles("*.jpg", SearchOption.TopDirectoryOnly).Length != 0 || d3.GetFiles("*.png", SearchOption.TopDirectoryOnly).Length != 0)
                                {
                                    list2.Add(d3.FullName);
                                }
                            }
                        }
                        if (list2.Count > 1)
                        {
                            Globals.AppendLine(textBox17, "More than on screenshot path detected. First found path set.");
                            ssdPath = list2[0];
                        }
                        else if (list2.Count == 1)
                        {
                            ssdPath = list2[0];
                        }
                        //ssdPath = Path.GetFullPath(list2[0].ToString() + @"\remote\275850\screenshots");
                    }
                }

                //Check the screenshot dir exists, if not return
                if (!Directory.Exists(ssdPath))
                {
                    Globals.AppendLine(textBox17, "Path to Screenshots doesn't exist");
                    return;
                }

                //look for jpg and png files and add to list
                List<FileInfo> list = new List<FileInfo>();
                DirectoryInfo dinfo2 = new DirectoryInfo(ssdPath);
                FileInfo[] Files = dinfo2.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
                FileInfo[] Files2 = dinfo2.GetFiles("*.png", SearchOption.TopDirectoryOnly);

                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                    {
                        list.Add(file);
                    }
                }

                if (Files2.Length != 0)
                {
                    foreach (FileInfo file in Files2.OrderByDescending(f => f.LastWriteTime))
                    {
                        list.Add(file);
                    }
                }

                if (Files2.Length == 0 && Files.Length == 0)
                {
                    //if no jpg or png files found, return
                    pictureBox25.Image = null;
                    Globals.AppendLine(textBox17, "No Screenshots found.");
                    return;
                }

                //Order the png or jpg by lastwrite times and set screenshot file path
                list.OrderByDescending(f => f.LastWriteTime);
                ssPath = list[0].FullName;
                Globals.AppendLine(textBox17, "ScreenShot: " + list[0].FullName);
                WriteTxt("ssdPath", ssdPath, nmscConfig);
                LoadSS();
            }
            catch
            {
                Globals.AppendLine(textBox17, "Screenshot Path Error! ERROR [55]");
                return;
            }
        }        
        private void LoadSS()
        {
            //Show picture from screenshot path
            pictureBox25.ImageLocation = ssPath;
            pictureBox25.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void SetGoGSSPath()
        {
            //Manually set screenshot path method
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        //look for jpg and png files and add to list
                        List<FileInfo> list = new List<FileInfo>();
                        DirectoryInfo dinfo2 = new DirectoryInfo(fbd.SelectedPath);
                        FileInfo[] Files = dinfo2.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
                        FileInfo[] Files2 = dinfo2.GetFiles("*.png", SearchOption.TopDirectoryOnly);

                        if (Files.Length != 0)
                        {
                            foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                            {
                                list.Add(file);
                            }
                        }

                        if (Files2.Length != 0)
                        {
                            foreach (FileInfo file in Files2.OrderByDescending(f => f.LastWriteTime))
                            {
                                list.Add(file);
                            }
                        }

                        if (Files2.Length == 0 && Files.Length == 0)
                        {
                            //if no jpg or png files found, return
                            pictureBox25.Image = null;
                            Globals.AppendLine(textBox17, "No Screenshots found.");
                            return;
                        }

                        //Order the png or jpg by lastwrite times and set screenshot file path
                        list.OrderByDescending(f => f.LastWriteTime);
                        ssPath = list[0].FullName;
                        Globals.AppendLine(textBox17, "ScreenShot: " + list[0].FullName);
                        WriteTxt("ssdPath", ssdPath, nmscConfig);
                        LoadSS();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Cancelled no path set!");
                    }
                }        
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n Screenshot path problem!");
                return;
            }
        }        
        private void ScreenshotPageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Open the screenshot page, pass ssdpath to it
            ScreenShotPreviewQuad f3 = new ScreenShotPreviewQuad();
            Globals.AppendLine(textBox17, ssdPath);
            f3.MyProperty2 = ssdPath;
            f3.Show();
        }
        private void ScreenshotPageToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //Manually select a SS path and save it in save.nmsc
            SetGoGSSPath();
            tabControl1.SelectedTab = tabPage1;
        }        
        private void SetSSDefaultSteamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //sets the default sspath
            SetGoGSShot();
            tabControl1.SelectedTab = tabPage1;
        }
        private void LoadTxt()
        {
            //Loads all the json files in listbox4 for selection
            listBox3.DataSource = null;
            textBox11.Clear();
            textBox13.Clear();
            textBox18.Clear();

            if (Directory.Exists(@".\backup") && Directory.Exists(@".\backup\locations"))
            {
                List<string> list = new List<string>();
                DirectoryInfo dinfo2 = new DirectoryInfo(@".\backup\locations");
                //FileInfo[] Files = dinfo2.GetFiles("locbackup*.txt", SearchOption.AllDirectories);
                FileInfo[] Files = dinfo2.GetFiles("*.json", SearchOption.AllDirectories);

                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                    {
                        if (file.Name.Contains("locbackup") || file.Name.Contains("player_loc"))
                            list.Add(file.Name);
                    }
                }
                else
                {
                    exportLocationRecordToolStripMenuItem.Enabled = false;
                    Globals.AppendLine(textBox11, "No location files found.");
                    return;
                }
                listBox4.DataSource = list;
                exportLocationRecordToolStripMenuItem.Enabled = true;
            }
        }
        private void LocationsLoad(string path)
        {
            //Method to load all location
            locjson = File.ReadAllText(path);
            var check = SavedLocationData.FromJson(locjson);

            try
            {
                if (check.Version != locVersion)
                {
                    locjson = LocationDataConverter.ConvertOldLocationFile(locjson, check.Version, locVersion, path);
                }

                SavedLocationData loc = SavedLocationData.FromJson(locjson);

                for (int i = 0; i < loc.Locations.TeleportEndpoints.Length; i++)
                {
                    string name = loc.Locations.TeleportEndpoints[i].Name;
                    string galaxy = loc.Locations.TeleportEndpoints[i].Details.Galaxy.ToString();
                    string datetime = loc.Locations.TeleportEndpoints[i].Details.DateTime;
                    string saveslot = loc.Locations.TeleportEndpoints[i].Details.SaveSlot.ToString();
                    //string filename = loc.Locations.Bases[i].Details.Filename;
                    string portalcode = loc.Locations.TeleportEndpoints[i].Details.PortalCode;
                    string galacticcoord = loc.Locations.TeleportEndpoints[i].Details.GalacticCoords;
                    string notes = loc.Locations.TeleportEndpoints[i].Details.Notes;

                    //listBox3.Items.Add("SaveSlot: " + SelectedSaveSlot + " " + filename + " " + name + " G: " + galaxy + " PC: " + portalcode + " GC: " + galacticcoord + " Notes: " + notes);
                    listBox3.Items.Add("Galaxy: (" + galaxy + ") / PC: " + portalcode + " / GC:  " + galacticcoord + " / " + name + " / " + notes);
                }
            }
            catch
            {
                Globals.AppendLine(textBox17, "** Code 1900 **");
                return;
            }
        }
        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Location summary viewer box
            if (listBox3.GetItemText(listBox3.SelectedItem) != "")
            {
                textBox18.Text = listBox3.Items.Count.ToString();

                if (textBox11.Text == "")
                {
                    Globals.AppendLine(textBox11, "---------------------");
                }
                if (locjson == null) // if text file problem
                {
                    textBox11.Clear();
                    Globals.AppendLine(textBox11, "File Error");
                    Globals.AppendLine(textBox11, "---------------------");

                    DialogResult dialogResult2 = MessageBox.Show("Something went wrong! \r\n\nSelected line has errors. \r\n\nWould you like to Delete the Line?", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult2 == DialogResult.Yes)
                    {
                        DeleteLocationRecordToolStripMenuItem_Click(this, new EventArgs());
                        return;
                    }
                    else if (dialogResult2 == DialogResult.No)
                    {
                        return;
                    }
                }
                var loc = SavedLocationData.FromJson(locjson);
                var name = loc.Locations.TeleportEndpoints[listBox3.SelectedIndex].Name;
                var pc = loc.Locations.TeleportEndpoints[listBox3.SelectedIndex].Details.PortalCode;
                var gc = loc.Locations.TeleportEndpoints[listBox3.SelectedIndex].Details.GalacticCoords;
                var notes = loc.Locations.TeleportEndpoints[listBox3.SelectedIndex].Details.Notes;

                Globals.AppendLine(textBox11, name);
                Globals.AppendLine(textBox11, pc);
                Globals.AppendLine(textBox11, gc);
                Globals.AppendLine(textBox11, notes);
                Globals.AppendLine(textBox11, "---------------------");

            }  
        }        
        public bool TextBoxPerm
        {
            //Check is manual travel is unlocked
            get { return textBox14.ReadOnly; }
        }        
        public string TextBoxValue
        {
            //bring over copied GC from Calculator
            get { return textBox14.Text; }
            set { textBox14.Text = value; }
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            //Coordinate Calculator button if not open, open
            if (f5 == null)
            {
                f5 = new CoordinateCalculator();
                f5.FormClosed += (_, arg) =>
                {
                    f5 = null;
                };
                f5.Show();
            }
            else
            {
                f5.BringToFront();
            }
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            //Move player button share coordinate tab
            try
            {
                if (!string.IsNullOrEmpty(listBox3.GetItemText(listBox3.SelectedItem)) && !string.IsNullOrEmpty(locjson))
                {
                    if (SelectedSaveSlot < 1 || SelectedSaveSlot > 15)
                    {
                        MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                        return;
                    }

                    DialogResult dialogResult = MessageBox.Show("Move Player? ", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //grabs the galactic coordinate
                        var loc = SavedLocationData.FromJson(locjson);
                        var basehx = loc.Locations.TeleportEndpoints[listBox3.SelectedIndex].Details.LongHex;
                        Destination dest = HexToAll(basehx);

                        //var gal = loc.Locations.TeleportEndpoints[listBox3.SelectedIndex].Details.Galaxy;
                        //var gc = loc.Locations.TeleportEndpoints[listBox3.SelectedIndex].Details.GalacticCoords;                        

                        if (string.IsNullOrEmpty(basehx))
                        {
                            DialogResult dialogResult2 = MessageBox.Show("Something went wrong! \r\n\nSelected line has errors. \r\n\nWould you like to Delete the Line?", "Fast Travel", MessageBoxButtons.YesNo);
                            if (dialogResult2 == DialogResult.Yes)
                            {
                                DeleteLocationRecordToolStripMenuItem_Click(this, new EventArgs());
                                return;
                            }
                            else if (dialogResult2 == DialogResult.No)
                            {
                                return;
                            }                                                       
                        }

                        //string[] value = gc.Split(':');

                        //Only take 4 digits from the last array so can add notes GC: 0000:0000:0000:[0000] A:B:C:D
                        //string A = value[0].Trim();
                        //string B = value[1].Trim();
                        //string C = value[2].Trim();
                        //string D = value[3].Trim().Substring(0, 4);

                        //Validate Coordinates
                        if (CoordCalculations.ValidateCoord(dest.GalacticCoordinate))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            return;
                        }

                        //Sets x,y,z,ssi ix,iy,iz,issi from given ABCD
                        //Destination dest = GalacticToVoxel(gal, A, B, C, D, textBox13);

                        Globals.AppendLine(textBox13, "Galaxy: " + dest.Galaxy + " -- X:" + dest.X + " -- Y:" + dest.Y + " -- Z:" + dest.Z + " -- SSI:" + dest.SSI);
                        
                        if (dest.Galaxy != "" && dest.X != "" && dest.Y != "" && dest.Z != "" && dest.SSI != "") // && SelectedSaveSlot >= 1 && SelectedSaveSlot <= 5)
                        {
                            //Check if location is the same as cuurent
                            if (CheckForSameLoc(dest))
                            {
                                MessageBox.Show("Same as Current location!", "Alert");
                                return;
                            }
                            Globals.AppendLine(textBox13, "Move Player to: Galaxy (" + dest.Galaxy + ") X:" + dest.X + " Y:" + dest.Y + " Z:" + dest.Z + " SSI:" + dest.SSI);

                            //Read - Edit - Write Json save file for move player
                            WriteSaveMove(SelectedSaveSlot, dest, progressBar3, textBox13);

                            //Read the new json and check portal interference state
                            var nms = GameSaveData.FromJson(json);
                            textBox12.Clear();
                            textBox12.Text = nms.PlayerStateData.OnOtherSideOfPortal.ToString();
                            GetPlayerCoord();

                            progressBar3.Invoke((System.Action)(() => progressBar3.Value = 100));
                            progressBar3.Visible = false;

                            //set the last write time box
                            textBox26.Clear();
                            FileInfo hgfile = new FileInfo(hgFilePath);
                            Globals.AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                            MessageBox.Show("Player moved successfully! \r\n\r\n Reload Save in game.", "Confirmation", MessageBoxButtons.OK);
                        }
                        else
                        {
                            //MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                            MessageBox.Show("Something went wrong!", "Alert", MessageBoxButtons.OK);
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    Globals.AppendLine(textBox13, "Please load and select a location!");
                    MessageBox.Show("Please load and select a location!", "Alert");
                }
            }
            catch
            {                
                Globals.AppendLine(textBox13, "Invalid Coordinates!");
            }
        }
        private void DeleteLocationRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //delete a single location record
            string record = listBox3.GetItemText(listBox3.SelectedItem);
            string filename = listBox4.GetItemText(listBox4.SelectedItem);
            int selectedindex = listBox3.SelectedIndex;
            var selectedfile = listBox4.SelectedItem;

            if (record != "" && filename != "")
            {
                string path = @".\backup\locations\" + filename;

                if (!File.Exists(path))
                {
                    MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                    LoadTxt();
                    return;
                }
                if (locjson == "")
                    return;

                string plocdata = File.ReadAllText(path);
                var loc = SavedLocationData.FromJson(plocdata);

                if (loc == null)
                {
                    MessageBox.Show("Problem with " + path + " json!", "Alert", MessageBoxButtons.OK);
                    return;
                }

                if (loc.Locations.TeleportEndpoints.Length <= 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Delete " + filename + " ?", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        File.Delete(path);
                        LoadTxt();
                        MessageBox.Show(filename + " deleted.", "Confirmation");
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    SavedLocationData sdata = Globals.CreateNewLocationJson(locVersion, loc.Locations.TeleportEndpoints.Length, 0);
                    List<LocationArray> basis = new List<LocationArray>();

                    for (int i = 0; i < loc.Locations.TeleportEndpoints.Length; i++)
                    {
                        if (i != selectedindex)
                            basis.Add(loc.Locations.TeleportEndpoints[i]);
                    }
                    //selectedindex = basis.Count;
                    sdata.Locations.TeleportEndpoints = basis.ToArray();
                    var backuplist = LocationData.Serialize.ToJson(sdata);
                    File.WriteAllText(path, backuplist);
                    LoadTxt();
                    Globals.AppendLine(textBox13, "Single Record deleted.");
                }
                //LoadTxt();

                if (File.Exists(path))
                {
                    listBox4.SelectedItem = selectedfile;
                    //Button6_Click(this, new EventArgs());

                    if (selectedindex == 0)
                        listBox3.SelectedIndex = selectedindex;
                    else
                        listBox3.SelectedIndex = selectedindex - 1;
                }
            }
            else
            {
                Globals.AppendLine(textBox13, "No record deleted! Please select a file!");
            }
        }
        private void ExportLocationRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItems.Count == 1)
            {
                //Save a single location record to a new json file
                if (!string.IsNullOrEmpty(listBox3.GetItemText(listBox3.SelectedItem)))
                {
                    var selectedrecord = listBox3.GetItemText(listBox3.SelectedItem);
                    var selectindex = listBox3.SelectedIndex;

                    //List<string> list = new List<string>();
                    //list.Add(listBox3.GetItemText(listBox3.SelectedItem));
                    string path2 = Globals.MakeUniqueLoc(@".\backup\locations\locbackup.json", SelectedSaveSlot);
                    var loc = SavedLocationData.FromJson(locjson);
                    var record = loc.Locations.TeleportEndpoints[selectindex];

                    // Create new locbackup json file
                    var loc2 = Globals.CreateNewLocationJson(locVersion, 1, 0);
                    //SavedLocationData loc2 = new SavedLocationData();
                    //loc2.Version = 1;
                    //loc2.Locations = new Locations();
                    //loc2.Locations.Bases = new Basis[1];
                    //loc2.Locations.Spacestations = new Basis[0];
                    loc2.Locations.TeleportEndpoints[0] = record;
                    string newrec = LocationData.Serialize.ToJson(loc2);
                    File.WriteAllText(path2, newrec);

                    //File.WriteAllLines(path2, list);
                    //Process.Start(path2); //v1.1.14

                    LoadTxt();
                    Globals.AppendLine(textBox13, "Single Record saved.");

                    if (File.Exists(path2))
                    {
                        listBox4.SelectedItem = Path.GetFileName(path2);
                        //Button6_Click(this, new EventArgs());
                        listBox3.SelectedItem = selectedrecord;
                    }
                }
                else
                {
                    Globals.AppendLine(textBox13, "No record saved! Please select a file!");
                }
            }
            else if (listBox3.SelectedItems.Count > 1)
            {
                string path2 = Globals.MakeUniqueLoc(@".\backup\locations\locbackup.json", SelectedSaveSlot);
                var loc = SavedLocationData.FromJson(locjson);
                var loc2 = Globals.CreateNewLocationJson(locVersion, listBox3.SelectedItems.Count, 0);

                foreach (String selected in listBox3.SelectedItems)
                {
                    //
                    if (string.IsNullOrEmpty(listBox3.GetItemText(selected)))
                    {
                        Globals.AppendLine(textBox13, "No record saved! Please select a file!");
                    }

                    var selectindex = listBox3.Items.IndexOf(selected);
                    LocationArray record = loc.Locations.TeleportEndpoints[selectindex];

                    // Create new locbackup json file                    
                    loc2.Locations.TeleportEndpoints[listBox3.SelectedItems.IndexOf(selected)] = record;
                    string newrec = LocationData.Serialize.ToJson(loc2);
                    File.WriteAllText(path2, newrec);                    
                }

                LoadTxt();
                Globals.AppendLine(textBox13, "Records saved.");

                if (File.Exists(path2))
                {
                    listBox4.SelectedItem = Path.GetFileName(path2);
                }
            }
        }
        private void OpenLocationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open a locbackup file
            if (string.IsNullOrEmpty(listBox4.GetItemText(listBox4.SelectedItem)))
            {
                return;
            }

            if (File.Exists(@".\backup\locations\" + listBox4.GetItemText(listBox4.SelectedItem)))
            {
                Process.Start(@".\backup\locations\" + listBox4.GetItemText(listBox4.SelectedItem));
                LoadTxt();
            }
            else
            {
                MessageBox.Show("No file found.", "Alert");
                LoadTxt();
            }
        }
        private void DeleteLocationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Delete a locbackup file
            if (!string.IsNullOrEmpty(listBox4.GetItemText(listBox4.SelectedItem)))
            {
                DialogResult dialogResult = MessageBox.Show("Delete " + listBox4.GetItemText(listBox4.SelectedItem) + " ? ", "Locbackup Manager", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (File.Exists(@".\backup\locations\" + listBox4.GetItemText(listBox4.SelectedItem)))
                    {
                        if (listBox4.GetItemText(listBox4.SelectedItem) != "")
                        {
                            File.Delete(@".\backup\locations\" + listBox4.GetItemText(listBox4.SelectedItem));
                            LoadTxt();
                            MessageBox.Show("File deleted Successfully.", "Confirmation");

                        }
                        else
                        {
                            MessageBox.Show("No file found, not deleted.", "Alert");
                            LoadTxt();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No file found, not deleted.", "Alert");
                        LoadTxt();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Cancelled! No file deleted.", "Alert");
                }
            }                
        }
        private static bool ValidateLocFiles(ListBox lb, string selected)
        {
            if (string.IsNullOrEmpty(lb.GetItemText(selected)))
            {
                return false;
            }

            if (!File.Exists(@".\backup\locations\" + lb.GetItemText(selected)))
            {
                return false;
            }

            return true;
        }
        private static List<LocationArray> FindDuplicateLocations(List<LocationArray> locList)
        {
            List<LocationArray> newloclist = new List<LocationArray>();

            foreach (LocationArray tpendpoint in locList)
            {
                bool found = false;

                foreach (LocationArray tpendpoint2 in newloclist)
                {
                    if (tpendpoint.Name == tpendpoint2.Name)
                    {
                        if (tpendpoint.Details.LongHex == tpendpoint2.Details.LongHex)
                        {
                            found = true;

                            if (tpendpoint.Details.Notes == "" && tpendpoint2.Details.Notes != "")
                                tpendpoint.Details.Notes = tpendpoint2.Details.Notes;

                            if (tpendpoint.Details.Notes != "" && tpendpoint2.Details.Notes == "")
                                tpendpoint2.Details.Notes = tpendpoint.Details.Notes;
                        }                        
                    }
                }

                if (!found)
                    newloclist.Add(tpendpoint);
            }

            return newloclist;

            /*
            List<LocationArray> newloclist = new List<LocationArray>();
            
            foreach (LocationArray tpendpoint in locList)
            {
                int count = 0;

                foreach (LocationArray tpendpoint2 in locList)
                {
                    if (tpendpoint.Name == tpendpoint2.Name)
                        count++;
                }

                if (count == 1)
                    newloclist.Add(tpendpoint);
            }

            return newloclist;*/
        }
        private void mergeLocationFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Merge location files
            if (listBox4.SelectedItems.Count > 1)
            {
                List<LocationArray> loclist = new List<LocationArray>();

                foreach (String selected in listBox4.SelectedItems)
                {
                    if (!ValidateLocFiles(listBox4, selected))
                    {
                        return;
                    }

                    string path = @".\backup\locations\" + selected;
                    string filejson = File.ReadAllText(path);
                    SavedLocationData loc = SavedLocationData.FromJson(filejson);

                    foreach (LocationArray tpendpoint in loc.Locations.TeleportEndpoints)
                    {
                        loclist.Add(tpendpoint);
                    }
                }
                //var duplicateThingCounts = loclist.Where(tc => tc. > 1)

                loclist = FindDuplicateLocations(loclist);

                SavedLocationData loc2 = Globals.CreateNewLocationJson(locVersion, loclist.Count, 0);
                loc2.Locations.TeleportEndpoints = loclist.ToArray();

                string path2 = Globals.MakeUniqueLoc(@".\backup\locations\locbackup.json", 0);
                string newjson = LocationData.Serialize.ToJson(loc2);
                File.WriteAllText(path2, newjson);

                LoadTxt();
                Globals.AppendLine(textBox13, "Files merged.");
            }
            else
            {
                MessageBox.Show("Please select 2 or more files!", "Alert");
            }
        }
        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (listBox4.SelectedItems.Count > 1)
            {
                openLocationFileToolStripMenuItem.Enabled = false;
                deleteLocationFileToolStripMenuItem.Enabled = false;

                if (!mergeLocationFilesToolStripMenuItem.Enabled)
                    mergeLocationFilesToolStripMenuItem.Enabled = true;
            }
            else
            {
                mergeLocationFilesToolStripMenuItem.Enabled = false;

                if (!openLocationFileToolStripMenuItem.Enabled)
                    openLocationFileToolStripMenuItem.Enabled = true;

                if (!deleteLocationFileToolStripMenuItem.Enabled)
                    deleteLocationFileToolStripMenuItem.Enabled = true;
            }
        }
        private void SetShortcutToGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Future use, doesn't save changed currently
            openFileDialog1.ShowDialog();
        }
        private void OpenFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GamePath = openFileDialog1.FileName;
            Globals.AppendLine(textBox17, GamePath);
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            //shortcut NMS button
            try
            {
                if (GamePath == "" || GamePath == null)
                {
                    DirectoryInfo dname = new DirectoryInfo(nmsPath);
                    if (dname.Name == "DefaultUser")
                        System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\GOG Galaxy\Games\No Man's Sky\Binaries\NMS.exe");
                    else
                        System.Diagnostics.Process.Start("steam://rungameid/275850");
                }
                else
                {
                    System.Diagnostics.Process.Start(GamePath);
                }
            }
            catch
            {
                return;
            }            
        }
        private void Button11_Click(object sender, EventArgs e)
        {
            //Move player button on Manual Travel Tab
            try
            {
                if (string.IsNullOrEmpty(textBox14.Text))
                {
                    MessageBox.Show("Cannot locate player!", "Alert");
                    return;
                }

                if (SelectedSaveSlot < 1 || SelectedSaveSlot > 15)
                {
                    MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                    return;
                }

                //Validate galaxy
                if (comboBox3.SelectedIndex > 255 || comboBox3.SelectedIndex == -1) //selectedindex 0-255 = galaxy 1-256
                {
                    MessageBox.Show("Invalid Galaxy!", "Alert");
                    return;
                }
                    
                //removes all spaces
                string inputcoord = textBox14.Text.Replace(" ", "");

                //if invalid format
                if (inputcoord.Replace(":", "").Length < 16 | inputcoord.Replace(":", "").Length > 16 | inputcoord.Length < 16)
                {
                    MessageBox.Show("Invalid Coordinate Input!", "Alert");
                    return;
                }

                //Validate Coordinates
                if (CoordCalculations.ValidateCoord(inputcoord))
                {
                    MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                    return;
                }

                int selectedindex = comboBox3.SelectedIndex;
                //Destination dest = new Destination();

                GalacticCoordinates gac = GetGalacticCoordHex(inputcoord);
                Destination dest = GalacticToVoxel(selectedindex, gac.HexX, gac.HexY, gac.HexZ, gac.HexSSI, textBox15);

                ////if format 0000:0000:0000:0000 A:B:C:D
                //if (t2.Contains(":") && t2.Length == 19)
                //{
                //    string[] value = t2.Replace(" ", "").Split(':');
                //    string A = value[0].Trim();
                //    string B = value[1].Trim();
                //    string C = value[2].Trim();
                //    string D = value[3].Trim();

                //    //Validate Coordinates
                //    if (CoordCalculations.ValidateCoord(t2))
                //    {
                //        MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                //        return;
                //    }

                //    //sets x,y,z,ssi ix,iy,iz,issi from given ABCD
                //    dest = GalacticToVoxel(selectedindex, A, B, C, D, textBox15);
                //}

                ////if format 0000000000000000
                //if (t2.Length == 16 && !t2.Contains(":"))
                //{
                //    //0000 0000 0000 0000  XXXX:YYYY:ZZZZ:SSIX  A B C D
                //    string A = t2.Substring(t2.Length - 16, 4);
                //    string B = t2.Substring(t2.Length - 12, 4);
                //    string C = t2.Substring(t2.Length - 8, 4);
                //    string D = t2.Substring(t2.Length - 4, 4);

                //    //Validate Coordinates
                //    if (CoordCalculations.ValidateCoord(A, B, C, D))
                //    {
                //        MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                //        return;
                //    }

                //    //sets x,y,z,ssi ix,iy,iz,issi from given ABCD
                //    dest = GalacticToVoxel(selectedindex, A, B, C, D, textBox15);
                //}

                if (dest.Galaxy != "" && dest.X != "" && dest.Y != "" && dest.Z != "" && dest.SSI != "")
                {
                    DialogResult dialogResult = MessageBox.Show("Move Player? ", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //Check if location is the same as cuurent
                        if (CheckForSameLoc(dest))
                        {
                            MessageBox.Show("Same as Current location!", "Alert");
                            return;
                        }
                        Globals.AppendLine(textBox15, "Move Player to: Galaxy (" + dest.Galaxy + ") X:" + dest.X + " Y:" + dest.Y + " Z:" + dest.Z + " SSI:" + dest.SSI);

                        WriteSaveMove(SelectedSaveSlot, dest, progressBar4, textBox15);

                        //Read - Edit - Write Json save file for move player
                        var nms = GameSaveData.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.PlayerStateData.OnOtherSideOfPortal.ToString();
                        GetPlayerCoord();

                        progressBar4.Invoke((System.Action)(() => progressBar4.Value = 100));
                        progressBar4.Visible = false;

                        //set the last write time box
                        textBox26.Clear();
                        FileInfo hgfile = new FileInfo(hgFilePath);
                        Globals.AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                        MessageBox.Show("Player moved successfully! \r\n\r\n Reload Save in game.", "Confirmation", MessageBoxButtons.OK);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        textBox15.Clear();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "Alert", MessageBoxButtons.OK);
                    return;
                }
            }
            catch
            {
                textBox15.Clear();
                comboBox3.SelectedIndex = -1;
                Globals.AppendLine(textBox15, "Incorrect Coordinate Input!");
            }
        }
        private void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //populates the combobox when moving tabs
            if (json != string.Empty && json != null)
            {
                Player player = PlayerGalaxy(json);

                if (player.Galaxy != null && player.Galaxy != "")
                {
                    comboBox3.Items.Clear();
                    label35.Text = "";
                    label36.Text = "";

                    //Add Galaxy numbers
                    for (int i = 1; i <= 256; i++) // increase to 256 1.1.16
                    {
                        string[] numbers = { i.ToString() };
                        comboBox3.Items.AddRange(numbers);
                    }

                    // Negative galaxies
                    // string[] neg = { "-6", "-5", "-4", "-3", "-2", "-1", "0" };
                    // comboBox3.Items.AddRange(neg);

                    int setgx = player.iGalaxy;
                    if (setgx >= 0 && setgx <= 255)
                    {
                        comboBox3.SelectedIndex = setgx;
                    }
                    else
                    {
                        // Do nothing
                    }

                    //Lookup and display galaxy name in label
                    label35.Text = Globals.GalaxyLookup(player.Galaxy);
                    label36.Text = Globals.CheckGalaxyType(player.Galaxy);

                    //only sets the textbox to current location if manual travel is locked
                    //if (unlockedToolStripMenuItem.Checked == false)
                    textBox14.Text = textBox22.Text;
                }
            }
        }
        private void ComboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //sets the player galaxy when combobox change committed to fast travel
            string selectedgalaxy = comboBox3.SelectedIndex.ToString();

            //Lookup and display galaxy name in label
            label35.Text = Globals.GalaxyLookup(selectedgalaxy);
            label36.Text = Globals.CheckGalaxyType(selectedgalaxy);
        }
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load a json file to view in listbox3
            try
            {
                if (listBox4.GetItemText(listBox4.SelectedItem) != "")
                {
                    listBox3.Items.Clear();

                    string path = @".\backup\locations\" + listBox4.SelectedItem.ToString();

                    if (!File.Exists(path))
                    {
                        MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                        LoadTxt();
                        return;
                    }
                    LocationsLoad(path);
                    textBox18.Text = listBox3.Items.Count.ToString();
                }
                else
                {
                    exportLocationRecordToolStripMenuItem.Enabled = false;
                    Globals.AppendLine(textBox11, "No File Selected or File Empty!");
                    Globals.AppendLine(textBox11, "---------------------");
                }
            }
            catch
            {
                exportLocationRecordToolStripMenuItem.Enabled = false;
                Globals.AppendLine(textBox11, "No File Selected or File Empty!");
                Globals.AppendLine(textBox11, "---------------------");
            }
        }
        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("https://kevin0m16.github.io/NMSCoordinates/");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("https://nomanssky.gamepedia.com/Galaxy");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }
        private void CheckSS()
        {
            //Take the list of current discoveries and add them to SSList for comparison to PrevSSlist
            GameSaveData nms = GameSaveData.FromJson(json);
            int teleportArrayLength = nms.PlayerStateData.TeleportEndpoints.Length;

            if (teleportArrayLength > 0)
            {
                // Set Minimum to 1 to represent the first file being copied.
                progressBar2.Minimum = 1;
                // Set Maximum to the total number of files to copy.
                progressBar2.Maximum = teleportArrayLength;
                // Set the initial value of the ProgressBar.
                progressBar2.Value = 1;
                // Set the Step property to a value of 1 to represent each file being copied.
                progressBar2.Step = 1;
                // Display the ProgressBar control.
                progressBar2.Visible = true;

                for (int i = 0; i < teleportArrayLength; i++)
                {
                    Destination dest = TeleportEndpoints(i, json);

                    SSlist.Add("Slot_" + SelectedSaveSlot + "_Loc: " + nms.PlayerStateData.TeleportEndpoints[i].Name + " - G: " + dest.Galaxy + " - PC: " + dest.PortalCode + " -- GC: " + dest.GalacticCoordinate);

                    progressBar2.PerformStep();
                }
                progressBar2.Visible = false;
                progressBar2.Maximum = 100;
            }
            else
            {
                return;
            }
        }
        private void SetPrevSS()
        {
            //Travel Mode, Store all discoveries to compare later
            if (SSlist.Count > 0)
            {
                // Set Minimum to 1 to represent the first file being copied.
                progressBar2.Minimum = 1;
                // Set Maximum to the total number of files to copy.
                progressBar2.Maximum = SSlist.Count;
                // Set the initial value of the ProgressBar.
                progressBar2.Value = 1;
                // Set the Step property to a value of 1 to represent each file being copied.
                progressBar2.Step = 1;
                // Display the ProgressBar control.
                progressBar2.Visible = true;

                //Add all discoveries from SSlist to PrevSSlist
                foreach (string item in SSlist)
                {
                    PrevSSlist.Add(item);
                    progressBar2.PerformStep();
                }
                progressBar2.Visible = false;
                Globals.AppendLine(textBox17, "Current Stored locations saved.");
                MessageBox.Show("Current Stored locations saved.", "Confirmation");
            }
            else
            {
                return;
            }
        }
        private void CheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            //Travel Mode checkbox
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "" && checkBox1.Checked)
            {
                //Locks to one save slot
                comboBox2.Enabled = false;
                label30.Visible = true;
                Globals.AppendLine(textBox17, "Save Slot Selection Disabled...");

                SSlist.Clear();
                PrevSSlist.Clear();
                Globals.AppendLine(textBox17, "Reading all locations...");

                //Checks current locations is save and stores them in prev list
                CheckSS();
                SetPrevSS();                
            }
            else if (selected == "" && checkBox1.Checked)
            {
                comboBox2.Enabled = true;
                label30.Visible = false;
                checkBox1.Checked = false;
                MessageBox.Show("Select a Save Slot!", "Alert");
            }
            if (selected != "" && checkBox1.Checked == false)
            {
                comboBox2.Enabled = true;
                label30.Visible = false;
                checkBox1.Checked = false;
            }
        }
        private void Button12_Click(object sender, EventArgs e)
        {
            //Check for deletions button
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "" && checkBox1.Checked)
            {
                ClearAll();
                Globals.AppendLine(textBox17, "Loading Save File...");
                GetSaveFile(selected);
                LoadTeleportEndpoints();
                //LoadPersistentPlayerBases();
                GetPlayerCoord();

                SSlist.Clear();
                Globals.AppendLine(textBox17, "Checking for deleted locations...");

                CheckSS();

                //Toggle For Testing
                //SSlist.RemoveRange(0, 4);
                //SSlist.Add("Slot_2_Loc: test Platform (SS) - G: 1 - PC: 000000000000 -- GC: 080B:0088:080F:019E");
                //SSlist.Add("Slot_2_Loc: test2 Platform (SS) - G: 2 - PC: 200000000000 -- GC: 080B:0088:080F:019E");

                //Looks for all adds and deletes and returns to list3
                List<string> list3 = Globals.Contains(PrevSSlist, SSlist);

                List<string> DeletedSSlist = new List<string>();

                foreach (string item in list3)
                {
                    //If an added loc, it will be in sslist, so everything else is deleted
                    if (!SSlist.Contains(item))
                        DeletedSSlist.Add(item);
                }

                if (!File.Exists(@".\backup\locations\locbackup_deleted.txt"))
                {
                    File.Create(@".\backup\locations\locbackup_deleted.txt").Dispose();
                }

                List<string> logFile = File.ReadAllLines(@".\backup\locations\locbackup_deleted.txt").ToList();

                List<string> noduplicates = new List<string>();
                foreach (string item in DeletedSSlist)
                {
                    noduplicates.Add(item);
                }

                foreach (string item in DeletedSSlist)
                {
                    //Looks thru the deleted.txt and if an item in deletedsslist is in there removes it from noduplicates
                    if (!logFile.Contains(item))
                    {
                        logFile.Add(item);
                    }
                    else
                    {
                        noduplicates.Remove(item);
                    }
                }

                //noduplicates after all the checks are done, if >= 1 that was deleted
                if (noduplicates.Count >= 1)
                {
                    File.WriteAllLines(@".\backup\locations\locbackup_deleted.txt", logFile);
                    Globals.AppendLine(textBox17, noduplicates.Count.ToString() + " Deleted locations Found.");
                    LoadTxt();
                }
                else
                {
                    Globals.AppendLine(textBox17, "No Deleted locations Found.");
                }
            }
            else
            {
                MessageBox.Show("Not Enabled!", "Alert");
            }
        }
        private void OnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Travel Mode ON
            tabControl1.SelectedTab = tabPage1;
            offToolStripMenuItem.Checked = false;
            groupBox20.Show();
            Globals.AppendLine(textBox17, "Travel Mode VISIBLE. Select a save and click the box");
        }
        private void OffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Travel Mode OFF
            tabControl1.SelectedTab = tabPage1;
            onToolStripMenuItem.Checked = false;
            if (checkBox1.Checked)
            {
                checkBox1.Checked = false;
            }
            groupBox20.Hide();
            Globals.AppendLine(textBox17, "Travel Mode HIDDEN.");
        }
        private void LockedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manual travel LOCK
            unlockedToolStripMenuItem.Checked = false;
            textBox14.ReadOnly = true;
            label33.Visible = false;
            textBox14.Text = textBox22.Text;
            Globals.AppendLine(textBox17, "Manual Travel LOCKED.");
            MessageBox.Show("Manual Travel LOCKED", "Confirmation");
        }
        private void UnlockedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manual Travel Unlock
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to UNLOCK Manual Travel?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                lockedToolStripMenuItem.Checked = false;
                textBox14.ReadOnly = false;
                label33.Visible = true;
                textBox14.Text = textBox22.Text;
                Globals.AppendLine(textBox17, "Manual Travel UNLOCKED, Enter Coord. on Manual Travel tab.");
                MessageBox.Show("Manual travel UNLOCKED, Enter Coord. on Manual Travel tab. \r\n\n Make sure Coordinates are correct!", "Warning");

                Player player = PlayerGalaxy(json);
                if (player.Galaxy != null && player.Galaxy != "")
                    tabControl1.SelectedTab = tabPage4;
            }
            else if (dialogResult == DialogResult.No)
            {
                lockedToolStripMenuItem.Checked = true;
                unlockedToolStripMenuItem.Checked = false;
                textBox14.ReadOnly = true;
                label33.Visible = false;
            }
        }
        private void PictureBox25_Click(object sender, EventArgs e)
        {
            //Click the pick to refresh the screenshot
            if (Directory.Exists(ssdPath))
            {
                List<string> list = new List<string>();
                DirectoryInfo dinfo2 = new DirectoryInfo(ssdPath);
                FileInfo[] Files = dinfo2.GetFiles("*.jpg", SearchOption.AllDirectories);

                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                    {
                        if (!file.DirectoryName.Contains("thumbnails"))
                            list.Add(file.FullName);
                    }
                    ssPath = list[0].ToString();
                    pictureBox25.Image = null;
                    pictureBox25.ImageLocation = ssPath;
                }
            }
        }
        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("https://www.reddit.com/r/NMSCoordinateExchange/");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }
        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Created by: Kevin0M16 \r\n\r\n 8-2019");
            About f2 = new About(NMSCVersion);
            f2.ShowDialog();

        }
        private void OpenBackupFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open the \backup dir in file explorer
            Process.Start(@".\backup");
        }
        private void SaveFileManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open the save file manager
            if (checkBox1.Checked == false)
            {
                comboBox1.DataSource = null;
                comboBox2.DataSource = null;
                comboBox1.SelectedIndex = -1;
                ClearAll();

                SaveFileManager f6 = new SaveFileManager();
                f6.nmsPath = nmsPath;
                f6.ShowDialog();

                tabControl1.SelectedTab = tabPage1;
                LoadCmbx();
            }
            else
            {
                MessageBox.Show("Turn off Travel Mode!", "Alert");
            }
        }
        private void FileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            //Watch for changes in hg files
            List<string> list = new List<string>();

            foreach (KeyValuePair<int, string> item in comboBox1.Items)
            {
                list.Add(item.Value);
            }

            if (list.Contains(e.Name) || list.Contains(e.Name.Replace("mf_", "")))
            {
                lock (_changedFiles)
                {
                    if (_changedFiles.Contains(e.FullPath))
                    {
                        return;
                    }
                    _changedFiles.Add(e.FullPath);
                }

                //if changes detected, show form7 files changed externally
                SaveModified f7 = new SaveModified();
                f7.ShowDialog();

                if (f7.SaveChanged == true)
                {
                    var selected = comboBox2.SelectedItem;
                    ClearAll();
                    LoadCmbx();
                    comboBox2.SelectedItem = selected;                 
                    ComboBox2_SelectionChangeCommitted(this, new EventArgs());
                    if (tabControl1.SelectedTab == tabPage4)
                    {
                        tabControl1.SelectedTab = tabPage1; //added v1.1.11
                    }                        
                }
                else
                {
                    Globals.AppendLine(textBox17, "Not Viewing the latest save!");
                }

                System.Timers.Timer timer = new System.Timers.Timer(1000) { AutoReset = false };
                timer.Elapsed += (timerElapsedSender, timerElapsedArgs) =>
                {
                    lock (_changedFiles)
                    {
                        _changedFiles.Remove(e.FullPath);
                    }
                };
                timer.Start();
            }
        }
        private void Button13_Click(object sender, EventArgs e)
        {
            //Save current player location to json
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(json))
            {
                MessageBox.Show("Problem with json! ERROR [14]", "Error", MessageBoxButtons.OK);
                return;
            }
            
            Player player = PlayerLocation(json);

            if (player.Galaxy != "")
            {
                string plocpath = @".\backup\locations\player_locs.json";
                string currentpc = textBox21.Text;
                string currentgc = textBox22.Text;
                int selectedindex = 0;

                if (!File.Exists(plocpath))
                {
                    selectedindex = 1;
                    SavedLocationData sdata = Globals.CreateNewLocationJson(locVersion, 1, 0);
                    List<LocationArray> basis = new List<LocationArray>();

                    basis.Add(new LocationArray()
                    {
                        Name = DateTime.Now.ToString("MM-dd-yyyy HH:mm"),
                        Details = new Details()
                        {
                            DateTime = DateTime.Now.ToString("MM-dd-yyyy HH:mm"),
                            SaveSlot = SelectedSaveSlot,
                            LongHex = player.LongHex,
                            Galaxy = player.iGalaxy,
                            PortalCode = currentpc,
                            GalacticCoords = currentgc,
                            Notes = ""
                        }
                    });
                    sdata.Locations.TeleportEndpoints = basis.ToArray();
                    var backuplist = LocationData.Serialize.ToJson(sdata);
                    File.WriteAllText(plocpath, backuplist);
                }
                else
                {                    
                    string plocdata = File.ReadAllText(plocpath);
                    var loc = SavedLocationData.FromJson(plocdata);

                    if (loc == null)
                    {
                        MessageBox.Show("Problem with player_loc.json!", "Alert", MessageBoxButtons.OK);
                        return;
                    }

                    SavedLocationData sdata = Globals.CreateNewLocationJson(locVersion, loc.Locations.TeleportEndpoints.Length, 0);
                    List<LocationArray> basis = new List<LocationArray>();

                    for (int i = 0; i < loc.Locations.TeleportEndpoints.Length; i++)
                    {
                        if (loc.Locations.TeleportEndpoints[i].Details.GalacticCoords == currentgc)
                        {
                            MessageBox.Show("Location already saved in player_loc.json!", "Alert", MessageBoxButtons.OK);
                            return;
                        }
                        basis.Add(loc.Locations.TeleportEndpoints[i]);
                    }

                    basis.Add(new LocationArray()
                    {
                        Name = DateTime.Now.ToString("MM-dd-yyyy HH:mm"),
                        Details = new Details()
                        {
                            DateTime = DateTime.Now.ToString("MM-dd-yyyy HH:mm"),
                            SaveSlot = SelectedSaveSlot,
                            LongHex = player.LongHex,
                            Galaxy = player.iGalaxy,
                            PortalCode = currentpc,
                            GalacticCoords = currentgc,
                            Notes = ""
                        }
                    });
                    selectedindex = basis.Count;
                    sdata.Locations.TeleportEndpoints = basis.ToArray();
                    var backuplist = LocationData.Serialize.ToJson(sdata);
                    File.WriteAllText(plocpath, backuplist);
                }

                //File.WriteAllLines(@".\backup\locations\player_locs.txt", playerloc);
                MessageBox.Show("Location added to player_loc.json \n\n\r Open in Coordinate Share Tab", "Confirmation", MessageBoxButtons.OK);
                LoadTxt();

                tabControl1.SelectedTab = tabPage3;
                if (File.Exists(@".\backup\locations\player_locs.json"))
                {
                    listBox4.SelectedItem = "player_locs.json";
                    //Button6_Click(this, new EventArgs());
                    listBox3.SelectedIndex = selectedindex - 1;
                }                
            }
            else
            {
                MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
            }
        }
        public int ProgressValue
        {
            get { return progressBar2.Value; }
            set { progressBar2.Value = value; }
        }
        public ProgressBar ProgressBar
        {
            get { return progressBar2; }
            //set { progressBar2; }
        }
        private void WriteSaveFB(ProgressBar pb, TextBox tb, int saveslot)
        {
            //Main method for writing a change for a freighter battle
            fileSystemWatcher1.EnableRaisingEvents = false;

            BackUpSaveSlot(tb, saveslot, false);
            //DecryptSave(saveslot);
            EditSaveFB(pb);
            //CreateNewSave(out json, modSave, ufmodSave, true);
            //EncryptSave(pb, saveslot);
            //SaveCompression.CompressSave(hgFilePath);

            GameSave.EncryptSave((uint)saveslot, hgFilePath, modSave);
            json = GameSave.DecryptSave(hgFilePath, Save).ToString();

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private void WriteSavePortal(ProgressBar pb, TextBox tb, int saveslot)
        {
            //Main method for writing a change in portal status
            fileSystemWatcher1.EnableRaisingEvents = false;

            BackUpSaveSlot(tb, saveslot, false);
            //DecryptSave(saveslot);
            EditSavePortal(pb);
            //CreateNewSave(out json, modSave, ufmodSave, true);
            //EncryptSave(pb, saveslot);
            //SaveCompression.CompressSave(hgFilePath);

            GameSave.EncryptSave((uint)saveslot, hgFilePath, modSave);
            json = GameSave.DecryptSave(hgFilePath, Save).ToString();

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private void WriteSaveMove(int saveslot, Destination dest, ProgressBar pb, TextBox tb)
        {
            //Main method for writing a player move
            fileSystemWatcher1.EnableRaisingEvents = false;

            BackUpSaveSlot(tb, saveslot, false);
            //DecryptSave(saveslot);
            EditSaveMove(dest, pb, tb);
            //CreateNewSave(out json, modSave, ufmodSave, true);
            //EncryptSave(pb, saveslot);
            //SaveCompression.CompressSave(hgFilePath);

            GameSave.EncryptSave((uint)saveslot, hgFilePath, modSave);
            json = GameSave.DecryptSave(hgFilePath, Save).ToString();

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        /*
        private void DecryptSave(int saveslot)
        {
            //LoadRun(saveslot);
            //uint GameSlot = Convert.ToUInt32(saveslot);

            //DoGameSlotCommon(saveslot);
            //GameSaveManager _gsm = new GameSaveManager(hgFileDir);
            //uint _gameSlot = Convert.ToUInt32(saveslot);

            try
            {
                string mf_hgFilePath = hgFilePath;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                //Sets the save to be the last modified
                File.SetLastWriteTime(mf_hgFilePath, DateTime.Now);
                File.SetLastWriteTime(hgFilePath, DateTime.Now);

                //_gs = _gsm.ReadSaveFile(GameSlot);
            }
            catch
            {
                return;
            }

            //RunDecrypt();
        }
        private void EncryptSave(ProgressBar pb, int saveslot)
        {
            //RunEncrypt(pb, saveslot);
            GameSaveManager _gsm = new GameSaveManager(hgFileDir);
            uint _gameSlot = Convert.ToUInt32(saveslot);

            try
            {
                //Read edited saveedit.json
                GameSave _gs = _gsm.ReadUnencryptedGameSave(ufmodSave);

                //Write and Encrypt new save files
                _gsm.WriteSaveFile(_gs, _gameSlot);

                pb.Invoke((System.Action)(() => pb.Value = 90));
            }
            catch
            {
                return;
            }
        }
        */
        private void RunBackupAll(string Path)
        {
            //DoCommon();
            GameSaveManager _gsm = new GameSaveManager(hgFileDir);

            try
            {
                string archivePath = Path; //hgFileDir;

                if (Directory.Exists(Path))
                {
                    //var baseName = string.Format("nmssavetool-backupall-{0}", _gsm.FindMostRecentSaveDateTime().ToString("yyyyMMdd-HHmmss"));
                    var basePath = @".\backup\saves\nms-backup-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                    archivePath = basePath + ".zip";
                    _gsm.ArchiveSaveDirTo(archivePath);
                    Globals.AppendLine(textBox17, "All saves backed up to zip file created in \\backup\\saves folder...");
                }              
            }
            catch
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);                
            }
        }
        /*
        private void DoGameSlotCommon(int saveslot)
        {
            //DoCommon();
            _gsm = new GameSaveManager(hgFileDir);
            _gameSlot = Convert.ToUInt32(saveslot);
        }
        private void DoCommon()
        {
            _gsm = new GameSaveManager(hgFileDir);
        }
        private void LoadRun(int saveslot)
        {
            uint GameSlot = Convert.ToUInt32(saveslot);

            //DoGameSlotCommon(saveslot);
            _gsm = new GameSaveManager(hgFileDir);
            _gameSlot = Convert.ToUInt32(saveslot);

            try
            {
                string mf_hgFilePath = hgFilePath;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                //Sets the save to be the last modified
                File.SetLastWriteTime(mf_hgFilePath, DateTime.Now);
                File.SetLastWriteTime(hgFilePath, DateTime.Now);

                //_gs = _gsm.ReadSaveFile(GameSlot);
            }
            catch
            {
                return;
            }
        }
        private void RunDecrypt()
        {
            //Parsing and formatting save game JSON
            string formattedJson;

            try
            {
                formattedJson = _gs.ToFormattedJsonString(); 
                File.WriteAllText(Save, formattedJson);
            }
            catch
            {
                return;
            }
        }
        private void RunEncrypt(ProgressBar pb, int saveslot)
        {
            //DoGameSlotCommon(saveslot);
            _gsm = new GameSaveManager(hgFileDir);
            _gameSlot = Convert.ToUInt32(saveslot);

            try
            {
                //Read edited saveedit.json
                _gs = _gsm.ReadUnencryptedGameSave(ufmodSave);
            }
            catch
            {
                return;
            }
            try
            {
                //Write and Encrypt new save files
                _gsm.WriteSaveFile(_gs, _gameSlot);
                pb.Invoke((System.Action)(() => pb.Value = 90));                
            }
            catch
            {
                return;
            }
        }
        */
        private void EditSaveFB(ProgressBar pb)
        {
            pb.Visible = true;
            pb.Invoke((System.Action)(() => pb.Value = 5)); //progressBar1.Value = 5));

            string jsonString = json;

            // Convert the JSON string to a JObject:
            JObject jObject = JsonConvert.DeserializeObject(jsonString) as JObject;

            // Select a nested property using a single string:
            JToken TimeLastSpaceBattle = jObject.SelectToken("PlayerStateData.TimeLastSpaceBattle");
            JToken WarpsLastSpaceBattle = jObject.SelectToken("PlayerStateData.WarpsLastSpaceBattle");
            JToken ActiveSpaceBattleUa = jObject.SelectToken("PlayerStateData.ActiveSpaceBattleUA");

            // Update the value of the property: 
            TimeLastSpaceBattle.Replace(0);
            WarpsLastSpaceBattle.Replace(0);
            ActiveSpaceBattleUa.Replace(0);

            // Convert the JObject back to a string:
            string updatedJsonString = jObject.ToString();
            File.WriteAllText(modSave, updatedJsonString);

            pb.Invoke((System.Action)(() => pb.Value = 60));
        }
        private void EditSavePortal(ProgressBar pb)
        {
            pb.Visible = true;
            pb.Invoke((System.Action)(() => pb.Value = 5)); //progressBar1.Value = 5));

            string jsonString = json;

            // Convert the JSON string to a JObject:
            JObject jObject = JsonConvert.DeserializeObject(jsonString) as JObject;

            // Select a nested property using a single string:
            JToken VisitedPortal = jObject.SelectToken("PlayerStateData.VisitedPortal.PortalSeed[0]");
            JToken OnOtherSideOfPortal = jObject.SelectToken("PlayerStateData.OnOtherSideOfPortal");

            // Update the value of the property: 
            VisitedPortal.Replace(false);
            OnOtherSideOfPortal.Replace(false);

            // Convert the JObject back to a string:
            string updatedJsonString = jObject.ToString();
            File.WriteAllText(modSave, updatedJsonString);

            pb.Invoke((System.Action)(() => pb.Value = 60));
        }
        private void EditSaveMove(Destination dest, ProgressBar pb, TextBox tb) //string json)
        {
            pb.Visible = true;
            pb.Invoke((System.Action)(() => pb.Value = 5)); //progressBar1.Value = 5));

            string jsonString = json;

            // Convert the JSON string to a JObject:
            JObject jObject = JsonConvert.DeserializeObject(jsonString) as JObject;

            // Select a nested property using a single string:
            JToken RealityIndex = jObject.SelectToken("PlayerStateData.UniverseAddress.RealityIndex");
            JToken VoxelX = jObject.SelectToken("PlayerStateData.UniverseAddress.GalacticAddress.VoxelX");
            JToken VoxelY = jObject.SelectToken("PlayerStateData.UniverseAddress.GalacticAddress.VoxelY");
            JToken VoxelZ = jObject.SelectToken("PlayerStateData.UniverseAddress.GalacticAddress.VoxelZ");
            JToken SolarSystemIndex = jObject.SelectToken("PlayerStateData.UniverseAddress.GalacticAddress.SolarSystemIndex");
            JToken PlanetIndex = jObject.SelectToken("PlayerStateData.UniverseAddress.GalacticAddress.PlanetIndex");
            JToken HomeRealityIteration = jObject.SelectToken("PlayerStateData.HomeRealityIteration");
            JToken LastKnownPlayerState = jObject.SelectToken("SpawnStateData.LastKnownPlayerState");

            // Update the value of the property: 
            RealityIndex.Replace(dest.iGalaxy);
            VoxelX.Replace(dest.iX);
            VoxelY.Replace(dest.iY);
            VoxelZ.Replace(dest.iZ);
            SolarSystemIndex.Replace(dest.iSSI);
            PlanetIndex.Replace(dest.iPI);
            HomeRealityIteration.Replace(dest.iGalaxy);
            LastKnownPlayerState.Replace("InShip");

            // Convert the JObject back to a string:
            string updatedJsonString = jObject.ToString();
            File.WriteAllText(modSave, updatedJsonString);

            pb.Invoke((System.Action)(() => pb.Value = 60));

            Globals.AppendLine(tb, "Player Move Data: ");
            Globals.AppendLine(tb, dest.Galaxy + " " + dest.X + " " + dest.Y + " " + dest.Z + " " + dest.iSSI + " " + dest.PI + " " + "InShip");
            pb.Invoke((System.Action)(() => pb.Value = 70));
        }
        private void Button15_Click(object sender, EventArgs e)
        {
            //Add Note Button
            if (listBox3.GetItemText(listBox3.SelectedItem) == "")
            {
                MessageBox.Show("No record selected!", "Alert", MessageBoxButtons.OK);
                //Globals.AppendLine(textBox13, "No record selected!");
                return;
            }

            if (textBox28.Text == "")
            {
                MessageBox.Show("No Note added. Please type a note!", "Alert", MessageBoxButtons.OK);
                //Globals.AppendLine(textBox13, "No Note added. Please type a note!");
                return;
            }

            //add a note to a single location record
            string record = listBox3.GetItemText(listBox3.SelectedItem);
            string filename = listBox4.GetItemText(listBox4.SelectedItem);
            int selectedrecord = listBox3.SelectedIndex;
            var selectedfile = listBox4.SelectedItem;

            if (record != "" && filename != "" && locjson != "")
            {
                string path = @".\backup\locations\" + filename;

                if(!File.Exists(path))
                {
                    MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                    LoadTxt();
                    return;
                }

                if (locjson != "")
                {
                    var loc = SavedLocationData.FromJson(locjson);
                    var newnote = textBox28.Text;                    

                    if (loc.Locations.TeleportEndpoints[selectedrecord].Details.Notes != "")
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure want to replace the note? ", "Replace/Change Note", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            loc.Locations.TeleportEndpoints[selectedrecord].Details.Notes = newnote;

                            //removes the last 4 of GC
                            //string E = value[3].Replace(D, "");

                            //removes the note from the record
                            //record = record.Replace(E, "");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            return;
                        }                        
                    }
                    else
                    {
                        loc.Locations.TeleportEndpoints[selectedrecord].Details.Notes = newnote;
                    }

                    var locj = LocationData.Serialize.ToJson(loc);
                    File.WriteAllText(path, locj);
                    
                    LoadTxt();
                    Globals.AppendLine(textBox13, "Note Added.");

                    if (File.Exists(path))
                    {
                        listBox4.SelectedItem = selectedfile;
                        //Button6_Click(this, new EventArgs());

                        listBox3.SelectedIndex = selectedrecord;
                    }                    

                    textBox28.Clear();
                }
            }
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            //Clear Note Button
            if (listBox3.GetItemText(listBox3.SelectedItem) == "")
            {
                MessageBox.Show("No record selected!", "Alert", MessageBoxButtons.OK);
                //Globals.AppendLine(textBox13, "No record selected!");
                return;
            }

            // remove a note to a single location record
            string record = listBox3.GetItemText(listBox3.SelectedItem);
            string filename = listBox4.GetItemText(listBox4.SelectedItem);
            int selectedrecord = listBox3.SelectedIndex;
            var selectedfile = listBox4.SelectedItem;

            if (record != "" && filename != "" && locjson != "")
            {
                string path = @".\backup\locations\" + filename;

                if (!File.Exists(path))
                {
                    MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                    LoadTxt();
                    return;
                }

                if (locjson != "")
                {
                    var loc = SavedLocationData.FromJson(locjson);
                    var newnote = "";

                    DialogResult dialogResult = MessageBox.Show("Are you sure want to remove the note? ", "Remove Note", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loc.Locations.TeleportEndpoints[selectedrecord].Details.Notes = newnote;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }

                    var locj = LocationData.Serialize.ToJson(loc);
                    File.WriteAllText(path, locj);
                    
                    LoadTxt();
                    Globals.AppendLine(textBox13, "Note Removed.");

                    if (File.Exists(path))
                    {
                        listBox4.SelectedItem = selectedfile;
                        //Button6_Click(this, new EventArgs());

                        listBox3.SelectedIndex = selectedrecord;
                    }
                    textBox28.Clear();
                }
            }
        }
        private void Button17_Click(object sender, EventArgs e)
        {
            LoadTxt();
        }
        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateChecker f9 = new UpdateChecker(NMSCVersion);
            f9.ShowDialog();
        }        
    }
}