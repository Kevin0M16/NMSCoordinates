using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
