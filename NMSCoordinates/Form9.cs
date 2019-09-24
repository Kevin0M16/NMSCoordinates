using System.Windows.Forms;
using Octokit;

namespace NMSCoordinates
{
    public partial class Form9 : Form
    {
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
                textBox2.Text = latest.Name;
            }
            catch
            {
                MessageBox.Show("Github Server not available. Could not check version", "Alert");
            }
        }   

        private void Button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
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
    }
}
