using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMSCoordinates
{
    public partial class Form8 : Form
    {
        public Form8(List<DirectoryInfo> SaveDirs)
        {
            InitializeComponent();
            GetSaveDirs(SaveDirs);
        }
        private void GetSaveDirs(List<DirectoryInfo> SaveDirs)
        {
            saveDict = new Dictionary<string, string>();            

            foreach (DirectoryInfo dir in SaveDirs)
            {
                //Only include st and gog dirs
                if (dir.Name.Contains("st_"))
                {
                    string sname = "[STEAM] " + dir;
                    listBox1.Items.Add(sname);
                    saveDict.Add(new KeyValuePair<string, string>(sname, dir.FullName));
                }
                else if (dir.Name.Contains("DefaultUser"))
                {
                    string gogname = "[GoG] " + dir;
                    listBox1.Items.Add(gogname);
                    saveDict.Add(new KeyValuePair<string, string>(gogname, dir.FullName));
                }
            }
        }
        
        public string GoGPath { get; set; }
        private void Button1_Click(object sender, EventArgs e)
        {
            string selected = listBox1.SelectedItem.ToString();
            if (saveDict.ContainsKey(selected))
                GoGPath = saveDict[selected];

            this.Close();
        }
    }
}
