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

    public partial class Form3 : Form
    {
        
        public Form3()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        private void SetSShot()
        {
            AppendLine(textBox1, ssdPath);
            try
            {
                List<string> list = new List<string>();

                if (Directory.Exists(ssdPath))
                {
                        
                    DirectoryInfo dinfo2 = new DirectoryInfo(ssdPath);
                    FileInfo[] Files = dinfo2.GetFiles("*.jpg", SearchOption.AllDirectories);

                    if (Files.Length != 0)
                    {
                        foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                        {
                            if (!file.DirectoryName.Contains("thumbnails"))
                                list.Add(file.FullName);
                            //AppendLine(textBox1, file.FullName.ToString());
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null;
                        pictureBox2.Image = null;
                        pictureBox3.Image = null;
                        pictureBox4.Image = null;
                        AppendLine(textBox1, "ssPath error! 855");
                        return;
                    }
                    
                    pictureBox1.ImageLocation = list[0].ToString();
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    
                    pictureBox2.ImageLocation = list[1].ToString();
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    
                    pictureBox3.ImageLocation = list[2].ToString();
                    pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                    
                    pictureBox4.ImageLocation = list[3].ToString();
                    pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;                                                                             

                }
                else
                {
                    AppendLine(textBox1, "ssPath error! 123");
                    return;
                }               
                
            }
            catch
            {
                //AppendLine(textBox1, "ssPath error! 155");
                return;
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            ssdPath = MyProperty2;
            SetSShot();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.MyProperty = pictureBox1.ImageLocation;
            f4.ShowDialog();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.MyProperty = pictureBox2.ImageLocation;
            f4.ShowDialog();
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.MyProperty = pictureBox3.ImageLocation;
            f4.ShowDialog();
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.MyProperty = pictureBox4.ImageLocation;
            f4.ShowDialog();
        }
    }
}
