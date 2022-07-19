using Microsoft.Win32;
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
    public partial class CoordinateCalculator : Form
    {
        public CoordinateCalculator()
        {
            InitializeComponent();

            //Check resolution
            CheckRes();

            glyphDict = Globals.Glyphs();
            ShowGlyphKey();
        }
        private int Planet;// { get; set; }
        public int _ScreenWidth { get; private set; }
        public int _ScreenHeight { get; private set; }

        private void Form5_Load(object sender, EventArgs e)
        {
            //Trigger if Display resolution changes
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
        }
        // This method is called when the display settings change.
        private async void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);
            CheckRes();
        }
        private void CheckRes()
        {
            _ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            _ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

            if (_ScreenHeight > 768)
            {
                this.MinimumSize = new Size(535, 750);
                this.Size = new Size(535, 790);
            }
            if (_ScreenHeight <= 768 && _ScreenHeight > 720)
            {
                this.MinimumSize = new Size(535, 730);
                this.Size = new Size(535, 735);
            }
            if (_ScreenHeight <= 720)
            {
                this.MinimumSize = new Size(535, 650);
                this.Size = new Size(535, 685);                
            }
        }
        private void ShowGlyphKey()
        { 
            pictureBox1.Image = Properties.Resources._0;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Properties.Resources._1;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.Image = Properties.Resources._2;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = Properties.Resources._3;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.Image = Properties.Resources._4;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Image = Properties.Resources._5;
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Image = Properties.Resources._6;
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.Image = Properties.Resources._7;
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox9.Image = Properties.Resources._8;
            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox10.Image = Properties.Resources._9;
            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox11.Image = Properties.Resources.A;
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox12.Image = Properties.Resources.B;
            pictureBox12.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox13.Image = Properties.Resources.C;
            pictureBox13.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox14.Image = Properties.Resources.D;
            pictureBox14.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.Image = Properties.Resources.E;
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox16.Image = Properties.Resources.F;
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void ShowGlyphs()
        {
            try
            {
                //Display selected location glyph images
                pictureBox17.Image = glyphDict[_gl1];
                pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox18.Image = glyphDict[_gl2];
                pictureBox18.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox19.Image = glyphDict[_gl3];
                pictureBox19.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox20.Image = glyphDict[_gl4];
                pictureBox20.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox21.Image = glyphDict[_gl5];
                pictureBox21.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox22.Image = glyphDict[_gl6];
                pictureBox22.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox23.Image = glyphDict[_gl7];
                pictureBox23.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox24.Image = glyphDict[_gl8];
                pictureBox24.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox25.Image = glyphDict[_gl9];
                pictureBox25.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox26.Image = glyphDict[_gl10];
                pictureBox26.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox27.Image = glyphDict[_gl11];
                pictureBox27.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox28.Image = glyphDict[_gl12];
                pictureBox28.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch
            {

            }
            
        }
        private void ClearHx()
        {
            //hxx = "";
            //hxe = 0;
            GalacticCoord2 = "";
            GalacticCoord = "";
            Planet = 0;
            PortalCode = "";
        }
        private void PortalLookup()
        {
            try
            {
                //Index chars in PortalCode
                if (PortalCode[0] != '\0')
                     _gl1 = PortalCode[0];
                if (PortalCode[1] != '\0')
                    _gl2 = PortalCode[1];
                if (PortalCode[2] != '\0')
                    _gl3 = PortalCode[2];
                if (PortalCode[3] != '\0')
                    _gl4 = PortalCode[3];
                if (PortalCode[4] != '\0')
                    _gl5 = PortalCode[4];
                if (PortalCode[5] != '\0')
                    _gl6 = PortalCode[5];
                if (PortalCode[6] != '\0')
                    _gl7 = PortalCode[6];
                if (PortalCode[7] != '\0')
                    _gl8 = PortalCode[7];
                if (PortalCode[8] != '\0')
                    _gl9 = PortalCode[8];
                if (PortalCode[9] != '\0')
                    _gl10 = PortalCode[9];
                if (PortalCode[10] != '\0')
                    _gl11 = PortalCode[10];
                if (PortalCode[11] != '\0')
                    _gl12 = PortalCode[11];
            }
            catch
            {
                return;
            }
            
        }
        private void Clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();

            pictureBox17.Image = null;
            pictureBox18.Image = null;
            pictureBox19.Image = null;
            pictureBox20.Image = null;
            pictureBox21.Image = null;
            pictureBox22.Image = null;
            pictureBox23.Image = null;
            pictureBox24.Image = null;
            pictureBox25.Image = null;
            pictureBox26.Image = null;
            pictureBox27.Image = null;
            pictureBox28.Image = null;

            PortalCode = "";

            _gl1 = '\0';
            _gl2 = '\0';
            _gl3 = '\0';
            _gl4 = '\0';
            _gl5 = '\0';
            _gl6 = '\0';
            _gl7 = '\0';
            _gl8 = '\0';
            _gl9 = '\0';
            _gl10 = '\0';
            _gl11 = '\0';
            _gl12 = '\0';
        }
        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button1_Click(this, new EventArgs());
            }

            if (e.KeyCode == Keys.Back)
            {
                Clear();
            }
        }
        private void TextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button2_Click(this, new EventArgs());
            }

            if (e.KeyCode == Keys.Back)
            {
                Clear();
            }
        }
        private void TextBox8_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button4_Click(this, new EventArgs());
            }

            if (e.KeyCode == Keys.Back)
            {
                Clear();
            }
        }
        private void TextBox6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button3_Click(this, new EventArgs());
            }

            if (e.KeyCode == Keys.Back)
            {
                Clear();
            }
        }
        private bool ValidateCoord(string A, string B, string C, string D)
        {
            bool x = Convert.ToInt32(A, 16) > 4096 || Convert.ToInt32(B, 16) > 255 || Convert.ToInt32(C, 16) > 4096 || Convert.ToInt32(D, 16) > 767;
            return x;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //Portal to Voxel
            try
            {
                if (textBox1.Text != "")
                {
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox7.Clear();
                    textBox8.Clear();

                    string t1 = textBox1.Text.Replace(" ", "");

                    if(t1.Length == 12 && !textBox1.Text.Contains(":"))
                    {
                        //Gives both Galactic and Voxel
                        GalacticCoord2 = CoordCalculations.PortalToVoxel(t1, textBox7);

                        string[] value = GalacticCoord2.Replace(" ", "").Split(':');
                        string A = value[0].Trim();
                        string B = value[1].Trim();
                        string C = value[2].Trim();
                        string D = value[3].Trim();

                        //Validate Coordinates
                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            Clear();
                            Globals.AppendLine(textBox7, "Invalid Coordinates!");
                            return;
                        }

                        textBox2.Text = GalacticCoord2;
                        PortalCode = textBox1.Text;
                    }
                    else
                    {
                        Clear();
                        Globals.AppendLine(textBox7, "Incorrect Coordinate Input!");
                    }
                        
                }
            }
            catch
            {
                Clear();
                Globals.AppendLine(textBox7, "Incorrect Coordinate Input!");
            }         
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            //Galactic Coordinate to Voxel Coordinate
            try
            {
                if (textBox2.Text != "")
                {
                    textBox1.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox7.Clear();
                    textBox8.Clear();

                    string t2 = textBox2.Text.Replace(" ", "");

                    if (t2.Contains(":") && t2.Length == 19)
                    {
                        string[] value = t2.Replace(" ", "").Split(':');

                        string A = value[0].Trim();
                        string B = value[1].Trim();
                        string C = value[2].Trim();
                        string D = value[3].Trim();

                        //Validate Coordinate
                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            Clear();
                            Globals.AppendLine(textBox7, "Invalid Coordinates!");
                            return;
                        }

                        CoordCalculations.GalacticToVoxel(A, B, C, D, textBox7);
                        Planet = 0; //Default 0 for Planet #
                        CoordCalculations.GalacticToPortal(Planet, A, B, C, D, out PortalCode, textBox7);                       

                        textBox1.Text = PortalCode;
                    }

                    if(t2.Length == 16 && !t2.Contains(":"))
                    {
                        //0000 0000 0000 0000  XXXX:YYYY:ZZZZ:SSIX  A B C D
                        string A = t2.Substring(t2.Length - 16, 4);
                        string B = t2.Substring(t2.Length - 12, 4);
                        string C = t2.Substring(t2.Length - 8, 4);
                        string D = t2.Substring(t2.Length - 4, 4);

                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            Clear();
                            Globals.AppendLine(textBox7, "Invalid Coordinates!");
                            return;
                        }

                        CoordCalculations.GalacticToVoxel(A, B, C, D, textBox7);
                        Planet = 0; //Default 0 for Planet #
                        CoordCalculations.GalacticToPortal(Planet, A, B, C, D, out PortalCode, textBox7);

                        textBox1.Text = PortalCode;
                    }

                    if(t2.Replace(":", "").Length < 16 | t2.Replace(":", "").Length > 16 | t2.Length < 16)
                    {
                        Clear();
                        Globals.AppendLine(textBox7, "Incorrect Coordinate Input!");
                    }
                    
                }
            }
            catch
            {
                Clear();                
                Globals.AppendLine(textBox7, "Incorrect Coordinate Input!");
            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            //Voxel Coordinate to Galactic Coordinate
            try
            {
                if (textBox3.Text != "")
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox7.Clear();
                    textBox8.Clear();

                    GalacticCoord = CoordCalculations.VoxelToGalacticCoord(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox6.Text), textBox7);

                    string[] value = GalacticCoord.Replace(" ", "").Split(':');
                    string A = value[0].Trim();
                    string B = value[1].Trim();
                    string C = value[2].Trim();
                    string D = value[3].Trim();

                    //Validate Coordinate
                    if (ValidateCoord(A, B, C, D))
                    {
                        MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                        Clear();
                        Globals.AppendLine(textBox7, "Invalid Coordinates!");
                        return;
                    }

                    textBox2.Text = GalacticCoord;

                    //string[] value = GalacticCoord.Replace(" ", "").Split(':');
                    Planet = 0; //Default 0 for Planet #
                    CoordCalculations.GalacticToPortal(Planet, A, B, C, D, out PortalCode, textBox7);
                    textBox1.Text = PortalCode;
                }
            }
            catch
            {
                Clear();
                Globals.AppendLine(textBox7, "Incorrect Coordinate Input!");
            }
        }        
        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox8.Text != "")
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox7.Clear();

                    string t8 = textBox8.Text.Replace(" ", "");

                    ClearHx();
                    //HexToVoxel(t8);
                    CoordCalculations.CalculateLongHex(t8, out GalacticCoord2, out Planet, out int gxy, textBox7);

                    string[] value = GalacticCoord2.Replace(" ", "").Split(':');
                    string A = value[0].Trim();
                    string B = value[1].Trim();
                    string C = value[2].Trim();
                    string D = value[3].Trim();

                    //Validate Coordinates
                    if (ValidateCoord(A, B, C, D))
                    {
                        MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                        Clear();
                        Globals.AppendLine(textBox7, "Invalid Coordinates!");
                        return;
                    }

                    //CalculateVoxelFromHex(t8);
                    textBox2.Text = GalacticCoord2;
                    CoordCalculations.GalacticToPortal(Planet, A, B, C, D, out PortalCode, textBox7);
                    textBox1.Text = PortalCode;
                }
            }
            catch
            {
                Clear();
                Globals.AppendLine(textBox7, "Incorrect Coordinate Input!");
            }
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("0");
            else
                Clear();
        }
        private void PictureBox2_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("1");
            else
                Clear();
        }
        private void PictureBox3_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("2");
            else
                Clear();
        }
        private void PictureBox4_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("3");
            else
                Clear();
        }
        private void PictureBox5_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("4");
            else
                Clear();
        }
        private void PictureBox6_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("5");
            else
                Clear();
        }
        private void PictureBox7_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("6");
            else
                Clear();
        }
        private void PictureBox8_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("7");
            else
                Clear();
        }
        private void PictureBox9_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("8");
            else
                Clear();
        }
        private void PictureBox10_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("9");
            else
                Clear();
        }
        private void PictureBox11_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("A");
            else
                Clear();
        }
        private void PictureBox12_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("B");
            else
                Clear();
        }
        private void PictureBox13_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("C");
            else
                Clear();
        }
        private void PictureBox14_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("D");
            else
                Clear();
        }
        private void PictureBox15_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("E");
            else
                Clear();
        }
        private void PictureBox16_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 12)
                textBox1.AppendText("F");
            else
                Clear();
        }
        private NMSCoordinatesMain f1;
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                f1 = (NMSCoordinatesMain)Application.OpenForms["Form1"];
                if (!f1.TextBoxPerm == true)
                {
                    f1.TextBoxValue = textBox2.Text;

                    if (f1 == null)
                    {
                        f1 = new NMSCoordinatesMain();
                        f1.FormClosed += (_, arg) =>
                        {
                            f1 = null;
                        };
                        f1.Show();
                    }
                    else
                    {
                        f1.BringToFront();
                    }
                }
                else
                {
                    Globals.AppendLine(textBox7, "Please UNLOCK Manual Travel!");
                    MessageBox.Show("PLease UNLOCK Manual Travel!", "Alert");
                }
            }
        }
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();  //Clear if any old value is there in Clipboard  
            if (textBox2.Text != "")
                Clipboard.SetText(textBox2.Text); //Copy text to Clipboard
        }
        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            textBox2.Text = Clipboard.GetText();
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            PortalCode = textBox1.Text;
            PortalCode = PortalCode.Replace(" ", "");
            PortalLookup();
            ShowGlyphs();
        }        
    }
}
