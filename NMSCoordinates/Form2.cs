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
            
            //string hgFilePath = "save4.hg";
            

        }
        private List<TreeNode> CurrentNodeMatches = new List<TreeNode>();

        private int LastNodeIndex = 0;

        private string LastSearchText;
        private string hgFilePath;

        private void Button1_Click_1(object sender, EventArgs e)
        {


            string searchText = this.textBox1.Text;
            if (String.IsNullOrEmpty(searchText))
            {
                return;
            };


            if (LastSearchText != searchText)
            {
                //It's a new Search
                CurrentNodeMatches.Clear();
                LastSearchText = searchText;
                LastNodeIndex = 0;
                SearchNodes(searchText, treeView1.Nodes[0]);
            }

            if (LastNodeIndex >= 0 && CurrentNodeMatches.Count > 0 && LastNodeIndex < CurrentNodeMatches.Count)
            {
                TreeNode selectedNode = CurrentNodeMatches[LastNodeIndex];
                LastNodeIndex++;
                this.treeView1.SelectedNode = selectedNode;
                this.treeView1.SelectedNode.Expand();
                this.treeView1.Select();

            }
        }

        private void SearchNodes(string SearchText, TreeNode StartNode)
        {
            TreeNode node = null;
            while (StartNode != null)
            {
                if (StartNode.Text.ToLower().Contains(SearchText.ToLower()))
                {
                    CurrentNodeMatches.Add(StartNode);
                };
                if (StartNode.Nodes.Count != 0)
                {
                    SearchNodes(SearchText, StartNode.Nodes[0]);//Recursive Search 
                };
                StartNode = StartNode.NextNode;
            };

        }        
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //treeView1.Dispose();
            Close();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            hgFilePath = MyProperty3;
            string json = File.ReadAllText(hgFilePath);

            JsonTreeViewLoader.LoadJsonToTreeView(treeView1, json);
        }
    }
}
