using System.Windows.Forms;
using Octokit;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ComponentModel;
using System;

namespace NMSCoordinates
{
    public partial class Form9 : Form
    {
        WebClient webClient;               // Our WebClient that will be doing the downloading for us
        Stopwatch sw = new Stopwatch();
        private string assetUrl;
        private string assetName;

        public Form9(string version)
        {
            InitializeComponent();
            textBox1.Text = version;
            CheckForUpdates();
        }
        private async void CheckForUpdates()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("NMSCoordinates"));
                var releases = await client.Repository.Release.GetAll("Kevin0M16", "NMSCoordinates");
                var latest = releases[0];
                assetUrl = releases[0].Assets[0].BrowserDownloadUrl;
                assetName = releases[0].Assets[0].Name;
                textBox2.Text = latest.Name;
                textBox3.Text = assetName;
            }
            catch
            {
                MessageBox.Show("Github Server not available. Could not check version", "Alert");
            }
        }   

        private void Button1_Click(object sender, System.EventArgs e)
        {
            //this.Close();
            //DownloadFile("https://github.com/Kevin0M16/NMSCoordinates/releases/download/v1.1.13/NMSCoordinates-v1.1.13.zip", "test.zip");
            DownloadFile(assetUrl, "test.zip");
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
                MessageBox.Show("Download has been canceled.", "NMSCoordinates Update", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Download completed!", "NMSCoordinates Update", MessageBoxButtons.OK);
            }
        }
    }
}
