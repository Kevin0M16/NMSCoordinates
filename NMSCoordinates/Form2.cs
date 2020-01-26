using System.Windows.Forms;

namespace NMSCoordinates
{

    public partial class Form2 : Form
    {
        public Form2(string version)
        {
            InitializeComponent();
            Version = version;
        }       

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("http://www.nmscoordinates.com/");
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
                System.Diagnostics.Process.Start("https://github.com/Kevin0M16/NMSCoordinates");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("https://github.com/Kevin0M16/NMSCoordinates/releases/latest");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }
        /*
        private void Button9_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9(Version);
            f9.ShowDialog();
        }
        */
    }
}
