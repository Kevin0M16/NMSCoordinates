using System.Windows.Forms;
using Octokit;
using System.Diagnostics;
using System.Net;
using System.ComponentModel;
using System;
using System.IO.Compression;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Drawing;
using IWshRuntimeLibrary;
using File = System.IO.File;

/**********************************************************\
|                                                          |
| NMSCoordinatesUpdater 2020  -- Form1.cs                  |
|                                                          |
| A fast travel application for No Man's Sky               |
|                                                          |
| Developed by:                                            |
|   Code Author: Kevin Lozano / Kevin0M16                  |
|   Email: <kevin@nmscoordinates.com>                      |
|                                                          |
|                                                          |
\**********************************************************/

namespace NMSCoordinatesUpdater
{
    public partial class Form1 : Form
    {
        private string Config { get; set; }
        private string Uconfig { get; set; }
        private string Oldconfig { get; set; }
        private string WorkingDir { get; set; }
        private string WorkingDirName { get; set; }

        private string ReadKey { get; set; }

        private string Version { get; set; }
        private string UpdateZip { get; set; }

        private string Repo { get; set; }
        private string User { get; set; }

        private string App { get; set; }
        private string ApprootDir { get; set; }
        private string AppexcludeDirName { get; set; }

        private string ApptempDir { get; set; }
        private string TempDir { get; set; }
        private string TempRootDir { get; set; }

        private string AssetUrl { get; set; }
        private string AssetName { get; set; }

        private WebClient webClient;
        private Stopwatch sw = new Stopwatch();

        private List<string> LogList = new List<string>();
        private List<string> ExcludeFileList = new List<string>();

        private bool Fullupdate { get; set; }
        private bool Success { get; set; }

        public Form1()
        {
            InitializeComponent();

            Config = "config.cfg"; //Path.Combine(Directory.GetCurrentDirectory(), "config.cfg");
            Uconfig = "uconfig.cfg";
            Oldconfig = "config_old.cfg"; //Path.Combine(Directory.GetCurrentDirectory(), "config_old.cfg");
            Success = true;

            //Main Update Button
            //button1.Visible = false;
            button1.Visible = true;

            //Debug toggle
            //Debug log box
            textBox4.Visible = false;
            //textBox4.Visible = true;    

            //Debug Backup/Install/Update Button
            //button2.Visible = false;
            button2.Visible = true;

            //Debug Reverse Update button
            //button3.Visible = false;
            button3.Visible = true;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            AppendLine(textBox4, "*****" + DateTime.Now.ToString("MM-dd-yyyy HH:mm" + "*****"));
            AppendLine(textBox4, "***Start of Operational Directories and Files***");

            WorkingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);//Directory.GetCurrentDirectory();
            AppendLine(textBox4, "WorkingDir: " + WorkingDir);

