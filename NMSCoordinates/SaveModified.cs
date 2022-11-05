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

        private void Button2_Click(object sender, EventArgs e)
        {
            //SaveChanged = false;
            this.Visible = false;
        }
    }
}
