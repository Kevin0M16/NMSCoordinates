using System;
using System.Windows.Forms;

namespace NMSCoordinates
{
    public partial class ScreenShotPreview : Form
    {
        public ScreenShotPreview()
        {
            InitializeComponent();            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_Shown(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = MyProperty;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