            if (!Directory.Exists(WorkingDir))
            {
                AppendLine(textBox4, "Error: WorkingDir: " + WorkingDir + "not found!");
                AppendLine(textBox4, "Error: Failed at Form1_Shown");
                AppendLine(textBox4, "Error: Failed to Load!");
                ErrorLog();
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(WorkingDir);
            WorkingDirName = dir.Name;
            AppendLine(textBox4, "WorkingDirName: " + WorkingDirName);
            AppendLine(textBox4, "Config: " + Config);
            AppendLine(textBox4, "Update Config: " + Uconfig);
            AppendLine(textBox4, "Old Config: " + Oldconfig);

            AppendLine(textBox4, "***End of Operational Directories and Files***");

            GetConfig();
            CheckForUpdates();
        }
        private void StartUp()
        {
            GetConfig();
            CheckForUpdates();
        }
        public void AppendLine(TextBox source, string value)
        {
            string log = "log.txt";            
            if (source.Text.Length == 0)
            {
                source.Text = value;

                LogList.Add(value);
                File.WriteAllLines(log, LogList);
            }                
            else
            {
                source.AppendText("\r\n" + value);

                LogList = File.ReadAllLines(log).ToList();
                LogList.Add(value);
                File.WriteAllLines(log, LogList);
            }                
        }
        public static void AppendLineB(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        private void CreateConfig()
        {
            if (!Success)
            {
                return;
            }
            try
            {
                File.Create("uconfig.nmsc").Close();
                TextWriter tw = new StreamWriter("uconfig.nmsc");
                tw.WriteLine("config=uconfig.nmsc");
                tw.WriteLine("oldconfig=config_old.nmsc");
                tw.WriteLine("repo=");
                tw.WriteLine("user=");
                tw.WriteLine("version=1.0.0.0");
                tw.WriteLine("app=test.exe");
                tw.WriteLine("approotdir=" + @"..\");
                tw.WriteLine("apptempdir=" + @".\app_temp");
                tw.WriteLine("tempdir=" + @".\temp");
                tw.WriteLine("appexcludedirname=backup"); //excludefiles
                tw.WriteLine("excludefiles{test.txt}");
                tw.WriteLine("updatezip=latest.zip");
                tw.WriteLine("fullupdate=false");
                tw.Close();
            }
            catch
            {
                AppendLine(textBox4, "Error: Write new Config file failed! Config:" + Config + " not created!");
                AppendLine(textBox4, "Error: Failed at CreateConfig()");
                ErrorLog();
                return;
            }
            
        }
        private void GetConfig() // update to (string cfgpath) for readalllines new cfg?
        {
            //Read Config file and set parameters
            if (!Success)
            {
                return;
            }
            AppendLine(textBox4, "***Start of Read Config file***");

            /*
            if (!File.Exists(config))
            {
                AppendLine(textBox4, "Info: config file: " + config + " not found, creating new config.");
                CreateConfig(); // no error catch
            }
            */

            if (!Validate(WorkingDir, Config))
            {
                AppendLine(textBox4, "Error: No Config file found // " + Config + " at " + Path.Combine(WorkingDir, Config));
                AppendLine(textBox4, "Error: Failed at GetConfig()");
                ErrorLog();
                return;
            }

            /*
            Read("config", config);
            config = readKey;

            Read("oldconfig", config);
            oldconfig = readKey;
            */

            Read("repo", Config);
            Repo = ReadKey;
            label2.Text = Repo;

            Read("user", Config);
            User = ReadKey;

            Read("version", Config);
            Version = ReadKey;

            Read("app", Config);
            App = ReadKey;

            Read("approotdir", Config);
            ApprootDir = Path.GetFullPath(ReadKey);
            
            Read("apptempdir", Config);
            ApptempDir = Path.GetFullPath(ReadKey);

            Read("tempdir", Config);
            TempDir = Path.GetFullPath(ReadKey);

            Read("updatezip", Config);
            UpdateZip = ReadKey;

            /*
            Read("appexcludedirname", config);
            appexcludeDirName = readKey; //just a string of dname  

            Read("fullupdate", config);
            if (readKey == "true" || readKey == "True")
            {
                fullupdate = true;
            }
            else if (readKey == "false" || readKey == "False")
            {
                fullupdate = false;
            }

            ExcludeFiles(); //no error catch
            */

            AppendLine(textBox4, "ApprootDir Full Path: " + ApprootDir);
            AppendLine(textBox4, "ApptempDir Full Path: " + ApptempDir);
            AppendLine(textBox4, "TempDir Full Path: " + TempDir);

            AppendLine(textBox4, "***End of Read Config file***\r\n");
        }
        private void GetUpdateConfig(string file)
        {
            //Read update Config file and set parameters
            if (!Success)
            {
                return;
            }
            AppendLine(textBox4, "***Start of Read Uconfig file***");
            
            Read("version", file);
            Version = ReadKey;

            Read("appexcludedirname", file);
            AppexcludeDirName = ReadKey; //just a string of dname  

            Read("fullupdate", file);
            if (ReadKey == "true" || ReadKey == "True")
            {
                Fullupdate = true;
            }
            else if (ReadKey == "false" || ReadKey == "False")
            {
                Fullupdate = false;
            }

            ExcludeFiles(file);
            AppendLine(textBox4, "***End of Read Uconfig file***\r\n");
        }
        private void Read(string key, string path)
        {
            if (!Success)
            {
                return;
            }
            ReadKey = "";
            try
            {
                if (!File.Exists(path))
                {
                    AppendLine(textBox4, "Error: Read path not found! // " + path);
                    AppendLine(textBox4, "Error: Failed at Read(" + key + ", " + path +")");
                    ErrorLog();
                    return;
                }
                List<string> filelist = File.ReadAllLines(path).ToList();
                foreach (string lst in filelist)
                {
                    if (lst.Contains(key))
                    {
                        //grabs the end of the line before key
                        Regex myRegexLS = new Regex(".*?=", RegexOptions.Multiline);
                        Match m1 = myRegexLS.Match(lst);
                        string line = m1.ToString();
                        line = line.Replace("=", "");

                        if (line == key)
                        {
                            string record = filelist.IndexOf(lst).ToString();

                            //grabs the end of the line after key
                            Regex myRegexRC = new Regex("=.*?$", RegexOptions.Multiline);
                            Match m2= myRegexRC.Match(lst);
                            string line2 = m2.ToString();

                            //line2 = line2.Replace("=", "");
                            //string value = line2.Replace(" ", "");

                            string value = line2.Replace("=", "");
                            ReadKey = value;
                            AppendLine(textBox4, "Read: " + key + "=" + value);
                        }    
                    }
                }
            }
            catch
            {
                AppendLine(textBox4, "Error: Failed at Read(" + key + ", " + path + ")");
                ErrorLog();
                return;
            }
        }
        private void ExcludeFiles(string file)
        {
            if (!Success)
            {
                return;
            }

            AppendLine(textBox4, "***Start List Excluded files***");
            /*
            if (!Validate(workingDir, config))
            {
                AppendLine(textBox4, "Error: No config file found // " + config + " at " + Path.Combine(workingDir, config));
                AppendLine(textBox4, "Error: Failed at ExcludeFiles()");
                ErrorLog();
                return;
            }
            */

            List<string> read = File.ReadAllLines(file).ToList();
            foreach (string lst in read)
            {
                Regex myRegexLS = new Regex("excludefiles{.*?}", RegexOptions.Multiline);
                Match m1 = myRegexLS.Match(lst);
                string files = m1.ToString();
                files = files.Replace("excludefiles{", "");
                files = files.Replace("}", "");
                ExcludeFileList = files.Split(',').ToList();
            }

            if (ExcludeFileList.Count > 0)
            {                
                foreach (string lst in ExcludeFileList)
                {
                    AppendLine(textBox4, "file: " + lst);
                }
            }
            else
            {
                AppendLine(textBox4, "Info: No excluded files listed on Uconfig.");
            }

            AppendLine(textBox4, "***End List Excluded files***");

        }
        private void Write(string key, string newKey, string path)
        {
            if (!Success)
            {
                return;
            }
            try
            {
                if (!File.Exists(path))
                {
                    AppendLine(textBox4, "Error: Write path not found! // " + path);
                    AppendLine(textBox4, "Error: Failed at Write(" + key + ", " + newKey + ", " + path + ")");
                    ErrorLog();
                    return;
                }
                List<string> filelist = File.ReadAllLines(path).ToList();
                foreach (string lst in filelist)
                {
                    if (lst.Contains(key))
                    {
                        //grabs the end of the line before key
                        Regex myRegexLS = new Regex(".*?=", RegexOptions.Multiline);
                        Match m1 = myRegexLS.Match(lst);
                        string line = m1.ToString();
                        line = line.Replace("=", "");

                        if (line == key)
                        {
                            //grabs the end of the line after key
                            Regex myRegexRC = new Regex("=.*?$", RegexOptions.Multiline);
                            Match m2 = myRegexRC.Match(lst);
                            string line2 = m2.ToString();

                            //line2 = line2.Replace("=", "");
                            //string value = line2.Replace(" ", "");

                            string value = line2.Replace("=", "");

                            //replaces the record at the selected filelist index
                            int index = filelist.IndexOf(lst);
                            string record = lst.Replace(value, newKey);
                            filelist[index] = record;
                            AppendLine(textBox4, "Write: " + key + "=" + newKey);
                            File.WriteAllLines(path, filelist);
                            return;
                        }
                    }
                }
            }
            catch
            {
                AppendLine(textBox4, "Error: Failed at Write(" + key + ", " + newKey + ", " + path + ")");
                ErrorLog();
                return;
            }
        }
        private async void CheckForUpdates()
        {
            if (!Success)
            {
                return;
            }
            try
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                
                var client = new GitHubClient(new ProductHeaderValue(Repo));                
                var releases = await client.Repository.Release.GetAll(User, Repo);                

                var latest = releases[0];
                AssetUrl = releases[0].Assets[0].BrowserDownloadUrl;
                AssetName = releases[0].Assets[0].Name;

                textBox3.Text = Version;
                textBox2.Text = latest.Name;
                textBox1.Text = AssetName;

                if (Version == latest.Name)
                {
                    button1.Text = "Repair";
                    
                    DialogResult dialogResult = MessageBox.Show("You have the latest version from Github\r\n\n" +
                        "Open" + App + 
                        " Now? ", 
                        "Confirmation", 
                        MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        OpenApp();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    button1.Text = "Update";
                }
            }
            catch
            {
                AppendLine(textBox4, "Error: Github Server Problem. Could not check version");
                AppendLine(textBox4, "Error: Failed at CheckForUpdates()");
                MessageBox.Show("Github Server Problem. Could not check version", "Alert");
                ErrorLog();
                return;
            }
        }
        private void OpenApp()
        {
            try
            {
                string apppath = Path.Combine(ApprootDir, App);
                if (File.Exists(apppath))
                {
                    var startInfo = new ProcessStartInfo();
                    startInfo.WorkingDirectory = ApprootDir; // working directory
                    startInfo.FileName = App;
                    Process.Start(startInfo);
                    Close();
                }
                else
                {
                    MessageBox.Show(App + " was not found!", "Alert", MessageBoxButtons.OK);
                    Close();
                }
            }
            catch
            {
                AppendLine(textBox4, "Error: Problem Opening " + App + " !");
                MessageBox.Show("Problem Opening " + App + " !", "Alert", MessageBoxButtons.OK);
            }
        }
        private void Button1_Click(object sender, System.EventArgs e)
        {
            //Main Center Update button
            //Check for errors
            if (!Success)
            {
                return;
            }
            //Download Update
            AppendLine(textBox4, "***Start of Download***");
            DownloadFile(AssetUrl, UpdateZip);
            AppendLine(textBox4, "***End of Download***");

            //Install_Update();           
        }
        public void DownloadFile(string urlAddress, string location)
        {
            AppendLine(textBox4, "Download: " + urlAddress + " to " + location);
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                //Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);
                Uri uri = new Uri(urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(uri, location);
                }
                catch //(Exception ex)
                {
                    AppendLine(textBox4, "Error: webClient Problem!");
                    ErrorLog();
                    return;
                    //MessageBox.Show(ex.Message);
                }
            }
        }
        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            labelSpd.Text = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar1.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            labelPerc.Text = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            labelDownloaded.Text = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }
        // The event that will trigger when the WebClient is completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.", Repo + " Update", MessageBoxButtons.OK);
            }
            else
            {
                webClient.Dispose();
                DialogResult dialogResult = MessageBox.Show("Download completed! Install Now? ", "Update", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Install_Update();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool ovrwrite, string dname1, string dname2)
        {
            //Copy directories method
            if (!Success)
            {
                return;
            }
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                AppendLine(textBox4, "Error: sourceDirName not found! // " + sourceDirName);
                AppendLine(textBox4, "Error: Failed at DirectoryCopy(" + sourceDirName + ", " + destDirName + ", " + copySubDirs + ", " + ovrwrite + ", " + dname1 + ", " + dname2 + ")");
                ErrorLog();
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it. unless WorkingDir
            if (!Directory.Exists(destDirName) && destDirName != WorkingDir && sourceDirName != WorkingDir)
            {
                Directory.CreateDirectory(destDirName);
                AppendLine(textBox4, "destDirName: " + destDirName + " not found, created");
            }
            if (destDirName == WorkingDir)
            {
                AppendLine(textBox4, "destDirName: " + destDirName + " is WorkingDir, not created");
            }
            if (sourceDirName == WorkingDir)
            {
                AppendLine(textBox4, "sourceDirName: " + sourceDirName + " is WorkingDir, not created");
            }

            // Get all the files in the SOURCE directory and copy them to DESTINATION the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                //if SOURCE file directory path is same as WorkingDir path, don't copy
                if (file.Directory.FullName == WorkingDir)
                {
                    AppendLine(textBox4, "File Copy: " + file.Name + " is in WorkingDir, not copied");
                }
                //if SOURCE file name is in directory path is in the excludedfilelist, don't copy
                if (ExcludeFileList.Contains(file.Name))
                {
                    AppendLine(textBox4, "File Copy: " + file.Name + " is in the excluded files list, not copied.");
                }
                if (file.Directory.FullName != WorkingDir && !ExcludeFileList.Contains(file.Name))//// filelist mod
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, ovrwrite);
                    AppendLine(textBox4, "File Copy: " + file + " to " + temppath + " // " + file.LastWriteTime);
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                //dirs is all the directories in SOURCE. Get all sudirs in the source directories one at a time then re-run
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    if (subdir.FullName == WorkingDir || temppath == WorkingDir)
                    {
                        AppendLine(textBox4, "Sub Dir Copy: " + subdir.Name + " is in WorkingDir, not copied.");
                    }
                    if (subdir.FullName != WorkingDir && temppath != WorkingDir)
                    {
                        if (subdir.Name != dname1 && subdir.Name != dname2)
                        {
                            AppendLine(textBox4, "Directory Copy: " + subdir.FullName + " to " + temppath + " exclude // " + dname1 + " // " + dname2);
                            DirectoryCopy(subdir.FullName, temppath, copySubDirs, ovrwrite, dname1, dname2);
                        }
                    }
                }
            }
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool ovrwrite, string dname)
        {
            //Copy directories method
            if (!Success)
            {
                return;
            }
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                AppendLine(textBox4, "Error: sourceDirName not found! // " + sourceDirName);
                AppendLine(textBox4, "Error: Failed at DirectoryCopy(" + sourceDirName + ", " + destDirName + ", " + copySubDirs + ", " + ovrwrite + ", " + dname + ")");
                ErrorLog();
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it. unless WorkingDir
            if (!Directory.Exists(destDirName) && destDirName != WorkingDir && sourceDirName != WorkingDir)
            {
                Directory.CreateDirectory(destDirName);
                AppendLine(textBox4, "destDirName: " + destDirName + " not found, created");
            }
            if (destDirName == WorkingDir)
            {
                AppendLine(textBox4, "destDirName: " + destDirName + " is WorkingDir, not created");
            }
            if (sourceDirName == WorkingDir)
            {
                AppendLine(textBox4, "sourceDirName: " + sourceDirName + " is WorkingDir, not created");
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Directory.FullName == WorkingDir)
                {
                    AppendLine(textBox4, "File Copy: " + file.Name + " is in WorkingDir, not copied");
                }
                if (file.Directory.FullName != WorkingDir)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, ovrwrite);
                    AppendLine(textBox4, "File Copy: " + file + " to " + temppath + " // " + file.LastWriteTime);
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    if (subdir.FullName == WorkingDir || temppath == WorkingDir)
                    {
                        AppendLine(textBox4, "Sub Dir Copy: " + subdir.Name + " is in WorkingDir, not copied.");
                    }
                    if (subdir.FullName != WorkingDir && temppath != WorkingDir)
                    {
                        if (subdir.Name == dname)
                        {
                            AppendLine(textBox4, "Sub Dir Copy: " + subdir.Name + " is in dname: " + dname + " not copied.");
                        }
                        if (subdir.Name != dname)
                        {
                            AppendLine(textBox4, "Directory Copy: " + subdir.FullName + " to " + temppath + " exclude // " + dname);
                            DirectoryCopy(subdir.FullName, temppath, copySubDirs, ovrwrite, dname);
                        }
                    }
                }
            }
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool ovrwrite)
        {
            //Copy directories method
            if (!Success)
            {
                return;
            }
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                AppendLine(textBox4, "Error: sourceDirName not found! // " + sourceDirName);
                AppendLine(textBox4, "Error: Failed at DirectoryCopy(" + sourceDirName + ", " + destDirName + ", " + copySubDirs + ", " + ovrwrite + ")");
                ErrorLog();
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it. unless WorkingDir
            if (!Directory.Exists(destDirName) && destDirName != WorkingDir && sourceDirName != WorkingDir)
            {
                Directory.CreateDirectory(destDirName);
                AppendLine(textBox4, "destDirName: " + destDirName + " not found, created");
            }
            if (destDirName == WorkingDir)
            {
                AppendLine(textBox4, "destDirName: " + destDirName + " is WorkingDir, not created");
            }
            if (sourceDirName == WorkingDir)
            {
                AppendLine(textBox4, "sourceDirName: " + sourceDirName + " is WorkingDir, not created");
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Directory.FullName == WorkingDir)
                {
                    AppendLine(textBox4, "File Copy: " + file.Name + " is in WorkingDir, not copied");
                }
                if (file.Directory.FullName != WorkingDir)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, ovrwrite);
                    AppendLine(textBox4, "File Copy: " + file + " to " + temppath + " // " + file.LastWriteTime);
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    if (subdir.FullName == WorkingDir || temppath == WorkingDir)
                    {
                        AppendLine(textBox4, "Sub Dir Copy: " + subdir.Name + " is in WorkingDir, not copied.");
                    }
                    if (subdir.FullName != WorkingDir && temppath != WorkingDir)
                    {
                        AppendLine(textBox4, "Directory Copy: " + subdir.FullName + " to " + temppath);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, ovrwrite);                     
                    }
                }
            }
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool ovrwrite, bool exlist)
        {
            //MAIN Copy directories method
            if (!Success)
            {
                return;
            }
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                AppendLine(textBox4, "Error: sourceDirName not found! // " + sourceDirName);
                AppendLine(textBox4, "Error: Failed at DirectoryCopy(" + sourceDirName + ", " + destDirName + ", " + copySubDirs + ", " + ovrwrite + ", " + exlist  + ")");
                ErrorLog();
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it. unless WorkingDir
            if (!Directory.Exists(destDirName) && destDirName != WorkingDir && sourceDirName != WorkingDir)
            {
                Directory.CreateDirectory(destDirName);
                AppendLine(textBox4, "destDirName: " + destDirName + " not found, created");
            }
            if (destDirName == WorkingDir)
            {
                AppendLine(textBox4, "destDirName: " + destDirName + " is WorkingDir, not created");
            }
            if (sourceDirName == WorkingDir)
            {
                AppendLine(textBox4, "sourceDirName: " + sourceDirName + " is WorkingDir, not created");
            }

            // Get all the files in the SOURCE directory and copy them to DESTINATION the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                //if SOURCE file directory path is same as WorkingDir path, don't copy
                if (file.Directory.FullName == WorkingDir)
                {
                    AppendLine(textBox4, "File Copy: " + file.Name + " is in WorkingDir, not copied");
                }
                if (file.Directory.FullName != WorkingDir)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    if (exlist)
                    {
                        //if SOURCE file name is in directory path is in the excludedfilelist, don't copy
                        if (ExcludeFileList.Contains(file.Name))
                        {
                            AppendLine(textBox4, "File Copy: " + file.Name + " is in the ExcludeFileList, not copied.");
                        }
                        if (!ExcludeFileList.Contains(file.Name))
                        {

                            file.CopyTo(temppath, ovrwrite);
                            AppendLine(textBox4, "File Copy: " + file + " to " + temppath + " // " + file.LastWriteTime);
                        }
                    }
                    else
                    {
                        file.CopyTo(temppath, ovrwrite);
                        AppendLine(textBox4, "File Copy: " + file + " to " + temppath + " // " + file.LastWriteTime);
                    }
                }                
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                //dirs is all the directories in SOURCE. Get all sudirs in the source directories one at a time then re-run
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    if (subdir.Name == AppexcludeDirName)
                    {
                        AppendLine(textBox4, "Sub Dir Copy: " + subdir.Name + " is AppexcludeDirName, not copied.");
                    }
                    if (subdir.FullName == WorkingDir || temppath == WorkingDir)
                    {
                        AppendLine(textBox4, "Sub Dir Copy: " + subdir.Name + " is in WorkingDir, not copied.");
                    }
                    if (subdir.FullName != WorkingDir && temppath != WorkingDir && subdir.Name != AppexcludeDirName)
                    {
                        AppendLine(textBox4, "Directory Copy: " + subdir.FullName + " to " + temppath);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, ovrwrite, exlist);                     
                    }
                }
            }
        }
        private void DirectoryDelete(string sourceDirName, bool copySubDirs, bool ovrwrite, bool exlist)
        {
            //Copy directories method
            if (!Success)
            {
                return;
            }
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                AppendLine(textBox4, "Error: sourceDirName not found! // " + sourceDirName);
                AppendLine(textBox4, "Error: Failed at DirectoryDelete(" + sourceDirName + ", " + copySubDirs + ", " + ovrwrite + ", " + exlist + ")");
                ErrorLog();
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            /*
            // If the destination directory doesn't exist, create it. unless workingDir
            if (!Directory.Exists(destDirName) && destDirName != workingDir && sourceDirName != workingDir)
            {
                Directory.CreateDirectory(destDirName);
                AppendLine(textBox4, "destDirName: " + destDirName + " not found, created");
            }
            if (destDirName == workingDir)
            {
                AppendLine(textBox4, "destDirName: " + destDirName + " is workingDir, not created");
            }
            if (sourceDirName == workingDir)
            {
                AppendLine(textBox4, "sourceDirName: " + sourceDirName + " is workingDir, not created");
            }
            */

            // Get all the files in the SOURCE directory and copy them to DESTINATION the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {                
                if (file.Directory.FullName == WorkingDir)
                {
                    AppendLine(textBox4, "File Delete: " + file.Name + " is in WorkingDir, not deleted");
                }
                if (file.Directory.FullName != WorkingDir)
                {
                    //string temppath = Path.Combine(destDirName, file.Name);
                    if (exlist)
                    {                        
                        if (ExcludeFileList.Contains(file.Name))
                        {
                            AppendLine(textBox4, "File Delete: " + file.Name + " is in the ExcludeFileList, not deleted.");
                        }
                        if (!ExcludeFileList.Contains(file.Name))//// filelist mod
                        {

                            File.Delete(file.FullName);
                            AppendLine(textBox4, "File Delete: " + file.FullName);
                        }
                    }
                    else
                    {
                        File.Delete(file.FullName);
                        AppendLine(textBox4, "File Delete: " + file.FullName);
                    }
                }
            }
            
            if (copySubDirs)
            {
                
                foreach (DirectoryInfo subdir in dirs)
                {
                    //string temppath = Path.Combine(destDirName, subdir.Name);
                    if (subdir.Name == AppexcludeDirName)
                    {
                        AppendLine(textBox4, "Sub Dir Delete: " + subdir.Name + " is AppexcludeDirName, not deleted.");
                    }
                    if (subdir.FullName == WorkingDir)
                    {
                        AppendLine(textBox4, "Sub Dir Delete: " + subdir.Name + " is in WorkingDir, not deleted.");
                    }
                    if (subdir.FullName != WorkingDir && subdir.Name != AppexcludeDirName)
                    {                        
                        AppendLine(textBox4, "Directory Delete: " + subdir.FullName);
                        DirectoryDelete(subdir.FullName, copySubDirs, ovrwrite, exlist);                        
                    }
                }
            }
        }
        private void ErrorLog()
        {
            //Main Error log triggers on fail
            Success = false; //set Success to false for failure

            AppendLine(textBox4, "***Start of Error Log***");
            ValidateLog();
            AppendLine(textBox4, "Log: config= " + Config);
            AppendLine(textBox4, "Log: uconfig= " + Uconfig);
            AppendLine(textBox4, "Log: oldconfig= " + Oldconfig);
            AppendLine(textBox4, "Log: workingDir= " + WorkingDir);
            AppendLine(textBox4, "Log: workingDirName= " + WorkingDirName);
            AppendLine(textBox4, "Log: readKey= " + ReadKey);
            AppendLine(textBox4, "Log: version= " + Version);
            AppendLine(textBox4, "Log: updateZip= " + UpdateZip);
            AppendLine(textBox4, "Log: repo= " + Repo);
            AppendLine(textBox4, "Log: user= " + User);
            AppendLine(textBox4, "Log: app= " + App);
            AppendLine(textBox4, "Log: approotDir= " + ApprootDir);
            AppendLine(textBox4, "Log: appexcludeDirName= " + AppexcludeDirName);
            AppendLine(textBox4, "Log: apptempDir= " + ApptempDir);
            AppendLine(textBox4, "Log: tempDir= " + TempDir);
            AppendLine(textBox4, "Log: tempRootDir= " + TempRootDir);
            AppendLine(textBox4, "Log: assetUrl= " + AssetUrl);
            AppendLine(textBox4, "Log: assetName= " + AssetName);
            AppendLine(textBox4, "Log: fullupdate= " + Fullupdate);
            AppendLine(textBox4, "Log: success= " + Success);
            AppendLine(textBox4, "***Start List Excluded files***");
            try
            {
                foreach(string file in ExcludeFileList)
                {
                    
                    AppendLine(textBox4, "file: " + file);
                    
                }
            }
            catch
            {
                AppendLine(textBox4, "File list problem!");
            }
            AppendLine(textBox4, "***End List Excluded files***");
            AppendLine(textBox4, "***End of Error Log***\r\n");

            MessageBox.Show("An Error has occurred!\r\n\nSee log.txt for more information.\r\n\n", "Error", MessageBoxButtons.OK);
        }
        private void ValidateLog()
        {
            try
            {
                AppendLine(textBox4, "***Start of Validate Directories and Files Log***");

                string tempappPath = Path.Combine(TempDir, App);
                string apptempappPath = Path.Combine(ApptempDir, App);
                string approotappPath = Path.Combine(ApprootDir, App);

                if (!Directory.Exists(TempDir))
                {
                    AppendLine(textBox4, "Error: TempDir: " + TempDir + " doesn't exist!");
                }
                else
                {
                    AppendLine(textBox4, "Ready: TempDir: " + TempDir + " found.");
                }
                if (!File.Exists(tempappPath))
                {
                    AppendLine(textBox4, "Error: tempappPath: " + tempappPath + " doesn't exist!");
                }
                else
                {
                    AppendLine(textBox4, "Ready: tempappPath: " + tempappPath + " found.");
                }
                if (!Directory.Exists(ApptempDir))
                {
                    AppendLine(textBox4, "Error: ApptempDir: " + ApptempDir + " doesn't exist!");
                }
                else
                {
                    AppendLine(textBox4, "Ready: ApptempDir: " + ApptempDir + " found.");
                }
                if (!File.Exists(apptempappPath))
                {
                    AppendLine(textBox4, "Error: apptempappPath: " + apptempappPath + " doesn't exist!");
                }
                else
                {
                    AppendLine(textBox4, "Ready: apptempappPath: " + apptempappPath + " found.");
                }
                if (!Directory.Exists(ApprootDir))
                {
                    AppendLine(textBox4, "Error: ApprootDir: " + ApprootDir + " doesn't exist!");
                }
                else
                {
                    AppendLine(textBox4, "Ready: ApprootDir: " + ApprootDir + " found.");
                }
                if (!File.Exists(approotappPath))
                {
                    AppendLine(textBox4, "Error: approotappPath: " + approotappPath + " doesn't exist!");
                }
                else
                {
                    AppendLine(textBox4, "Ready: approotappPath: " + approotappPath + " found.");
                }

                AppendLine(textBox4, "***End of Validate Directories and Files Log***");
            }
            catch
            {
                AppendLine(textBox4, "Error: Something went wrong.");
                return;
            }            
        }        
        private static bool Validate(string dirpath, string file)
        {
            string filepath = Path.Combine(dirpath, file);

            if (!Directory.Exists(dirpath))
            {
                return false;
            }
            if (!File.Exists(filepath))
            {
                return false;
            }

            return true;
        }
        private static bool Validate(string apptempDir, string approotDir, string app)
        {            
            string apptempappPath = Path.Combine(apptempDir, app);
            string approotappPath = Path.Combine(approotDir, app);
                        
            if (!Directory.Exists(apptempDir))
            {
                return false;
            }
            if (!File.Exists(apptempappPath))
            {
                return false;
            }
            if (!Directory.Exists(approotDir))
            {
                return false;
            }
            if (!File.Exists(approotappPath))
            {
                return false;
            }

            return true;
        }
        private static bool Validate(string tempDir, string apptempDir, string approotDir, string app)
        {
            string tempappPath = Path.Combine(tempDir, app);
            string apptempappPath = Path.Combine(apptempDir, app);
            string approotappPath = Path.Combine(approotDir, app);

            if (!Directory.Exists(tempDir))
            {
                return false;
            }
            if (!File.Exists(tempappPath))
            {
                return false;
            }
            if (!Directory.Exists(apptempDir))
            {
                return false;
            }
            if(!File.Exists(apptempappPath))
            {
                return false;
            }
            if (!Directory.Exists(approotDir))
            {
                return false;
            }
            if (!File.Exists(approotappPath))
            {
                return false;
            }

            return true;
        }        
        private void Backup()
        {
            //Main Backup method
            if (!Success)
            {
                return;
            }
            AppendLine(textBox4, "***Start of Backup***");

            if (!Validate(ApprootDir, App))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No App file found! // " + App + " at " + Path.Combine(ApprootDir, App));
                AppendLine(textBox4, "Error: Failed at Backup()");
                AppendLine(textBox4, "Error: Backup Failed!");
                ErrorLog();
                return;
            }

            //Deletes old apptemp if found
            if (Directory.Exists(ApptempDir))
            {
                Directory.Delete(ApptempDir, true);
                AppendLine(textBox4, "Remove: ApptempDir: " + ApptempDir + " found and deleted");
            }

            /*
            //Copies entire approotdir to apptempDir for backup exclude apptemp and temp workingdir from DC method
            AppendLine(textBox4, "***Start of Directory Copy - approotDir to apptempDir***");
            DirectoryInfo temp1dinfo = new DirectoryInfo(apptempDir);
            string temp1 = temp1dinfo.Name; //directory name of apptempDir path
            DirectoryInfo temp2dinfo = new DirectoryInfo(tempDir);
            string temp2 = temp2dinfo.Name; //directory name of tempDir path
            AppendLine(textBox4, "Directory Copy: " + approotDir + " to " + apptempDir + " exclude // " + temp1 + " // " + temp2);            
            DirectoryCopy(approotDir, apptempDir, true, true, temp1, temp2);
            AppendLine(textBox4, "***End of Directory Copy - approotDir to apptempDir***");
            AppendLine(textBox4, "***End of Backup***\r\n");
            */

            //Copies entire ApprootDir to ApptempDir for backup exclude apptemp and temp WorkingDir from DC method
            AppendLine(textBox4, "***Start of Directory Copy - ApprootDir to ApptempDir***");            
            AppendLine(textBox4, "Directory Copy: " + ApprootDir + " to " + ApptempDir + " // No Exclusions");
            DirectoryCopy(ApprootDir, ApptempDir, true, true);//, false); //no exclusions
            AppendLine(textBox4, "***End of Directory Copy - ApprootDir to ApptempDir***");
            AppendLine(textBox4, "***End of Backup***\r\n");
        }
        private void Install()
        {
            //Main install method
            if (!Success)
            {
                return;
            }

            AppendLine(textBox4, "***Start of Install***");

            if (!Validate(ApptempDir, ApprootDir, App))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No App file found! // " + Path.Combine(ApptempDir, App) + " or " + Path.Combine(ApprootDir, App));
                AppendLine(textBox4, "Error: Failed at Install()");
                AppendLine(textBox4, "Error: Install Failed!");
                ErrorLog();
                return;
            }

            /*
            //Copies entore approotdir to apptempDir for backup exclude apptemp and temp workingdir from DC method
            AppendLine(textBox4, "***Start of Directory Copy - approotDir to apptempDir***");
            DirectoryInfo temp1dinfo = new DirectoryInfo(apptempDir);
            string temp1 = temp1dinfo.Name; //directory name of apptempDir path
            DirectoryInfo temp2dinfo = new DirectoryInfo(tempDir);
            string temp2 = temp2dinfo.Name; //directory name of tempDir path
            AppendLine(textBox4, "Copy: " + approotDir + " to " + apptempDir + " exclude // " + temp1 + " // " + temp2);
            DirectoryCopy(approotDir, apptempDir, true, true, temp1, temp2);
            AppendLine(textBox4, "***End of Directory Copy - approotDir to apptempDir***\r\n");
            */

            //Deletes if old temp directory exists
            if (Directory.Exists(TempDir))
            {
                Directory.Delete(TempDir, true);
                AppendLine(textBox4, "Remove: Old TempDir: " + TempDir + " found and deleted");
            }

            //Checks if UpdateZip exists
            if (!File.Exists(UpdateZip))
            {
                AppendLine(textBox4, "Error: UpdateZip not found! // " + UpdateZip);
                AppendLine(textBox4, "Error: Failed at Install()");
                AppendLine(textBox4, "Error: Install Failed!");
                ErrorLog();
                return;
            }
            else
            {
                AppendLine(textBox4, "Ready: UpdateZip:" + UpdateZip + " found.");
            }

            //Extract Zip File to TempDir \temp
            ZipFile.ExtractToDirectory(UpdateZip, TempDir);
            AppendLine(textBox4, "Extract: " + UpdateZip + " to " + TempDir);

            //Check if \temp exists
            if (!Directory.Exists(TempDir))
            {
                AppendLine(textBox4, "Error: TempDir not found! // " + TempDir);
                AppendLine(textBox4, "Error: Failed at Install()");
                AppendLine(textBox4, "Error: Install Failed!");
                ErrorLog();
                return;
            }
            else
            {
                AppendLine(textBox4, "Ready: TempDir: " + TempDir + " found.");
            }

            //find the root folder in temp that contains the App and set TempRootDir
            DirectoryInfo dinfo = new DirectoryInfo(TempDir);
            FileInfo[] Files = dinfo.GetFiles(App, SearchOption.AllDirectories);
            if (Files.Length != 0)
            {
                foreach (FileInfo file in Files)//.OrderByDescending(f => f.LastWriteTime))
                {
                    TempRootDir = file.DirectoryName;                    
                    AppendLine(textBox4, "TempRootDir: " + TempRootDir);
                }
                
                //Looks to see if TempDir is already the root if not move it
                if (TempRootDir != TempDir)
                {
                    DirectoryInfo temprootdinfo = new DirectoryInfo(TempRootDir);
                    string temprootdirname = temprootdinfo.Name;
                    AppendLine(textBox4, "***Start of Directory Copy - TempRootDir to TempDir***");
                    AppendLine(textBox4, "Directory Copy: " + TempRootDir + " to " + TempDir + " exclude // " + temprootdirname); //TempRootDir);
                    DirectoryCopy(TempRootDir, TempDir, true, true, temprootdirname); //TempRootDir);
                    AppendLine(textBox4, "***End of Directory Copy - TempRootDir to TempDir***");

                    //Delete TempRootDir now its empty
                    if (Directory.Exists(TempRootDir))
                    {
                        Directory.Delete(TempRootDir, true);
                        AppendLine(textBox4, "Remove: TempRootDir: " + TempRootDir + " found and deleted");
                    }
                    else
                    {
                        AppendLine(textBox4, "Atypical: TempRootDir: " + TempRootDir + " not deleted, not found.");
                    }
                }
            }
            AppendLine(textBox4, "***End of Install***\r\n");
        }

        private void UpdateApp()
        {
            //Main Update Method
            if (!Success)
                return;
            

            AppendLine(textBox4, "***Start of Update***");

            //Validate all directories for App file
            if (!Validate(TempDir, ApptempDir, ApprootDir, App))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No App file found // " + Path.Combine(TempDir, App) + " or " + Path.Combine(ApptempDir, App) + " or " + Path.Combine(ApprootDir, App));
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }

            //Validate Config is in WorkingDir
            if (!Validate(WorkingDir, Config))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No Config file found // " + Config + " at " + Path.Combine(WorkingDir, Config));
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }

            //Check Uconfig is in TempDir look in entire folder and subs
            //Make sure update always contains one Uconfig file ***Uconfig file critical
            DirectoryInfo tempdinfo = new DirectoryInfo(TempDir); //TempDir = Path.GetFullPath(@".\temp");
            FileInfo[] tempdfilesCheck = tempdinfo.GetFiles(Uconfig, SearchOption.AllDirectories);
            if (tempdfilesCheck.Length == 0)
            {
                AppendLine(textBox4, "Error: No Update Config file found in TempDir! // " + Uconfig + " // tempdfilesCheck.Length = 0");
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }
            foreach (FileInfo file in tempdfilesCheck)
            {
                AppendLine(textBox4, "TempDir Uconfig: " + file.FullName);
                AppendLine(textBox4, "existing Config: " + Path.GetFullPath(Config));

                Read("version", Config); //read from current Config
                string existversion = ReadKey;
                AppendLine(textBox4, "Existing version: " + existversion);

                Read("version", file.FullName); //read from new Config
                string tempversion = ReadKey;
                AppendLine(textBox4, "Update version: " + tempversion);

                //make copy of Config and rename based on current Config old name
                File.Copy(Config, Oldconfig, true);

                //Make sure update always contains one uconfig file ***uconfig file critical
                /*
                List<string> linelist = File.ReadAllLines(file.FullName).ToList();
                File.WriteAllLines(config, linelist);
                */
                AppendLine(textBox4, "Update Config to new version: " + tempversion);
                Write("version", tempversion, Config);                

                //get update Config parameters and set
                AppendLine(textBox4, "Read New Config parameters: " + Config);
                //GetConfig(); //error control
                GetUpdateConfig(file.FullName);
            }            

            //Update files
            AppendLine(textBox4, "***Start of Files Update***");

            /*
            //ADDED SAFETY After setting config, remove updater from temp
            string updaterpath = Path.Combine(tempDir, workingDirName);
            if (Directory.Exists(updaterpath))
            {
                Directory.Delete(updaterpath, true);
                AppendLine(textBox4, "Delete: workingDirName: " + workingDirName + " found in " + tempDir + " deleted at: " + updaterpath);
            }
            else
            {
                AppendLine(textBox4, "Ready: No workingDirName: " + workingDirName + " found in " + tempDir);
            }
            */

            if (AppexcludeDirName != "none")
            {
                //Removes AppexcludeDirName from temp if exists in approot
                string appexcludedDirPath = Path.Combine(ApprootDir, AppexcludeDirName);
                string tempexcludedDirPath = Path.Combine(TempDir, AppexcludeDirName);

                if (Path.GetFullPath(appexcludedDirPath) == Path.GetFullPath(ApprootDir))
                {
                    AppendLine(textBox4, "Error: AppexcludeDirName not set in Config or null! // appexcludedDirPath: " + appexcludedDirPath + " // AppexcludeDirName: " + AppexcludeDirName);
                    AppendLine(textBox4, "Error: Failed at UpdateApp()");
                    AppendLine(textBox4, "Error: Update Failed!");
                    ErrorLog();
                    return;
                }
                if (Path.GetFullPath(tempexcludedDirPath) == Path.GetFullPath(TempDir))
                {
                    AppendLine(textBox4, "Error: tempexcludedDirPath not set in Config or null! // tempexcludedDirPath: " + tempexcludedDirPath + " // AppexcludeDirName: " + AppexcludeDirName);
                    AppendLine(textBox4, "Error: Failed at UpdateApp()");
                    AppendLine(textBox4, "Error: Update Failed!");
                    ErrorLog();
                    return;
                }
                if (Directory.Exists(appexcludedDirPath))
                {
                    Directory.Delete(tempexcludedDirPath, true);
                    AppendLine(textBox4, "Remove: AppexcludeDirName: " + AppexcludeDirName + " found at " + appexcludedDirPath + " deleted at: " + tempexcludedDirPath);
                }
                if (Directory.Exists(tempexcludedDirPath))
                {
                    Directory.Delete(tempexcludedDirPath, true);
                    AppendLine(textBox4, "Remove: AppexcludeDirName: " + AppexcludeDirName + " found at " + tempexcludedDirPath + " deleted at: " + tempexcludedDirPath);
                }
            }
            if (AppexcludeDirName == "none")
            {
                AppendLine(textBox4, "Ready: No directory excluded. AppexcludeDirName is " + AppexcludeDirName);
            }
            if (Fullupdate)
            {
                //Double check for App in ApprootDir before deleting for Fullupdate
                if (!Validate(ApprootDir, App))
                {
                    AppendLine(textBox4, "Error: Failed to Validate, No App file found! // " + App + " at " + Path.Combine(ApprootDir, App));
                    AppendLine(textBox4, "Error: Failed at Backup()");
                    AppendLine(textBox4, "Error: Backup Failed!");
                    ErrorLog();
                    return;
                }
                AppendLine(textBox4, "Confirmation: Validate Successful, App file found. // " + App + " at " + Path.Combine(ApprootDir, App));

                AppendLine(textBox4, "***Start of Directory Delete - ApprootDir***");
                AppendLine(textBox4, "Directory Delete for Fullupdate // " + ApprootDir + " // use ExcludeFileList");
                DirectoryDelete(ApprootDir, true, true, true);
                AppendLine(textBox4, "***End of Directory Delete - ApprootDir***");
            }

            /*AppendLine(textBox4, "*********STOP*************");
            ErrorLog();
            return;*/

            /*
            //Copies directories and files from tempDir to approotDir
            DirectoryInfo temp1dinfo = new DirectoryInfo(apptempDir);
            string temp1 = temp1dinfo.Name;
            DirectoryInfo temp2dinfo = new DirectoryInfo(tempDir);
            string temp2 = temp2dinfo.Name;
            AppendLine(textBox4, "***Start of Directory Copy - tempDir to approotDir***");
            AppendLine(textBox4, "Directory Copy: " + tempDir + " to " + approotDir + " exclude // " + temp1 + " // " + temp2);
            DirectoryCopy(tempDir, approotDir, true, true, temp1, temp2);
            AppendLine(textBox4, "***End of Directory Copy - tempDir to approotDir***");            
            AppendLine(textBox4, "***End of Files Update***\r\n");
            */

            //Copies directories and files from TempDir to ApprootDir
            AppendLine(textBox4, "***Start of Directory Copy - TempDir to ApprootDir***");
            AppendLine(textBox4, "Directory Copy: " + TempDir + " to " + ApprootDir + " // use ExcludeFileList");
            DirectoryCopy(TempDir, ApprootDir, true, true, true); //needs excl list and exc dir
            AppendLine(textBox4, "***End of Directory Copy - TempDir to ApprootDir***");
            AppendLine(textBox4, "***End of Files Update***\r\n");

            //Final Check after update if approot contains App
            if (!Validate(ApprootDir, App))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No App file found // " + App + " at " + Path.Combine(ApprootDir, App));
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }

            //Delete temp directory if it exists
            if (Directory.Exists(TempDir))
            {
                Directory.Delete(TempDir, true);
                AppendLine(textBox4, "Remove: TempDir: " + TempDir + " found and deleted");
            }
            else
            {
                AppendLine(textBox4, "Atypical: TempDir: " + TempDir + " not deleted, not found.");
            }

            AppendLine(textBox4, "***End of Update***\r\n");
        }        
        private void ReverseUpdateApp()
        {
            //Main revert update method
            Success = true; //reset Success
            AppendLine(textBox4, "***Start of Reverse Update***");

            if (!Validate(ApptempDir, App))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No App file found // " + App + " at " + Path.Combine(ApptempDir, App));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                return;
            }

            //Delete temp directory if it exists
            if (Directory.Exists(TempDir))
            {
                Directory.Delete(TempDir, true);
                AppendLine(textBox4, "Remove Temp: TempDir: " + TempDir + " found and deleted");
            }
            else
            {
                AppendLine(textBox4, "Info Temp: TempDir: " + TempDir + " not deleted, not found.");
            }

            //Check for Oldconfig exists
            if (!Validate(WorkingDir, Oldconfig))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No Oldconfig file found // " + Oldconfig + " at " + Path.Combine(WorkingDir, Oldconfig));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                return;
            }
            AppendLine(textBox4, "old Config name: " + Oldconfig);

            //Check for Config exists
            if (!Validate(WorkingDir, Config))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No Config file found // " + Config + " at " + Path.Combine(WorkingDir, Config));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                return;
            }
            AppendLine(textBox4, "Config name: " + Config);
            AppendLine(textBox4, "old Config path: " + Path.GetFullPath(Oldconfig));
            AppendLine(textBox4, "existing Config path: " + Path.GetFullPath(Config));

            Read("version", Config); //read from current Config
            string existversion = ReadKey;
            AppendLine(textBox4, "Existing version: " + existversion);

            //copy old config to config overwrite
            //File.Copy(oldconfig, config, true);

            //Make sure update always contains one Config file ***Config file critical
            List<string> linelist = File.ReadAllLines(Oldconfig).ToList();
            File.WriteAllLines(Config, linelist);

            Read("version", Config); //read from new Config
            string tempversion = ReadKey;
            AppendLine(textBox4, "Update version: " + tempversion);

            //get update Config parameters and set
            AppendLine(textBox4, "Read New Config parameters: " + Config);
            GetConfig();

            //Remove all directories and files from approot for reverse update
            AppendLine(textBox4, "***Start of Reverse Update Delete - ApprootDir***");
            DirectoryInfo fuinfo = new DirectoryInfo(ApprootDir);
            FileInfo[] fufiles = fuinfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in fufiles.OrderByDescending(f => f.LastWriteTime))
            {
                File.Delete(file.FullName);
                AppendLine(textBox4, "File Delete: " + file.FullName);
            }
            DirectoryInfo[] dirs = fuinfo.GetDirectories();
            foreach (DirectoryInfo subdir in dirs)
            {
                if (subdir.Name != WorkingDirName)
                {
                    Directory.Delete(subdir.FullName, true);
                    AppendLine(textBox4, "Directory Delete: " + subdir.FullName);
                }
            }
            AppendLine(textBox4, "***End of Reverse Update Delete - ApprootDir***");

            /*
            //Copy all files in apptempDir to approotDir except workingDirName
            AppendLine(textBox4, "***Start of Directory Copy - apptempDir to approotDir***");
            AppendLine(textBox4, "Directory Copy: " + apptempDir + " to " + approotDir + " exclude // " + workingDirName);
            DirectoryCopy(apptempDir, approotDir, true, true, workingDirName);
            AppendLine(textBox4, "***End of Directory Copy - apptempDir to approotDir***");
            */

            //Reverse Copy all files in ApptempDir to ApprootDir except WorkingDirName
            AppendLine(textBox4, "***Start of Directory Copy - ApptempDir to ApprootDir***");
            AppendLine(textBox4, "Directory Copy: " + ApptempDir + " to " + ApprootDir + " // No Exclusions");
            DirectoryCopy(ApptempDir, ApprootDir, true, true);//, false); //no exclusions
            AppendLine(textBox4, "***End of Directory Copy - ApptempDir to ApprootDir***");


            if (!Validate(ApprootDir, App))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No App file found // " + App + " at " + Path.Combine(ApprootDir, App));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                MessageBox.Show("Failed!", "Reverse Update", MessageBoxButtons.OK);  
                return;
            }

            MessageBox.Show("Reverse Successful!", "Reverse Update", MessageBoxButtons.OK);
            AppendLine(textBox4, "Confirmation: Reverse Successful!");
            AppendLine(textBox4, "***End of Reverse Update***\r\n");
            StartUp();
        }
        private void Install_Update()
        {
            //Main Backup Install and Update Method

            //Check if any errors prior
            if (!Success)
            {
                return;
            }
            AppendLine(textBox4, "***Start of Install_Update***");

            //Check to make sure App is in approot
            if (!Validate(ApprootDir, App))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No App file found! // " + App + " at " + Path.Combine(ApprootDir, App));
                AppendLine(textBox4, "Error: Failed at Install_Update()");
                AppendLine(textBox4, "Error: Install_Update Failed!");
                ErrorLog();
                return;
            }

            // Run the main methods if Success is true
            if (Success)
            {
                Backup();
                Install();
                UpdateApp();
                StartUp();                
            }

            //Create shortcut on desktop when finished
            if (Success)
            {                
                CreateShortcut();
            }

            if (Success)
            {
                AppendLine(textBox4, "***End of Install_Update***");
            }
            else
            {
                //Delete temp directory if it exists
                if (Directory.Exists(TempDir))
                {
                    Directory.Delete(TempDir, true);
                    AppendLine(textBox4, "Remove: TempDir: " + TempDir + " found and deleted");
                }
            }

            //Debug
            //Log();
        }
        private void CreateShortcut()
        {
            try
            {
                string exepath = Path.Combine(ApprootDir, App);
                if (File.Exists(exepath))
                {
                    AppendLine(textBox4, "Create Shortcut: " + App + "-" + Version + " - shortcut.lnk");
                    object shDesktop = (object)"Desktop";
                    WshShell shell = new WshShell();
                    string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + App + "-" + Version + " - shortcut.lnk";
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                    shortcut.WorkingDirectory = ApprootDir;
                    shortcut.Description = "New shortcut for a NMSC";
                    shortcut.TargetPath = exepath; //Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\notepad.exe";
                    shortcut.Save();
                }
                else
                {
                    AppendLine(textBox4, "No Shortcut Created, App path not found! // App:" + App + " Version: " + Version + " - shortcut.lnk");
                    ErrorLog();
                    return;
                }                
            }
            catch
            {
                AppendLine(textBox4, "Error: Cannot Create Shortcut!");
                ErrorLog();
                return;
            }            
        }
        private void CreateZipBackup()
        {
            if (Success)
            {
                if (Directory.Exists(ApptempDir))
                {
                    string zipname = "update-backup-" + App + "-" + Version + ".zip";
                    ZipFile.CreateFromDirectory(ApptempDir, zipname);

                    string zippath = Path.Combine(WorkingDir, zipname);
                    if (File.Exists(zippath))
                    {
                        Directory.Delete(ApptempDir, true);
                    }
                }
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            //Debug Backup/Install/Update Button
            
            Backup();
            Install();
            UpdateApp();
            StartUp();
            

            //Install_Update();

            //CreateShortcut();

            //Debug
            //Log();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            //Debug Reverse update button
            ReverseUpdateApp();

            //Debug
            //Log();
        }
        private void Log()
        {
            try
            {
                if (textBox4.TextLength != 0)
                {
                    List<string> list = textBox4.Lines.ToList();
                    //list.Insert(0, "*****" + DateTime.Now.ToString("MM-dd-yyyy HH:mm" + "*****"));
                    list.Add("*****End log. " + DateTime.Now.ToString("MM-dd-yyyy HH:mm" + "*****"));
                    string notepad = "notepad++.exe";
                    string logfile = "log.txt";

                    if (File.Exists(logfile))
                    {
                        File.WriteAllLines(logfile, list);
                        Process.Start(notepad, logfile);
                    }
                }         
            }
            catch
            {
                MessageBox.Show("Problem opening log file!\r\n\nTry opening log.txt manually.", "Alert", MessageBoxButtons.OK);
                AppendLine(textBox4, "Problem opening log file!");
            }    
        }
        private void OpenLogtxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log();
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {            
            AutoSizeTextBox(sender as TextBox);
        }
        // Make the TextBox fit its contents.
        private void AutoSizeTextBox(TextBox txt)
        {
            const int x_margin = 20;
            Size size = TextRenderer.MeasureText(txt.Text, txt.Font);
            txt.ClientSize = new Size(size.Width + x_margin, textBox4.Height);
        }
    }
}
