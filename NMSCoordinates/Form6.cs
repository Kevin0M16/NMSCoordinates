using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace NMSCoordinates
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        private void PopulateTreeView1()
        {
            treeView1.Nodes.Clear();
            TreeNode rootNode;
            DirectoryInfo info = new DirectoryInfo(@".\backup");

            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                //rootNode.StateImageIndex = 0;
                rootNode.Tag = info;
                //GetDirectories(info.GetDirectories(), rootNode);
                GetFiles(info.GetFiles(), rootNode);
                treeView1.Nodes.Add(rootNode);

                //if (treeView1.Nodes[0].Nodes.Count >= 1)
                //    treeView1.SelectedNode = treeView1.Nodes[0].LastNode;
                //else
                    //treeView1.SelectedNode = treeView1.TopNode;
            }
        }
        private void PopulateTreeView2()
        {
            treeView2.Nodes.Clear();
            TreeNode rootNode2;
            DirectoryInfo info2 = new DirectoryInfo(nmsPath);

            if (info2.Exists)
            {
                rootNode2 = new TreeNode(info2.Name, 0, 0);
                //rootNode2.StateImageIndex = 0;
                rootNode2.Tag = info2;
                GetDirectories(info2.GetDirectories(), rootNode2);
                treeView2.Nodes.Add(rootNode2);

                //if (treeView2.Nodes[0].Nodes.Count >= 1)
                //    treeView2.SelectedNode = treeView2.Nodes[0].LastNode;
                //else
                    treeView2.SelectedNode = treeView2.TopNode;
            }
        }
        private void GetFiles(FileInfo[] files, TreeNode nodeToAddTo)
        {
            try
            {
                TreeNode aNode;
                foreach (FileInfo file in files)
                {
                    if (file.Extension == ".zip")
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(file.FullName))
                        {
                            if (archive.Entries.Count == 0)
                                File.Delete(file.FullName);
                        }

                        aNode = new TreeNode(file.Name, 1, 1);
                        //aNode.StateImageIndex = 1;
                        aNode.Tag = file;
                        aNode.ImageKey = "file";
                        nodeToAddTo.Nodes.Add(aNode);
                    }
                }
            }
            catch
            {
                return;
            }            
        }
        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;

            DirectoryInfo[] subSubDirs;

            foreach (DirectoryInfo subDir in subDirs)
            {

                if (subDir.Name != "cache")
                {
                    aNode = new TreeNode(subDir.Name, 0, 0);
                    //aNode.StateImageIndex = 0;
                    aNode.Tag = subDir;
                    aNode.ImageKey = "folder";
                    subSubDirs = subDir.GetDirectories();
                    if (subSubDirs.Length != 0)
                    {
                        GetDirectories(subSubDirs, aNode);
                    }
                    nodeToAddTo.Nodes.Add(aNode);
                }
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode newSelected = e.Node;
            if (newSelected != null)
            {
                listView1.Items.Clear();
                ListViewItem.ListViewSubItem[] subItems;
                ListViewItem item = null;

                if (File.Exists(@".\backup\" + newSelected.Tag.ToString()))
                {
                    FileInfo file = (FileInfo)newSelected.Tag;
                    
                    using (ZipArchive archive = ZipFile.OpenRead(file.FullName))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries.OrderByDescending(f => f.LastWriteTime))
                        {
                            item = new ListViewItem(entry.Name, 1);
                            subItems = new ListViewItem.ListViewSubItem[]
                                { new ListViewItem.ListViewSubItem(item, "HG File"),
                            new ListViewItem.ListViewSubItem(item,
                            entry.LastWriteTime.ToString("MM/dd/yyyy HH:mm:ss")) };// ToShortDateString() + " " + file.LastWriteTime.ToLongTimeString())};

                            item.SubItems.AddRange(subItems);
                            listView1.Items.Add(item);
                        }
                    }
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);   
                }
            }
        }

        private void Form6_Shown(object sender, EventArgs e)
        {
            PopulateTreeView1();
            PopulateTreeView2();
        }

        private void TreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode newSelected = e.Node;
            if (newSelected != null)
            {
                listView2.Items.Clear();
                DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
                ListViewItem.ListViewSubItem[] subItems;
                ListViewItem item = null;

                foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
                {
                    if (dir.Name != "cache")
                    {
                        item = new ListViewItem(dir.Name, 0);
                        subItems = new ListViewItem.ListViewSubItem[]
                            { new ListViewItem.ListViewSubItem(item, "Directory"),
                        new ListViewItem.ListViewSubItem(item,
                        dir.LastWriteTime.ToString("MM/dd/yyyy HH:mm:ss"))};
                        item.SubItems.AddRange(subItems);
                        listView2.Items.Add(item);
                    }
                }
                foreach (FileInfo file in nodeDirInfo.GetFiles("*.hg").OrderByDescending(f => f.LastWriteTime))
                {
                    item = new ListViewItem(file.Name, 1);
                    subItems = new ListViewItem.ListViewSubItem[]
                        { new ListViewItem.ListViewSubItem(item, "HG File"),
                    new ListViewItem.ListViewSubItem(item,
                    file.LastWriteTime.ToString("MM/dd/yyyy HH:mm:ss"))};

                    item.SubItems.AddRange(subItems);
                    listView2.Items.Add(item);
                }

                listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TreeNode newSelected = treeView1.SelectedNode;

            if (newSelected != null && listView1.SelectedItems.Count != 0)
            {
                FileInfo file = (FileInfo)newSelected.Tag;
                if (file.Exists)// && ZipFile.OpenRead(file.FullName).Entries.Count != 0)
                {
                    if (Directory.Exists(@".\backup\temp"))
                    {
                        Directory.Delete(@".\backup\temp", true);
                    }

                    ZipFile.ExtractToDirectory(file.FullName, @".\backup\temp");

                    DirectoryInfo temp = new DirectoryInfo(@".\backup\temp");
                    DirectoryInfo nmssave = (DirectoryInfo)treeView2.SelectedNode.Tag;

                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        FileInfo[] files = temp.GetFiles(item.Text);
                        //textBox1.Text = item.Text;
                        foreach (FileInfo f in files)
                        {
                            //Directory.CreateDirectory(@".\backup\temp2\");
                            //File.Copy(f.FullName, @".\backup\temp2\" + f.Name, true);
                            if (!File.Exists(nmssave.FullName + @"\" + f.Name))
                            {
                                File.Copy(f.FullName, nmssave.FullName + @"\" + f.Name, true);
                                textBox1.AppendText("Restored: " + f.Name + "\r\n");
                            }
                            else
                            {
                                MessageBox.Show("File Already Exist!", "Alert");
                                Directory.Delete(@".\backup\temp", true);
                                return;
                            }
                        }
                    }
                    Directory.Delete(@".\backup\temp", true);

                    //listView1.Items.Clear();
                    listView2.Items.Clear();
                    PopulateTreeView2();
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {            
            TreeNode newSelected = treeView2.SelectedNode;
            
            if (newSelected != null && listView2.SelectedItems.Count != 0)
            {
                DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
                if (nodeDirInfo.Exists)
                {
                    if (Directory.Exists(@".\backup\temp"))
                    {
                        Directory.Delete(@".\backup\temp", true);
                    }
                    try
                    {
                        foreach (ListViewItem item in listView2.SelectedItems)
                        {
                            FileInfo[] files = nodeDirInfo.GetFiles(item.Text);
                            //textBox1.Text = item.Text;
                            foreach (FileInfo file in files)
                            {
                                Directory.CreateDirectory(@".\backup\temp\");
                                File.Copy(file.FullName, @".\backup\temp\" + file.Name, true);
                                textBox1.AppendText("Zipped: " + file.Name + "\r\n");
                                
                            }
                        }
                        string now = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                        if (Directory.GetDirectories(@".\backup\temp").Length != 0)
                        {
                            ZipFile.CreateFromDirectory(@".\backup\temp", @".\backup\savemanager_" + now + ".zip");
                            textBox1.AppendText("Created: savemanager_" + now + ".zip \r\n");
                        }                        

                        Directory.Delete(@".\backup\temp", true);

                        listView1.Items.Clear();
                        //listView2.Items.Clear();
                        PopulateTreeView1();
                        if (treeView1.Nodes[0].Nodes.Count >= 1)
                            treeView1.SelectedNode = treeView1.Nodes[0].LastNode;
                    }
                    catch
                    {
                        return;
                    }                    
                }
            }
        }
        private TreeNode FindRootNode(TreeNode treeNode)
        {
            while (treeNode.Parent != null)
            {
                treeNode = treeNode.Parent;
            }
            return treeNode;
        }
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {            
            if (treeView1.SelectedNode != null)
            {
                TreeNode newSelected = treeView1.SelectedNode;

                if (newSelected.Tag.ToString() != @".\backup")
                {
                    FileInfo file = (FileInfo)newSelected.Tag;

                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete: \n\n\r " + file.Name + "? ", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        textBox1.AppendText("Deleted: " + file.Name + "\r\n");
                        File.Delete(file.FullName);                        

                        listView1.Items.Clear();
                        PopulateTreeView1();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }
        }
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete \n\n\r " + listView2.SelectedItems.Count.ToString() + " Save Files? ", "Confirmation", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DirectoryInfo nmssave = (DirectoryInfo)treeView2.SelectedNode.Tag;

                    foreach (ListViewItem item in listView2.SelectedItems)
                    {
                        FileInfo[] files = nmssave.GetFiles(item.Text);

                        foreach (FileInfo file in files)
                        {
                            textBox1.AppendText("Deleted: " + file.Name + "\r\n");
                            File.Delete(file.FullName);
                        }
                    }
                    listView2.Items.Clear();
                    PopulateTreeView2();                    
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }
        private void TreeView2_AfterExpand(object sender, TreeViewEventArgs e)
        {
            //treeView2.SelectedNode = treeView2.Nodes[0].LastNode;
            treeView2.SelectedNode = treeView2.TopNode;
        }

        private void TreeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            treeView1.SelectedNode = treeView1.Nodes[0].LastNode;
        }        
    }
}
