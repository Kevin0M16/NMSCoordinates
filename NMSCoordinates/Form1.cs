
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
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuickType;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections;

namespace NMSCoordinates
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public static string GetNewestZip(string path)
        {
            DirectoryInfo dinfo = new DirectoryInfo(path);
            FileInfo[] Files = dinfo.GetFiles("savebackup*.zip");

            if (dinfo == null || !dinfo.Exists)
                return null;

            DateTime recentWrite = DateTime.MinValue;
            FileInfo recentFile = null;

            foreach (FileInfo file in Files)
            {
                if (file.LastWriteTime > recentWrite)
                {
                    recentWrite = file.LastWriteTime;
                    recentFile = file;
                }
            }
            return recentFile.Name;
        }

        public void LoadCmbx()
        {
            //Load save file names in combobox1
            //comboBox1.Items.Clear();

            if (!Directory.Exists(nmsPath))
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);
            }
            DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
            FileInfo[] Files = dinfo.GetFiles("save*.hg", SearchOption.AllDirectories);

            if (Files.Length != 0)
            {
                Dictionary<int, string> sl1 = new Dictionary<int, string>();
                sn1 = new Dictionary<int, string>();
                sn2 = new Dictionary<int, string>();
                sn3 = new Dictionary<int, string>();
                sn4 = new Dictionary<int, string>();
                sn5 = new Dictionary<int, string>();

                foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                {
                    if (file.Name == "save.hg" | file.Name == "save2.hg")
                    {
                        if (!sn1.ContainsKey(1))
                            sn1.Add(1, file.Name);
                        else sn1.Add(2, file.Name);

                        if (!sl1.ContainsValue("Slot 1"))
                            sl1.Add(1, "Slot 1");

                    }
                    if (file.Name == "save3.hg" | file.Name == "save4.hg")
                    {
                        if (!sn2.ContainsKey(3))
                            sn2.Add(3, file.Name);
                        else sn2.Add(4, file.Name);

                        if (!sl1.ContainsValue("Slot 2"))
                            sl1.Add(2, "Slot 2");
                    }
                    if (file.Name == "save5.hg" | file.Name == "save6.hg")
                    {
                        if (!sn3.ContainsKey(5))
                            sn3.Add(5, file.Name);
                        else sn3.Add(6, file.Name);

                        if (!sl1.ContainsValue("Slot 3"))
                            sl1.Add(3, "Slot 3");
                    }
                    if (file.Name == "save7.hg" | file.Name == "save8.hg")
                    {
                        if (!sn4.ContainsKey(7))
                            sn4.Add(7, file.Name);
                        else sn4.Add(8, file.Name);

                        if (!sl1.ContainsValue("Slot 4"))
                            sl1.Add(4, "Slot 4");
                    }
                    if (file.Name == "save9.hg" | file.Name == "save10.hg")
                    {
                        if (!sn5.ContainsKey(9))
                            sn5.Add(9, file.Name);
                        else sn5.Add(10, file.Name);

                        if (!sl1.ContainsValue("Slot 5"))
                            sl1.Add(5, "Slot 5");
                    }
                }

                sl1.Add(0, "(Select Save Slot)");
                comboBox2.DataSource = sl1.ToArray();
                comboBox2.DisplayMember = "VALUE";
                comboBox2.ValueMember = "KEY";

                //comboBox2.SelectedIndex = 0;                

                hgFileDir = Path.GetDirectoryName(Files[0].FullName);

                //AppendLine(textBox16, Path.GetDirectoryName(Files[0].FullName));
                //AppendLine(textBox26, Files[0].LastWriteTime.ToLongDateString() + " " + Files[0].LastWriteTime.ToLongTimeString());

            }
            else
            {
                AppendLine(textBox16, "No save files found!");
                return;
            }
        }
        private void CheckSS()
        {
            if (DiscList.Count > 0)
            {
                for (int i = 0; i < DiscList.Count; i++)
                {
                    JsonMap(i);
                    GetPortalCoord(iX, iY, iZ, iSSI);
                    GetGalacticCoord(iX, iY, iZ, iSSI);
                    SSlist.Add("Loc: " + DiscList[i] + " - G: " + galaxy + " - PC: " + PortalCode + " -- GC: " + GalacticCoord);
                }
            }
            else
            {
                return;
            }
        }
        private void BackupLoc(string path)
        {
            if (DiscList.Count > 0)
            {
                for (int i = 0; i < DiscList.Count; i++)
                {
                    JsonMap(i);
                    GetPortalCoord(iX, iY, iZ, iSSI);
                    GetGalacticCoord(iX, iY, iZ, iSSI);
                    Backuplist.Add("Loc: " + DiscList[i] + " - G: " + galaxy + " - PC: " + PortalCode + " -- GC: " + GalacticCoord);
                }

                string path2 = MakeUnique(path).ToString();
                File.WriteAllLines(path2, Backuplist);
                Process.Start(path2);
                LoadTxt();
            }
            else
            {
                toolStripMenuItem1.Enabled = false;
                MessageBox.Show("No Locations found! ", "Message");
            }
        }
        public FileInfo MakeUnique(string path)
        {
            path = String.Format("{0}{1}{2}{3}{4}", @".\backup\", Path.GetFileNameWithoutExtension(path), "_" + saveslot + "_", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"), Path.GetExtension(path));
            return new FileInfo(path);

        }
        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        private void GetPlayerCoord()
        {
            //Gets the player position off the save file and prints the info on tab1
            textBox22.Clear();
            textBox21.Clear();
            textBox23.Clear();

            var nms = Nms.FromJson(json);

            pgalaxy = nms.The6F.YhJ.Iis.ToString();//.ToString();
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
            //AppendLine(textBox23, galaxyDict[pgalaxy]);
            GalaxyLookup(textBox23, pgalaxy);
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
            //textBox11.Clear();
            textBox12.Clear();
            //textBox13.Clear();
            //textBox14.Clear();
            //textBox15.Clear();
            textBox22.Clear();
            textBox21.Clear();
            textBox23.Clear();
            textBox26.Clear();

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
            Backuplist.Clear();
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            //listBox3.DataSource = null;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            //listBox3.Items.Clear();

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
            textBox1.Clear();
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
            galaxyDict.Add(new KeyValuePair<string, string>("3", "Hesperius"));
            galaxyDict.Add(new KeyValuePair<string, string>("4", "Hyades"));
            galaxyDict.Add(new KeyValuePair<string, string>("5", "Ickjamatew"));
            galaxyDict.Add(new KeyValuePair<string, string>("6", "Bullangr"));
            galaxyDict.Add(new KeyValuePair<string, string>("7", "Kikolgallr"));
            galaxyDict.Add(new KeyValuePair<string, string>("8", "Eltiensleem"));
            galaxyDict.Add(new KeyValuePair<string, string>("9", "Eissentam"));
            galaxyDict.Add(new KeyValuePair<string, string>("10", "Elkupalos"));
            galaxyDict.Add(new KeyValuePair<string, string>("11", "Aptarkaba"));
            galaxyDict.Add(new KeyValuePair<string, string>("12", "Ontiniangp"));
            galaxyDict.Add(new KeyValuePair<string, string>("13", "Odiwagiri"));
            galaxyDict.Add(new KeyValuePair<string, string>("14", "Ogtialabi"));
            galaxyDict.Add(new KeyValuePair<string, string>("15", "Muhacksonto"));
            galaxyDict.Add(new KeyValuePair<string, string>("16", "Hitonskyer"));
            galaxyDict.Add(new KeyValuePair<string, string>("17", "Rerasmutul"));
            galaxyDict.Add(new KeyValuePair<string, string>("18", "Isdoraijung"));
            galaxyDict.Add(new KeyValuePair<string, string>("19", "Doctinawyra"));
            galaxyDict.Add(new KeyValuePair<string, string>("20", "Loychazinq"));
            galaxyDict.Add(new KeyValuePair<string, string>("21", "Zukasizawa"));
            galaxyDict.Add(new KeyValuePair<string, string>("22", "Ekwathore"));
            galaxyDict.Add(new KeyValuePair<string, string>("23", "Yeberhahne"));
            galaxyDict.Add(new KeyValuePair<string, string>("24", "Twerbetek"));
            galaxyDict.Add(new KeyValuePair<string, string>("25", "Sivarates"));
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
                for (int i = 0; i < nms.The6F.NlG.Length; i++)
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
                AppendLine(textBox17, "** Code 111 **");
                return;
            }
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
        }
        private void Loadlsb3()
        {
            var nms = Nms.FromJson(json);
            try
            {
                for (int i = 0; i < nms.The6F.F0.Length; i++)
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
                AppendLine(textBox17, "** Code 11 **");
                return;
            }
            //listBox3.DataSource = BaseList;
            //textBox18.Text = listBox3.Items.Count.ToString();


        }
        private void GalaxyLookup(TextBox source, string galaxy)
        {
            try
            {
                source.Text = galaxyDict[galaxy];
            }
            catch
            {
                source.Text = galaxy;
                AppendLine(textBox17, "Galaxy Not Found, update needed.");
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
                    //textBox10.Text = galaxyDict[galaxy];
                    GalaxyLookup(textBox10, galaxy);
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
                        for (int i = 0; i < nms.The6F.NlG.Length; i++)
                        {
                            if (nms.The6F.NlG[i].NKm.ToString() == si)
                            {
                                JsonMap(i);
                                TextBoxes();
                                GalaxyLookup(textBox10, galaxy);
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
                        AppendLine(textBox17, "** Code 51 **");
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

        private void GetGalacticCoord(int X, int Y, int Z, int SSI)
        {
            //Voxel Coordinates to Galactic Coordinate
            //textBox1.Clear();
            textBox3.Clear();

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            AppendLine(textBox3, "DEC: " + X + " " + Y + " " + Z);

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


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        int Find(ListBox lb, string searchString, int startIndex)
        {
            for (int i = startIndex; i < lb.Items.Count; ++i)
            {
                //string lbString = lb.Items[i].ToString();
                if (lb.Items[i].ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    return i;
            }
            return -1;
        }
       
        private void Button4_Click(object sender, EventArgs e)
        {
            ClearAll();

            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if(selected != "")
            {
                GetSaveFile(selected);
                Loadlsb1();
                Loadlsb3();
                GetPlayerCoord();
            }
            else
            {
                MessageBox.Show("No Save Slot Selected!", "Alert");
            }                
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
            comboBox1.DataSource = null;
            comboBox2.DataSource = null;
            comboBox1.SelectedIndex = -1;
            ClearAll();
            LoadCmbx();
            
        }
        private void ManuallySelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSavePath();
            comboBox1.DataSource = null;
            comboBox2.DataSource = null;
            comboBox1.SelectedIndex = -1;
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
            listBox2.SelectedIndex = Find(listBox2, textBox25.Text, 0);
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
        public void BackUpSaveSlotNoMsg(int slot)
        {
            if (saveslot >= 1 && saveslot <= 5)
            {
                string hgFileName = Path.GetFileNameWithoutExtension(hgFilePath);

                string mf_hgFilePath = hgFilePath;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                string mf_hgFileName = Path.GetFileNameWithoutExtension(mf_hgFilePath);

                Directory.CreateDirectory(@".\temp");
                File.Copy(hgFilePath, @".\temp\" + hgFileName + Path.GetExtension(hgFilePath));
                File.Copy(mf_hgFilePath, @".\temp\" + mf_hgFileName + Path.GetExtension(mf_hgFilePath));
                ZipFile.CreateFromDirectory(@".\temp", @".\backup\savebackup_" + slot + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".zip");
                Directory.Delete(@".\temp", true);
                if (File.Exists(@".\backup\" + GetNewestZip(@".\backup")))
                {
                    return;// MessageBox.Show("Save slot backup up to: " + GetNewestZip(@".\backup"), "Save Backup", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("No File backed up!", "Alert");
                }
            }
            else
            {
                MessageBox.Show("No File Found / Select Save Slot!", "Alert");
            }
        }
        public void BackUpSaveSlot(int slot)
        {
            if (saveslot >= 1 && saveslot <= 5)
            {
                string hgFileName = Path.GetFileNameWithoutExtension(hgFilePath);

                string mf_hgFilePath = hgFilePath;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                string mf_hgFileName = Path.GetFileNameWithoutExtension(mf_hgFilePath);

                Directory.CreateDirectory(@".\temp");
                File.Copy(hgFilePath, @".\temp\" + hgFileName + Path.GetExtension(hgFilePath));
                File.Copy(mf_hgFilePath, @".\temp\" + mf_hgFileName + Path.GetExtension(mf_hgFilePath));
                ZipFile.CreateFromDirectory(@".\temp", @".\backup\savebackup_" + slot + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".zip");
                Directory.Delete(@".\temp", true);
                if (File.Exists(@".\backup\" + GetNewestZip(@".\backup")))
                {
                    MessageBox.Show("Save slot backup up to: " + GetNewestZip(@".\backup"), "Save Backup", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("No File backed up!", "Alert");
                }                
            }
            else
            {
                MessageBox.Show("No File Found / Select Save Slot!", "Alert");
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            BackUpSaveSlot(saveslot);
            
        }
        private async Task ReadSave(int slot)
        {
            //Backup original save file
            BackUpSaveSlotNoMsg(slot);

            string mf_hgFilePath = hgFilePath;
            mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

            //Sets the save to be the last modified for nmssavetool
            File.SetLastWriteTime(mf_hgFilePath, DateTime.Now);
            File.SetLastWriteTime(hgFilePath, DateTime.Now);

            switch (slot)
            {
                case 1:
                    decrypt = @"/c .\nmssavetool\nmssavetool.exe decrypt -g1 -f .\nmssavetool\save.json";
                    break;
                case 2:
                    decrypt = @"/c .\nmssavetool\nmssavetool.exe decrypt -g2 -f .\nmssavetool\save.json";
                    break;
                case 3:
                    decrypt = @"/c .\nmssavetool\nmssavetool.exe decrypt -g3 -f .\nmssavetool\save.json";
                    break;
                case 4:
                    decrypt = @"/c .\nmssavetool\nmssavetool.exe decrypt -g4 -f .\nmssavetool\save.json";
                    break;
                case 5:
                    decrypt = @"/c .\nmssavetool\nmssavetool.exe decrypt -g5 -f .\nmssavetool\save.json";
                    break;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(@"Powershell.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = decrypt;
            Process.Start(startInfo);

            await Task.Delay(2000);

        }
        private async Task WriteSave(int slot)
        {
            switch (slot)
            {
                case 1:
                    encrypt = @"/c .\nmssavetool\nmssavetool.exe encrypt -g1 -f .\nmssavetool\saveedit.json";
                    break;
                case 2:
                    encrypt = @"/c .\nmssavetool\nmssavetool.exe encrypt -g2 -f .\nmssavetool\saveedit.json";
                    break;
                case 3:
                    encrypt = @"/c .\nmssavetool\nmssavetool.exe encrypt -g3 -f .\nmssavetool\saveedit.json";
                    break;
                case 4:
                    encrypt = @"/c .\nmssavetool\nmssavetool.exe encrypt -g4 -f .\nmssavetool\saveedit.json";
                    break;
                case 5:
                    encrypt = @"/c .\nmssavetool\nmssavetool.exe encrypt -g5 -f .\nmssavetool\saveedit.json";
                    break;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(@"Powershell.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = encrypt;
            Process.Start(startInfo);

            await Task.Delay(2000);

            AppendLine(textBox27, "Save file on Slot: ( " + saveslot + " ) backed up to \\backup folder...");
        }        
        private async void Button3_Click(object sender, EventArgs e)
        {
            if (saveslot >= 1 && saveslot <= 5)
            {
                if (textBox12.Text == "False" || textBox12.Text == "false")
                {
                    MessageBox.Show("No Portal Interference Found!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Clear Portal Interference ? ", "Portal Interference", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //Set the JSON search patterns
                        JsonSet("all");

                        progressBar1.Visible = true;
                        progressBar1.Invoke((Action)(() => progressBar1.Value = 5));

                        //Read and decrypt save file on slot to save.json
                        await ReadSave(saveslot);
                        
                        progressBar1.Invoke((Action)(() => progressBar1.Value = 30));

                        //Read save.json to a string
                        string jsons = File.ReadAllText(@".\nmssavetool\save.json");

                        progressBar1.Invoke((Action)(() => progressBar1.Value = 45));

                        //Replace OnTheOtherSideOfPortal to false or DaC
                        //jsons = jsons.Replace("\"DaC\": true", "\"DaC\": false");

                        //Set Portal Interference false DaC
                        //Get the portal interf. state object
                        Regex myRegexPrtl = new Regex(rxPatternPrtl, RegexOptions.Multiline);
                        Match prtl = myRegexPrtl.Match(jsons);

                        //Set Portal Interference state rxValPrtl preset to false
                        jsons = Regex.Replace(jsons, rxPatternPrtl, rxValPrtl, RegexOptions.Multiline);

                        //Beyond - Find "VisitedPortal" or 3fO to false to cancel portal
                        Regex myRegexPrtl2 = new Regex(rxPatternPrtl2, RegexOptions.Singleline);
                        Match prtl2 = myRegexPrtl2.Match(jsons);
                        rxValPrtl2 = prtl2.ToString();
                        //AppendLine(textBox27, rxValPrtl2);
                        Regex myRegexPrtl3 = new Regex(rxPatternPrtl3, RegexOptions.Multiline);
                        rxValPrtl2 = Regex.Replace(rxValPrtl2, rxPatternPrtl3, rxValPrtl3, RegexOptions.Multiline);
                        //AppendLine(textBox27, rxValPrtl2);

                        //Set the visited portal state array after changes made
                        jsons = Regex.Replace(jsons, rxPatternPrtl2, rxValPrtl2, RegexOptions.Singleline);

                        //Write the modified JSON string to saveedit.json
                        File.WriteAllText(@".\nmssavetool\saveedit.json", jsons); 

                        progressBar1.Invoke((Action)(() => progressBar1.Value = 60));

                        //Encrypt and write saveedit.json to selected save slot
                        await WriteSave(saveslot);

                        progressBar1.Invoke((Action)(() => progressBar1.Value = 90));

                        //Read and check save file
                        json = File.ReadAllText(hgFilePath);

                        //Check save file edits
                        var nms = Nms.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.The6F.DaC.ToString();

                        Regex myRegexPrtl4 = new Regex(rxPatternPrtl, RegexOptions.Multiline);
                        Match prtl4 = myRegexPrtl4.Match(jsons);
                        AppendLine(textBox27, prtl4.ToString());

                        if (textBox12.Text == "False" || textBox12.Text == "false")
                        {
                            progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
                            progressBar1.Visible = false;

                            MessageBox.Show("Portal Interference removal successful!", "Confirmation", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Portal Interference Problem!", "Error");
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Save slot not selected!", "Alert");
            }            
        }
        private void DiscoveriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackupLoc(@".\backup\locbackup.txt");
            //MessageBox.Show("Backup successful!", "Confirmation", MessageBoxButtons.OK);            
        }
        private void JsonKey()
        {
            rxPatternG = "\"Iis\".*?,";
            rxPatternX = "\"dZj\".*?$";
            rxPatternY = "\"IyE\".*?$";
            rxPatternZ = "\"uXE\".*?$";
            rxPatternSSI = "\"vby\".*?$";
            rxPatternPI = "\"jsv\".*?$";

            rxPatternP = "\"6f=\".*?}";
            rxPatternSt = "\"rnc\".*?}";
            rxPatternPs = "\"jk4\".*?,";
            rxPatternPrtl = "\"DaC\".*?,";
            rxPatternPrtl2 = "\"3fO\".*?,";
            rxPatternPrtl3 = "true.*?";
        }
        private void JsonSet(string value)
        {
            switch (value)
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
                case "all":
                    rxValG = "\"Iis\": " + galaxy + ",";
                    rxValX = "\"dZj\": " + X + ",";
                    rxValY = "\"IyE\": " + Y + ",";
                    rxValZ = "\"uXE\": " + Z + ",";
                    rxValSSI = "\"vby\": " + SSI + ",";
                    rxValPI = "\"jsv\": 0";
                    rxValPs = "\"jk4\": \"InShip\",";
                    rxValPrtl = "\"DaC\": false,";
                    rxValPrtl3 = "false";

                    break;
            }
        }
        private async void Button5_ClickAsync(object sender, EventArgs e)
        {
            if (listBox1.GetItemText(listBox1.SelectedItem) != "" || listBox2.GetItemText(listBox2.SelectedItem) != "")
            {
                DialogResult dialogResult = MessageBox.Show("Move Player to: " + listBox1.GetItemText(listBox1.SelectedItem) + listBox2.GetItemText(listBox2.SelectedItem) + " ? ", "Fast Travel", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (galaxy != "" && X != "" && Y != "" && Z != "" && SSI != "" && saveslot >= 1 && saveslot <= 5)
                    {
                        progressBar1.Visible = true;
                        progressBar1.Invoke((Action)(() => progressBar1.Value = 5));

                        //await BackupSave();

                        progressBar1.Invoke((Action)(() => progressBar1.Value = 15));

                        //Read and decrypt save (?)
                        await ReadSave(saveslot);

                        //Set all Regex values
                        JsonSet("all");

                        progressBar1.Invoke((Action)(() => progressBar1.Value = 25));

                        //Read all the JSON text from nmssavetool decrypt
                        string jsons = File.ReadAllText(@".\nmssavetool\save.json");

                        ////Set Player Location
                        //Get the Player location text array
                        Regex myRegex = new Regex(rxPatternP, RegexOptions.Singleline);
                        Match m = myRegex.Match(jsons);
                        rxValP = m.ToString();

                        //Get and Set Galaxy
                        Regex myRegex1 = new Regex(rxPatternG, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternG, rxValG, RegexOptions.Multiline);

                        //Get and Set X
                        Regex myRegex2 = new Regex(rxPatternX, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternX, rxValX, RegexOptions.Multiline);

                        //Get amd Set Y
                        Regex myRegex3 = new Regex(rxPatternY, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternY, rxValY, RegexOptions.Multiline);

                        //Get and Set Z
                        Regex myRegex4 = new Regex(rxPatternZ, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternZ, rxValZ, RegexOptions.Multiline);

                        //Get and Set Solar System index (SSI)
                        Regex myRegex5 = new Regex(rxPatternSSI, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternSSI, rxValSSI, RegexOptions.Multiline);

                        //Get and Set Planet Index
                        Regex myRegex6 = new Regex(rxPatternPI, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternPI, rxValPI, RegexOptions.Multiline);

                        //Set the player location array after changes made
                        jsons = Regex.Replace(jsons, rxPatternP, rxValP, RegexOptions.Singleline);

                        ////Set Spawn State
                        // Get the Spawn state array
                        Regex myRegexs = new Regex(rxPatternSt, RegexOptions.Singleline);
                        Match ms = myRegexs.Match(jsons);
                        rxValSt = ms.ToString();

                        //Get and set Player last known location in Spwn state array
                        Regex myRegexps = new Regex(rxPatternPs, RegexOptions.Multiline);
                        rxValSt = Regex.Replace(rxValSt, rxPatternPs, rxValPs, RegexOptions.Multiline);

                        //Set the spawn state array after changes made
                        jsons = Regex.Replace(jsons, rxPatternSt, rxValSt, RegexOptions.Singleline);

                        //Set Portal Interference false DaC
                        //Get the portal interf. state object
                        Regex myRegexPrtl = new Regex(rxPatternPrtl, RegexOptions.Multiline);
                        Match prtl = myRegexPrtl.Match(jsons);

                        //Set Portal Interference state rxValPrtl preset to false
                        jsons = Regex.Replace(jsons, rxPatternPrtl, rxValPrtl, RegexOptions.Multiline);

                        //Beyond - Find "VisitedPortal" or 3fO to false to cancel portal
                        Regex myRegexPrtl2 = new Regex(rxPatternPrtl2, RegexOptions.Singleline);
                        Match prtl2 = myRegexPrtl2.Match(jsons);
                        rxValPrtl2 = prtl2.ToString();
                        AppendLine(textBox12, rxValPrtl2);
                        Regex myRegexPrtl3 = new Regex(rxPatternPrtl3, RegexOptions.Multiline);
                        rxValPrtl2 = Regex.Replace(rxValPrtl2, rxPatternPrtl3, rxValPrtl3, RegexOptions.Multiline);
                        AppendLine(textBox12, rxValPrtl2);

                        //Set the visited portal state array after changes made
                        jsons = Regex.Replace(jsons, rxPatternPrtl2, rxValPrtl2, RegexOptions.Singleline);
                                               
                        progressBar1.Invoke((Action)(() => progressBar1.Value = 40));

                        //Write all modifications of file to saveedit.json
                        File.WriteAllText(@".\nmssavetool\saveedit.json", jsons);

                        //Show log of changes in txtbox
                        Match g = myRegex1.Match(jsons);
                        Match x = myRegex2.Match(jsons);
                        Match y = myRegex3.Match(jsons);
                        Match z = myRegex4.Match(jsons);
                        Match ssi = myRegex5.Match(jsons);
                        Match pi = myRegex6.Match(jsons);
                        Match ps = myRegexs.Match(myRegexps.Match(jsons).ToString());
                        AppendLine(textBox27, "Player Move Data: " + g.ToString() + x.ToString() + y.ToString() + z.ToString() + ssi.ToString() + pi.ToString() + ps.ToString());

                        progressBar1.Invoke((Action)(() => progressBar1.Value = 70));

                        //Write the new save file 
                        await WriteSave(saveslot);

                        //Set json to the new modified hg file
                        json = File.ReadAllText(hgFilePath);

                        //Read the new json and check portal interference state
                        var nms = Nms.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.The6F.DaC.ToString();
                        GetPlayerCoord();

                        progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
                        progressBar1.Visible = false;

                        //set the last write time box
                        textBox26.Clear();
                        FileInfo hgfile = new FileInfo(hgFilePath);
                        AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                        MessageBox.Show("Player moved successfully!", "Confirmation", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Please click a location!", "Confirmation", MessageBoxButtons.OK);
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please click a location!", "Confirmation", MessageBoxButtons.OK);
            }
        }
        private void ComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selected = this.comboBox2.GetItemText(this.comboBox2.SelectedItem);
            //Gets the dictionarys set in loadcmbbx and sets the data source for save dropdown
            if (selected == "Slot 1")
            {
                saveslot = 1;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn1.ToArray();
            }
            if (selected == "Slot 2")
            {
                saveslot = 2;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn2.ToArray();

            }
            if (selected == "Slot 3")
            {
                saveslot = 3;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn3.ToArray();
            }
            if (selected == "Slot 4")
            {
                saveslot = 4;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn4.ToArray();

            }
            if (selected == "Slot 5")
            {
                saveslot = 5;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn5.ToArray();
            }
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void GetSaveFile(string selected)
        {
            if (Directory.Exists(hgFileDir) && selected != "")
            {
                DirectoryInfo dinfo = new DirectoryInfo(hgFileDir);
                FileInfo[] Files = dinfo.GetFiles(selected, SearchOption.AllDirectories);
                
                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        hgFilePath = file.FullName;
                    }
                }
                else
                {
                    AppendLine(textBox17, "** Code 3 ** " + selected);
                    return;
                }

                textBox16.Clear();

                AppendLine(textBox16, hgFilePath);

                FileInfo hgfile = new FileInfo(hgFilePath);
                textBox26.Clear();
                AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());
                json = File.ReadAllText(hgFilePath);
                
            }
        }
        private void ComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            GetSaveFile(selected);

            Loadlsb1();
            Loadlsb3();
            GetPlayerCoord();
        }
        private async void Form1_Shown(object sender, EventArgs e)
        {
            Glyphs();
            GIndex();
            JsonKey();
            nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            LoadCmbx();
            SetSShot();
            LoadSS();
            LoadTxt();
            DiscList = new List<string>();
            BaseList = new List<string>();

            //Toggle for testing
            await BackupSave();
        }
        private async Task BackupSave()
        {
            if (Directory.Exists(nmsPath))
            {
                progressBar2.Visible = true;
                progressBar2.Invoke((Action)(() => progressBar2.Value = 10));
                ProcessStartInfo startInfo = new ProcessStartInfo(@"Powershell.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = @"/c .\nmssavetool\nmssavetool.exe backupall -b .\backup\nms-backup-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".zip";
                progressBar2.Invoke((Action)(() => progressBar2.Value = 30));
                Process.Start(startInfo);
                progressBar2.Invoke((Action)(() => progressBar2.Value = 50));
                await Task.Delay(2000);
                progressBar2.Invoke((Action)(() => progressBar2.Value = 100));
                progressBar2.Visible = false;
                AppendLine(textBox17, "All saves backed up to zip file created in \\backup folder...");
            }
            else
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);
            }
        }
        private async void BackupALLSaveFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await BackupSave();
            MessageBox.Show("Save Backup Completed!", "Confirmation", MessageBoxButtons.OK);
        }
        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);


            return bmp;
        }

        private void SetSShot()
        {
            try
            {
                stmPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Steam\userdata\";// 307405899\";
                //ScreenShot Path -- C:\Program Files (x86)\Steam\userdata\307405899\760\remote\275850\screenshots\thumbnails
                //AppendLine(textBox17, "1: " + stmPath);

                if (Directory.Exists(stmPath))
                {
                    List<string> list = new List<string>();
                    List<string> list2 = new List<string>();
                    DirectoryInfo dinfo1 = new DirectoryInfo(stmPath);
                    DirectoryInfo[] dinfoss = dinfo1.GetDirectories("760", SearchOption.AllDirectories);

                    foreach (DirectoryInfo di in dinfoss)//.OrderByDescending(f => f.LastWriteTime))
                    {
                        if (di.GetFiles("*.jpg", SearchOption.AllDirectories).Length != 0)
                        {
                            list2.Add(di.FullName);
                        }
                    }

                    ssdPath = Path.GetFullPath(list2[0].ToString() + @"\remote\275850\screenshots");//\thumbnails");
                    //AppendLine(textBox17, "2: " + ssdPath);

                    if (Directory.Exists(ssdPath))
                    {
                        foreach (var item in list2)
                        {
                            DirectoryInfo dinfo2 = new DirectoryInfo(ssdPath);
                            FileInfo[] Files = dinfo2.GetFiles("*.jpg", SearchOption.AllDirectories);

                            if (Files.Length != 0)
                            {
                                foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                                {
                                    if (!file.DirectoryName.Contains("thumbnails"))
                                        list.Add(file.FullName);
                                }
                            }
                            else
                            {
                                pictureBox25.Image = null;
                                AppendLine(textBox17, "ssPath error! 855");
                                return;
                            }
                        }
                        ssPath = list[0].ToString();

                        AppendLine(textBox17, "ScreenShot: " + list[0].ToString());                      
                    }
                    else
                    {
                        AppendLine(textBox17, "ssPath error! 123");
                        return;
                    }
                }
                else
                {
                    AppendLine(textBox17, "ssPath error! 145");
                    return;
                }
            }
            catch
            {
                AppendLine(textBox17, "ssPath error! 155");
                return;
            }
        }
        private void LoadSS()
        {
            pictureBox25.ImageLocation = ssPath;
            pictureBox25.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void SetSSPath()
        {
            try
            {
                List<string> list2 = new List<string>();
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string[] files = Directory.GetFiles(fbd.SelectedPath, "*.jpg");

                        if (files.Length != 0)
                        {
                            ssdPath = fbd.SelectedPath;
                            MessageBox.Show(files.Length.ToString() + "\r\n\r\nScreenshot files found... ", "Message");
                        }
                        else
                        {
                            MessageBox.Show("No Screenshot files found! ", "Message");
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Cancelled no path set!");
                    }
                }

                if (Directory.Exists(ssdPath))
                {
                    DirectoryInfo dinfo2 = new DirectoryInfo(ssdPath);
                    FileInfo[] Files = dinfo2.GetFiles("*.jpg", SearchOption.AllDirectories);

                    if (Files.Length != 0)
                    {
                        foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                        {
                            if (!file.DirectoryName.Contains("thumbnails"))
                                list2.Add(file.FullName);
                        }
                    }
                    else
                    {
                        //pictureBox25.Image = null;
                        AppendLine(textBox17, "ssPath error! 855");
                        return;
                    }
                    ssPath = list2[0].ToString();
                    AppendLine(textBox17, "ScreenShot: " + list2[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n Screenshot path problem!");
                return;
            }
        }
        private void ScreenshotPageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            AppendLine(textBox17, ssdPath);
            f3.MyProperty2 = ssdPath;
            f3.Show();
        }
        private void ScreenshotPageToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetSSPath();
            LoadSS();
        }
        private void LoadTxt()
        {
            listBox3.DataSource = null;

            if (Directory.Exists(@".\backup"))
            {
                List<string> list = new List<string>();
                DirectoryInfo dinfo2 = new DirectoryInfo(@".\backup");
                FileInfo[] Files = dinfo2.GetFiles("locbackup*.txt", SearchOption.AllDirectories);

                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                    {
                        list.Add(file.Name);
                    }
                }
                else
                {
                    toolStripMenuItem1.Enabled = false;
                    AppendLine(textBox11, "error! 855");
                    return;
                }
                listBox4.DataSource = list;
                toolStripMenuItem1.Enabled = true;
            }
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                string[] locFile = File.ReadAllLines(@".\backup\" + listBox4.SelectedItem.ToString());
                if (locFile[0].ToString() != "")
                {
                    listBox3.DataSource = locFile;
                    listBox3.SelectedIndex = 0;
                    toolStripMenuItem1.Enabled = true;
                }
                else
                {
                    toolStripMenuItem1.Enabled = false;
                    AppendLine(textBox11, "File is Empty! Select another file.");
                    AppendLine(textBox11, "---------------------");
                }
            }
            catch
            {
                toolStripMenuItem1.Enabled = false;
                AppendLine(textBox11, "No File Selected!");
            }

        }
        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Regex myRegex1 = new Regex("GC:.*?$", RegexOptions.Multiline);
            Match m1 = myRegex1.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
            string line1 = m1.ToString();
            //line1 = line1.Replace("GC: ", ""); 
            AppendLine(textBox11, line1);

            Regex myRegex2 = new Regex("PC.*?--", RegexOptions.Multiline);
            Match m2 = myRegex2.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
            string line2 = m2.ToString();
            //line2 = line2.Replace("PC: ", "");
            line2 = line2.Replace(" --", "");
            AppendLine(textBox11, line2);

            Regex myRegex3 = new Regex("^.*?\\)", RegexOptions.Multiline);
            Match m3 = myRegex3.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
            string line3 = m3.ToString();
            //line3 = line3.Replace("Loc: ", "");
            AppendLine(textBox11, line3);
            AppendLine(textBox11, "---------------------");

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
        }
        private void GalacticToVoxel(string oX, string oY, string oZ, string oSSI)
        {
            //Galactic Coordinate to Voxel Coordinates 
            textBox13.Clear();

            //HEX in
            AppendLine(textBox13, "Galactic Coordinates HEX: SSI:" + oSSI + " Y:" + oY + " Z:" + oZ + " X:" + oX);

            //HEX to DEC
            int icX = Convert.ToInt32(oX, 16);
            int icY = Convert.ToInt32(oY, 16);
            int icZ = Convert.ToInt32(oZ, 16);
            int icSSI = Convert.ToInt32(oSSI, 16);
            AppendLine(textBox13, "Galactic Coordinates DEC: SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX);

            //SHIFT DEC 2047 127 2047 ssi-same
            int vX = icX - 2047;
            int vY = icY - 127;
            int vZ = icZ - 2047;

            //voxel dec  x y z ssi-same
            AppendLine(textBox13, "*** Voxel Coordinates DEC: SSI:" + icSSI + " Y:" + vY + " Z:" + vZ + " X:" + vX + " ***");
            //voxel = "SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX;

            X = vX.ToString();
            Y = vY.ToString();
            Z = vZ.ToString();
            SSI = icSSI.ToString();
        }
        private async void Button8_Click(object sender, EventArgs e)
        {
            if (listBox3.GetItemText(listBox3.SelectedItem) != "")
            {
                DialogResult dialogResult = MessageBox.Show("Move Player? ", "Fast Travel", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Regex myRegexGC = new Regex("GC:.*?$", RegexOptions.Multiline);
                    Match m1 = myRegexGC.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                    string line1 = m1.ToString();
                    line1 = line1.Replace("GC: ", "");
                    line1 = line1.Replace(" ", "");
                    string[] value = line1.Split(':');
                    GalacticToVoxel(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());

                    Regex myRegexG = new Regex("G:.*?-", RegexOptions.Multiline);
                    Match m2 = myRegexG.Match(listBox3.GetItemText(listBox3.SelectedItem));   // m is the first match
                    string line2 = m2.ToString();
                    line2 = line2.Replace("G: ", "");
                    line2 = line2.Replace("-", "");
                    line2 = line2.Replace(" ", "");
                    galaxy = line2;
                    AppendLine(textBox13, "Galaxy: " + galaxy + " -- X:" + X + " -- Y:" + Y + " -- Z:" + Z + " -- SSI:" + SSI);

                    
                    if (galaxy != "" && X != "" && Y != "" && Z != "" && SSI != "" && saveslot >= 1 && saveslot <= 5)
                    {
                        progressBar3.Visible = true;
                        progressBar3.Invoke((Action)(() => progressBar3.Value = 10));

                        //Read and decrypt save (?)
                        await ReadSave(saveslot);

                        //Set all Regex values
                        JsonSet("all");

                        progressBar3.Invoke((Action)(() => progressBar3.Value = 25));

                        //Read all the JSON text from nmssavetool decrypt
                        string jsons = File.ReadAllText(@".\nmssavetool\save.json");

                        ////Set Player Location
                        //Get the Player location text array
                        Regex myRegex = new Regex(rxPatternP, RegexOptions.Singleline);
                        Match m = myRegex.Match(jsons);
                        rxValP = m.ToString();

                        //Get and Set Galaxy
                        Regex myRegex1 = new Regex(rxPatternG, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternG, rxValG, RegexOptions.Multiline);

                        //Get and Set X
                        Regex myRegex2 = new Regex(rxPatternX, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternX, rxValX, RegexOptions.Multiline);

                        //Get amd Set Y
                        Regex myRegex3 = new Regex(rxPatternY, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternY, rxValY, RegexOptions.Multiline);

                        //Get and Set Z
                        Regex myRegex4 = new Regex(rxPatternZ, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternZ, rxValZ, RegexOptions.Multiline);

                        //Get and Set Solar System index (SSI)
                        Regex myRegex5 = new Regex(rxPatternSSI, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternSSI, rxValSSI, RegexOptions.Multiline);

                        //Get and Set Planet Index
                        Regex myRegex6 = new Regex(rxPatternPI, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternPI, rxValPI, RegexOptions.Multiline);

                        //Set the player location array after changes made
                        jsons = Regex.Replace(jsons, rxPatternP, rxValP, RegexOptions.Singleline);

                        ////Set Spawn State
                        // Get the Spawn state array
                        Regex myRegexs = new Regex(rxPatternSt, RegexOptions.Singleline);
                        Match ms = myRegexs.Match(jsons);
                        rxValSt = ms.ToString();

                        //Get and set Player last known location in Spwn state array
                        Regex myRegexps = new Regex(rxPatternPs, RegexOptions.Multiline);
                        rxValSt = Regex.Replace(rxValSt, rxPatternPs, rxValPs, RegexOptions.Multiline);

                        //Set the spawn state array after changes made
                        jsons = Regex.Replace(jsons, rxPatternSt, rxValSt, RegexOptions.Singleline);

                        //Set Portal Interference false DaC
                        Regex myRegexPrtl = new Regex(rxPatternPrtl, RegexOptions.Multiline);
                        Match prtl = myRegexPrtl.Match(jsons);
                        //rxValPrtl = prtl.ToString();
                        //AppendLine(textBox3, rxValPrtl);

                        //Set Portal Interference state rxValPrtl preset to false
                        jsons = Regex.Replace(jsons, rxPatternPrtl, rxValPrtl, RegexOptions.Multiline);

                        progressBar3.Invoke((Action)(() => progressBar3.Value = 40));

                        //Write all modifications of file to saveedit.json
                        File.WriteAllText(@".\nmssavetool\saveedit.json", jsons);

                        //Show log of changes in txtbox
                        Match g = myRegex1.Match(jsons);
                        Match x = myRegex2.Match(jsons);
                        Match y = myRegex3.Match(jsons);
                        Match z = myRegex4.Match(jsons);
                        Match ssi = myRegex5.Match(jsons);
                        Match pi = myRegex6.Match(jsons);
                        Match ps = myRegexs.Match(myRegexps.Match(jsons).ToString());
                        AppendLine(textBox27, "Player Move Data: " + g.ToString() + x.ToString() + y.ToString() + z.ToString() + ssi.ToString() + pi.ToString() + ps.ToString());

                        progressBar3.Invoke((Action)(() => progressBar3.Value = 70));

                        //Write the new save file 
                        await WriteSave(saveslot);

                        //Set json to the new modified hg file
                        json = File.ReadAllText(hgFilePath);

                        //Read the new json and check portal interference state
                        var nms = Nms.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.The6F.DaC.ToString();
                        GetPlayerCoord();

                        progressBar3.Invoke((Action)(() => progressBar3.Value = 100));
                        progressBar3.Visible = false;

                        //set the last write time box
                        textBox26.Clear();
                        FileInfo hgfile = new FileInfo(hgFilePath);
                        AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                        MessageBox.Show("Player moved successfully!", "Confirmation", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }                
            }
            else
            {
                AppendLine(textBox13, "Please load and select a location!");
                MessageBox.Show("Please load and select a location!", "Alert");
            }
        }
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(listBox3.GetItemText(listBox3.SelectedItem) != "")
            {
                List<string> list = new List<string>();
                //list.Add("Loc: " + DiscList[i] + " - G: " + galaxy + " - PC: " + PortalCode + " -- GC: " + GalacticCoord);
                list.Add(listBox3.GetItemText(listBox3.SelectedItem));
                string path2 = MakeUnique(@".\backup\locbackup.txt").ToString();
                File.WriteAllLines(path2, list);
                Process.Start(path2);
                AppendLine(textBox13, "Single Record saved!");
                LoadTxt();
            }
            else
            {
                AppendLine(textBox13, "No record saved! Please select a location!");
            }
            
        }
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Delete " + listBox4.GetItemText(listBox4.SelectedItem) + " ? ", "Locbackup Manager", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists(@".\backup\" + listBox4.GetItemText(listBox4.SelectedItem)))
                {
                    if (listBox4.GetItemText(listBox4.SelectedItem) != "")
                    {
                        File.Delete(@".\backup\" + listBox4.GetItemText(listBox4.SelectedItem));
                        MessageBox.Show("File deleted Successfully.", "Confirmation");
                        LoadTxt();
                    }
                    else
                    {
                        MessageBox.Show("No file found, not deleted.", "Alert");
                        LoadTxt();
                    }
                }
                else
                {
                    MessageBox.Show("No file found, not deleted.", "Alert");
                    LoadTxt();
                }
                
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Cancelled! No file deleted.", "Alert");
            }
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("steam://rungameid/275850");
        }

        private void GalacticToVoxelMan(string oX, string oY, string oZ, string oSSI)
        {
            //Galactic Coordinate to Voxel Coordinates 
            textBox15.Clear();

            //HEX in
            //AppendLine(textBox15, "Galactic Coordinates HEX: SSI:" + oSSI + " Y:" + oY + " Z:" + oZ + " X:" + oX);

            //HEX to DEC
            int icX = Convert.ToInt32(oX, 16);
            int icY = Convert.ToInt32(oY, 16);
            int icZ = Convert.ToInt32(oZ, 16);
            int icSSI = Convert.ToInt32(oSSI, 16);
            AppendLine(textBox15, "Galactic Coordinates DEC: SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX);

            //SHIFT DEC 2047 127 2047 ssi-same
            int vX = icX - 2047;
            int vY = icY - 127;
            int vZ = icZ - 2047;

            //voxel dec  x y z ssi-same
            AppendLine(textBox15, "*** Voxel Coordinates DEC: SSI:" + icSSI + " Y:" + vY + " Z:" + vZ + " X:" + vX + " ***");
            //voxel = "SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX;

            X = vX.ToString();
            Y = vY.ToString();
            Z = vZ.ToString();
            SSI = icSSI.ToString();
        }
        private async void Button11_Click(object sender, EventArgs e)
        {
            //Galactic coordinates to Voxel
            try
            {
                //Read-only galactic coord for now   
                if (textBox14.Text != "")
                {
                    string[] value = textBox14.Text.Replace(" ", "").Split(':');

                    GalacticToVoxelMan(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());
                    if(Convert.ToInt32(textBox18.Text) < 255)
                    {
                        galaxy = textBox18.Text;                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid Galaxy!", "Alert");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Cannot locate player!", "Alert");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Move Player? ", "Fast Travel", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    
                    if (galaxy != "" && X != "" && Y != "" && Z != "" && SSI != "" && saveslot >= 1 && saveslot <= 5)
                    {
                        AppendLine(textBox15, "Move Player to: Galaxy: " + galaxy + " -- X:" + X + " -- Y:" + Y + " -- Z:" + Z + " -- SSI:" + SSI);
                        progressBar4.Visible = true;
                        progressBar4.Invoke((Action)(() => progressBar4.Value = 10));

                        //Read and decrypt save (?)
                        await ReadSave(saveslot);

                        //Set all Regex values
                        JsonSet("all");

                        progressBar4.Invoke((Action)(() => progressBar4.Value = 25));

                        //Read all the JSON text from nmssavetool decrypt
                        string jsons = File.ReadAllText(@".\nmssavetool\save.json");

                        ////Set Player Location
                        //Get the Player location text array
                        Regex myRegex = new Regex(rxPatternP, RegexOptions.Singleline);
                        Match m = myRegex.Match(jsons);
                        rxValP = m.ToString();

                        //Get and Set Galaxy
                        Regex myRegex1 = new Regex(rxPatternG, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternG, rxValG, RegexOptions.Multiline);

                        //Get and Set X
                        Regex myRegex2 = new Regex(rxPatternX, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternX, rxValX, RegexOptions.Multiline);

                        //Get amd Set Y
                        Regex myRegex3 = new Regex(rxPatternY, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternY, rxValY, RegexOptions.Multiline);

                        //Get and Set Z
                        Regex myRegex4 = new Regex(rxPatternZ, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternZ, rxValZ, RegexOptions.Multiline);

                        //Get and Set Solar System index (SSI)
                        Regex myRegex5 = new Regex(rxPatternSSI, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternSSI, rxValSSI, RegexOptions.Multiline);

                        //Get and Set Planet Index
                        Regex myRegex6 = new Regex(rxPatternPI, RegexOptions.Multiline);
                        rxValP = Regex.Replace(rxValP, rxPatternPI, rxValPI, RegexOptions.Multiline);

                        //Set the player location array after changes made
                        jsons = Regex.Replace(jsons, rxPatternP, rxValP, RegexOptions.Singleline);

                        ////Set Spawn State
                        // Get the Spawn state array
                        Regex myRegexs = new Regex(rxPatternSt, RegexOptions.Singleline);
                        Match ms = myRegexs.Match(jsons);
                        rxValSt = ms.ToString();

                        //Get and set Player last known location in Spwn state array
                        Regex myRegexps = new Regex(rxPatternPs, RegexOptions.Multiline);
                        rxValSt = Regex.Replace(rxValSt, rxPatternPs, rxValPs, RegexOptions.Multiline);

                        //Set the spawn state array after changes made
                        jsons = Regex.Replace(jsons, rxPatternSt, rxValSt, RegexOptions.Singleline);

                        //Set Portal Interference false DaC
                        Regex myRegexPrtl = new Regex(rxPatternPrtl, RegexOptions.Multiline);
                        Match prtl = myRegexPrtl.Match(jsons);
                        //rxValPrtl = prtl.ToString();
                        //AppendLine(textBox3, rxValPrtl);

                        //Set Portal Interference state rxValPrtl preset to false
                        jsons = Regex.Replace(jsons, rxPatternPrtl, rxValPrtl, RegexOptions.Multiline);

                        progressBar4.Invoke((Action)(() => progressBar4.Value = 40));

                        //Write all modifications of file to saveedit.json
                        File.WriteAllText(@".\nmssavetool\saveedit.json", jsons);

                        //Show log of changes in txtbox
                        Match g = myRegex1.Match(jsons);
                        Match x = myRegex2.Match(jsons);
                        Match y = myRegex3.Match(jsons);
                        Match z = myRegex4.Match(jsons);
                        Match ssi = myRegex5.Match(jsons);
                        Match pi = myRegex6.Match(jsons);
                        Match ps = myRegexs.Match(myRegexps.Match(jsons).ToString());
                        AppendLine(textBox15, "Player Move Data: " + g.ToString() + x.ToString() + y.ToString() + z.ToString() + ssi.ToString() + pi.ToString() + ps.ToString());

                        progressBar4.Invoke((Action)(() => progressBar4.Value = 70));

                        //Write the new save file 
                        await WriteSave(saveslot);

                        //Set json to the new modified hg file
                        json = File.ReadAllText(hgFilePath);

                        //Read the new json and check portal interference state
                        var nms = Nms.FromJson(json);
                        textBox12.Clear();
                        textBox12.Text = nms.The6F.DaC.ToString();
                        GetPlayerCoord();

                        progressBar4.Invoke((Action)(() => progressBar4.Value = 100));
                        progressBar4.Visible = false;

                        //set the last write time box
                        textBox26.Clear();
                        FileInfo hgfile = new FileInfo(hgFilePath);
                        AppendLine(textBox26, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                        MessageBox.Show("Player moved successfully!", "Confirmation", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Please select a save slot!", "Confirmation", MessageBoxButtons.OK);
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    textBox15.Clear();
                    return;
                }
            }
            catch
            {
                //textBox14.Clear();
                textBox15.Clear();
                textBox18.Clear();
                AppendLine(textBox15, "Incorrect Coordinate Input!");
            }
        }

        private void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //Read-only galactic coord for now
            textBox14.Text = textBox22.Text;
            textBox18.Text = pgalaxy;
        }
    }
}
    
    

