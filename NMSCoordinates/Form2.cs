using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JsonTreeView;
using QuickType;

namespace NMSCoordinates
{

    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }
        
        private string hgFilePath;
    
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //treeView1.Dispose();
            Close();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            hgFilePath = MyProperty3;
            string json = File.ReadAllText(hgFilePath);
        }
    }
}
