
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuickType;
using System.Threading;
using System.Runtime.InteropServices;

namespace NMSCoordinates
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Glyphs();
            GIndex();
            JsonKey();
            nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            LoadCmbx();

            DiscList = new List<string>();
            BaseList = new List<string>();
        }
        private void LoadCmbx()
        {
            //Load save file names in combobox1
            comboBox1.Items.Clear();

            if (!Directory.Exists(nmsPath))
            {
                throw new FileNotFoundException(string.Format("No Man's Sky save game folder not found at expected location: {0}", nmsPath));
            }
            DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
            FileInfo[] Files = dinfo.GetFiles("save*.hg", SearchOption.AllDirectories);
            if (Files.Length != 0)
            {
                //hgFilePath = Files[0].FullName;
                foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                {
                    comboBox1.Items.Add(file);
                    
                }                

                //comboBox1.SelectedIndex = 0;
                AppendLine(textBox16, Path.GetDirectoryName(Files[0].FullName));
                AppendLine(textBox26, Files[0].LastWriteTime.ToLongDateString() + " " + Files[0].LastWriteTime.ToLongTimeString());

            }
            else
            {
                AppendLine(textBox16, "No save files found!");
                return;
            }
        }
        private void BackupLoc(string path)
        {
            for (int i = 0; i < DiscList.Count; i++)
            {
                JsonMap(i);
                GetPortalCoord(iX, iY, iZ, iSSI);
                GetGalacticCoord(iX, iY, iZ, iSSI);
                Backuplist.Add("Loc: " + DiscList[i] + " - PC: " + PortalCode + " -- GC: " + GalacticCoord);                
            }
            File.WriteAllLines(path, Backuplist);
            Process.Start("backup.txt");
        }
        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        private void SetPlayerCoord()
        {   
            //won't work need replace text
            ipgalaxy = Convert.ToInt32(textBox4.Text);
            ipX = Convert.ToInt32(textBox5.Text);
            ipY = Convert.ToInt32(textBox6.Text);
            ipZ = Convert.ToInt32(textBox7.Text);
            ipSSI = Convert.ToInt32(textBox8.Text);
            ipPI = Convert.ToInt32(textBox9.Text);

            var nms = Nms.FromJson(json);

            nms.The6F.YhJ.Iis = ipgalaxy;
            nms.The6F.YhJ.OZw["dZj"] = ipX;
            nms.The6F.YhJ.OZw["IyE"] = ipY;
            nms.The6F.YhJ.OZw["uXE"] = ipZ;
            nms.The6F.YhJ.OZw["vby"] = ipSSI;
            nms.The6F.YhJ.OZw["jsv"] = ipPI;
        }
        private void GetPlayerCoord()
        {
            //Gets the player position off the save file and prints the info on tab1
            textBox22.Clear();
            textBox21.Clear();
            textBox23.Clear();

            var nms = Nms.FromJson(json);

            var pgalaxy = nms.The6F.YhJ.Iis.ToString();//.ToString();
            var pX = nms.The6F.YhJ.OZw["dZj"];//.ToString();
            var pY = nms.The6F.YhJ.OZw["IyE"];//.ToString();
            var pZ = nms.The6F.YhJ.OZw["uXE"];//.ToString();
            var pSSI = nms.The6F.YhJ.OZw["vby"];//.ToString();
            var pPI = nms.The6F.YhJ.OZw["jsv"];//.ToString();

            GetGalacticCoord(Convert.ToInt32(pX), Convert.ToInt32(pY), Convert.ToInt32(pZ), Convert.ToInt32(pSSI));
            AppendLine(textBox22, GalacticCoord);
            GetPortalCoord(Convert.ToInt32(pX), Convert.ToInt32(pY), Convert.ToInt32(pZ), Convert.ToInt32(pSSI));
            ShowPGlyphs();
            AppendLine(textBox21, PortalCode);
            AppendLine(textBox23, galaxyDict[pgalaxy]);
        }
        private void Clearforsearch()
        {
            textBox1.Clear();
            textBox2.Clear();            
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;
            pictureBox11.Image = null;
            pictureBox12.Image = null;

        }
        private void ClearAll()
        {   
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox22.Clear();
            textBox21.Clear();
            textBox23.Clear();

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;
            pictureBox11.Image = null;
            pictureBox12.Image = null;
            pictureBox13.Image = null;
            pictureBox14.Image = null;
            pictureBox15.Image = null;
            pictureBox16.Image = null;
            pictureBox17.Image = null;
            pictureBox18.Image = null;
            pictureBox19.Image = null;
            pictureBox20.Image = null;
            pictureBox21.Image = null;
            pictureBox22.Image = null;
            pictureBox23.Image = null;
            pictureBox24.Image = null;

            DiscList.Clear();
            BaseList.Clear();
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox3.DataSource = null;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            galaxy = "";
            X = "";
            Y = "";
            Z = "";
            SSI = "";
            PI = "";

            PortalCode = "";
            GalacticCoord = "";
            GalacticCoord2 = "";
        }
        private void TextBoxes()
        {
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();

            textBox4.Text = galaxy;
            textBox5.Text = X;
            textBox6.Text = Y;
            textBox7.Text = Z;
            textBox8.Text = SSI;
            textBox9.Text = PI;
        }
        private void JsonMap(int i)
        {
            try
            {
                var nms = Nms.FromJson(json);
                galaxy = nms.The6F.NlG[i].YhJ.Iis.ToString();
                X = nms.The6F.NlG[i].YhJ.OZw["dZj"].ToString();
                Y = nms.The6F.NlG[i].YhJ.OZw["IyE"].ToString();
                Z = nms.The6F.NlG[i].YhJ.OZw["uXE"].ToString();
                SSI = nms.The6F.NlG[i].YhJ.OZw["vby"].ToString();
                PI = nms.The6F.NlG[i].YhJ.OZw["jsv"].ToString();

                igalaxy = Convert.ToInt32(nms.The6F.NlG[i].YhJ.Iis);
                iX = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["dZj"]);
                iY = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["IyE"]);
                iZ = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["uXE"]);
                iSSI = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["vby"]);
                iPI = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["jsv"]);
            }
            catch
            {
                AppendLine(textBox17, "** Code 2 **");
                return;
            }
        }
        private void Glyphs()
        {
            glyphDict = new Dictionary<char, Bitmap>();
            glyphDict.Add('0', Properties.Resources._0);
            glyphDict.Add('1', Properties.Resources._1);
            glyphDict.Add('2', Properties.Resources._2);
            glyphDict.Add('3', Properties.Resources._3);
            glyphDict.Add('4', Properties.Resources._4);
            glyphDict.Add('5', Properties.Resources._5);
            glyphDict.Add('6', Properties.Resources._6);
            glyphDict.Add('7', Properties.Resources._7);
            glyphDict.Add('8', Properties.Resources._8);
            glyphDict.Add('9', Properties.Resources._9);
            glyphDict.Add('A', Properties.Resources.A);
            glyphDict.Add('B', Properties.Resources.B);
            glyphDict.Add('C', Properties.Resources.C);
            glyphDict.Add('D', Properties.Resources.D);
            glyphDict.Add('E', Properties.Resources.E);
            glyphDict.Add('F', Properties.Resources.F);
        }       

        private void GIndex()
        {
            galaxyDict = new Dictionary<string, string>();
            galaxyDict.Add(new KeyValuePair<string, string>("0", "Euclid"));
            galaxyDict.Add(new KeyValuePair<string, string>("1", "Hilbert"));
            galaxyDict.Add(new KeyValuePair<string, string>("2", "Calypso"));
            galaxyDict.Add(new KeyValuePair<string, string>("3", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("4", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("5", "Haydes"));
            galaxyDict.Add(new KeyValuePair<string, string>("6", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("7", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("8", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("9", "Eissentam"));
            galaxyDict.Add(new KeyValuePair<string, string>("10", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("11", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("12", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("13", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("14", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("15", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("16", "Hitonskyer"));
            galaxyDict.Add(new KeyValuePair<string, string>("17", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("18", "Isdoraijung"));
            galaxyDict.Add(new KeyValuePair<string, string>("19", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("20", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("21", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("22", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("23", "unknown"));
            galaxyDict.Add(new KeyValuePair<string, string>("24", "Twerbetek"));
            galaxyDict.Add(new KeyValuePair<string, string>("140", "Kimycuristh"));
            //galaxyDict.Add(new KeyValuePair<string, string>("24", "Twerbetek"));
        }
        private void Loadlsb1()
        {
            DiscList.Clear();
            listBox2.Items.Clear();
            TextBoxes();

            var nms = Nms.FromJson(json);
            try
            {
                for (int i = 0; i < 500; i++)
                {
                    //string discd = jsonObj["6f="]["nlG"][i]["NKm"];
                    string discd = nms.The6F.NlG[i].NKm;

                    if (nms.The6F.NlG[i].IAf == "Spacestation")
                    {
                        string ss = discd + " (SS)";
                        DiscList.Add(ss);
                        listBox2.Items.Add(ss);
                    }
                    else
                    {
                        string bl = discd + " (B)";
                        DiscList.Add(bl);
                    }
                }
            }
            catch
            {
                listBox1.DataSource = DiscList;
                textBox19.Text = listBox1.Items.Count.ToString();
                textBox20.Text = listBox2.Items.Count.ToString();
                listBox1.SelectedIndex = -1;

                if (nms.The6F.DaC == true)
                {
                    textBox12.Text = "True";
                }
                else
                {
                    textBox12.Text = "False";
                }

                return;
            }                     
        }
        private void Loadlsb3()
        {
            var nms = Nms.FromJson(json);
            try
            {
                for (int i = 0; i < 500; i++)
                {
                    string baseN = nms.The6F.F0[i].NKm;
                    if (baseN != "")
                    {                        
                        BaseList.Add(baseN);                                                   
                    }
                    else
                    {
                        BaseList.Add(baseN + "Not Named");
                    }
                }
            }
            catch
            {
                listBox3.DataSource = BaseList;
                textBox18.Text = listBox3.Items.Count.ToString();
                return;
            }
        }
        private void ListBox1_MouseClick(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = -1;
            try
            {
                if (listBox1.SelectedItem.ToString() != "")
                {
                    int i = listBox1.SelectedIndex;
                    JsonMap(i);
                    TextBoxes();
                    textBox10.Text = galaxyDict[galaxy];
                    GetGalacticCoord(iX, iY, iZ, iSSI);
                    GetPortalCoord(iX, iY, iZ, iSSI);
                    ShowGlyphs();
                    AppendLine(textBox1, GalacticCoord);
                    AppendLine(textBox2, PortalCode);
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 1 **");
                return;
            }
        }
        private void ListBox2_MouseClick(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
            try
            {
                if (listBox2.SelectedItem.ToString() != "")
                {
                    object selecteditem = listBox2.SelectedItem;
                    string si = selecteditem.ToString();
                    si = si.Replace(" (SS)", "");
                    var nms = Nms.FromJson(json);
                    try
                    {
                        for (int i = 0; i < 500; i++)
                        {
                            if (nms.The6F.NlG[i].NKm.ToString() == si)
                            {
                                JsonMap(i);
                                TextBoxes();
                                textBox10.Text = galaxyDict[galaxy];
                                GetGalacticCoord(iX, iY, iZ, iSSI);
                                GetPortalCoord(iX, iY, iZ, iSSI);
                                ShowGlyphs();
                                AppendLine(textBox1, GalacticCoord);
                                AppendLine(textBox2, PortalCode);
                            }
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 5 **");
                return;
            }
        }
        private void ListBox3_MouseClick(object sender, EventArgs e)
        {
            try
            {
                if (listBox3.SelectedItem.ToString() != "")
                {
                    textBox11.Clear();
                    textBox14.Clear();
                    textBox15.Clear();
                    var nms = Nms.FromJson(json);
                    int i = listBox3.SelectedIndex;
                    string selected = nms.The6F.F0[i].OZw;                    
                    AppendLine(textBox11, selected);
                    PortalToVoxel(selected);
                    AppendLine(textBox13, voxel);
                    AppendLine(textBox15, GalacticCoord2);
                }
            }
            catch
            {
                AppendLine(textBox17, "** Code 4 **");
                return;
            }
        }
        private void GetGalacticCoord(int X, int Y, int Z, int SSI)
        {
            //Voxel Coordinates to Galactic Coordinate
            textBox1.Clear();
            textBox3.Clear();

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            AppendLine(textBox3, "DEC: " + iX + " " + iY + " " + iZ);

            int dd1 = X + 2047;
            int dd2 = Y + 127;
            int dd3 = Z + 2047;
            AppendLine(textBox3, "SHIFT: " + dd1 + " " + dd2 + " " + dd3);

            string g1 = dd1.ToString("X");
            string g2 = dd2.ToString("X");
            string g3 = dd3.ToString("X");
            string g4 = SSI.ToString("X");
            AppendLine(textBox3, "Galactic HEX numbers: " + g1 + " " + g2 + " " + g3 + " " + g4);

            int ig1 = Convert.ToInt32(g1, 16); // X[HEX] to X[DEC]
            int ig2 = Convert.ToInt32(g2, 16); // Y[HEX] to Y[DEC]
            int ig3 = Convert.ToInt32(g3, 16); // Z[HEX] to Z[DEC]
            int ig4 = Convert.ToInt32(g4, 16); // SSI[HEX] to SSI[DEC]
            AppendLine(textBox3, "Galactic DEC numbers: " + ig1 + " " + ig2 + " " + ig3 + " " + ig4);

            GalacticCoord = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ig1, ig2, ig3, ig4); //Format to 4 digit seperated by colon
            AppendLine(textBox3, "Galactic Coordinates: " + GalacticCoord);
        }
        private void GetPortalCoord(int X, int Y, int Z, int SSI)
        {
            //Galactic Coordinate to Portal Code

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            int dd1 = X + 2047;
            int dd2 = Y + 127;
            int dd3 = Z + 2047;

            string g1 = dd1.ToString("X");
            string g2 = dd2.ToString("X");
            string g3 = dd3.ToString("X");
            string g4 = SSI.ToString("X");

            int dec1 = Convert.ToInt32(g1, 16); // X[HEX] to X[DEC]
            int dec2 = Convert.ToInt32(g2, 16); // Y[HEX] to X[DEC]
            int dec3 = Convert.ToInt32(g3, 16); // Z[HEX] to X[DEC]
            int dec4 = Convert.ToInt32(g4, 16); // SSI[HEX] to SSI[DEC]
            AppendLine(textBox3, "Galactic HEX to DEC: " + dec1.ToString() + " " + dec2.ToString() + " " + dec3.ToString() + " " + dec4);

            int dec5 = Convert.ToInt32("801", 16); // 801[HEX] to 801[DEC]
            int dec6 = Convert.ToInt32("81", 16); // 81[HEX] to 81[DEC]
            int dec7 = Convert.ToInt32("1000", 16); // 100[HEX] to 1000[DEC]
            int dec8 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            AppendLine(textBox3, "Shift HEX to DEC: " + "801:" + dec5.ToString() + " 81:" + dec6.ToString() + " 1000:" + dec7.ToString() + " 100:" + dec8.ToString());

            int calc1 = (dec1 + dec5) % dec7; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (dec2 + dec6) % dec8; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (dec3 + dec5) % dec7; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            AppendLine(textBox3, "Calculate Portal DEC: " + "X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + dec4);

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            AppendLine(textBox3, "Portal HEX numbers: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + g4);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFF); // Z[HEX] to Z[DEC] 3 digits
            int ihexSSI = (Convert.ToInt32(g4, 16) & 0xFFF); // SSI[HEX] to SSI[DEC] 3 digits

            PortalCode = string.Format("0{0:X3}{1:X2}{2:X3}{3:X3}", ihexSSI, ihexY, ihexZ, ihexX); // Format digits 0 3 2 3 3
            AppendLine(textBox3, "[SSI][Y][Z][X] Portal Code: " + PortalCode);

            //Index chars in PortalCode
            _gl1 = PortalCode[0];
            _gl2 = PortalCode[1];
            _gl3 = PortalCode[2];
            _gl4 = PortalCode[3];
            _gl5 = PortalCode[4];
            _gl6 = PortalCode[5];
            _gl7 = PortalCode[6];
            _gl8 = PortalCode[7];
            _gl9 = PortalCode[8];
            _gl10 = PortalCode[9];
            _gl11 = PortalCode[10];
            _gl12 = PortalCode[11];

            //Display Glyph images
            //ShowGlyphs();
        }
        private void ShowPGlyphs()
        {
            //Display player glyph images
            pictureBox13.Image = glyphDict[_gl1];
            pictureBox13.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox14.Image = glyphDict[_gl2];
            pictureBox14.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.Image = glyphDict[_gl3];
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox16.Image = glyphDict[_gl4];
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox17.Image = glyphDict[_gl5];
            pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox18.Image = glyphDict[_gl6];
            pictureBox18.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox19.Image = glyphDict[_gl7];
            pictureBox19.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox20.Image = glyphDict[_gl8];
            pictureBox20.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox21.Image = glyphDict[_gl9];
            pictureBox21.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox22.Image = glyphDict[_gl10];
            pictureBox22.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox23.Image = glyphDict[_gl11];
            pictureBox23.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox24.Image = glyphDict[_gl12];
            pictureBox24.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void ShowGlyphs()
        {
            //Display selected location glyph images
            pictureBox1.Image = glyphDict[_gl1];
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = glyphDict[_gl2];
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.Image = glyphDict[_gl3];
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = glyphDict[_gl4];
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.Image = glyphDict[_gl5];
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Image = glyphDict[_gl6];
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Image = glyphDict[_gl7];
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.Image = glyphDict[_gl8];
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox9.Image = glyphDict[_gl9];
            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox10.Image = glyphDict[_gl10];
            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox11.Image = glyphDict[_gl11];
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox12.Image = glyphDict[_gl12];
            pictureBox12.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void CalculateVoxel()
        {
            //PlanetNumber--SolarSystemIndex--GalaxyNumber--VoxelY--VoxelZ--VoxelX
            //4 bit--12 bit--8 bit--8 bit--12 bit--12 bit
            //"0x2 049 00 01 D37 652" 460 475 89 64 091 954
            textBox14.Clear();

            string basehx = textBox11.Text;
            string b6 = basehx.Substring(basehx.Length - 3, 3);
            string b5 = basehx.Substring(basehx.Length - 6, 3);
            string b4 = basehx.Substring(basehx.Length - 8, 2);
            string b3 = basehx.Substring(basehx.Length - 10, 2);
            string b2 = basehx.Substring(basehx.Length - 13, 3);
            string b1 = basehx.Substring(basehx.Length - 16, 3);
            AppendLine(textBox14, "Base Hex Split: " + b1 + " " + b2 + " " + b3 + " " + b4 + " " + b5 + " " + b6);
            AppendLine(textBox14, "Base Hex id's: Planet #:" + b1 + " SSI:" + b2 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6);

            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            AppendLine(textBox14, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4);

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)
            int pidec = Convert.ToInt32(b1, 16); //HEX to DEC
            int ssidec = Convert.ToInt32(b2, 16);
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            AppendLine(textBox14, "Base Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX);

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            AppendLine(textBox14, "Base Voxel Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString());

            int shiftX = calc1 - 2047;
            int shiftY = calc2 - 127;
            int shiftZ = calc3 - 2047;
            //AppendLine(textBox3, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            AppendLine(textBox14, "Base Voxel: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX);
        }
        private void PortalToVoxel(string basehx)
        {
            //PlanetNumber--SolarSystemIndex--GalaxyNumber--VoxelY--VoxelZ--VoxelX
            //4 bit--12 bit--8 bit--8 bit--12 bit--12 bit
            //"0x2 049 00 01 D37 652" 460 475 89 64 091 954

            string b6 = basehx.Substring(basehx.Length - 3, 3);
            string b5 = basehx.Substring(basehx.Length - 6, 3);
            string b4 = basehx.Substring(basehx.Length - 8, 2);
            string b3 = basehx.Substring(basehx.Length - 10, 2);
            string b2 = basehx.Substring(basehx.Length - 13, 3);
            string b1 = basehx.Substring(basehx.Length - 16, 3);
            
            
            AppendLine(textBox14, "Base Hex Split: " + b1 + " " + b2 + " " + b3 + " " + b4 + " " + b5 + " " + b6);
            AppendLine(textBox14, "Base Hex id's: Planet #:" + b1 + " SSI:" + b2 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6);

            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            AppendLine(textBox14, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4);

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)
            int pidec = Convert.ToInt32(b1, 16); //HEX to DEC
            int ssidec = Convert.ToInt32(b2, 16);
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            AppendLine(textBox14, "Base Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX);

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            AppendLine(textBox14, "Base Voxel Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString());

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFFFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFFF); // Z[HEX] to Z[DEC] 3 digits
            //int ihexSSI = (Convert.ToInt32(ssidec, 16) & 0xFFFF); // SSI[HEX] to SSI[DEC] 3 digits

            //AppendLine(textBox14, "P: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + ssidec);
            GalacticCoord2 = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ihexX, ihexY, ihexZ, ssidec & 0xFFFF); //Format to 4 digit seperated by colon
            AppendLine(textBox14, "Galactic Coordinates: " + GalacticCoord2);

            int shiftX = calc1 - 2047;
            int shiftY = calc2 - 127;
            int shiftZ = calc3 - 2047;
            //AppendLine(textBox3, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            AppendLine(textBox14, "Base Voxel: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX);
            voxel = "Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        int Find(ListBox lb, string searchString, int startIndex)
        {
            for (int i = startIndex; i < lb.Items.Count; ++i)
            {
                string lbString = lb.Items[i].ToString();
                if (lb.Items[i].ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    return i;
            }
            return -1;
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            ClearAll();
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
            FileInfo[] Files = dinfo.GetFiles(selected, SearchOption.AllDirectories);

            if (Files.Length != 0)
            {
                foreach (FileInfo file in Files)
                {
                    hgFilePath = file.FullName;
                    AppendLine(textBox16, file.FullName);
                }
            }
            else
            {
                AppendLine(textBox17, "** Code 3 **");
                return;
            }
            FileInfo hgfile = new FileInfo(hgFilePath);
            AppendLine(textBox26, hgfile.LastWriteTime.ToLongDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());
            json = File.ReadAllText(hgFilePath);           

            Loadlsb1();
            Loadlsb3();
            GetPlayerCoord();          

        }
        private void SetSavePath()
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string[] files = Directory.GetFiles(fbd.SelectedPath, "save*.hg");

                        if (files.Length != 0)
                        {
                            nmsPath = fbd.SelectedPath;
                            //AppendLine(textBox16, fbd.SelectedPath + "save.hg");
                            MessageBox.Show(files.Length.ToString() + "\r\n\r\nSave files found... ", "Message");
                        }
                        else
                        {
                            MessageBox.Show("No Save files found! ", "Message");
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Cancelled no path set!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n Save path problem!");
                return;
            }
        }
        private void AppDataDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            ClearAll();
            LoadCmbx();
        }
        private void ManuallySelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSavePath();
            ClearAll();
            LoadCmbx();
        }
        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by: Kevin0M16 \r\n\r\n 8-25-19");
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Clearforsearch();
            listBox1.SelectedIndex = Find(listBox1, textBox24.Text, 0);
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Clearforsearch();
            listBox2.SelectedIndex =  Find(listBox2, textBox25.Text, 0);
        }
        private void TextBox24_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button1_Click(this, new EventArgs());
            }
        }
        private void TextBox25_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button2_Click(this, new EventArgs());
            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            if (textBox12.Text == "False" || textBox12.Text == "false")
            {
                MessageBox.Show("No Portal Interference Found!", "Alert", MessageBoxButtons.OK);
                return;
            }
            else
            {                
                string decrypt = @"/c .\nmssavetool\nmssavetool.exe decrypt -g2 -f .\nmssavetool\save1.json";
                ProcessStartInfo startInfo = new ProcessStartInfo(@"Powershell.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //Process.Start(startInfo);
                startInfo.Arguments = decrypt;
                Process.Start(startInfo);
                Thread.Sleep(2000);

                //Process.Start(@"Powershell.exe", decrypt);
                string jsons = File.ReadAllText(@".\nmssavetool\save1.json");

                jsons = jsons.Replace("\"DaC\": true", "\"DaC\": false");

                File.WriteAllText(@".\nmssavetool\save1.json", jsons);
                Thread.Sleep(2000);

                string encrypt = @"/c .\nmssavetool\nmssavetool.exe encrypt -g2 -f .\nmssavetool\save1.json";
                //Process.Start("Powershell.exe -WindowStyle Hidden", encrypt);
                //Process.Start(startInfo);
                startInfo.Arguments = encrypt;
                Process.Start(startInfo);
                Thread.Sleep(2000);

                json = File.ReadAllText(hgFilePath);
                var nms = Nms.FromJson(json);
                textBox12.Clear();
                textBox12.Text = nms.The6F.DaC.ToString();
                if (textBox12.Text == "False" || textBox12.Text == "false")
                {   
                    MessageBox.Show("Portal Interference removal successful!", "Confirmation", MessageBoxButtons.OK);
                }                
            }
        }
        private void DiscoveriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackupLoc("backup.txt");
            MessageBox.Show("Backup successful!", "Confirmation", MessageBoxButtons.OK);
        }
        private void FastTr(int X, int Y, int Z, int galaxy)
        {
            //var nms = Nms.FromJson(json);
            //nms.The6F.YhJ.OZw;
        }
        private void JsonKey()
        {
            rxPatternG = "\"Iis\".*?$";
            rxPatternX = "\"dZj\".*?$";
            rxPatternY = "\"IyE\".*?$";
            rxPatternZ = "\"uXE\".*?$";
            rxPatternSSI = "\"vby\".*?$";
            rxPatternPI = "\"jsv\".*?$";
        }
        private void JsonSet(string value)
        {
            switch(value)
            {
                case "g":                    
                    rxValG = "\"Iis\": " + textBox4.Text + ",";
                    break;
                case "x":                    
                    rxValX = "\"dZj\": " + textBox5.Text + ",";
                    break;
                case "y":                    
                    rxValY = "\"IyE\": " + textBox6.Text + ",";
                    break;
                case "z":
                    rxValZ = "\"uXE\": " + textBox7.Text + ",";
                    break;
                case "ssi":                    
                    rxValSSI = "\"vby\": " + textBox8.Text + ",";
                    break;
                case "pi":                    
                    rxValPI = "\"jsv\": " + textBox9.Text + ",";
                    break;
                case "all": ///
                    rxValG = "\"Iis\": " + galaxy + ",";
                    rxValX = "\"dZj\": " + X + ",";
                    rxValY = "\"IyE\": " + Y + ",";
                    rxValZ = "\"uXE\": " + Z + ",";
                    rxValSSI = "\"vby\": " + SSI + ",";
                    rxValPI = "\"jsv\": 0";
                    break;
            }            
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            string jsons = File.ReadAllText(@".\nmssavetool\save1.json");
            //Match m = myRegex.Match(json);   // m is the first match
            JsonSet("all");
            Regex myRegex1 = new Regex(rxPatternG, RegexOptions.Multiline);
            jsons = Regex.Replace(jsons, rxPatternG, rxValG);

            Regex myRegex2 = new Regex(rxPatternX, RegexOptions.Multiline);
            jsons = Regex.Replace(jsons, rxPatternX, rxValX, RegexOptions.Multiline);

            Regex myRegex3 = new Regex(rxPatternY, RegexOptions.Multiline);
            jsons = Regex.Replace(jsons, rxPatternY, rxValY, RegexOptions.Multiline);

            Regex myRegex4 = new Regex(rxPatternZ, RegexOptions.Multiline);
            jsons = Regex.Replace(jsons, rxPatternZ, rxValZ, RegexOptions.Multiline);

            Regex myRegex5 = new Regex(rxPatternSSI, RegexOptions.Multiline);
            jsons = Regex.Replace(jsons, rxPatternSSI, rxValSSI, RegexOptions.Multiline);

            Regex myRegex6 = new Regex(rxPatternPI, RegexOptions.Multiline);
            jsons = Regex.Replace(jsons, rxPatternPI, rxValPI, RegexOptions.Multiline);

            File.WriteAllText(@".\nmssavetool\save1.json", jsons);

            Match g = myRegex1.Match(jsons);
            Match x = myRegex2.Match(jsons);
            Match y = myRegex3.Match(jsons);
            Match z = myRegex4.Match(jsons);
            Match ssi = myRegex5.Match(jsons);
            Match pi = myRegex6.Match(jsons);

            AppendLine(textBox3, g.ToString() + x.ToString() + y.ToString() + z.ToString() + ssi.ToString() + pi.ToString());

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox26.Clear();
            comboBox2.Items.Add("Slot 1");
            comboBox2.Items.Add("Slot 2");
            comboBox2.Items.Add("Slot 3");
            comboBox2.Items.Add("Slot 4");
            comboBox2.Items.Add("Slot 5");

            if(comboBox2.SelectedText == "Slot 1")
            {
               // comboBox1.SelectedText.la
            }
        }
    }

}
