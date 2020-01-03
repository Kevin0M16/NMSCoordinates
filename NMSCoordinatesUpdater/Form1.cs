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

namespace NMSCoordinatesUpdater
{
    public partial class Form1 : Form
    {
        private string config;
        private string oldconfig;
        private string workingDir;
        private string workingDirName;
        
        private string readKey;

        private string version;
        private string updateZip;

        private string repo;
        private string user;

        private string app;
        private string approotDir;
        private string appexcludeDirName;
        //private string appexcDirName;

        private string apptempDir;
        private string tempDir;
        private string tempRootDir;

        private string assetUrl;
        private string assetName;

        WebClient webClient;
        Stopwatch sw = new Stopwatch();

        private List<string> loglist = new List<string>();
        private List<string> filelist = new List<string>();

        private bool fullupdate;
        private bool success { get; set; }

        public Form1()
        {
            InitializeComponent();
                        
            config = Path.Combine(Directory.GetCurrentDirectory(), "uconfig.nmsc");
            oldconfig = Path.Combine(Directory.GetCurrentDirectory(), "config_old.nmsc");
            success = true;

            //log box
            textBox4.Visible = false;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            workingDir = Directory.GetCurrentDirectory();
            AppendLine(textBox4, "workingDir: " + workingDir);

            if (!Directory.Exists(workingDir))
            {
                AppendLine(textBox4, "Error: workingDir: " + workingDir + "not found!");
                AppendLine(textBox4, "Error: Failed at Form1_Shown");
                AppendLine(textBox4, "Error: Failed to Load!");
                ErrorLog();
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(workingDir);
            workingDirName = dir.Name;
            AppendLine(textBox4, "workingDirName: " + workingDirName);

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

                loglist.Add(value);
                File.WriteAllLines(log, loglist);
            }                
            else
            {
                source.AppendText("\r\n" + value);

                loglist = File.ReadAllLines(log).ToList();
                loglist.Add(value);
                File.WriteAllLines(log, loglist);
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
            if (!success)
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
                tw.WriteLine("appexcludedirname=backup");
                tw.WriteLine("updatezip=latest.zip");
                tw.WriteLine("fullupdate=false");
                tw.Close();
            }
            catch
            {
                AppendLine(textBox4, "Error: Write new config file failed! config:" + config + " not created!");
                AppendLine(textBox4, "Error: Failed at CreateConfig()");
                ErrorLog();
                return;
            }
            
        }
        private void GetConfig() // update to (string cfgpath) for readalllines new cfg?
        {
            //Read config file and set parameters
            if (!success)
            {
                return;
            }
            AppendLine(textBox4, "***Start of Read config file***");

            if (!File.Exists(config))
            {
                AppendLine(textBox4, "Info: config file: " + config + " not found, creating new config.");
                CreateConfig();
            }

            if (!Validate(workingDir, config))
            {
                AppendLine(textBox4, "Error: No config file found // " + config + " at " + Path.Combine(workingDir, config));
                AppendLine(textBox4, "Error: Failed at GetConfig()");
                ErrorLog();
                return;
            }

            Read("config", config);
            config = readKey;

            Read("oldconfig", config);
            oldconfig = readKey;

            Read("repo", config);
            repo = readKey;
            label2.Text = repo;

            Read("user", config);
            user = readKey;

            Read("version", config);
            version = readKey;

            Read("app", config);
            app = readKey;

            Read("approotdir", config);
            approotDir = Path.GetFullPath(readKey);

            Read("apptempdir", config);
            apptempDir = Path.GetFullPath(readKey);

            Read("tempdir", config);
            tempDir = Path.GetFullPath(readKey);

            Read("appexcludedirname", config);
            appexcludeDirName = readKey; //just a string of dname

            Read("updatezip", config);
            updateZip = readKey;

            Read("fullupdate", config);
            if (readKey == "true" || readKey == "True")
            {
                fullupdate = true;
            }
            else if (readKey == "false" || readKey == "False")
            {
                fullupdate = false;
            }

            AppendLine(textBox4, "approotDir Full Path: " + approotDir);
            AppendLine(textBox4, "apptempDir Full Path: " + apptempDir);
            AppendLine(textBox4, "tempDir Full Path: " + tempDir);

            AppendLine(textBox4, "***End of Read config file***\r\n");
        }
        private void Read(string key, string path)
        {
            if (!success)
            {
                return;
            }
            readKey = "";
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
                            readKey = value;
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
        private void ReadFiles()
        {
            List<string> read = File.ReadAllLines(config).ToList();
            foreach (string lst in read)
            {
                Regex myRegexLS = new Regex("includefiles{.*?}", RegexOptions.Multiline);
                Match m1 = myRegexLS.Match(lst);
                string files = m1.ToString();
                files = files.Replace("includefiles{", "");
                files = files.Replace("}", "");
                filelist = files.Split(',').ToList();
            }
            /*
            foreach(string lst in filelist)
            {
                AppendLine(textBox4, lst);
            }
            */  
        }
        private void Write(string key, string newKey, string path)
        {
            if (!success)
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

                            line2 = line2.Replace("=", "");
                            string value = line2.Replace(" ", "");

                            //replaces the record at the selected filelist index
                            int index = filelist.IndexOf(lst);
                            string record = lst.Replace(value, newKey);
                            filelist[index] = record;
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
            if (!success)
            {
                return;
            }
            try
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                
                var client = new GitHubClient(new ProductHeaderValue(repo));                
                var releases = await client.Repository.Release.GetAll(user, repo);                

                var latest = releases[0];
                assetUrl = releases[0].Assets[0].BrowserDownloadUrl;
                assetName = releases[0].Assets[0].Name;

                textBox3.Text = version;
                textBox2.Text = latest.Name;
                textBox1.Text = assetName;

                if (version == latest.Name)
                {
                    button2.Text = "Repair";
                    
                    DialogResult dialogResult = MessageBox.Show("You have the latest version from Github\r\n\n" +
                        "Open" + app + 
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
                    button2.Text = "Update";
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
                string apppath = Path.Combine(approotDir, app);
                if (File.Exists(apppath))
                {
                    var startInfo = new ProcessStartInfo();
                    startInfo.WorkingDirectory = approotDir; // working directory
                    startInfo.FileName = app;
                    Process.Start(startInfo);
                    Close();
                }
                else
                {
                    MessageBox.Show(app + " was not found!", "Alert", MessageBoxButtons.OK);
                    Close();
                }
            }
            catch
            {
                AppendLine(textBox4, "Error: Problem Opening " + app + " !");
                MessageBox.Show("Problem Opening " + app + " !", "Alert", MessageBoxButtons.OK);
            }
        }
        private void Button1_Click(object sender, System.EventArgs e)
        {
            if (!success)
            {
                return;
            }
            AppendLine(textBox4, "***Start of Download***");
            DownloadFile(assetUrl, updateZip);
            AppendLine(textBox4, "***End of Download***");
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
                    AppendLine(textBox4, "Error: webClient Error!");
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
                MessageBox.Show("Download has been canceled.", repo + " Update", MessageBoxButtons.OK);
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
            if (!success)
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

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Directory.FullName != workingDir)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, ovrwrite);
                    AppendLine(textBox4, "Copy: " + file + " to " + temppath + " // " + file.LastWriteTime);
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    if (temppath != workingDir)
                    {
                        if (subdir.Name != dname1 && subdir.Name != dname2)// && subdir.FullName != workingDir)// && subdir.Name != workingDirName)
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
            if (!success)
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

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Directory.FullName != workingDir)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, ovrwrite);
                    AppendLine(textBox4, "Copy: " + file + " to " + temppath + " // " + file.LastWriteTime);
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    if (temppath != workingDir)
                    {
                        if (subdir.Name != dname)// && subdir.FullName != workingDir)// && subdir.Name != workingDirName) //!subdir.Name.Contains(dname))
                        {
                            AppendLine(textBox4, "Directory Copy: " + subdir.FullName + " to " + temppath + " exclude // " + dname);
                            DirectoryCopy(subdir.FullName, temppath, copySubDirs, ovrwrite, dname);
                        }
                    }
                }
            }
        }
        private void ErrorLog()
        {
            //Main Error log triggers on fail
            success = false; //set success to false for failure

            AppendLine(textBox4, "***Start of Error Log***");
            ValidateLog();
            AppendLine(textBox4, "Log: config= " + config);
            AppendLine(textBox4, "Log: oldconfig= " + oldconfig);
            AppendLine(textBox4, "Log: workingDir= " + workingDir);
            AppendLine(textBox4, "Log: workingDirName= " + workingDirName);
            AppendLine(textBox4, "Log: readKey= " + readKey);
            AppendLine(textBox4, "Log: version= " + version);
            AppendLine(textBox4, "Log: updateZip= " + updateZip);
            AppendLine(textBox4, "Log: repo= " + repo);
            AppendLine(textBox4, "Log: user= " + user);
            AppendLine(textBox4, "Log: app= " + app);
            AppendLine(textBox4, "Log: approotDir= " + approotDir);
            AppendLine(textBox4, "Log: appexcludeDirName= " + appexcludeDirName);
            AppendLine(textBox4, "Log: apptempDir= " + apptempDir);
            AppendLine(textBox4, "Log: tempDir= " + tempDir);
            AppendLine(textBox4, "Log: tempRootDir= " + tempRootDir);
            AppendLine(textBox4, "Log: assetUrl= " + assetUrl);
            AppendLine(textBox4, "Log: assetName= " + assetName);
            AppendLine(textBox4, "Log: fullupdate= " + fullupdate);
            AppendLine(textBox4, "***End of Error Log***\r\n");

            MessageBox.Show("An Error has occurred!\r\n\nSee log.txt for more information.\r\n\n", "Error", MessageBoxButtons.OK);
        }
        private void ValidateLog()
        {
            AppendLine(textBox4, "***Start of Validate Directories and Files Log***");

            string tempappPath = Path.Combine(tempDir, app);
            string apptempappPath = Path.Combine(apptempDir, app);
            string approotappPath = Path.Combine(approotDir, app);

            if (!Directory.Exists(tempDir))
            {
                AppendLine(textBox4, "Error: tempDir: " + tempDir + " doesn't exist!");
            }
            else
            {
                AppendLine(textBox4, "Ready: tempDir: " + tempDir + " found.");
            }
            if (!File.Exists(tempappPath))
            {
                AppendLine(textBox4, "Error: tempappPath: " + tempappPath + " doesn't exist!");
            }
            else
            {
                AppendLine(textBox4, "Ready: tempappPath: " + tempappPath + " found.");
            }
            if (!Directory.Exists(apptempDir))
            {
                AppendLine(textBox4, "Error: apptempDir: " + apptempDir + " doesn't exist!");
            }
            else
            {
                AppendLine(textBox4, "Ready: apptempDir: " + apptempDir + " found.");
            }
            if (!File.Exists(apptempappPath))
            {
                AppendLine(textBox4, "Error: apptempappPath: " + apptempappPath + " doesn't exist!");
            }
            else
            {
                AppendLine(textBox4, "Ready: apptempappPath: " + apptempappPath + " found.");
            }
            if (!Directory.Exists(approotDir))
            {
                AppendLine(textBox4, "Error: approotDir: " + approotDir + " doesn't exist!");
            }
            else
            {
                AppendLine(textBox4, "Ready: approotDir: " + approotDir + " found.");
            }
            if (!File.Exists(approotappPath))
            {
                AppendLine(textBox4, "Error: approotappPath" + approotappPath + " doesn't exist!");
            }
            else
            {
                AppendLine(textBox4, "Ready: approotappPath" + approotappPath + " found.");
            }

            AppendLine(textBox4, "***End of Validate Directories and Files Log***");
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
            if (!success)
            {
                return;
            }
            AppendLine(textBox4, "***Start of Backup***");

            if (!Validate(approotDir, app))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No app file found! // " + app + " at " + Path.Combine(approotDir, app));
                AppendLine(textBox4, "Error: Failed at Backup()");
                AppendLine(textBox4, "Error: Backup Failed!");
                ErrorLog();
                return;
            }

            //Deletes old apptemp if found
            if (Directory.Exists(apptempDir))
            {
                Directory.Delete(apptempDir, true);
                AppendLine(textBox4, "Remove: apptempDir: " + apptempDir + " found and deleted");
            }

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
        }
        private void Install()
        {
            //Main install method
            if (!success)
            {
                return;
            }

            AppendLine(textBox4, "***Start of Install***");

            if (!Validate(apptempDir, approotDir, app))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No app file found! // " + Path.Combine(apptempDir, app) + " or " + Path.Combine(approotDir, app));
                AppendLine(textBox4, "Error: Failed at Install()");
                AppendLine(textBox4, "Error: Install Failed!");
                ErrorLog();
                return;
            }

            /*
            //Looks in approot directory for the app
            if (!Directory.Exists(approotDir))
            {
                AppendLine(textBox4, "Error: approotDir: " + approotDir + " not found!");
                return;
            }
            DirectoryInfo approotdinfo = new DirectoryInfo(approotDir);
            FileInfo[] approotfiles = approotdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (approotfiles.Length == 0)
            {
                AppendLine(textBox4, "Error: approotfiles.Length is 0");
                return;
            }
            */

            /*
            //Deletes old apptemp if found
            if (Directory.Exists(apptempDir))
            {
                Directory.Delete(apptempDir, true);
                AppendLine(textBox4, "Remove: apptempDir: " + apptempDir + " found and deleted");
            }

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
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
                AppendLine(textBox4, "Remove: Old tempDir: " + tempDir + " found and deleted");
            }

            //Checks if updateZip exists
            if (!File.Exists(updateZip))
            {
                AppendLine(textBox4, "Error: updateZip not found! // " + updateZip);
                AppendLine(textBox4, "Error: Failed at Install()");
                AppendLine(textBox4, "Error: Install Failed!");
                ErrorLog();
                return;
            }
            else
            {
                AppendLine(textBox4, "Ready: updateZip:" + updateZip + " found.");
            }

            //Extract Zip File to tempDir \temp
            ZipFile.ExtractToDirectory(updateZip, tempDir);
            AppendLine(textBox4, "Extract: " + updateZip + " to " + tempDir);

            //Check if \temp exists
            if (!Directory.Exists(tempDir))
            {
                AppendLine(textBox4, "Error: tempDir not found! // " + tempDir);
                AppendLine(textBox4, "Error: Failed at Install()");
                AppendLine(textBox4, "Error: Install Failed!");
                ErrorLog();
                return;
            }
            else
            {
                AppendLine(textBox4, "Ready: tempDir: " + tempDir + " found.");
            }

            //find the root folder in temp that contains the app and set temprootDir
            DirectoryInfo dinfo = new DirectoryInfo(tempDir);
            FileInfo[] Files = dinfo.GetFiles(app, SearchOption.AllDirectories);
            if (Files.Length != 0)
            {
                foreach (FileInfo file in Files)//.OrderByDescending(f => f.LastWriteTime))
                {
                    tempRootDir = file.DirectoryName;                    
                    AppendLine(textBox4, "tempRootDir: " + tempRootDir);
                }
                
                //Looks to see if tempDir is already the root if not move it
                if (tempRootDir != tempDir)
                {
                    DirectoryInfo temprootdinfo = new DirectoryInfo(tempRootDir);
                    string temprootdirname = temprootdinfo.Name;
                    AppendLine(textBox4, "***Start of Directory Copy - tempRootDir to tempDir***");
                    AppendLine(textBox4, "Directory Copy: " + tempRootDir + " to " + tempDir + " exclude // " + temprootdirname); //tempRootDir);
                    DirectoryCopy(tempRootDir, tempDir, true, true, temprootdirname); //tempRootDir);
                    AppendLine(textBox4, "***End of Directory Copy - tempRootDir to tempDir***");

                    //Delete temprootDir now its empty
                    if (Directory.Exists(tempRootDir))
                    {
                        Directory.Delete(tempRootDir, true);
                        AppendLine(textBox4, "Remove: tempRootDir: " + tempRootDir + " found and deleted");
                    }
                    else
                    {
                        AppendLine(textBox4, "Atypical: tempRootDir: " + tempRootDir + " not deleted, not found.");
                    }
                }

                /*
                if (Directory.Exists(appexcDirName))
                {
                    Directory.Delete(tempRootDir, true);
                    AppendLine(textBox4, "Remove: tempRootDir: " + tempRootDir + " deleted");
                }
                */

                /*
                //If tempRootDir exist delete it
                if (Directory.Exists(tempRootDir))
                {
                    Directory.Delete(tempRootDir, true);
                    AppendLine(textBox4, "Remove: tempRootDir: " + tempRootDir + " deleted");
                }
                */

                /*
                //Checks if tempDir\app and apptempDir\app exists
                if (!Directory.Exists(tempDir) || !File.Exists(Path.Combine(tempDir, app)))
                {
                    AppendLine(textBox4, "Error: tempDir: " + tempDir + " or " + Path.Combine(tempDir, app) + " not found!");
                    return;
                }
                if (!Directory.Exists(apptempDir) || !File.Exists(Path.Combine(apptempDir, app))) 
                {
                    AppendLine(textBox4, "Error: apptempDir: " + apptempDir + " or " + Path.Combine(apptempDir, app) + " not found!");
                    return;
                }
                */
            }
            AppendLine(textBox4, "***End of Install***\r\n");
        }              
        private void UpdateApp()
        {
            //Main Update Method
            if (!success)
            {
                return;
            }

            AppendLine(textBox4, "***Start of Update***");

            if (!Validate(tempDir, apptempDir, approotDir, app))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No app file found // " + Path.Combine(tempDir, app) + " or " + Path.Combine(apptempDir, app) + " or " + Path.Combine(approotDir, app));
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }

            /*
            //Check if backup of files exists app_temp
            if (!Directory.Exists(apptempDir)) //apptempDir = Path.GetFullPath(@".\app_temp");
            {
                AppendLine(textBox4, "Error: apptempDir: " + apptempDir + " doesn't exist!");
                return;
            }
            DirectoryInfo apptempdinfo = new DirectoryInfo(apptempDir);
            FileInfo[] apptempfilesCheck = apptempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (apptempfilesCheck.Length == 0)
            {
                AppendLine(textBox4, "Error: apptempfilesCheck.Length = 0");
                return;
            }

            //Check if approot contains app
            if (!Directory.Exists(approotDir)) //approotDir = Path.GetFullPath(@"..\");
            {
                AppendLine(textBox4, "Error: approotDir: " + approotDir + " doesn't exist!");
                return;
            }
            DirectoryInfo adinfo = new DirectoryInfo(approotDir);
            FileInfo[] afilesCheck = adinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (afilesCheck.Length == 0)
            {
                AppendLine(textBox4, "Error: afilesCheck.Length = 0");
                return;
            }

            // Check if tempdir exists and contains app and check config
            if (!Directory.Exists(tempDir))
            {
                AppendLine(textBox4, "Error: tempDir: " + tempDir + " doesn't exist!");
                return;
            }
            DirectoryInfo tempdinfo = new DirectoryInfo(tempDir); //tempDir = Path.GetFullPath(@".\temp");
            FileInfo[] tempfilesCheck = tempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (tempfilesCheck.Length == 0)
            {
                AppendLine(textBox4, "Error: tempfilesCheck.Length = 0");
                return;
            }
            */

            if (!Validate(workingDir, config))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No config file found // " + config + " at " + Path.Combine(workingDir,config));
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }
            
            /*
            //Check if existing config file exist
            if (!File.Exists(config))
            {
                AppendLine(textBox4, "Error: No config file found // "+ config);
                return;
            }
            AppendLine(textBox4, "config name: " + config);
            */

            //Check config is in tempDir look in entire folder and subs
            DirectoryInfo tempdinfo = new DirectoryInfo(tempDir); //tempDir = Path.GetFullPath(@".\temp");
            FileInfo[] tempdfilesCheck = tempdinfo.GetFiles(config, SearchOption.AllDirectories);
            if (tempdfilesCheck.Length == 0)
            {
                AppendLine(textBox4, "Error: No config file found in tempDir! // " + config + " // tempdfilesCheck.Length = 0");
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }
            foreach (FileInfo file in tempdfilesCheck)
            {
                AppendLine(textBox4, "tempDir config: " + file.FullName);
                AppendLine(textBox4, "existing config: " + Path.GetFullPath(config));

                Read("version", config); //read from current config
                string existversion = readKey;
                AppendLine(textBox4, "Existing version: " + existversion);

                //make copy of config and rename
                File.Copy(config, oldconfig, true);

                //Make sure update always contains one config file ***config file critical
                List<string> linelist = File.ReadAllLines(file.FullName).ToList(); //REVERSE ADD
                File.WriteAllLines(config, linelist);

                Read("version", config); //read from new config
                string tempversion = readKey;
                AppendLine(textBox4, "Update version: " + tempversion);

                //get update config parameters and set
                AppendLine(textBox4, "Read New Config parameters: " + config);
                GetConfig();
            }            

            //Update files
            AppendLine(textBox4, "***Start of Files Update***");            
            
            if (appexcludeDirName != "none")
            {
                //Removes appexcludeDirName from temp if exists in approot
                string appexcludedDirPath = Path.Combine(approotDir, appexcludeDirName);
                string tempexcludedDirPath = Path.Combine(tempDir, appexcludeDirName);

                if (Path.GetFullPath(appexcludedDirPath) == Path.GetFullPath(approotDir))
                {
                    AppendLine(textBox4, "Error: appexcludeDirName not set in config or null! // appexcludedDirPath: " + appexcludedDirPath + " // appexcludeDirName: " + appexcludeDirName);
                    AppendLine(textBox4, "Error: Failed at UpdateApp()");
                    AppendLine(textBox4, "Error: Update Failed!");
                    ErrorLog();
                    return;
                }
                if (Path.GetFullPath(tempexcludedDirPath) == Path.GetFullPath(tempDir))
                {
                    AppendLine(textBox4, "Error: tempexcludedDirPath not set in config or null! // tempexcludedDirPath: " + tempexcludedDirPath + " // appexcludeDirName: " + appexcludeDirName);
                    AppendLine(textBox4, "Error: Failed at UpdateApp()");
                    AppendLine(textBox4, "Error: Update Failed!");
                    ErrorLog();
                    return;
                }

                /*
                if (Directory.Exists(appexcludedDirPath)) //Directory.Exists(tempexcludedDirPath)
                {
                    Directory.Delete(tempexcludedDirPath, true);
                    AppendLine(textBox4, "Remove: appexcludeDirName: " + appexcludeDirName + " found at " + appexcludedDirPath + " deleted at: " + tempexcludedDirPath);
                }
                else
                {
                    AppendLine(textBox4, "Ready: appexcludeDirName:" + appexcludeDirName + " not found in " + approotDir + ", no directory excluded.");
                }
                */

                if (Directory.Exists(appexcludedDirPath))
                {
                    Directory.Delete(tempexcludedDirPath, true);
                    AppendLine(textBox4, "Remove: appexcludeDirName: " + appexcludeDirName + " found at " + appexcludedDirPath + " deleted at: " + tempexcludedDirPath);
                }
                if (Directory.Exists(tempexcludedDirPath))
                {
                    Directory.Delete(tempexcludedDirPath, true);
                    AppendLine(textBox4, "Remove: appexcludeDirName: " + appexcludeDirName + " found at " + tempexcludedDirPath + " deleted at: " + tempexcludedDirPath);
                }
            }
            if (appexcludeDirName == "none")
            {
                AppendLine(textBox4, "Ready: No directory excluded. appexcludeDirName is " + appexcludeDirName);
            }

            //Deletes all files and folders in approot for full update
            if (fullupdate == true)
            {                
                DirectoryInfo fuinfo = new DirectoryInfo(approotDir);
                FileInfo[] fufiles = fuinfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in fufiles.OrderByDescending(f => f.LastWriteTime))
                {
                    File.Delete(file.FullName);
                    AppendLine(textBox4, "Remove: fullupdate file: " + file.FullName + " deleted");
                }
                DirectoryInfo[] dirs = fuinfo.GetDirectories();
                foreach (DirectoryInfo subdir in dirs)
                {
                    if (subdir.Name != appexcludeDirName && subdir.Name != workingDirName)
                    {
                        Directory.Delete(subdir.FullName,true);
                        AppendLine(textBox4, "Remove: fullupdate directory: " + subdir.FullName + " deleted");
                    }
                }
            }

            /*
            if (fullupdate == false)
            {
                //Removes appexcdirname from temp if exists in approot
                string appexcDirPath = Path.Combine(approotDir, appexcDirName);
                string tempexcDirPath = Path.Combine(tempDir, appexcDirName);
                if (Directory.Exists(appexcDirPath))// && Directory.Exists(tempexcDirPath))
                {
                    Directory.Delete(tempexcDirPath, true);
                    AppendLine(textBox4, "Remove: Excluded Dir Name: " + appexcDirName + " appexcDirName found and deleted at: " + tempexcDirPath);
                }
                else
                {
                    AppendLine(textBox4, "Ready: Excluded Dir Name:" + appexcDirName + " appexcDirName not found in " + tempDir + ", no directory excluded");
                }
            }
            */

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

            //Final Check after update if approot contains app
            if (!Validate(approotDir, app))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No app file found // " + app + " at " + Path.Combine(approotDir, app));
                AppendLine(textBox4, "Error: Failed at UpdateApp()");
                AppendLine(textBox4, "Error: Update Failed!");
                ErrorLog();
                return;
            }

            /*
            //Final Check after update if approot contains app
            if (!Directory.Exists(approotDir)) //approotDir = Path.GetFullPath(@"..\");
            {
                AppendLine(textBox4, "Error: approotDir: " + approotDir + " doesn't exist!");
                return;
            }
            DirectoryInfo finaldinfo = new DirectoryInfo(approotDir);
            FileInfo[] finalfilesCheck = finaldinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (finalfilesCheck.Length == 0)
            {
                AppendLine(textBox4, "Error: finalfilesCheck.Length = 0 app: " + app + " doesn't exist in approotDir: " + approotDir);                
            }
            */

            //Delete temp directory if it exists
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
                AppendLine(textBox4, "Remove: tempDir: " + tempDir + " found and deleted");
            }
            else
            {
                AppendLine(textBox4, "Atypical: tempDir: " + tempDir + " not deleted, not found.");
            }

            AppendLine(textBox4, "***End of Update***\r\n");
        }        
        private void ReverseUpdateApp()
        {
            //Main revert update method
            success = true; //reset success
            AppendLine(textBox4, "***Start of Revert Update***");

            if (!Validate(apptempDir, app))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No app file found // " + app + " at " + Path.Combine(apptempDir, app));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                return;
            }

            //Delete temp directory if it exists
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
                AppendLine(textBox4, "Remove: tempDir: " + tempDir + " found and deleted");
            }
            else
            {
                AppendLine(textBox4, "Info: tempDir: " + tempDir + " not deleted, not found.");
            }

            /*
            //Check if app_temp backup of files exists
            if (!Directory.Exists(apptempDir)) //apptempDir = Path.GetFullPath(@".\app_temp");
            {
                AppendLine(textBox4, "Error: apptempDir: " + apptempDir + " doesn't exist!");
                return;
            }
            //Check if app is in app_temp
            DirectoryInfo apptempdinfo = new DirectoryInfo(apptempDir);
            FileInfo[] apptempfilesCheck = apptempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (apptempfilesCheck.Length == 0)
            {
                AppendLine(textBox4, "Error: apptempfilesCheck.Length = 0");
                return;
            }
            */

            //Check for oldconfig exists
            if (!Validate(workingDir, oldconfig))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No oldconfig file found // " + oldconfig + " at " + Path.Combine(workingDir, oldconfig));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                return;
            }
            AppendLine(textBox4, "old config name: " + oldconfig);

            /*
            //Check for oldconfig exists
            if (!File.Exists(oldconfig))
            {
                AppendLine(textBox4, "Error: No oldconfig file found // " + oldconfig);
                return;
            }
            AppendLine(textBox4, "old config name: " + oldconfig);
            */

            //Check for config exists
            if (!Validate(workingDir, config))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No config file found // " + config + " at " + Path.Combine(workingDir, config));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                return;
            }
            AppendLine(textBox4, "config name: " + config);

            /*
            //Check for config exists
            if (!File.Exists(config))
            {
                AppendLine(textBox4, "Error: No config file found // " + config);
                return;
            }
            AppendLine(textBox4, "config name: " + config);
            */

            AppendLine(textBox4, "old config path: " + Path.GetFullPath(oldconfig));
            AppendLine(textBox4, "existing config path: " + Path.GetFullPath(config));

            Read("version", config); //read from current config
            string existversion = readKey;
            AppendLine(textBox4, "Existing version: " + existversion);

            //copy old config to config overwrite
            //File.Copy(oldconfig, config, true);

            //Make sure update always contains one config file ***config file critical
            List<string> linelist = File.ReadAllLines(oldconfig).ToList();
            File.WriteAllLines(config, linelist);

            Read("version", config); //read from new config
            string tempversion = readKey;
            AppendLine(textBox4, "Update version: " + tempversion);

            //get update config parameters and set
            AppendLine(textBox4, "Read New Config parameters: " + config);
            GetConfig();
            
            //Remove all directories and files from approot for reverse update
            DirectoryInfo fuinfo = new DirectoryInfo(approotDir);
            FileInfo[] fufiles = fuinfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in fufiles.OrderByDescending(f => f.LastWriteTime))
            {
                File.Delete(file.FullName);
                AppendLine(textBox4, "Remove: fullupdate file: " + file.FullName + " deleted");
            }
            DirectoryInfo[] dirs = fuinfo.GetDirectories();
            foreach (DirectoryInfo subdir in dirs)
            {
                if (subdir.Name != workingDirName)
                {
                    Directory.Delete(subdir.FullName, true);
                    AppendLine(textBox4, "Remove: fullupdate directory: " + subdir.FullName + " deleted");
                }
            }            

            //Copy all files in apptempDir to approotDir except workingDirName
            AppendLine(textBox4, "***Start of Directory Copy - apptempDir to approotDir***");
            AppendLine(textBox4, "Directory Copy: " + apptempDir + " to " + approotDir + " exclude // " + workingDirName);
            DirectoryCopy(apptempDir, approotDir, true, true, workingDirName);
            AppendLine(textBox4, "***End of Directory Copy - apptempDir to approotDir***");

            if (!Validate(approotDir, app))
            {
                AppendLine(textBox4, "Error: Failed to Validate, No app file found // " + app + " at " + Path.Combine(approotDir, app));
                AppendLine(textBox4, "Error: Failed at ReverseUpdateApp()");
                AppendLine(textBox4, "Error: Reverse Update Failed!");
                ErrorLog();
                MessageBox.Show("Failed!", "Reverse Update", MessageBoxButtons.OK);  
                return;
            }

            MessageBox.Show("Reverse Successful!", "Reverse Update", MessageBoxButtons.OK);
            AppendLine(textBox4, "Confirmation: Reverse Successful!");

            /*
            //Final files check for app
            if (!Directory.Exists(approotDir)) //approotDir = Path.GetFullPath(@"..\");
            {
                AppendLine(textBox4, "Error: approotDir: " + approotDir + " doesn't exist!");
                return;
            }
            DirectoryInfo finaldinfo = new DirectoryInfo(approotDir);
            FileInfo[] finalfilesCheck = finaldinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (finalfilesCheck.Length != 0)
            {
                MessageBox.Show("Reverse Successful!", "Reverse Update", MessageBoxButtons.OK);
                AppendLine(textBox4, "Confirmation: Reverse Successful!");
            }
            else
            {
                MessageBox.Show("Failed!", "Reverse Update", MessageBoxButtons.OK);
                AppendLine(textBox4, "Error: Reverse Update Failed!");
            }
            */

            AppendLine(textBox4, "***End of Revert Update***\r\n");
            StartUp();
        }
        private void Install_Update()
        {
            //Backup Install and Update Method
            Backup();
            Install();
            UpdateApp();
            StartUp();

            //Debug
            //Log();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            //Update or Repair Button            
            Backup();
            Install();
            UpdateApp();
            StartUp();

            //Debug
            //Log();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            //Reverse update button
            ReverseUpdateApp();
            //StartUp();

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
                    list.Insert(0, "*****" + DateTime.Now.ToString("MM-dd-yyyy HH:mm" + "*****"));
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
    }
}
