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

namespace NMSCoordinatesUpdater
{
    public partial class Form1 : Form
    {
        private string config;
        private string readKey;

        private string version;
        private string updateZip;

        private string repo;
        private string user;

        private string app;
        private string approotDir;

        //temp file strings
        private string apptempDir;
        private string tempDir;
        private string tempRootDir;

        private string assetUrl;
        private string assetName;

        WebClient webClient;
        Stopwatch sw = new Stopwatch();

        private List<string> tempappfiles = new List<string>();
        private List<string> appfiles = new List<string>();

        private bool fullupdate;

        public Form1()
        {
            InitializeComponent();

            //config = Path.GetFullPath("uconfig.nmsc");
            config = Path.Combine(Directory.GetCurrentDirectory(), "uconfig.nmsc");

            //repo = "NMSCoordinates";
            //user = "Kevin0M16";

            //version = "1.1.14";
            //updateZip = "latest.zip";

            //app = "NMSCoordinates.exe";
            //approotDir = Path.GetFullPath(@"..\");

            //apptempDir = Path.GetFullPath(@".\app_temp");
            //tempDir = Path.GetFullPath(@".\temp");

            //fullupdate = false;

            //StartUp();
            //GetConfig();
            //CheckForUpdates();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            GetConfig();
            CheckForUpdates();
        }
        private void StartUp()
        {
            GetConfig();
            CheckForUpdates();
        }
        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        private void CreateConfig()
        {
            File.Create("uconfig.nmsc").Close();
            TextWriter tw = new StreamWriter("uconfig.nmsc");
            tw.WriteLine("config=uconfig.nmsc");
            tw.WriteLine("repo=");
            tw.WriteLine("user=");
            tw.WriteLine("version=1.0.0.0");
            tw.WriteLine("updatezip=latest.zip");
            tw.WriteLine("app=");
            tw.WriteLine("approotdir=" + @"..\");
            tw.WriteLine("apptempdir=" + @".\app_temp");
            tw.WriteLine("tempdir=" + @".\temp");
            tw.WriteLine("fullupdate=false");
            tw.Close();
        }
        private void GetConfig()
        {
            if (!File.Exists(config))
            {
                CreateConfig();
            }

            Read("config", config);
            config = readKey;

            Read("repo", config);
            repo = readKey;

            Read("user", config);
            user = readKey;

            Read("version", config);
            version = readKey;

            Read("updatezip", config);
            updateZip = readKey;

            Read("app", config);
            app = readKey;

            Read("approotdir", config);
            approotDir = Path.GetFullPath(readKey);

            Read("apptempdir", config);
            apptempDir = Path.GetFullPath(readKey);

            Read("tempdir", config);
            tempDir= Path.GetFullPath(readKey);

            Read("fullupdate", config);
            if (readKey == "true" || readKey == "True")
            {
                fullupdate = true;
            }
            else if (readKey == "false" || readKey == "False")
            {
                fullupdate = false;
            }
        }
        private void Read(string key, string path)
        {
            readKey = "";
            //Read the config file and get the value from key
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
                            readKey = s.Replace(" ", "");
                        }

                    }
                    return;
                }
            }
            catch
            {
                AppendLine(textBox4, "Config File Corrupted!");
                return;
            }
        }
        public void Write(string key, string newKey, string path)
        {
            //Write to config
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
                    AppendLine(textBox4, "Error config file not found!");
                    return;
                }
            }
            catch
            {
                AppendLine(textBox4, "Config file Corrupted!");
                return;
            }
        }
        private async void CheckForUpdates()
        {
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
                    //MessageBox.Show("You have the latest version from Github", "Alert", MessageBoxButtons.OK);  
                    DialogResult dialogResult = MessageBox.Show("You have the latest version from Github\r\n\nOpen" + app + " Now? ", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        OpenApp();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Github Server Problem. Could not check version", "Alert");
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
                MessageBox.Show("Problem Opening " + app + " !", "Alert", MessageBoxButtons.OK);
            }
        }
        private void Button1_Click(object sender, System.EventArgs e)
        {
            DownloadFile(assetUrl, updateZip);       
        }
        public void DownloadFile(string urlAddress, string location)
        {
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
        private void Install()
        {
            //Looks in approot directory for the app and backup to app_temp
            if (!Directory.Exists(approotDir))
            {
                return;
            }
            DirectoryInfo NMSCdinfo = new DirectoryInfo(approotDir);
            FileInfo[] NMSCFiles = NMSCdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (NMSCFiles.Length == 0) //!= 0)
            {
                return;
            }
            if (Directory.Exists(apptempDir))
            {
                Directory.Delete(apptempDir, true);
            }
            DirectoryCopy(approotDir, apptempDir, true, true, null, "temp");


            //Looks in temp directory for the app then copies temp to approot
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
            if (!File.Exists(updateZip))
            {
                return;
            }
            ZipFile.ExtractToDirectory(updateZip, tempDir);
            if (!Directory.Exists(tempDir))
            {
                return;
            }
            DirectoryInfo dinfo = new DirectoryInfo(tempDir);
            FileInfo[] Files = dinfo.GetFiles(app, SearchOption.AllDirectories);
            if (Files.Length != 0)
            {
                foreach (FileInfo file in Files)//.OrderByDescending(f => f.LastWriteTime))
                {
                    //find the root folder in temp that contains the app and set it
                    tempRootDir = file.DirectoryName;
                }
                DirectoryCopy(tempRootDir, tempDir, true, true, null, tempRootDir);

                //Don't move backup if exists
                if (Directory.Exists(Path.Combine(approotDir, "backup")))
                {
                    Directory.Delete(Path.Combine(tempDir, "backup"), true);
                }

                if (Directory.Exists(tempRootDir))
                {
                    Directory.Delete(tempRootDir, true);
                }

                if (!Directory.Exists(tempDir) || !File.Exists(Path.Combine(tempDir, app)))
                {
                    return;
                }

                if (!Directory.Exists(apptempDir) || !File.Exists(Path.Combine(apptempDir, app))) 
                {
                    return;
                }
            }
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool ovrwrite, string ext, string dname)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                //if (file.Extension != ext && !file.DirectoryName.Contains(dname))
                if (Path.GetExtension(temppath) != ext)// && Path.GetDirectoryName(temppath).Contains(dname))
                    file.CopyTo(temppath, ovrwrite);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    if (!subdir.Name.Contains(dname))
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, ovrwrite, null, dname);
                }
            }
        }

        private void UpdateApp()
        {
            //Check if backup of files exists
            if (!Directory.Exists(apptempDir)) //apptempDir = Path.GetFullPath(@".\app_temp");
            {
                return;
            }
            DirectoryInfo apptempdinfo = new DirectoryInfo(apptempDir);
            FileInfo[] apptempfilesCheck = apptempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (apptempfilesCheck.Length == 0)
            {
                return;
            }

            //Check if approot contains app
            if (!Directory.Exists(approotDir)) //approotDir = Path.GetFullPath(@"..\");
            {
                return;
            }
            DirectoryInfo adinfo = new DirectoryInfo(approotDir);
            FileInfo[] afilesCheck = adinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (afilesCheck.Length == 0)
            {
                return;
            }

            // Check if tempdir contains app and check config
            if (!Directory.Exists(tempDir))
            {
                return;
            }
            DirectoryInfo tempdinfo = new DirectoryInfo(tempDir); //tempDir = Path.GetFullPath(@".\temp");
            FileInfo[] tempfilesCheck = tempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (tempfilesCheck.Length == 0)
            {
                return;
            }
            FileInfo[] tempfilesCheck2 = tempdinfo.GetFiles(config, SearchOption.AllDirectories);
            if (tempfilesCheck2.Length != 0)
            {
                foreach (FileInfo file in tempfilesCheck2)
                {
                    Read("version", file.FullName); //read from new config
                    string newversion = readKey;
                    Write("version", newversion, config); //write to existing config
                    Read("version", config); //read to check config

                    if (newversion == readKey) // set if successful
                        version = readKey;

                    string updaterPath = Path.GetDirectoryName(file.FullName);
                    Directory.Delete(updaterPath, true);
                }
            }

            //Update files
            DirectoryInfo workingdir = new DirectoryInfo(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
            if (fullupdate == false)
            {
                DirectoryCopy(tempDir, approotDir, true, true, null, "temp");
            }
            if (fullupdate == true)
            {               
                FileInfo[] afiles = adinfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in afiles.OrderByDescending(f => f.LastWriteTime))
                {
                    File.Delete(file.FullName);
                }
                DirectoryInfo[] dirs = adinfo.GetDirectories();
                foreach (DirectoryInfo subdir in dirs)
                {
                    if (subdir.Name != "backup" && subdir.Name != workingdir.Name)
                        Directory.Delete(subdir.Name);
                }
                DirectoryCopy(tempDir, approotDir, true, true, null, "temp");
            }

            //Final Check after update if approot contains app
            if (!Directory.Exists(approotDir)) //approotDir = Path.GetFullPath(@"..\");
            {
                return;
            }
            DirectoryInfo finaldinfo = new DirectoryInfo(approotDir);
            FileInfo[] finalfilesCheck = finaldinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (finalfilesCheck.Length != 0)
            {
                Directory.Delete(tempDir, true);
            }
        }
        private void UpdateAppB()
        {
            //Check if backup of files exists
            if (!Directory.Exists(apptempDir)) //apptempDir = Path.GetFullPath(@".\app_temp");
            {
                return;
            }
            DirectoryInfo apptempdinfo = new DirectoryInfo(apptempDir);
            FileInfo[] apptempfilesCheck = apptempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (apptempfilesCheck.Length == 0)
            {
                return;
            }

            //Add downloaded temp files to list
            if (!Directory.Exists(tempDir))
            {
                return;
            }
            DirectoryInfo tempdinfo = new DirectoryInfo(tempDir); //tempDir = Path.GetFullPath(@".\temp");
            FileInfo[] tempfilesCheck = tempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (tempfilesCheck.Length == 0)
            {
                return;
            }
            FileInfo[] tempfilesCheck2 = tempdinfo.GetFiles(config, SearchOption.AllDirectories);
            if (tempfilesCheck2.Length != 0)
            {
                foreach (FileInfo file in tempfilesCheck2)
                {
                    Read("version", file.FullName);
                    version = readKey;
                    Write("version", version, config);
                }                
            }
            FileInfo[] tempfiles = tempdinfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in tempfiles.OrderByDescending(f => f.LastWriteTime))
            {
                tempappfiles.Add(file.Name); //tempappfiles is .\temp files
            }

            //Add app files to a list
            if (!Directory.Exists(approotDir)) //approotDir = Path.GetFullPath(@"..\");
            {
                return;
            }
            DirectoryInfo adinfo = new DirectoryInfo(approotDir);
            FileInfo[] afilesCheck = adinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (afilesCheck.Length == 0)
            {
                return;
            }
            FileInfo[] afiles = adinfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in afiles.OrderByDescending(f => f.LastWriteTime))
            {
                appfiles.Add(file.Name); //appfiles is ..\ files
            }

            if (fullupdate == false)
            {
                foreach (string file in tempappfiles) //tempappfiles is .\temp files
                {
                    if (appfiles.Contains(file))
                        File.Copy(Path.Combine(tempDir, file), Path.Combine(approotDir, file), true);
                }
            }

            if (fullupdate == true)
            {
                foreach (string file in appfiles) //appfiles is ..\ files
                {
                    if (Path.GetExtension(file) != ".zip")
                        File.Delete(Path.Combine(approotDir, file));
                }
                foreach (string file in tempappfiles) //tempappfiles is .\temp files
                {
                    File.Copy(Path.Combine(tempDir, file), Path.Combine(approotDir, file), true);
                }
            }
        }
        private void ReverseUpdateApp()
        {
            //Check if backup of files exists
            if (!Directory.Exists(apptempDir)) //apptempDir = Path.GetFullPath(@".\app_temp");
            {
                return;
            }
            DirectoryInfo apptempdinfo = new DirectoryInfo(apptempDir);
            FileInfo[] apptempfilesCheck = apptempdinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (apptempfilesCheck.Length == 0)
            {
                return;
            }
            DirectoryInfo curconfigdir = new DirectoryInfo(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
            DirectoryInfo prevconfigdir = new DirectoryInfo(Path.Combine(apptempDir, curconfigdir.Name));
            FileInfo[] apptempfilesCheck2 = prevconfigdir.GetFiles(config);
            if (apptempfilesCheck2.Length != 0)
            {
                foreach (FileInfo file in apptempfilesCheck2)
                {
                    Read("version", file.FullName); //read from new config
                    string prevversion = readKey;
                    Write("version", prevversion, config); //write to existing config
                    Read("version", config); //read to check config

                    if (prevversion == readKey) // set if successful
                        version = readKey;
                }
            }

            DirectoryInfo workingdir = new DirectoryInfo(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
            DirectoryCopy(apptempDir, approotDir, true, true, null, workingdir.Name);

            if (!Directory.Exists(approotDir)) //approotDir = Path.GetFullPath(@"..\");
            {
                return;
            }
            DirectoryInfo adinfo = new DirectoryInfo(approotDir);
            FileInfo[] afilesCheck = adinfo.GetFiles(app, SearchOption.TopDirectoryOnly);
            if (afilesCheck.Length != 0)
            {
                MessageBox.Show("Reverse Successful", "Reverse", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Failed!", "Reverse", MessageBoxButtons.OK);
            }
        }
        private void Install_Update()
        {
            Install();
            UpdateApp();
            StartUp();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Install();
            UpdateApp();
            StartUp();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            ReverseUpdateApp();
            StartUp();
        }
    }
}
