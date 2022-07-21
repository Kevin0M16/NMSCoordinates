using System;
using System.Windows.Forms;

namespace NMSCoordinates
{
    public partial class SaveModified : Form
    {
        
        public SaveModified()
        {
            InitializeComponent();
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SaveChanged = true;
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SaveChanged = false;
            this.Close();
        }
    }
}
