using System.Windows.Forms;
using Octokit;
using System.Diagnostics;
using System.Net;

namespace NMSCoordinates
{
    public partial class UpdateChecker : Form
    {
        WebClient webClient;               // Our WebClient that will be doing the downloading for us
        Stopwatch sw = new Stopwatch();
        private string assetUrl;
        private string assetName;

        public UpdateChecker(string version)
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
                //textBox3.Text = assetName;
            }
            catch
            {
                MessageBox.Show("Github Server not available. Could not check version", "Alert");
            }
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
    }
}
