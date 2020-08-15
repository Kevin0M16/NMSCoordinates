
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuickType;
using Octokit;
using Microsoft.Win32;

/**********************************************************\
|                                                          |
| NMSCoordinates 2019  -- Form1.cs                         |
|                                                          | 
| A fast travel application for No Man's Sky               |
|                                                          |
| Developed by:                                            |
|   Code Author: Kevin Lozano / Kevin0M16                  |
|   Email: <kevin@nmscoordinates.com>                      |
|   Website: https://nmscoordinates.com                    |
|                                                          |
\**********************************************************/


namespace NMSCoordinates
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Set Version here
            NMSCVersion = "1.1.16"; //"v1.1.16";
            label29.Text = "Version " + NMSCVersion;

            Glyphs();
            GIndex();
            GMode();
            JsonKey();

            //Default Paths
            nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            savePath = System.Windows.Forms.Application.CommonAppDataPath + "\\save.nmsc";
            oldsavePath = System.Windows.Forms.Application.CommonAppDataPath + "\\save.txt";
        }

        public int _ScreenWidth { get; private set; }
        public int _ScreenHeight { get; private set; }

        // This method is called when the display settings change.
        private async void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);
            CheckRes();
        }
        private string DistanceToCenter(double x, double y, double z)
        {
            return string.Format("{0:0}", (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)) * 100) * 4) + " ly"; //"{0:0.##}"
        }        
        private void Form1_Load(object sender, EventArgs e)
        {
            //Trigger if Display resolution changes
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);

            //Check resolution
            CheckRes();

            //Save preference file
            BuildSaveFile();
            ReloadSave();

            //Make sure backup dir exists or create it
            CreateBackupDir();

            //loads the saved locbackup and playerloc files
            LoadTxt();

            DiscList = new List<string>();
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
            //CheckForUpdates(true); //Toggle until updater
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
                    AppendLine(textBox17, "This Version: " + NMSCVersion + " Latest Version: " + latestversion); // latest.Name);
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
                    AppendLine(textBox17, "This Version: " + NMSCVersion + " is a pre-release or experimental version, Version " + latestversion + " is the lastest release.");
                }

                if (NMSCVersion == latestversion) //latest.Name)
                {
                    //IsUpdated(true, first, latest.Name);

                    AppendLine(textBox17, "This Version: " + latest.Name + " is the latest version");
                }
            }
            catch
            {
                AppendLine(textBox17, "Github Server not available. Could not check version");
            }
        }
        private async void CheckForUpdates(bool first)
        {
            //Check Github releases for a newer version method
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("NMSCoordinates"));
                var releases = await client.Repository.Release.GetAll("Kevin0M16", "NMSCoordinates");
                var latest = releases[0];

                if (NMSCVersion != latest.Name)
                {
                    IsUpdated(false, first, latest.Name);

                    /*linkLabel4.Text = "Version " + latest.Name + " Available";
                    linkLabel4.Visible = true;
                    AppendLine(textBox17, "Current Version: " + Version + " Latest Version: " + latest.Name);
                    //MessageBox.Show("A newer version of NMSCoordinates is available\r\n\nLatest Version: " + latest.Name + "  Now available for download.\r\n\n", "Update Available", MessageBoxButtons.OK);
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

                if (NMSCVersion == latest.Name)
                {
                    IsUpdated(true, first, latest.Name);
                    //AppendLine(textBox17, "Current Version: " + latest.Name + " is the latest version");
                }
            }
            catch
            {
                AppendLine(textBox17, "Github Server not available. Could not check version");
            }
        }
        private void IsUpdated(bool uptodate, bool first, string latest)
        {
            if (uptodate && first)
            {
                AppendLine(textBox17, "Current Version: " + latest + " is the latest version");
            }

            if (uptodate && !first)
            {
                MessageBox.Show("You have the latest version of NMSCoordinates\r\n\n" +
                    "Latest Version: " + latest + 
                    "  No Update Available\r\n\n", 
                    "Update", 
                    MessageBoxButtons.OK);
            }                      

            if (!uptodate)
            {
                linkLabel4.Text = "Version " + latest + " Available";
                linkLabel4.Visible = true;
                AppendLine(textBox17, 
                    "Current Version: " + NMSCVersion + 
                    " Latest Version: " + latest +
                    " ***Newer Version Available***"
                    );

                DialogResult dialogResult = MessageBox.Show(
                    "A newer version of NMSCoordinates is available\r\n\n" +
                    "Latest Version: " + latest + "\r\n\n" +
                    "Update Now?\r\n\n", 
                    "Update Available", 
                    MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    UpdateApp();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }

        public Form8 f8;

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
                        f8 = new Form8(SaveDirs);
                        f8.ShowDialog();

                        //After the path to nmsPath is selected on form8, set nmsPath
                        nmsPath = f8.GoGPath;

                        if (Directory.Exists(nmsPath))
                        {
                            WriteTxt("nmsPath", nmsPath, savePath);
                            return;
                        }
                    }
                    else if (SaveDirs.Count() == 1)
                    {
                        WriteTxt("nmsPath", nmsPath, savePath);
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
                        WriteTxt("nmsPath", nmsPath, savePath);
                        //AppendLine(textBox17, "Set Dir: " + nmsPath);
                        return;
                    }

                    if (dinfo.GetDirectories("st_*", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check for Steam Folder, if found set nmsPath
                        DirectoryInfo[] dirname1 = dinfo.GetDirectories("st_*", SearchOption.TopDirectoryOnly);
                        nmsPath = dirname1[0].FullName;
                        WriteTxt("nmsPath", nmsPath, savePath);
                        //AppendLine(textBox17, "Set Dir: " + nmsPath);
                        return;
                    }

                    if (dinfo.GetDirectories("DefaultUser", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check for GoG Folder, if found set nmsPath
                        DirectoryInfo[] dirname2 = dinfo.GetDirectories("DefaultUser", SearchOption.TopDirectoryOnly);
                        nmsPath = dirname2[0].FullName;
                        WriteTxt("nmsPath", nmsPath, savePath);
                        //AppendLine(textBox17, "Set Dir: " + nmsPath);
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
                }

                sl1.Add(0, "(Select Save Slot)");
                comboBox2.DataSource = sl1.ToArray();
                comboBox2.DisplayMember = "VALUE";
                comboBox2.ValueMember = "KEY";

                hgFileDir = Path.GetDirectoryName(Files[0].FullName);
                fileSystemWatcher1.Path = hgFileDir;

                textBox16.Clear();
                AppendLine(textBox16, hgFileDir);
            }
            else
            {
                AppendLine(textBox16, "No save files found!");
                return;
            }
        }
        private void ComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selected = this.comboBox2.GetItemText(this.comboBox2.SelectedItem);

            //Gets the dictionaries set in loadcmbbx and sets the data source for save dropdown
            if (selected == "Slot 1")
            {
                saveslot = 1;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn1.ToArray();
                return;
            }
            if (selected == "Slot 2")
            {
                saveslot = 2;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn2.ToArray();
                return;
            }
            if (selected == "Slot 3")
            {
                saveslot = 3;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn3.ToArray();
                return;
            }
            if (selected == "Slot 4")
            {
                saveslot = 4;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn4.ToArray();
                return;
            }
            if (selected == "Slot 5")
            {
                saveslot = 5;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn5.ToArray();
                return;
            }
            if (selected == "(Select Save Slot)")
            {
                saveslot = -1;
                comboBox1.DataSource = null;
                ClearAll();
                LoadCmbx(); //insert here?
                return;
            }
        }        
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //After selecting a saveslot, this triggers + after selecting a different save   
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "")
            {
                ClearAll();
                GetSaveFile(selected);
                Loadlsb1();
                LoadBaselsbx();
                GetPlayerCoord();
            }            
        }
        private void GetSaveFile(string selected)
        {
            //Main save file loader
            if (Directory.Exists(hgFileDir) && selected != "")
            {
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
                    AppendLine(textBox17, "** Code 3 ** " + selected);
                    return;
                }

                //shows the file path in the path textbox
                textBox16.Clear();
                AppendLine(textBox16, hgFilePath);

                //displays the last write time
                FileInfo hgfile = new FileInfo(hgFilePath);
                textBox26.Clear();
                AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                //Sets json from the selected save file
                json = File.ReadAllText(hgFilePath);

                //looksup and then displays the game mode
                var nms = Nms.FromJson(json);
                //gamemode = nms.F2P.ToString(); // removed v1.1.13
                //GameModeLookup(label28, gamemode);
                try
                {
                    gamemodeint = Convert.ToInt32(nms.F2P);
                    GameModeLookupInt(gamemodeint);
                }
                catch
                {
                    return;
                }
            }
        }
        private void GameModeLookupInt(int mode)
        {
            //Looks up game mode in ranges to prevent "not found"
            try
            {
                if (mode > 4600 & mode < 4700)
                {
                    label28.Text = "Normal";
                }
                if (mode > 5600 & mode < 5700)
                {
                    label28.Text = "Survival";
                }
                if (mode > 6600 & mode < 6700)
                {
                    label28.Text = "Permadeath";
                }
                if (mode > 5100 & mode < 5200)
                {
                    label28.Text = "Creative";
                }
            }
            catch
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
                AppendLine(textBox17, "Current Stored locations saved.");
                MessageBox.Show("Current Stored locations saved.", "Confirmation");
            }
            else
            {
                return;
            }
        }
        private void CheckSS()
        {
            //Take the list of current discoveries and add them to SSList for comparison to PrevSSlist
            if (DiscList.Count > 0)
            {
                // Set Minimum to 1 to represent the first file being copied.
                progressBar2.Minimum = 1;
                // Set Maximum to the total number of files to copy.
                progressBar2.Maximum = DiscList.Count;
                // Set the initial value of the ProgressBar.
                progressBar2.Value = 1;
                // Set the Step property to a value of 1 to represent each file being copied.
                progressBar2.Step = 1;
                // Display the ProgressBar control.
                progressBar2.Visible = true;

                for (int i = 0; i < DiscList.Count; i++)
                {
                    JsonMap(i);
                    GetPortalCoord(iX, iY, iZ, iSSI);
                    GetGalacticCoord(iX, iY, iZ, iSSI);
                    SSlist.Add("Slot_" + saveslot + "_Loc: " + DiscList[i] + " - G: " + galaxy + " - PC: " + PortalCode + " -- GC: " + GalacticCoord);

                    progressBar2.PerformStep();

                    //if (SSlist.Count == i + 1)
                    //{
                    // Perform the increment on the ProgressBar.
                    //   progressBar2.PerformStep();
                    //}
                }
                progressBar2.Visible = false;
            }
            else
            {
                return;
            }
        }
        private async Task BackupLoc(string path)
        {
            //Backup all locations to a new locbackup file
            if (DiscList.Count > 0)
            {
                tabControl1.SelectedTab = tabPage1;
                await Task.Delay(300);
                // Set Minimum to 1 to represent the first file being copied.
                progressBar2.Minimum = 1;
                // Set Maximum to the total number of files to copy.
                progressBar2.Maximum = DiscList.Count;
                // Set the initial value of the ProgressBar.
                progressBar2.Value = 1;
                // Set the Step property to a value of 1 to represent each file being copied.
                progressBar2.Step = 1;
                // Display the ProgressBar control.
                progressBar2.Visible = true;

                for (int i = 0; i < DiscList.Count; i++)
                {
                    JsonMap(i);
                    GetPortalCoord(iX, iY, iZ, iSSI);
                    GetGalacticCoord(iX, iY, iZ, iSSI);
                    Backuplist.Add("Slot_" + saveslot + "_Loc: " + DiscList[i] + " - G: " + galaxy + " - PC: " + PortalCode + " -- GC: " + GalacticCoord);

                    progressBar2.PerformStep();

                    //if (Backuplist.Count == i + 1)
                    //{
                    // Perform the increment on the ProgressBar.
                    //    progressBar2.PerformStep();
                    //}
                }
                progressBar2.Visible = false;

                //Make a unique path name for the locbackup file and create file
                string path2 = MakeUnique(path).ToString();
                File.WriteAllLines(path2, Backuplist);
                MessageBox.Show("Locations Backed up to .txt \n\n\r Open in Coordinate Share Tab", "Confirmation", MessageBoxButtons.OK);
                LoadTxt();

                tabControl1.SelectedTab = tabPage3;
                if (File.Exists(path2))
                {                    
                    listBox4.SelectedItem = Path.GetFileName(path2);
                    Button6_Click(this, new EventArgs());
                }                
            }
            else
            {
                toolStripMenuItem1.Enabled = false;
                MessageBox.Show("No Locations found! ", "Message");
            }
        }
        public FileInfo MakeUnique(string path)
        {
            //Makes path in \backup unique by date.time
            path = String.Format("{0}{1}{2}{3}{4}", @".\backup\", Path.GetFileNameWithoutExtension(path), "_" + saveslot + "_", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"), Path.GetExtension(path));
            return new FileInfo(path);

        }
        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        private void GetPlayerCoord()
        {
            //Gets the player position off the save file and prints the info on tab1
            textBox22.Clear();
            textBox21.Clear();
            textBox23.Clear();
            textBox29.Clear();

            var nms = Nms.FromJson(json);

            pgalaxy = nms.The6F.YhJ.Iis.ToString();//.ToString();
            var pX = nms.The6F.YhJ.OZw["dZj"];//.ToString();
            var pY = nms.The6F.YhJ.OZw["IyE"];//.ToString();
            var pZ = nms.The6F.YhJ.OZw["uXE"];//.ToString();
            var pSSI = nms.The6F.YhJ.OZw["vby"];//.ToString();

            GetGalacticCoord(Convert.ToInt32(pX), Convert.ToInt32(pY), Convert.ToInt32(pZ), Convert.ToInt32(pSSI));
            AppendLine(textBox22, GalacticCoord);
            GetPortalCoord(Convert.ToInt32(pX), Convert.ToInt32(pY), Convert.ToInt32(pZ), Convert.ToInt32(pSSI));
            ShowPGlyphs();
            AppendLine(textBox21, PortalCode);
            GalaxyLookup(textBox23, pgalaxy);
            AppendLine(textBox29, DistanceToCenter(pX, pY, pZ));
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

            DiscList.Clear();
            BaseList.Clear();
            Backuplist.Clear();
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            SaveDirs.Clear();

            //json = ""; Insert here 1.1.8??

            comboBox3.Items.Clear();
            pgalaxy = "";
            label35.Text = "Galaxy Name";
            label36.Text = "TYPE";

            galaxy = "";
            X = "";
            Y = "";
            Z = "";
            SSI = "";
            PI = "";

            PortalCode = "";
            GalacticCoord = "";
            GalacticCoord2 = "";

            textBox16.Text = nmsPath;
        }
        private void TextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();

            textBox4.Text = galaxy;
            textBox5.Text = X;
            textBox6.Text = Y;
            textBox7.Text = Z;
            textBox8.Text = SSI;
            textBox9.Text = PI;
        }
        private void JsonMap(int i)
        {
            //lookup info from the Json hg file
            try
            {
                var nms = Nms.FromJson(json);
                galaxy = nms.The6F.NlG[i].YhJ.Iis.ToString();
                X = nms.The6F.NlG[i].YhJ.OZw["dZj"].ToString();
                Y = nms.The6F.NlG[i].YhJ.OZw["IyE"].ToString();
                Z = nms.The6F.NlG[i].YhJ.OZw["uXE"].ToString();
                SSI = nms.The6F.NlG[i].YhJ.OZw["vby"].ToString();
                PI = nms.The6F.NlG[i].YhJ.OZw["jsv"].ToString();

                igalaxy = Convert.ToInt32(nms.The6F.NlG[i].YhJ.Iis);
                iX = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["dZj"]);
                iY = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["IyE"]);
                iZ = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["uXE"]);
                iSSI = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["vby"]);
                iPI = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["jsv"]);
            }
            catch
            {
                AppendLine(textBox17, "** Code 2 **");
                return;
            }
        }
        private void Glyphs()
        {
            //Set the dictionary to find glyphs
            glyphDict = new Dictionary<char, Bitmap>();
            glyphDict.Add('0', Properties.Resources._0);
            glyphDict.Add('1', Properties.Resources._1);
            glyphDict.Add('2', Properties.Resources._2);
            glyphDict.Add('3', Properties.Resources._3);
            glyphDict.Add('4', Properties.Resources._4);
            glyphDict.Add('5', Properties.Resources._5);
            glyphDict.Add('6', Properties.Resources._6);
            glyphDict.Add('7', Properties.Resources._7);
            glyphDict.Add('8', Properties.Resources._8);
            glyphDict.Add('9', Properties.Resources._9);
            glyphDict.Add('A', Properties.Resources.A);
            glyphDict.Add('B', Properties.Resources.B);
            glyphDict.Add('C', Properties.Resources.C);
            glyphDict.Add('D', Properties.Resources.D);
            glyphDict.Add('E', Properties.Resources.E);
            glyphDict.Add('F', Properties.Resources.F);
        }
        private void GMode()
        {
            //Set the dictionary for game modes
            gameMode = new Dictionary<string, string>();
            gameMode.Add(new KeyValuePair<string, string>("4631", "Normal"));
            gameMode.Add(new KeyValuePair<string, string>("5655", "Survival"));
            gameMode.Add(new KeyValuePair<string, string>("6679", "Permadeath"));
            gameMode.Add(new KeyValuePair<string, string>("5143", "Creative"));

            gameMode.Add(new KeyValuePair<string, string>("4628", "Normal"));
            gameMode.Add(new KeyValuePair<string, string>("5652", "Survival"));
            gameMode.Add(new KeyValuePair<string, string>("6676", "Permadeath"));
            //gameMode.Add(new KeyValuePair<string, string>("", "Creative"));

            gameMode.Add(new KeyValuePair<string, string>("4634", "Normal"));
            gameMode.Add(new KeyValuePair<string, string>("5658", "Survival"));
            gameMode.Add(new KeyValuePair<string, string>("6682", "Permadeath"));
            gameMode.Add(new KeyValuePair<string, string>("5146", "Creative"));
        }
        private void GIndex()
        {
            //Main dictionary for galaxies
            galaxyDict = new Dictionary<string, string>();
            galaxyDict.Add(new KeyValuePair<string, string>("0", "Euclid"));
            galaxyDict.Add(new KeyValuePair<string, string>("1", "Hilbert"));
            galaxyDict.Add(new KeyValuePair<string, string>("2", "Calypso"));
            galaxyDict.Add(new KeyValuePair<string, string>("3", "Hesperius"));
            galaxyDict.Add(new KeyValuePair<string, string>("4", "Hyades"));
            galaxyDict.Add(new KeyValuePair<string, string>("5", "Ickjamatew"));
            galaxyDict.Add(new KeyValuePair<string, string>("6", "Bullangr"));
            galaxyDict.Add(new KeyValuePair<string, string>("7", "Kikolgallr"));
            galaxyDict.Add(new KeyValuePair<string, string>("8", "Eltiensleem"));
            galaxyDict.Add(new KeyValuePair<string, string>("9", "Eissentam"));
            galaxyDict.Add(new KeyValuePair<string, string>("10", "Elkupalos"));
            galaxyDict.Add(new KeyValuePair<string, string>("11", "Aptarkaba"));
            galaxyDict.Add(new KeyValuePair<string, string>("12", "Ontiniangp"));
            galaxyDict.Add(new KeyValuePair<string, string>("13", "Odiwagiri"));
            galaxyDict.Add(new KeyValuePair<string, string>("14", "Ogtialabi"));
            galaxyDict.Add(new KeyValuePair<string, string>("15", "Muhacksonto"));
            galaxyDict.Add(new KeyValuePair<string, string>("16", "Hitonskyer"));
            galaxyDict.Add(new KeyValuePair<string, string>("17", "Rerasmutul"));
            galaxyDict.Add(new KeyValuePair<string, string>("18", "Isdoraijung"));
            galaxyDict.Add(new KeyValuePair<string, string>("19", "Doctinawyra"));
            galaxyDict.Add(new KeyValuePair<string, string>("20", "Loychazinq"));
            galaxyDict.Add(new KeyValuePair<string, string>("21", "Zukasizawa"));
            galaxyDict.Add(new KeyValuePair<string, string>("22", "Ekwathore"));
            galaxyDict.Add(new KeyValuePair<string, string>("23", "Yeberhahne"));
            galaxyDict.Add(new KeyValuePair<string, string>("24", "Twerbetek"));
            galaxyDict.Add(new KeyValuePair<string, string>("25", "Sivarates"));
            galaxyDict.Add(new KeyValuePair<string, string>("26", "Eajerandal"));
            galaxyDict.Add(new KeyValuePair<string, string>("27", "Aldukesci"));
            galaxyDict.Add(new KeyValuePair<string, string>("28", "Wotyarogii"));
            galaxyDict.Add(new KeyValuePair<string, string>("29", "Sudzerbal"));
            galaxyDict.Add(new KeyValuePair<string, string>("30", "Maupenzhay"));
            galaxyDict.Add(new KeyValuePair<string, string>("31", "Sugueziume"));
            galaxyDict.Add(new KeyValuePair<string, string>("32", "Brogoweldian"));
            galaxyDict.Add(new KeyValuePair<string, string>("33", "Ehbogdenbu"));
            galaxyDict.Add(new KeyValuePair<string, string>("34", "Ijsenufryos"));
            galaxyDict.Add(new KeyValuePair<string, string>("35", "Nipikulha"));
            galaxyDict.Add(new KeyValuePair<string, string>("36", "Autsurabin"));
            galaxyDict.Add(new KeyValuePair<string, string>("37", "Lusontrygiamh"));
            galaxyDict.Add(new KeyValuePair<string, string>("38", "Rewmanawa"));
            galaxyDict.Add(new KeyValuePair<string, string>("39", "Ethiophodhe"));
            galaxyDict.Add(new KeyValuePair<string, string>("40", "Urastrykle"));
            galaxyDict.Add(new KeyValuePair<string, string>("41", "Xobeurindj"));
            galaxyDict.Add(new KeyValuePair<string, string>("42", "Oniijialdu"));
            galaxyDict.Add(new KeyValuePair<string, string>("43", "Wucetosucc"));
            galaxyDict.Add(new KeyValuePair<string, string>("44", "Ebyeloof"));
            galaxyDict.Add(new KeyValuePair<string, string>("45", "Odyavanta"));
            galaxyDict.Add(new KeyValuePair<string, string>("46", "Milekistri"));
            galaxyDict.Add(new KeyValuePair<string, string>("47", "Waferganh"));
            galaxyDict.Add(new KeyValuePair<string, string>("48", "Agnusopwit"));
            galaxyDict.Add(new KeyValuePair<string, string>("49", "Teyaypilny"));
            galaxyDict.Add(new KeyValuePair<string, string>("50", "Zalienkosm"));
            galaxyDict.Add(new KeyValuePair<string, string>("51", "Ladgudiraf"));
            galaxyDict.Add(new KeyValuePair<string, string>("52", "Mushonponte"));
            galaxyDict.Add(new KeyValuePair<string, string>("53", "Amsentisz"));
            galaxyDict.Add(new KeyValuePair<string, string>("54", "Fladiselm"));
            galaxyDict.Add(new KeyValuePair<string, string>("55", "Laanawemb"));
            galaxyDict.Add(new KeyValuePair<string, string>("56", "Ilkerloor"));
            galaxyDict.Add(new KeyValuePair<string, string>("57", "Davanossi"));
            galaxyDict.Add(new KeyValuePair<string, string>("58", "Ploehrliou"));
            galaxyDict.Add(new KeyValuePair<string, string>("59", "Corpinyaya"));
            galaxyDict.Add(new KeyValuePair<string, string>("60", "Leckandmeram"));
            galaxyDict.Add(new KeyValuePair<string, string>("61", "Quulngais"));
            galaxyDict.Add(new KeyValuePair<string, string>("62", "Nokokipsechl"));
            galaxyDict.Add(new KeyValuePair<string, string>("63", "Rinblodesa"));
            galaxyDict.Add(new KeyValuePair<string, string>("64", "Loydporpen"));
            galaxyDict.Add(new KeyValuePair<string, string>("65", "Ibtrevskip"));
            galaxyDict.Add(new KeyValuePair<string, string>("66", "Elkowaldb"));
            galaxyDict.Add(new KeyValuePair<string, string>("67", "Heholhofsko"));
            galaxyDict.Add(new KeyValuePair<string, string>("68", "Yebrilowisod"));
            galaxyDict.Add(new KeyValuePair<string, string>("69", "Husalvangewi"));
            galaxyDict.Add(new KeyValuePair<string, string>("70", "Ovna'uesed"));
            galaxyDict.Add(new KeyValuePair<string, string>("71", "Bahibusey"));
            galaxyDict.Add(new KeyValuePair<string, string>("72", "Nuybeliaure"));
            galaxyDict.Add(new KeyValuePair<string, string>("73", "Doshawchuc"));
            galaxyDict.Add(new KeyValuePair<string, string>("74", "Ruckinarkh"));
            galaxyDict.Add(new KeyValuePair<string, string>("75", "Thorettac"));
            galaxyDict.Add(new KeyValuePair<string, string>("76", "Nuponoparau"));
            galaxyDict.Add(new KeyValuePair<string, string>("77", "Moglaschil"));
            galaxyDict.Add(new KeyValuePair<string, string>("78", "Uiweupose"));
            galaxyDict.Add(new KeyValuePair<string, string>("79", "Nasmilete"));
            galaxyDict.Add(new KeyValuePair<string, string>("80", "Ekdaluskin"));
            galaxyDict.Add(new KeyValuePair<string, string>("81", "Hakapanasy"));
            galaxyDict.Add(new KeyValuePair<string, string>("82", "Dimonimba"));
            galaxyDict.Add(new KeyValuePair<string, string>("83", "Cajaccari"));
            galaxyDict.Add(new KeyValuePair<string, string>("84", "Olonerovo"));
            galaxyDict.Add(new KeyValuePair<string, string>("85", "Umlanswick"));
            galaxyDict.Add(new KeyValuePair<string, string>("86", "Henayliszm"));
            galaxyDict.Add(new KeyValuePair<string, string>("87", "Utzenmate"));
            galaxyDict.Add(new KeyValuePair<string, string>("88", "Umirpaiya"));
            galaxyDict.Add(new KeyValuePair<string, string>("89", "Paholiang"));
            galaxyDict.Add(new KeyValuePair<string, string>("90", "Iaereznika"));
            galaxyDict.Add(new KeyValuePair<string, string>("91", "Yudukagath"));
            galaxyDict.Add(new KeyValuePair<string, string>("92", "Boealalosnj"));
            galaxyDict.Add(new KeyValuePair<string, string>("93", "Yaevarcko"));
            galaxyDict.Add(new KeyValuePair<string, string>("94", "Coellosipp"));
            galaxyDict.Add(new KeyValuePair<string, string>("95", "Wayndohalou"));
            galaxyDict.Add(new KeyValuePair<string, string>("96", "Smoduraykl"));
            galaxyDict.Add(new KeyValuePair<string, string>("97", "Apmaneessu"));
            galaxyDict.Add(new KeyValuePair<string, string>("98", "Hicanpaav"));
            galaxyDict.Add(new KeyValuePair<string, string>("99", "Akvasanta"));
            galaxyDict.Add(new KeyValuePair<string, string>("100", "Tuychelisaor"));
            galaxyDict.Add(new KeyValuePair<string, string>("109", "Nudquathsenfe"));
            galaxyDict.Add(new KeyValuePair<string, string>("118", "Torweierf"));
            galaxyDict.Add(new KeyValuePair<string, string>("129", "Broomerrai"));
            galaxyDict.Add(new KeyValuePair<string, string>("138", "Emiekereks"));
            galaxyDict.Add(new KeyValuePair<string, string>("140", "Kimycuristh"));
            galaxyDict.Add(new KeyValuePair<string, string>("149", "Zavainlani"));
            galaxyDict.Add(new KeyValuePair<string, string>("158", "Rycempler"));
            galaxyDict.Add(new KeyValuePair<string, string>("169", "Ezdaranit"));
            galaxyDict.Add(new KeyValuePair<string, string>("178", "Wepaitvas"));
            galaxyDict.Add(new KeyValuePair<string, string>("189", "Cugnatachh"));
            galaxyDict.Add(new KeyValuePair<string, string>("198", "Horeroedsh"));
            galaxyDict.Add(new KeyValuePair<string, string>("209", "Digarlewena"));
            galaxyDict.Add(new KeyValuePair<string, string>("218", "Chmageaki"));
            galaxyDict.Add(new KeyValuePair<string, string>("229", "Raldwicarn"));
            galaxyDict.Add(new KeyValuePair<string, string>("238", "Yuwarugha"));
            galaxyDict.Add(new KeyValuePair<string, string>("249", "Nepitzaspru"));
            galaxyDict.Add(new KeyValuePair<string, string>("254", "Iousongola"));
            galaxyDict.Add(new KeyValuePair<string, string>("255", "Odyalutai"));
            galaxyDict.Add(new KeyValuePair<string, string>("256", "Yilsrussimil"));

            //galaxyDict.Add(new KeyValuePair<string, string>("-1", "Pequibanu"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-2", "Uewamoisow"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-3", "Hiteshamij"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-4", "Usgraikik"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-5", "Helqvishap"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-6", "Enyokudohkiw"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-7", "Loqvishess"));

            //galaxyDict.Add(new KeyValuePair<string, string>("", ""));


        }
        private void Loadlsb1()
        {
            //Method to load all location discovered in listbox1
            DiscList.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            TextBoxes();

            var nms = Nms.FromJson(json);
            try
            {
                for (int i = 0; i < nms.The6F.NlG.Length; i++)
                {
                    string discd = nms.The6F.NlG[i].NKm;

                    if (nms.The6F.NlG[i].IAf == "Spacestation" || nms.The6F.NlG[i].IAf == "SpacestationFixPosition") // 1.1.16
                    {
                        string ss = discd + " (SS)";
                        DiscList.Add(ss);
                        listBox2.Items.Add(ss);
                    }
                    if (nms.The6F.NlG[i].IAf == "Base") // v1.1.16
                    {
                        string bl = discd + " (B)";
                        DiscList.Add(bl);
                        listBox1.Items.Add(bl);
                    }
                    if (nms.The6F.NlG[i].IAf == "ExternalBase") // v1.1.16
                    {
                        string bl = discd + " (EB)";
                        DiscList.Add(bl);
                        listBox1.Items.Add(bl);
                    }
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 111 **");
                return;
            }
            
            //listBox1.DataSource = DiscList; //Removed v1.1.11
            textBox19.Text = listBox1.Items.Count.ToString();
            textBox20.Text = listBox2.Items.Count.ToString();
            listBox1.SelectedIndex = -1;

            if (nms.The6F.DaC == true)
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
        private void LoadBaselsbx()
        {
            //Future use to add Persistent Bases to a Listbox
            var nms = Nms.FromJson(json);
            try
            {
                for (int i = 0; i < nms.The6F.F0.Length; i++)
                {
                    string baseN = nms.The6F.F0[i].NKm;
                    if (baseN != "")
                    {
                        BaseList.Add(baseN);
                    }
                    else
                    {
                        BaseList.Add(baseN + "Not Named");
                    }
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 11 **");
                return;
            }
        }
        private void GameModeLookup(System.Windows.Forms.Label source, string mode)
        {
            //lookup game modes, if not in gameMode dict, display the number
            try
            {
                source.Text = gameMode[mode];
            }
            catch
            {
                source.Text = mode;
                AppendLine(textBox17, "Game mode Not Found, update needed.");
            }
        }
        private void GalaxyLookupLbl(System.Windows.Forms.Label source, string galaxy)
        {
            //lookup the galaxy and if not in galaxy dict, display the number
            try
            {
                string value;
                if (galaxyDict.TryGetValue(galaxy, out value))
                {
                    source.Text = value;
                }
                else
                {
                   source.Text = "";
                }
                //source.Text = galaxyDict[galaxy];                
            }
            catch
            {
                //source.Text = "";//(Convert.ToInt32(galaxy) + 1).ToString();
                AppendLine(textBox15, "Galaxy Not Found, update needed.");
            }
        }
        private void GalaxyLookup(TextBox source, string galaxy)
        {
            //lookup the galaxy and if not in galaxy dict, display the number
            try
            {
                string value;
                if (galaxyDict.TryGetValue(galaxy, out value))
                {
                    source.Text = value;
                }
                else
                {
                    source.Text = (Convert.ToInt32(galaxy) + 1).ToString();
                }
                //source.Text = galaxyDict[galaxy];
            }
            catch
            {
              
                //source.Text = (Convert.ToInt32(galaxy) + 1).ToString();
                AppendLine(textBox17, "Galaxy Not Found, update needed.");
            }
        }
        private void lsb1mclick() //backup of changes made on v1.1.11
        {
            //When a location is clicked on listbox1, get all the info
            listBox2.SelectedIndex = -1;
            try
            {
                if (listBox1.GetItemText(listBox1.SelectedItem) != "")
                {
                    int i = listBox1.SelectedIndex;
                    JsonMap(i);
                    TextBoxes();
                    //textBox10.Text = galaxyDict[galaxy];
                    GalaxyLookup(textBox10, galaxy);
                    textBox30.Text = DistanceToCenter(iX, iY, iZ);
                    GetGalacticCoord(iX, iY, iZ, iSSI);
                    GetPortalCoord(iPI, iX, iY, iZ, iSSI, textBox3);
                    ShowGlyphs();
                    AppendLine(textBox1, GalacticCoord);
                    AppendLine(textBox2, PortalCode);
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 1 **");
                return;
            }
        }
        private void ListBox1_MouseClick(object sender, EventArgs e)
        {
            //When a location is clicked on listbox1, get all the info
            listBox2.SelectedIndex = -1;
            try
            {
                if (listBox1.GetItemText(listBox1.SelectedItem) != "")
                {
                    object selecteditem = listBox1.SelectedItem;
                    string si = selecteditem.ToString();
                    si = si.Replace(" (B)", "");
                    si = si.Replace(" (EB)", ""); //v1.1.16
                    var nms = Nms.FromJson(json);
                    try
                    {
                        for (int i = 0; i < nms.The6F.NlG.Length; i++)
                        {
                            if (nms.The6F.NlG[i].NKm.ToString() == si)
                            {
                                JsonMap(i);
                                TextBoxes();
                                GalaxyLookup(textBox10, galaxy);
                                textBox30.Text = DistanceToCenter(iX, iY, iZ);
                                GetGalacticCoord(iX, iY, iZ, iSSI);
                                GetPortalCoord(iPI, iX, iY, iZ, iSSI, textBox3);
                                ShowGlyphs();
                                AppendLine(textBox1, GalacticCoord);
                                AppendLine(textBox2, PortalCode);
                            }
                        }
                    }
                    catch
                    {
                        AppendLine(textBox17, "** Code 51l1 **");
                        return;
                    }
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 5l1 **");
                return;
            }
        }
        private void ListBox2_MouseClick(object sender, EventArgs e)
        {
            //When a location is clicked on listbox2, get all the info
            listBox1.SelectedIndex = -1;
            try
            {
                if (listBox2.GetItemText(listBox2.SelectedItem) != "")
                {
                    object selecteditem = listBox2.SelectedItem;
                    string si = selecteditem.ToString();
                    si = si.Replace(" (SS)", "");
                    var nms = Nms.FromJson(json);
                    try
                    {
                        for (int i = 0; i < nms.The6F.NlG.Length; i++)
                        {
                            if (nms.The6F.NlG[i].NKm.ToString() == si)
                            {
                                JsonMap(i);
                                TextBoxes();
                                GalaxyLookup(textBox10, galaxy);
                                textBox30.Text = DistanceToCenter(iX, iY, iZ);
                                GetGalacticCoord(iX, iY, iZ, iSSI);
                                GetPortalCoord(iPI, iX, iY, iZ, iSSI, textBox3);
                                ShowGlyphs();
                                AppendLine(textBox1, GalacticCoord);
                                AppendLine(textBox2, PortalCode);
                            }
                        }
                    }
                    catch
                    {
                        AppendLine(textBox17, "** Code 51l2 **");
                        return;
                    }
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 5l2 **");
                return;
            }
        }
        private void GetGalacticCoord(int X, int Y, int Z, int SSI)
        {
            //Voxel Coordinate to Galactic Coordinate
            textBox3.Clear();

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            AppendLine(textBox3, "DEC: " + X + " " + Y + " " + Z);

            int dd1 = X + 2047;
            int dd2 = Y + 127;
            int dd3 = Z + 2047;
            AppendLine(textBox3, "SHIFT: " + dd1 + " " + dd2 + " " + dd3);

            string g1 = dd1.ToString("X");
            string g2 = dd2.ToString("X");
            string g3 = dd3.ToString("X");
            string g4 = SSI.ToString("X");
            AppendLine(textBox3, "Galactic HEX numbers: " + g1 + " " + g2 + " " + g3 + " " + g4);

            int ig1 = Convert.ToInt32(g1, 16); // X[HEX] to X[DEC]
            int ig2 = Convert.ToInt32(g2, 16); // Y[HEX] to Y[DEC]
            int ig3 = Convert.ToInt32(g3, 16); // Z[HEX] to Z[DEC]
            int ig4 = Convert.ToInt32(g4, 16); // SSI[HEX] to SSI[DEC]
            AppendLine(textBox3, "Galactic DEC numbers: " + ig1 + " " + ig2 + " " + ig3 + " " + ig4);

            GalacticCoord = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ig1, ig2, ig3, ig4); //Format to 4 digit seperated by colon
            AppendLine(textBox3, "Galactic Coordinates: " + GalacticCoord);
        }
        private void GetPortalCoord(int P, int X, int Y, int Z, int SSI, TextBox tb)
        {
            //Voxel Coordinate to Portal Code

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            int dd1 = X + 2047;
            int dd2 = Y + 127;
            int dd3 = Z + 2047;

            string g1 = dd1.ToString("X");
            string g2 = dd2.ToString("X");
            string g3 = dd3.ToString("X");
            string g4 = SSI.ToString("X");

            int dec1 = Convert.ToInt32(g1, 16); // X[HEX] to X[DEC]
            int dec2 = Convert.ToInt32(g2, 16); // Y[HEX] to X[DEC]
            int dec3 = Convert.ToInt32(g3, 16); // Z[HEX] to X[DEC]
            int dec4 = Convert.ToInt32(g4, 16); // SSI[HEX] to SSI[DEC]
            AppendLine(tb, "Galactic HEX to DEC: " + dec1.ToString() + " " + dec2.ToString() + " " + dec3.ToString() + " " + dec4);

            int dec5 = Convert.ToInt32("801", 16); // 801[HEX] to 801[DEC]
            int dec6 = Convert.ToInt32("81", 16); // 81[HEX] to 81[DEC]
            int dec7 = Convert.ToInt32("1000", 16); // 100[HEX] to 1000[DEC]
            int dec8 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            AppendLine(tb, "Shift HEX to DEC: " + "801:" + dec5.ToString() + " 81:" + dec6.ToString() + " 1000:" + dec7.ToString() + " 100:" + dec8.ToString());

            int calc1 = (dec1 + dec5) % dec7; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (dec2 + dec6) % dec8; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (dec3 + dec5) % dec7; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            AppendLine(tb, "Calculate Portal DEC: " + "X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + dec4);

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            AppendLine(tb, "Portal HEX numbers: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + g4);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFF); // Z[HEX] to Z[DEC] 3 digits
            int ihexSSI = (Convert.ToInt32(g4, 16) & 0xFFF); // SSI[HEX] to SSI[DEC] 3 digits

            PortalCode = string.Format(P + "{0:X3}{1:X2}{2:X3}{3:X3}", ihexSSI, ihexY, ihexZ, ihexX); // Format digits 1 3 2 3 3
            AppendLine(tb, "[P][SSI][Y][Z][X] Portal Code: " + PortalCode);

            //Index chars in PortalCode
            _gl1 = PortalCode[0];
            _gl2 = PortalCode[1];
            _gl3 = PortalCode[2];
            _gl4 = PortalCode[3];
            _gl5 = PortalCode[4];
            _gl6 = PortalCode[5];
            _gl7 = PortalCode[6];
            _gl8 = PortalCode[7];
            _gl9 = PortalCode[8];
            _gl10 = PortalCode[9];
            _gl11 = PortalCode[10];
            _gl12 = PortalCode[11];

            //Display Glyph images
            //ShowGlyphs();
        }
        private void GetPortalCoord(int X, int Y, int Z, int SSI)
        {
            //Voxel Coordinates to Portal Code

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            int dd1 = X + 2047;
            int dd2 = Y + 127;
            int dd3 = Z + 2047;

            string g1 = dd1.ToString("X");
            string g2 = dd2.ToString("X");
            string g3 = dd3.ToString("X");
            string g4 = SSI.ToString("X");

            int dec1 = Convert.ToInt32(g1, 16); // X[HEX] to X[DEC]
            int dec2 = Convert.ToInt32(g2, 16); // Y[HEX] to X[DEC]
            int dec3 = Convert.ToInt32(g3, 16); // Z[HEX] to X[DEC]
            //int dec4 = Convert.ToInt32(g4, 16); // SSI[HEX] to SSI[DEC]
            //AppendLine(textBox3, "Galactic HEX to DEC: " + dec1.ToString() + " " + dec2.ToString() + " " + dec3.ToString() + " " + dec4);

            int dec5 = Convert.ToInt32("801", 16); // 801[HEX] to 801[DEC]
            int dec6 = Convert.ToInt32("81", 16); // 81[HEX] to 81[DEC]
            int dec7 = Convert.ToInt32("1000", 16); // 100[HEX] to 1000[DEC]
            int dec8 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            //AppendLine(textBox3, "Shift HEX to DEC: " + "801:" + dec5.ToString() + " 81:" + dec6.ToString() + " 1000:" + dec7.ToString() + " 100:" + dec8.ToString());

            int calc1 = (dec1 + dec5) % dec7; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (dec2 + dec6) % dec8; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (dec3 + dec5) % dec7; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //AppendLine(textBox3, "Calculate Portal DEC: " + "X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + dec4);

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            //AppendLine(textBox3, "Portal HEX numbers: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + g4);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFF); // Z[HEX] to Z[DEC] 3 digits
            int ihexSSI = (Convert.ToInt32(g4, 16) & 0xFFF); // SSI[HEX] to SSI[DEC] 3 digits

            PortalCode = string.Format("0{0:X3}{1:X2}{2:X3}{3:X3}", ihexSSI, ihexY, ihexZ, ihexX); // Format digits 1 3 2 3 3
            //AppendLine(textBox3, "[SSI][Y][Z][X] Portal Code: " + PortalCode);
        }
        private void ShowPGlyphs()
        {
            //Index chars in PortalCode
            _gl1 = PortalCode[0];
            _gl2 = PortalCode[1];
            _gl3 = PortalCode[2];
            _gl4 = PortalCode[3];
            _gl5 = PortalCode[4];
            _gl6 = PortalCode[5];
            _gl7 = PortalCode[6];
            _gl8 = PortalCode[7];
            _gl9 = PortalCode[8];
            _gl10 = PortalCode[9];
            _gl11 = PortalCode[10];
            _gl12 = PortalCode[11];

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
        private void ShowGlyphs()
        {
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
        int Find(ListBox lb, string searchString, int startIndex)
        {
            //Find method for search bars on top of listboxes
            for (int i = startIndex; i < lb.Items.Count; ++i)
            {
                //string lbString = lb.Items[i].ToString();
                if (lb.Items[i].ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    return i;
            }
            return -1; //Find(lb, searchString, 0);
        }
        private List<string> Contains(List<string> list1, List<string> list2)
        {
            //Compare two lists and send back differences
            List<string> result = new List<string>();

            result.AddRange(list1.Except(list2, StringComparer.OrdinalIgnoreCase));
            result.AddRange(list2.Except(list1, StringComparer.OrdinalIgnoreCase));

            return result;
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            //Reload save button
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "")
            {
                ClearAll();
                AppendLine(textBox17, "Loading Save File...");
                GetSaveFile(selected);
                Loadlsb1();
                LoadBaselsbx();
                GetPlayerCoord();
                LoadTxt();
                AppendLine(textBox17, "Save File Reloaded.");
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
                            //AppendLine(textBox16, fbd.SelectedPath + "save.hg");
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
            //Checks for the json edit dir \json and creates if don't exist
            if (!Directory.Exists(@".\backup"))
                Directory.CreateDirectory(@".\backup");
            Directory.CreateDirectory(@".\backup\json");

            if (!Directory.Exists(@".\backup\json"))
                Directory.CreateDirectory(@".\backup\json");
        }

        public void BuildSaveFile()
        {
            //if the old save.txt exists delete it
            if (File.Exists(oldsavePath))
            {
                File.Delete(oldsavePath);
            }
            
            //if save.nmsc doesn't exist, create it
            if (!File.Exists(savePath))
            {
                File.Create(savePath).Close();
                TextWriter tw = new StreamWriter(savePath);
                tw.WriteLine("nmsPath=" + nmsPath);
                tw.WriteLine("ssdPath=" + ssdPath);
                tw.Close();
            }
            else if (File.Exists(savePath))
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
                //AppendLine(textBox1, "ini File Corrupted! See Log!");
                return;
            }
        }
        public void ReloadSave()
        {            
            Read("nmsPath", savePath);
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
            
            Read("ssdPath", savePath);
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
                    //AppendLine(textBox1, "Error File not found!");
                    return;
                }
            }
            catch
            {
                //AppendLine(textBox1, "ini File Corrupted! See Log!");
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
                listBox1.SelectedIndex = Find(listBox1, textBox24.Text, listBox1.SelectedIndex + 1);
            if (listBox1.SelectedIndex != -1)
                ListBox1_MouseClick(this, new EventArgs());
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            //Right search button
            Clearforsearch();
            if (listBox2.Items.Count >= 1)
                listBox2.SelectedIndex = Find(listBox2, textBox25.Text, listBox2.SelectedIndex + 1);
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
            if (saveslot >= 1 && saveslot <= 5)
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
                ZipFile.CreateFromDirectory(@".\temp", @".\backup\savebackup_" + slot + "_" + datetime + ".zip");

                Directory.Delete(@".\temp", true);

                if (File.Exists(@".\backup\savebackup_" + slot + "_" + datetime + ".zip")) //@".\backup\" + GetNewestZip(@".\backup")))
                {
                    AppendLine(tb, "Save file on Slot: ( " + slot + " ) backed up to \\backup folder...");

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
                        AppendLine(tb, "Something went wrong, no file backed up Error BU:5567");
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
                BackUpSaveSlot(textBox17, saveslot, true);
            else
                MessageBox.Show("Please select a save slot!", "Alert");
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            //Clear Interference Button
            if (saveslot >= 1 && saveslot <= 5 && textBox12.Text != "")
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
                        WriteSavePortal(progressBar1, textBox27, saveslot);

                        //Read and check save file
                        json = File.ReadAllText(hgFilePath);

                        //Check save file edits
                        var nms = Nms.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.The6F.DaC.ToString();

                        Regex myRegexPrtl4 = new Regex(rxPatternPrtl, RegexOptions.Multiline);
                        Match prtl4 = myRegexPrtl4.Match(json);
                        AppendLine(textBox27, prtl4.ToString());

                        if (textBox12.Text == "False" || textBox12.Text == "false")
                        {
                            progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
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
            if (saveslot >= 1 && saveslot <= 5)
            {
                DialogResult dialogResult = MessageBox.Show("Trigger a Freighter Battle ? ", "Freighter Battle", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //Read - Edit - Write Json save file for portal
                    WriteSaveFB(progressBar4, textBox15, saveslot);

                    //Read and check save file
                    json = File.ReadAllText(hgFilePath);

                    var nms = Nms.FromJson(json);
                    bool O5J = nms.The6F.O5J == 0;
                    bool Ebr = nms.The6F.Ebr == 0;
                    bool Exx = nms.The6F.Exx == 0;

                    //Check save file edits         
                    Regex myRegexFB1 = new Regex(rxPatternTLFB, RegexOptions.Multiline);
                    Match FB1 = myRegexFB1.Match(json);
                    string fb1 = FB1.ToString();
                    //AppendLine(textBox15, fb1);

                    Regex myRegexFB2 = new Regex(rxPatternWLFB, RegexOptions.Multiline);
                    Match FB2 = myRegexFB2.Match(json);
                    string fb2 = FB2.ToString();
                    //AppendLine(textBox15, fb2);

                    Regex myRegexFB3 = new Regex(rxPatternAFBUA, RegexOptions.Multiline);
                    Match FB3 = myRegexFB3.Match(json);
                    string fb3 = FB3.ToString();
                    AppendLine(textBox15, fb1 + " " + fb2 + " " + fb3);

                    if (O5J && Ebr && Exx)
                    {
                        progressBar4.Invoke((Action)(() => progressBar1.Value = 100));
                        progressBar4.Visible = false;

                        AppendLine(textBox15, "Freighter Battle Triggered, Reload save in game and warp.");
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
            //backup all discoveries to txt file
            Backuplist.Clear();
            await BackupLoc(@".\backup\locbackup.txt");
        }
        private void JsonKey()
        {
            //regex lookup values
            rxPatternG = "\"Iis\".*?,";
            rxPatternX = "\"dZj\".*?$";
            rxPatternY = "\"IyE\".*?$";
            rxPatternZ = "\"uXE\".*?$";
            rxPatternSSI = "\"vby\".*?$";
            rxPatternPI = "\"jsv\".*?$";

            rxPatternP = "\"6f=\".*?}";
            rxPatternSt = "\"rnc\".*?}";
            rxPatternPs = "\"jk4\".*?,";
            rxPatternPrtl = "\"DaC\".*?,";
            rxPatternPrtl2 = "\"3fO\".*?,";
            rxPatternPrtl3 = "true.*?";

            rxPatternTLFB = "\"05J\".*?,";
            rxPatternWLFB = "\"8br\".*?,";
            rxPatternAFBUA = "\"8xx\".*?,";
        }
        private void JsonSet(string value)
        {
            switch (value)
            {
                //Main regex replace values
                case "all":
                    rxValG = "\"Iis\": " + galaxy + ",";
                    rxValX = "\"dZj\": " + X + ",";
                    rxValY = "\"IyE\": " + Y + ",";
                    rxValZ = "\"uXE\": " + Z + ",";
                    rxValSSI = "\"vby\": " + SSI + ",";
                    rxValPI = "\"jsv\": 0";
                    rxValPs = "\"jk4\": \"InShip\",";
                    rxValPrtl = "\"DaC\": false,";
                    rxValPrtl3 = "false";
                    break;
                //Freighter battle values
                case "fb":
                    rxValTLFB = "\"05J\": 0,";
                    rxValWLFB = "\"8br\": 0,";
                    rxValAFBUA = "\"8xx\": 0,";
                    break;
            }
        }
        private bool ValidateCoord(string A, string B, string C, string D)
        {
            bool x = Convert.ToInt32(A, 16) > 4096 || Convert.ToInt32(B, 16) > 255 || Convert.ToInt32(C, 16) > 4096 || Convert.ToInt32(D, 16) > 767;
            return x;
        }
        private bool CheckForSameLoc()
        {
            //looks up the players current location
            var nms = Nms.FromJson(json);
            var galaxy = nms.The6F.YhJ.Iis.ToString();
            var pX = nms.The6F.YhJ.OZw["dZj"].ToString();
            var pY = nms.The6F.YhJ.OZw["IyE"].ToString();
            var pZ = nms.The6F.YhJ.OZw["uXE"].ToString();
            var pSSI = nms.The6F.YhJ.OZw["vby"].ToString();

            //checks to see if the current location is the same as stored (no move if same)
            bool b = X == pX && Y == pY && Z == pZ && SSI == pSSI && pgalaxy == galaxy;
            return b;
        }
        private void Button5_ClickAsync(object sender, EventArgs e)
        {
            //Move Player button on Base and Space Station tab
            try
            {
                if (saveslot < 1 || saveslot > 5)
                {
                    MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                    return;
                }
                
                if (listBox1.GetItemText(listBox1.SelectedItem) != "" || listBox2.GetItemText(listBox2.SelectedItem) != "")
                {
                    DialogResult dialogResult = MessageBox.Show("Move Player to: " + listBox1.GetItemText(listBox1.SelectedItem) + listBox2.GetItemText(listBox2.SelectedItem) + " ? ", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (galaxy != "" && X != "" && Y != "" && Z != "" && SSI != "")// && saveslot >= 1 && saveslot <= 5)
                        {
                            //Check if location is the same as cuurent
                            if (CheckForSameLoc())
                            {
                                MessageBox.Show("Same as Current location!", "Alert");
                                return;
                            }
                            AppendLine(textBox27, "Move Player to: Galaxy: " + galaxy + " -- X:" + X + " -- Y:" + Y + " -- Z:" + Z + " -- SSI:" + SSI);
                        
                            //Read - Edit - Write Json save file for move player
                            WriteSaveMove(progressBar1, textBox27, saveslot);

                            //Set json to the new modified hg file
                            json = File.ReadAllText(hgFilePath);

                            //Read the new json and check portal interference state
                            var nms = Nms.FromJson(json);
                            textBox12.Clear();
                            textBox12.Text = nms.The6F.DaC.ToString();
                            GetPlayerCoord();

                            progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
                            progressBar1.Visible = false;

                            //set the last write time box
                            textBox26.Clear();
                            FileInfo hgfile = new FileInfo(hgFilePath);
                            AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

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
                AppendLine(textBox27, "Invalid Coordinates!");
            }
        }
        private void BackupALLSaveFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manually backup all save files in nmspath dir
            RunBackupAll(hgFileDir);
            MessageBox.Show("Save Backup Completed!", "Confirmation", MessageBoxButtons.OK);
        }
        //Not used, future?
        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
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
                        /*
                        foreach (DirectoryInfo di in dinfoss)//.OrderByDescending(f => f.LastWriteTime))
                        {
                            if (di.GetFiles("*.jpg", SearchOption.AllDirectories).Length != 0)
                            {
                                list2.Add(di.FullName);
                            }
                        }
                        */
                        foreach (DirectoryInfo di in dinfoss)
                        {
                            string spath = di.FullName + @"\remote\275850\screenshots";
                            if (Directory.Exists(spath))
                            {
                                DirectoryInfo d3 = new DirectoryInfo(spath);
                                //AppendLine(textBox17, d3.FullName);

                                if (d3.GetFiles("*.jpg", SearchOption.TopDirectoryOnly).Length != 0 || d3.GetFiles("*.png", SearchOption.TopDirectoryOnly).Length != 0)
                                {
                                    list2.Add(d3.FullName);
                                }
                            }
                        }
                        if (list2.Count > 1)
                        {
                            AppendLine(textBox17, "More than on screenshot path detected. First found path set.");
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
                    AppendLine(textBox17, "Path to Screenshots doesn't exist");
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
                    AppendLine(textBox17, "No Screenshots found.");
                    return;
                }

                //Order the png or jpg by lastwrite times and set screenshot file path
                list.OrderByDescending(f => f.LastWriteTime);
                ssPath = list[0].FullName;
                AppendLine(textBox17, "ScreenShot: " + list[0].FullName);
                WriteTxt("ssdPath", ssdPath, savePath);
                LoadSS();
            }
            catch
            {
                AppendLine(textBox17, "ssPath error! ss155");
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
                            AppendLine(textBox17, "No Screenshots found.");
                            return;
                        }

                        //Order the png or jpg by lastwrite times and set screenshot file path
                        list.OrderByDescending(f => f.LastWriteTime);
                        ssPath = list[0].FullName;
                        AppendLine(textBox17, "ScreenShot: " + list[0].FullName);
                        WriteTxt("ssdPath", ssdPath, savePath);
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
            Form3 f3 = new Form3();
            AppendLine(textBox17, ssdPath);
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
            //Loads all the txt files in listbox4 for selection
            listBox3.DataSource = null;
            textBox11.Clear();
            textBox13.Clear();
            textBox18.Clear();

            if (Directory.Exists(@".\backup"))
            {
                List<string> list = new List<string>();
                DirectoryInfo dinfo2 = new DirectoryInfo(@".\backup");
                //FileInfo[] Files = dinfo2.GetFiles("locbackup*.txt", SearchOption.AllDirectories);
                FileInfo[] Files = dinfo2.GetFiles("*.txt", SearchOption.AllDirectories);

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
                    toolStripMenuItem1.Enabled = false;
                    AppendLine(textBox11, "Error! 855");
                    return;
                }
                listBox4.DataSource = list;
                toolStripMenuItem1.Enabled = true;
            }
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            //Load a txt file to view in listbox3
            try
            {
                if (listBox4.GetItemText(listBox4.SelectedItem) != "")
                {
                    textBox18.Text = listBox3.Items.Count.ToString();
                    string path = @".\backup\" + listBox4.SelectedItem.ToString();

                    if (!File.Exists(path))
                    {
                        MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                        LoadTxt();
                        return;
                    }

                    string[] locFile = File.ReadAllLines(path);
                    if (locFile[0].ToString() != "")
                    {
                        listBox3.DataSource = locFile;
                        listBox3.SelectedIndex = 0;
                        toolStripMenuItem1.Enabled = true;
                    }
                    else
                    {
                        toolStripMenuItem1.Enabled = false;
                        AppendLine(textBox11, "File is Empty! Select another file.");
                        AppendLine(textBox11, "---------------------");
                    }
                }
                else
                {
                    toolStripMenuItem1.Enabled = false;
                    AppendLine(textBox11, "No File Selected or File Empty!");
                    AppendLine(textBox11, "---------------------");
                }
            }
            catch
            {
                toolStripMenuItem1.Enabled = false;
                AppendLine(textBox11, "No File Selected or File Empty!");
                AppendLine(textBox11, "---------------------");
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
                    AppendLine(textBox11, "---------------------");
                }

                Regex myRegex1 = new Regex("GC:.*?$", RegexOptions.Multiline);
                Match m1 = myRegex1.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                string line1 = m1.ToString();

                if (line1.Length < 23) // if text file problem
                {
                    textBox11.Clear();
                    AppendLine(textBox11, "File Error");
                    AppendLine(textBox11, "---------------------");

                    DialogResult dialogResult2 = MessageBox.Show("Something went wrong! \r\n\nSelected line has errors. \r\n\nWould you like to Delete the Line?", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult2 == DialogResult.Yes)
                    {
                        DeleteSingleRecordToolStripMenuItem_Click(this, new EventArgs());
                        return;
                    }
                    else if (dialogResult2 == DialogResult.No)
                    {
                        return;
                    }
                }
                    
                string g1 = line1.Substring(0, 23);
                //AppendLine(textBox11, g1);

                if (line1.Length > 23)
                {
                    string gN = line1.Substring(line1.Length - (line1.Length - 23), line1.Length - 23);
                    AppendLine(textBox11, gN);
                    AppendLine(textBox11, g1);
                }
                else
                {
                    AppendLine(textBox11, " ");
                    AppendLine(textBox11, g1);
                }

                Regex myRegex2 = new Regex("PC.*?--", RegexOptions.Multiline);
                Match m2 = myRegex2.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                string line2 = m2.ToString();
                line2 = line2.Replace(" --", "");
                AppendLine(textBox11, line2);

                Regex myRegex3 = new Regex("^.*?\\)", RegexOptions.Multiline);
                Match m3 = myRegex3.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                string line3 = m3.ToString();

                Regex myRegex4 = new Regex(" .*?#", RegexOptions.Multiline);
                Match m4 = myRegex4.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                string line3_2 = m4.ToString().Replace("#", "");

                if (m3.Success)
                {
                    AppendLine(textBox11, line3);
                }
                else if (m4.Success)
                {
                    AppendLine(textBox11, line3_2);
                }

                AppendLine(textBox11, "---------------------");
            }  
        }

        //Check is manual travel is unlocked
        public bool TextBoxPerm
        {
            get { return textBox14.ReadOnly; }
        }

        //bring over copied GC from Calculator
        public string TextBoxValue
        {
            get { return textBox14.Text; }
            set { textBox14.Text = value; }
        }

        private Form5 f5;

        private void Button7_Click(object sender, EventArgs e)
        {
            //Coordinate Calculator button if not open, open
            if (f5 == null)
            {
                f5 = new Form5();
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
        private void GalacticToVoxel(string oX, string oY, string oZ, string oSSI)
        {
            //Galactic Coordinate to Voxel Coordinates 
            textBox13.Clear();

            //HEX in
            AppendLine(textBox13, "Galactic Coordinates HEX: SSI:" + oSSI + " Y:" + oY + " Z:" + oZ + " X:" + oX);

            //HEX to DEC
            int icX = Convert.ToInt32(oX, 16);
            int icY = Convert.ToInt32(oY, 16);
            int icZ = Convert.ToInt32(oZ, 16);
            int icSSI = Convert.ToInt32(oSSI, 16);
            AppendLine(textBox13, "Galactic Coordinates DEC: SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX);

            //SHIFT DEC 2047 127 2047 ssi-same
            int vX = icX - 2047;
            int vY = icY - 127;
            int vZ = icZ - 2047;

            //voxel dec  x y z ssi-same
            AppendLine(textBox13, "*** Voxel Coordinates DEC: SSI:" + icSSI + " Y:" + vY + " Z:" + vZ + " X:" + vX + " ***");
            //voxel = "SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX;

            //sets all voxel for future use
            X = vX.ToString();
            Y = vY.ToString();
            Z = vZ.ToString();
            SSI = icSSI.ToString();
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            //Move player button share coordinate tab
            try
            {
                if (listBox3.GetItemText(listBox3.SelectedItem) != "")
                {
                    if (saveslot < 1 || saveslot > 5)
                    {
                        MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                        return;
                    }

                    DialogResult dialogResult = MessageBox.Show("Move Player? ", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //grabs the galactic coordinate
                        Regex myRegexGC = new Regex("GC:.*?$", RegexOptions.Multiline);
                        Match m1 = myRegexGC.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                        string line1 = m1.ToString();
                        line1 = line1.Replace("GC: ", "");
                        line1 = line1.Replace(" ", "");
                        string[] value = line1.Split(':');

                        //Check for errors on GC
                        string gc = line1.Replace(":", "");
                        if (gc.Length < 16)
                        {
                            DialogResult dialogResult2 = MessageBox.Show("Something went wrong! \r\n\nSelected line has errors. \r\n\nWould you like to Delete the Line?", "Fast Travel", MessageBoxButtons.YesNo);
                            if (dialogResult2 == DialogResult.Yes)
                            {
                                DeleteSingleRecordToolStripMenuItem_Click(this, new EventArgs());
                                return;
                            }
                            else if (dialogResult2 == DialogResult.No)
                            {
                                return;
                            }                                                       
                        }

                        //Only take 4 digits from the last array so can add notes GC: 0000:0000:0000:[0000] A:B:C:D
                        string A = value[0].Trim();
                        string B = value[1].Trim();
                        string C = value[2].Trim();
                        string D = value[3].Trim().Substring(0, 4);

                        //Validate Coordinates
                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            return;
                        }

                        //sets x,y,z,ssi ix,iy,iz,issi from given ABCD
                        GalacticToVoxel(A, B, C, D);

                        //grabs the galaxy
                        Regex myRegexG = new Regex("G:.*?-", RegexOptions.Multiline);
                        Match m2 = myRegexG.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                        string line2 = m2.ToString();
                        line2 = line2.Replace("G: ", "");
                        line2 = line2.Replace("-", "");
                        line2 = line2.Replace(" ", "");
                        galaxy = line2;

                        AppendLine(textBox13, "Galaxy: " + galaxy + " -- X:" + X + " -- Y:" + Y + " -- Z:" + Z + " -- SSI:" + SSI);
                        
                        if (galaxy != "" && X != "" && Y != "" && Z != "" && SSI != "") // && saveslot >= 1 && saveslot <= 5)
                        {
                            //Check if location is the same as cuurent
                            if (CheckForSameLoc())
                            {
                                MessageBox.Show("Same as Current location!", "Alert");
                                return;
                            }
                            AppendLine(textBox13, "Move Player to: Galaxy: " + galaxy + " -- X:" + X + " -- Y:" + Y + " -- Z:" + Z + " -- SSI:" + SSI);

                            //Main save writer
                            WriteSaveMove(progressBar3, textBox13, saveslot);

                            //Set json to the new modified hg file
                            json = File.ReadAllText(hgFilePath);

                            //Read the new json and check portal interference state
                            var nms = Nms.FromJson(json);
                            textBox12.Clear();
                            textBox12.Text = nms.The6F.DaC.ToString();
                            GetPlayerCoord();

                            progressBar3.Invoke((Action)(() => progressBar3.Value = 100));
                            progressBar3.Visible = false;

                            //set the last write time box
                            textBox26.Clear();
                            FileInfo hgfile = new FileInfo(hgFilePath);
                            AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

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
                    AppendLine(textBox13, "Please load and select a location!");
                    MessageBox.Show("Please load and select a location!", "Alert");
                }
            }
            catch
            {                
                AppendLine(textBox13, "Invalid Coordinates!");
            }
        }
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Save a single location record to a new txt file
            if (listBox3.GetItemText(listBox3.SelectedItem) != "")
            {
                var selectedrecord = listBox3.GetItemText(listBox3.SelectedItem);

                List<string> list = new List<string>();
                list.Add(listBox3.GetItemText(listBox3.SelectedItem));
                string path2 = MakeUnique(@".\backup\locbackup.txt").ToString();
                File.WriteAllLines(path2, list);
                //Process.Start(path2); //v1.1.14
                AppendLine(textBox13, "Single Record saved!");
                LoadTxt();

                if (File.Exists(path2))
                {
                    listBox4.SelectedItem = Path.GetFileName(path2);
                    Button6_Click(this, new EventArgs());
                    listBox3.SelectedItem = selectedrecord;
                }                    
            }
            else
            {
                AppendLine(textBox13, "No record saved! Please select a txt!");
            }

        }
        private void OpenLocationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open a locbackup file
            if (listBox4.GetItemText(listBox4.SelectedItem) == "")
            {
                return;
            }

            if (File.Exists(@".\backup\" + listBox4.GetItemText(listBox4.SelectedItem)))
            {
                Process.Start(@".\backup\" + listBox4.GetItemText(listBox4.SelectedItem));
                LoadTxt();
            }
            else
            {
                MessageBox.Show("No file found.", "Alert");
                LoadTxt();
            }
        }
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //Delete a locbackup file
            if (listBox4.GetItemText(listBox4.SelectedItem) != "")
            {
                DialogResult dialogResult = MessageBox.Show("Delete " + listBox4.GetItemText(listBox4.SelectedItem) + " ? ", "Locbackup Manager", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (File.Exists(@".\backup\" + listBox4.GetItemText(listBox4.SelectedItem)))
                    {
                        if (listBox4.GetItemText(listBox4.SelectedItem) != "")
                        {
                            File.Delete(@".\backup\" + listBox4.GetItemText(listBox4.SelectedItem));
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

        //Future use, doesn't save changed currently
        private void SetShortcutToGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void OpenFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GamePath = openFileDialog1.FileName;
            AppendLine(textBox17, GamePath);
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
        private void GalacticToVoxelMan(string oX, string oY, string oZ, string oSSI)
        {
            //Galactic Coordinate to Voxel Coordinate
            textBox15.Clear();

            //HEX in
            //AppendLine(textBox15, "Galactic Coordinates HEX: SSI:" + oSSI + " Y:" + oY + " Z:" + oZ + " X:" + oX);

            //HEX to DEC
            int icX = Convert.ToInt32(oX, 16);
            int icY = Convert.ToInt32(oY, 16);
            int icZ = Convert.ToInt32(oZ, 16);
            int icSSI = Convert.ToInt32(oSSI, 16);
            AppendLine(textBox15, "Galactic Coordinates DEC: SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX);

            //SHIFT DEC 2047 127 2047 ssi-same
            int vX = icX - 2047;
            int vY = icY - 127;
            int vZ = icZ - 2047;

            //voxel dec  x y z ssi-same
            AppendLine(textBox15, "*** Voxel Coordinates DEC: SSI:" + icSSI + " Y:" + vY + " Z:" + vZ + " X:" + vX + " ***");
            //voxel = "SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX;

            //sets all voxels for use after method
            iX = vX;
            iY = vY;
            iZ = vZ;
            iSSI = icSSI;

            X = vX.ToString();
            Y = vY.ToString();
            Z = vZ.ToString();
            SSI = icSSI.ToString();
        }
        private void Button11_Click(object sender, EventArgs e)
        {
            //Move player button on Manual Travel Tab
            //clears all voxels for upcoming steps
            X = "";
            Y = "";
            Z = "";
            SSI = "";

            //Galactic coordinates to Voxel
            try
            {
                if (textBox14.Text != "")
                {
                    if (saveslot < 1 || saveslot > 5)
                    {
                        MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                        return;
                    }

                    //removes all spaces
                    string t2 = textBox14.Text.Replace(" ", "");

                    //if format 0000:0000:0000:0000 A:B:C:D
                    if (t2.Contains(":") && t2.Length == 19)
                    {
                        string[] value = t2.Replace(" ", "").Split(':');
                        string A = value[0].Trim();
                        string B = value[1].Trim();
                        string C = value[2].Trim();
                        string D = value[3].Trim();

                        //Validate Coordinates
                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            return;
                        }

                        //sets x,y,z,ssi ix,iy,iz,issi from given ABCD
                        GalacticToVoxelMan(A, B, C, D);
                        GetPortalCoord(iX, iY, iZ, iSSI);
                    }

                    //if format 0000000000000000
                    if (t2.Length == 16 && !t2.Contains(":"))
                    {
                        //0000 0000 0000 0000  XXXX:YYYY:ZZZZ:SSIX  A B C D
                        string A = t2.Substring(t2.Length - 16, 4);
                        string B = t2.Substring(t2.Length - 12, 4);
                        string C = t2.Substring(t2.Length - 8, 4);
                        string D = t2.Substring(t2.Length - 4, 4);

                        //Validate Coordinates
                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            return;
                        }

                        //sets x,y,z,ssi ix,iy,iz,issi from given ABCD
                        GalacticToVoxelMan(A, B, C, D);
                        GetPortalCoord(iX, iY, iZ, iSSI);
                    }

                    //if invalid format
                    if (t2.Replace(":", "").Length < 16 | t2.Replace(":", "").Length > 16 | t2.Length < 16)
                    {
                        //AppendLine(textBox7, "Incorrect Coordinate Input!");
                        MessageBox.Show("Invalid Coordinate Input!", "Alert");
                        return;
                    }

                    //Validate and set galaxy
                    if (comboBox3.SelectedIndex <= 263) //selectedindex 0-256 + 7 = galaxy 1-257 + 0 to -7
                    {
                        galaxy = comboBox3.SelectedIndex.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Galaxy!", "Alert");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Cannot locate player!", "Alert");
                    return;
                }

                if (galaxy != "" && X != "" && Y != "" && Z != "" && SSI != "")// && saveslot >= 1 && saveslot <= 5)
                {
                    DialogResult dialogResult = MessageBox.Show("Move Player? ", "Fast Travel", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //Check if location is the same as cuurent
                        if (CheckForSameLoc())
                        {
                            MessageBox.Show("Same as Current location!", "Alert");
                            return;
                        }
                        AppendLine(textBox15, "Move Player to: Galaxy: " + galaxy + " -- X:" + X + " -- Y:" + Y + " -- Z:" + Z + " -- SSI:" + SSI);

                        WriteSaveMove(progressBar4, textBox15, saveslot);

                        //Set json to the new modified hg file
                        json = File.ReadAllText(hgFilePath);

                        //Read the new json and check portal interference state
                        var nms = Nms.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.The6F.DaC.ToString();
                        GetPlayerCoord();

                        progressBar4.Invoke((Action)(() => progressBar4.Value = 100));
                        progressBar4.Visible = false;

                        //set the last write time box
                        textBox26.Clear();
                        FileInfo hgfile = new FileInfo(hgFilePath);
                        AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

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
                AppendLine(textBox15, "Incorrect Coordinate Input!");
            }

            X = "";
            Y = "";
            Z = "";
            SSI = "";
        }
        private void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //populates the combobox when moving tabs
            if (pgalaxy != null && pgalaxy != "")
            {
                comboBox3.Items.Clear();
                label35.Text = "";
                label36.Text = "";

                //Add Galaxy numbers
                for (int i = 1; i <= 257; i++) // increase to 257 1.1.16
                {
                    string[] numbers = { i.ToString() };
                    comboBox3.Items.AddRange(numbers);
                }

                // Negative galaxies
                string[] neg = { "-6", "-5", "-4", "-3", "-2", "-1", "0" };
                comboBox3.Items.AddRange(neg);

                comboBox3.SelectedIndex = Convert.ToInt32(pgalaxy);

                //Lookup and display galaxy name in label
                GalaxyLookupLbl(label35, pgalaxy);
                CheckLush();

                //only sets the textbox to current location if manual travel is locked
                if (unlockedToolStripMenuItem.Checked == false)
                    textBox14.Text = textBox22.Text;
            }
        }
        private void CheckLush()
        {
            var galaxy = Convert.ToInt32(pgalaxy);

            var lushlist = new List<int>
            {
                9, 18, 29, 38, 49, 58, 69, 78, 89, 98, 109, 118,
                129, 138, 149, 158, 169, 178, 189, 198, 209, 218,
                229, 238, 249
            };

            var harshlist = new List<int>
            {
                2, 14, 22, 34, 42, 54, 62, 74, 82, 94, 102, 114, 122, 
                134, 142, 154, 162, 174, 182, 194, 202, 214, 222, 234,
                242, 254
            };

            var emptylist = new List<int>
            {
                6, 11, 26, 31, 46, 51, 66, 71, 86, 91, 106, 111, 126, 
                131, 146, 151, 166, 171, 186, 191, 206, 211, 226, 231,
                246, 251
            };

            if (lushlist.Contains(galaxy) || harshlist.Contains(galaxy) || emptylist.Contains(galaxy))
            {
                if (lushlist.Contains(galaxy))
                {
                    //label36.Visible = true;
                    label36.Text = "LUSH";
                }
                if (harshlist.Contains(galaxy))
                {
                    //label36.Visible = true;
                    label36.Text = "HARSH";
                }
                if (emptylist.Contains(galaxy))
                {
                    //label36.Visible = true;
                    label36.Text = "EMPTY";
                }
            }
            else
            {
                label36.Text = "NORMAL";
                //label36.Visible = false;
            }
        }

        private void ComboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //sets the player galaxy when combobox change committed to fast travel
            pgalaxy = comboBox3.SelectedIndex.ToString();

            //Lookup and display galaxy name in label
            GalaxyLookupLbl(label35, pgalaxy);
            CheckLush();
        }
        private void ListBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Double click a location txt file to load
            button6.PerformClick();
        }
        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("http://nmscoordinates.com");
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
        private void CheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            //Travel Mode checkbox
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "" && checkBox1.Checked)
            {
                //Locks to one save slot
                comboBox2.Enabled = false;
                label30.Visible = true;
                AppendLine(textBox17, "Save Slot Selection Disabled...");

                SSlist.Clear();
                PrevSSlist.Clear();
                AppendLine(textBox17, "Reading all locations...");

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
                AppendLine(textBox17, "Loading Save File...");
                GetSaveFile(selected);
                Loadlsb1();
                LoadBaselsbx();
                GetPlayerCoord();

                SSlist.Clear();
                AppendLine(textBox17, "Checking for deleted locations...");

                CheckSS();

                //Toggle For Testing
                //SSlist.RemoveRange(0, 4);
                //SSlist.Add("Slot_2_Loc: test Platform (SS) - G: 1 - PC: 000000000000 -- GC: 080B:0088:080F:019E");
                //SSlist.Add("Slot_2_Loc: test2 Platform (SS) - G: 2 - PC: 200000000000 -- GC: 080B:0088:080F:019E");

                List<string> list3 = new List<string>();

                //Looks for all adds and deletes and returns to list3
                list3 = Contains(PrevSSlist, SSlist);

                List<string> DeletedSSlist = new List<string>();

                foreach (string item in list3)
                {
                    //If an added loc, it will be in sslist, so everything else is deleted
                    if (!SSlist.Contains(item))
                        DeletedSSlist.Add(item);
                }

                if (!File.Exists(@".\backup\locbackup_deleted.txt"))
                {
                    File.Create(@".\backup\locbackup_deleted.txt").Dispose();
                }

                List<string> logFile = File.ReadAllLines(@".\backup\locbackup_deleted.txt").ToList();

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
                    File.WriteAllLines(@".\backup\locbackup_deleted.txt", logFile);
                    AppendLine(textBox17, noduplicates.Count.ToString() + " Deleted locations Found.");
                    LoadTxt();
                }
                else
                {
                    AppendLine(textBox17, "No Deleted locations Found.");
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
            AppendLine(textBox17, "Travel Mode VISIBLE. Select a save and click the box");
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
            AppendLine(textBox17, "Travel Mode HIDDEN.");
        }
        private void LockedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manual travel LOCK
            unlockedToolStripMenuItem.Checked = false;
            textBox14.ReadOnly = true;
            label33.Visible = false;
            textBox14.Text = textBox22.Text;
            AppendLine(textBox17, "Manual Travel LOCKED.");
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
                AppendLine(textBox17, "Manual Travel UNLOCKED, Enter Coord. on Manual Travel tab.");
                MessageBox.Show("Manual travel UNLOCKED, Enter Coord. on Manual Travel tab. \r\n\n Make sure Coordinates are correct!", "Warning");
                if(pgalaxy != null && pgalaxy != "")
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
        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("https://www.nexusmods.com/nomanssky/mods/1312");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }

        }
        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Created by: Kevin0M16 \r\n\r\n 8-2019");
            Form2 f2 = new Form2(NMSCVersion);
            f2.ShowDialog();

        }

        //private void RunPowerToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Process.Start(@"Powershell.exe", @"-NoExit function prompt {\""NMSC >\""} cd nmssavetool;
        //                    write-host 
        //                    \""************ NMSCoordinates ***** Save File Editing ***************************
        //
        //                        First Decrypt Save: .\nmssavetool.exe decrypt -g[saveslot] -f [filename].json
        //
        //                       ----- Now you can Modify [filename].json file externally - Ex. Notepad++ ------
        //
        //                        Last Encrypt Save: .\nmssavetool.exe encrypt -g[saveslot] -f [filename].json
        //
        //                      -------------------------------------------------------------------------------
        //                    \""");
        //}

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

                Form6 f6 = new Form6();
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

        private List<String> _changedFiles = new List<string>();

        private void FileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            //Watch for changes in hg files
            List<string> list = new List<string>();

            foreach (KeyValuePair<int, string> item in comboBox1.Items)
            {
                list.Add(item.Value);
            }

            if (list.Contains(e.Name))
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
                Form7 f7 = new Form7();
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
                    AppendLine(textBox17, "Not Viewing the latest save!");
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
            //Save current player location to txt
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "" && pgalaxy != "")
            {
                if (!File.Exists(@".\backup\player_locs.txt"))
                    File.Create(@".\backup\player_locs.txt").Dispose();

                string currentloc = "Date\\Time: " + DateTime.Now.ToString("MM-dd-yyyy HH:mm") + " ## File: " + selected + " ## G: " + pgalaxy + " - PC: " + textBox21.Text + " -- GC: " + textBox22.Text;

                List<string> playerloc = File.ReadAllLines(@".\backup\player_locs.txt").ToList();

                if (!playerloc.Contains(currentloc))
                {
                    playerloc.Add(currentloc);
                }
                else
                {
                    MessageBox.Show("Location already saved in player_loc.txt!", "Alert", MessageBoxButtons.OK);
                    return;
                }

                File.WriteAllLines(@".\backup\player_locs.txt", playerloc);
                MessageBox.Show("Location added to player_loc.txt \n\n\r Open in Coordinate Share Tab", "Confirmation", MessageBoxButtons.OK);
                LoadTxt();

                tabControl1.SelectedTab = tabPage3;
                if (File.Exists(@".\backup\player_locs.txt"))
                {
                    listBox4.SelectedItem = "player_locs.txt";
                    Button6_Click(this, new EventArgs());
                    listBox3.SelectedItem = currentloc;
                }                
            }
            else
            {
                MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
            }
        }
        private void DeleteSingleRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //delete a single location record
            string record = listBox3.GetItemText(listBox3.SelectedItem);
            string filename = listBox4.GetItemText(listBox4.SelectedItem);
            int selectedrecord = listBox3.SelectedIndex;
            var selectedfile = listBox4.SelectedItem;

            if (record != "" && filename != "")
            {
                string path = @".\backup\" + filename;

                if (!File.Exists(path))
                {
                    MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                    LoadTxt();
                    return;
                }

                List<string> filelist = File.ReadAllLines(path).ToList();

                if (filelist.Contains(record) && filelist.Count <= 1)
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
                else if (filelist.Contains(record) && filelist.Count > 1)
                {
                    filelist.Remove(record);

                    File.WriteAllLines(path, filelist);
                    AppendLine(textBox13, "Single Record deleted!");
                    LoadTxt();

                    if (File.Exists(path))
                    {
                        listBox4.SelectedItem = selectedfile;
                        Button6_Click(this, new EventArgs());
                        
                        if (selectedrecord == 0)
                            listBox3.SelectedIndex = selectedrecord;
                        else
                            listBox3.SelectedIndex = selectedrecord - 1;
                    }
                }
            }
            else
            {
                AppendLine(textBox13, "No record deleted! Please select a txt!");
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

        private GameSave _gs;
        private GameSaveManager _gsm;
        private uint _gameSlot;

        private void WriteSaveFB(ProgressBar pb, TextBox tb, int saveslot)
        {
            //Main method for writing a change for a freighter battle
            fileSystemWatcher1.EnableRaisingEvents = false;

            BackUpSaveSlot(tb, saveslot, false);
            DecryptSave(saveslot);
            EditSaveFB(pb);
            EncryptSave(pb, saveslot);

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private void WriteSavePortal(ProgressBar pb, TextBox tb, int saveslot)
        {
            //Main method for writing a change in portal status
            fileSystemWatcher1.EnableRaisingEvents = false;

            BackUpSaveSlot(tb, saveslot, false);
            DecryptSave(saveslot);
            EditSavePortal(pb);
            EncryptSave(pb, saveslot);

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private void WriteSaveMove(ProgressBar pb, TextBox tb, int saveslot)
        {
            //Main method for writing a player move
            fileSystemWatcher1.EnableRaisingEvents = false;

            BackUpSaveSlot(tb, saveslot, false);
            DecryptSave(saveslot);
            EditSaveMove(pb, tb);
            EncryptSave(pb, saveslot);

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private void DecryptSave(int saveslot)
        {
            LoadRun(saveslot);
            RunDecrypt();
        }
        private void EncryptSave(ProgressBar pb, int saveslot)
        {
            RunEncrypt(pb, saveslot);
        }
        private void RunBackupAll(string Path)
        {
            DoCommon();

            try
            {
                string archivePath = Path; //hgFileDir;

                if (Directory.Exists(Path))
                {
                    //var baseName = string.Format("nmssavetool-backupall-{0}", _gsm.FindMostRecentSaveDateTime().ToString("yyyyMMdd-HHmmss"));
                    var basePath = @".\backup\nms-backup-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                    archivePath = basePath + ".zip";
                    _gsm.ArchiveSaveDirTo(archivePath);
                    AppendLine(textBox17, "All saves backed up to zip file created in \\backup folder...");
                }              
            }
            catch
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);                
            }
        }
        private void DoGameSlotCommon(int saveslot)
        {
            DoCommon();
            _gameSlot = Convert.ToUInt32(saveslot);
        }
        private void DoCommon()
        {
            _gsm = new GameSaveManager(hgFileDir);
        }
        private void LoadRun(int saveslot)
        {
            uint GameSlot = Convert.ToUInt32(saveslot);

            DoGameSlotCommon(saveslot);

            try
            {
                string mf_hgFilePath = hgFilePath;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                //Sets the save to be the last modified
                File.SetLastWriteTime(mf_hgFilePath, DateTime.Now);
                File.SetLastWriteTime(hgFilePath, DateTime.Now);

                _gs = _gsm.ReadSaveFile(GameSlot);
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
                File.WriteAllText(@".\backup\json\save.json", formattedJson);
            }
            catch
            {
                return;
            }
        }
        private void EditSaveFB(ProgressBar pb)
        {
            //Set JSON search pattern
            JsonSet("fb");

            pb.Visible = true;
            pb.Invoke((Action)(() => pb.Value = 5)); //progressBar1.Value = 5));

            //Read decrypted save.json to a string
            string jsons = File.ReadAllText(@".\backup\json\save.json");

            pb.Invoke((Action)(() => pb.Value = 45));

            //Find the value for Time Last Freighter Battle
            Regex myRegexFB1 = new Regex(rxPatternTLFB, RegexOptions.Multiline);
            Match FB1 = myRegexFB1.Match(jsons);
            string fb1 = FB1.ToString();

            //Set the value for Time Last Freighter Battle
            jsons = Regex.Replace(jsons, rxPatternTLFB, rxValTLFB, RegexOptions.Multiline);

            //Find the value for Warps Last Freighter Battle
            Regex myRegexFB2 = new Regex(rxPatternWLFB, RegexOptions.Multiline);
            Match FB2 = myRegexFB2.Match(jsons);
            string fb2 = FB2.ToString();

            //Set the value for Warps Last Freighter Battle
            jsons = Regex.Replace(jsons, rxPatternWLFB, rxValWLFB, RegexOptions.Multiline);

            //Find the value for Active Space Battle UA
            Regex myRegexFB3 = new Regex(rxPatternAFBUA, RegexOptions.Multiline);
            Match FB3 = myRegexFB3.Match(jsons);
            string fb3 = FB3.ToString();

            //Set the value for Active Space Battle UA
            jsons = Regex.Replace(jsons, rxPatternAFBUA, rxValAFBUA, RegexOptions.Multiline);

            //Write the modified JSON string to saveedit.json
            File.WriteAllText(@".\backup\json\saveedit.json", jsons);

            pb.Invoke((Action)(() => pb.Value = 60));
        }
        private void EditSavePortal(ProgressBar pb)
        {
            //Set the JSON search patterns
            JsonSet("all");

            pb.Visible = true;
            pb.Invoke((Action)(() => pb.Value = 5));//progressBar1.Value = 5));

            //Read decrypted save.json to a string
            string jsons = File.ReadAllText(@".\backup\json\save.json");

            pb.Invoke((Action)(() => pb.Value = 45));

            //Set Portal Interference false DaC
            //Get the portal interf. state object
            Regex myRegexPrtl = new Regex(rxPatternPrtl, RegexOptions.Multiline);
            Match prtl = myRegexPrtl.Match(jsons);

            //Set Portal Interference state rxValPrtl preset to false
            jsons = Regex.Replace(jsons, rxPatternPrtl, rxValPrtl, RegexOptions.Multiline);

            //Beyond - Find "VisitedPortal" or 3fO to false to cancel portal
            Regex myRegexPrtl2 = new Regex(rxPatternPrtl2, RegexOptions.Singleline);
            Match prtl2 = myRegexPrtl2.Match(jsons);
            rxValPrtl2 = prtl2.ToString();
            
            Regex myRegexPrtl3 = new Regex(rxPatternPrtl3, RegexOptions.Multiline);
            rxValPrtl2 = Regex.Replace(rxValPrtl2, rxPatternPrtl3, rxValPrtl3, RegexOptions.Multiline);            

            //Set the visited portal state array after changes made
            jsons = Regex.Replace(jsons, rxPatternPrtl2, rxValPrtl2, RegexOptions.Singleline);

            //Write the modified JSON string to saveedit.json
            File.WriteAllText(@".\backup\json\saveedit.json", jsons);

            pb.Invoke((Action)(() => pb.Value = 60));
        }
        private void EditSaveMove(ProgressBar pb, TextBox tb)
        {
            //Set all Regex values
            JsonSet("all");

            pb.Visible = true;
            pb.Invoke((Action)(() => pb.Value = 5));

            //progressBar1.Invoke((Action)(() => progressBar1.Value = 25));

            //Read all the JSON text from nmssavetool decrypt
            string jsons = File.ReadAllText(@".\backup\json\save.json");

            ////Set Player Location
            //Get the Player location text array
            Regex myRegex = new Regex(rxPatternP, RegexOptions.Singleline);
            Match m = myRegex.Match(jsons);
            rxValP = m.ToString();

            //Get and Set Galaxy
            Regex myRegex1 = new Regex(rxPatternG, RegexOptions.Multiline);
            rxValP = Regex.Replace(rxValP, rxPatternG, rxValG, RegexOptions.Multiline);

            //Get and Set X
            Regex myRegex2 = new Regex(rxPatternX, RegexOptions.Multiline);
            rxValP = Regex.Replace(rxValP, rxPatternX, rxValX, RegexOptions.Multiline);

            //Get amd Set Y
            Regex myRegex3 = new Regex(rxPatternY, RegexOptions.Multiline);
            rxValP = Regex.Replace(rxValP, rxPatternY, rxValY, RegexOptions.Multiline);

            //Get and Set Z
            Regex myRegex4 = new Regex(rxPatternZ, RegexOptions.Multiline);
            rxValP = Regex.Replace(rxValP, rxPatternZ, rxValZ, RegexOptions.Multiline);

            //Get and Set Solar System index (SSI)
            Regex myRegex5 = new Regex(rxPatternSSI, RegexOptions.Multiline);
            rxValP = Regex.Replace(rxValP, rxPatternSSI, rxValSSI, RegexOptions.Multiline);

            //Get and Set Planet Index
            Regex myRegex6 = new Regex(rxPatternPI, RegexOptions.Multiline);
            rxValP = Regex.Replace(rxValP, rxPatternPI, rxValPI, RegexOptions.Multiline);

            //Set the player location array after changes made
            jsons = Regex.Replace(jsons, rxPatternP, rxValP, RegexOptions.Singleline);

            //Player Local Galaxy
            Regex myRegexQQp = new Regex("\"QQp\".*?,", RegexOptions.Multiline);
            Match mqqp = myRegexQQp.Match(jsons);
            string QQP = mqqp.ToString();

            //Set local galaxy to the selected in jsons
            jsons = Regex.Replace(jsons, "\"QQp\".*?,", "\"QQp\": " + galaxy + ",", RegexOptions.Multiline);

            ////Set Spawn State
            // Get the Spawn state array
            Regex myRegexs = new Regex(rxPatternSt, RegexOptions.Singleline);
            Match ms = myRegexs.Match(jsons);
            rxValSt = ms.ToString();

            //Get and set Player last known location in Spwn state array
            Regex myRegexps = new Regex(rxPatternPs, RegexOptions.Multiline);
            rxValSt = Regex.Replace(rxValSt, rxPatternPs, rxValPs, RegexOptions.Multiline);

            //Set the spawn state array after changes made
            jsons = Regex.Replace(jsons, rxPatternSt, rxValSt, RegexOptions.Singleline);

            //Set Portal Interference false DaC
            //Get the portal interf. state object
            Regex myRegexPrtl = new Regex(rxPatternPrtl, RegexOptions.Multiline);
            Match prtl = myRegexPrtl.Match(jsons);

            //Set Portal Interference state rxValPrtl preset to false
            jsons = Regex.Replace(jsons, rxPatternPrtl, rxValPrtl, RegexOptions.Multiline);

            //Beyond - Find "VisitedPortal" or 3fO to false to cancel portal
            Regex myRegexPrtl2 = new Regex(rxPatternPrtl2, RegexOptions.Singleline);
            Match prtl2 = myRegexPrtl2.Match(jsons);
            rxValPrtl2 = prtl2.ToString();
            
            Regex myRegexPrtl3 = new Regex(rxPatternPrtl3, RegexOptions.Multiline);
            rxValPrtl2 = Regex.Replace(rxValPrtl2, rxPatternPrtl3, rxValPrtl3, RegexOptions.Multiline);

            //Set the visited portal state array after changes made
            jsons = Regex.Replace(jsons, rxPatternPrtl2, rxValPrtl2, RegexOptions.Singleline);

            pb.Invoke((Action)(() => pb.Value = 40));

            //Write all modifications of file to saveedit.json
            File.WriteAllText(@".\backup\json\saveedit.json", jsons);

            //Show log of changes in txtbox
            Match g = myRegex1.Match(jsons);
            Match x = myRegex2.Match(jsons);
            Match y = myRegex3.Match(jsons);
            Match z = myRegex4.Match(jsons);
            Match ssi = myRegex5.Match(jsons);
            Match pi = myRegex6.Match(jsons);
            Match ps = myRegexs.Match(myRegexps.Match(jsons).ToString());
            AppendLine(tb, "Player Move Data: ");
            AppendLine(tb, g.ToString() + x.ToString() + y.ToString() + z.ToString() + ssi.ToString() + pi.ToString() + ps.ToString());
            pb.Invoke((Action)(() => pb.Value = 70));
        }
        private void RunEncrypt(ProgressBar pb, int saveslot)
        {
            DoGameSlotCommon(saveslot);

            try
            {
                //Read edited saveedit.json
                _gs = _gsm.ReadUnencryptedGameSave(@".\backup\json\saveedit.json");
            }
            catch
            {
                return;
            }
            try
            {
                //Write and Encrypt new save files
                _gsm.WriteSaveFile(_gs, _gameSlot);
                pb.Invoke((Action)(() => pb.Value = 90));
            }
            catch
            {
                return;
            }
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            //Add Note Button
            if (listBox3.GetItemText(listBox3.SelectedItem) == "")
            {
                MessageBox.Show("No record selected!", "Alert", MessageBoxButtons.OK);
                //AppendLine(textBox13, "No record selected!");
                return;
            }

            if (textBox28.Text == "")
            {
                MessageBox.Show("No Note added. Please type a note!", "Alert", MessageBoxButtons.OK);
                //AppendLine(textBox13, "No Note added. Please type a note!");
                return;
            }

            //add a note to a single location record
            string record = listBox3.GetItemText(listBox3.SelectedItem);
            string filename = listBox4.GetItemText(listBox4.SelectedItem);
            int selectedrecord = listBox3.SelectedIndex;
            var selectedfile = listBox4.SelectedItem;

            if (record != "" && filename != "")
            {
                string path = @".\backup\" + filename;

                if(!File.Exists(path))
                {
                    MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                    LoadTxt();
                    return;
                }

                List<string> filelist = File.ReadAllLines(path).ToList();
                int index = filelist.FindIndex(a => a.Contains(record));

                if (filelist.Contains(record) && filelist.Count > 0)
                {
                    //grabs the end of the line after GC:
                    Regex myRegexGC = new Regex("GC:.*?$", RegexOptions.Multiline);
                    Match m1 = myRegexGC.Match(listBox3.GetItemText(listBox3.SelectedItem));
                    string line1 = m1.ToString();
                    line1 = line1.Replace("GC: ", "");
                    //line1 = line1.Replace(" ", "");
                    string[] value = line1.Split(':');

                    //Check for errors on GC
                    string gc = line1.Replace(":", "");
                    if (gc.Length < 16)
                    {
                        DialogResult dialogResult2 = MessageBox.Show("Something went wrong! \r\n\nSelected line has errors. \r\n\nWould you like to Delete the Line?", "Add Note", MessageBoxButtons.YesNo);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            DeleteSingleRecordToolStripMenuItem_Click(this, new EventArgs());
                            return;
                        }
                        else if (dialogResult2 == DialogResult.No)
                        {
                            return;
                        }
                    }

                    //grab the end of the line and the last 4 of GC
                    string D = value[3].Trim().Substring(0, 4);

                    if (value[3].Length > 4)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure want to replace the note? ", "Replace/Change Note", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //removes the last 4 of GC
                            string E = value[3].Replace(D, "");

                            //removes the note from the record
                            record = record.Replace(E, "");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            return;
                        }                        
                    }

                    //replaces the record at the selected filelist index
                    filelist[index] = record.Replace(record, record + " -- " + textBox28.Text);

                    //writes the new file from the modified filelist
                    File.WriteAllLines(path, filelist);
                    AppendLine(textBox13, "Note Added.");

                    LoadTxt();

                    if (File.Exists(path))
                    {
                        listBox4.SelectedItem = selectedfile;
                        Button6_Click(this, new EventArgs());

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
                //AppendLine(textBox13, "No record selected!");
                return;
            }

            //delete a note to a single location record
            string record = listBox3.GetItemText(listBox3.SelectedItem);
            string filename = listBox4.GetItemText(listBox4.SelectedItem);
            int selectedrecord = listBox3.SelectedIndex;
            var selectedfile = listBox4.SelectedItem;

            if (record != "" && filename != "")
            {
                string path = @".\backup\" + filename;

                if (!File.Exists(path))
                {
                    MessageBox.Show("File Not Found!", "Alert", MessageBoxButtons.OK);
                    LoadTxt();
                    return;
                }

                List<string> filelist = File.ReadAllLines(path).ToList();
                int index = filelist.FindIndex(a => a.Contains(record));

                if (filelist.Contains(record) && filelist.Count > 0)
                {
                    //grabs the end of the line after GC:
                    Regex myRegexGC = new Regex("GC:.*?$", RegexOptions.Multiline);
                    Match m1 = myRegexGC.Match(listBox3.GetItemText(listBox3.SelectedItem));
                    string line1 = m1.ToString();
                    line1 = line1.Replace("GC: ", "");
                    //line1 = line1.Replace(" ", "");
                    string[] value = line1.Split(':');

                    //Check for errors on GC
                    string gc = line1.Replace(":", "");
                    if (gc.Length < 16)
                    {
                        DialogResult dialogResult2 = MessageBox.Show("Something went wrong! \r\n\nSelected line has errors. \r\n\nWould you like to Delete the Line?", "Remove Note", MessageBoxButtons.YesNo);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            DeleteSingleRecordToolStripMenuItem_Click(this, new EventArgs());
                            return;
                        }
                        else if (dialogResult2 == DialogResult.No)
                        {
                            return;
                        }
                    }

                    //grab the end of the line and the last 4 of GC
                    string D = value[3].Trim().Substring(0, 4);

                    if (value[3].Length > 4)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure want to Remove the note? ", "Remove Note", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //removes the last 4 of GC
                            string E = value[3].Replace(D, "");

                            //removes the note from the record
                            record = record.Replace(E, "");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Note Found!", "Alert", MessageBoxButtons.OK);
                        //AppendLine(textBox13, "No Note Removed.");
                        return;
                    }

                    //replaces the record at the selected filelist index
                    filelist[index] = record;

                    File.WriteAllLines(path, filelist);
                    AppendLine(textBox13, "Note Removed.");

                    LoadTxt();

                    if (File.Exists(path))
                    {
                        listBox4.SelectedItem = selectedfile;
                        Button6_Click(this, new EventArgs());

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

        private void UpdateApp()
        {
            string apath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            if (!Directory.Exists(apath))
            {
                MessageBox.Show("Updater Not Found!", "Alert", MessageBoxButtons.OK);
                Process.Start("http://nmscoordinates.com");
                return;
            }

            DirectoryInfo workingdir = new DirectoryInfo(apath);
            string bpath = Path.Combine(workingdir.FullName, "updater");
            if (!Directory.Exists(bpath))
            {
                MessageBox.Show("Updater Not Found!", "Alert", MessageBoxButtons.OK);
                Process.Start("http://nmscoordinates.com");
                return;
            }

            FileInfo[] files = workingdir.GetFiles("NMSCoordinatesUpdater.exe", SearchOption.AllDirectories);
            if (files.Length != 0)
            {
                foreach (FileInfo file in files)//.OrderByDescending(f => f.LastWriteTime))
                {
                    if (file.DirectoryName == bpath)
                    {
                        var startInfo = new ProcessStartInfo();
                        startInfo.WorkingDirectory = Path.GetDirectoryName(files[0].FullName); // working directory
                        startInfo.FileName = "NMSCoordinatesUpdater.exe";
                        Process.Start(startInfo);
                        Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Updater Not Found!", "Alert", MessageBoxButtons.OK);
                Process.Start("http://nmscoordinates.com");
                return;
            }
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9(NMSCVersion);
            f9.ShowDialog();

            //Toggle until updater
            //CheckForUpdates(false);
        }
    }
}
    
    

