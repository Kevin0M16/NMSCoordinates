using QuickType;
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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            Glyphs();
            ShowGlyphKey();
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

        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }        
        private void CalculateVoxelFromHex(TextBox textBox)
        {
            //PlanetNumber--SolarSystemIndex--GalaxyNumber--VoxelY--VoxelZ--VoxelX
            //4 bit--12 bit--8 bit--8 bit--12 bit--12 bit
            //"0x2 049 00 01 D37 652" 460 475 89 64 091 954  0 04A FB 9F6 C9D
            //textBox7.Clear();

            string basehx = textBox.Text;
            string b6 = basehx.Substring(basehx.Length - 3, 3);
            string b5 = basehx.Substring(basehx.Length - 6, 3);
            string b4 = basehx.Substring(basehx.Length - 8, 2);
            string b3 = basehx.Substring(basehx.Length - 10, 2);
            string b2 = basehx.Substring(basehx.Length - 13, 3);
            string b1 = basehx.Substring(basehx.Length - 16, 3);
            AppendLine(textBox7, "Base Hex Split: " + b1 + " " + b2 + " " + b3 + " " + b4 + " " + b5 + " " + b6);
            AppendLine(textBox7, "Base Hex id's: Planet #:" + b1 + " SSI:" + b2 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6);

            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            AppendLine(textBox7, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4);

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)
            int pidec = Convert.ToInt32(b1, 16); //HEX to DEC
            int ssidec = Convert.ToInt32(b2, 16);
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            AppendLine(textBox7, "Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX);

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            AppendLine(textBox7, "Voxel Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString());

            int shiftX = calc1 - 2047;
            int shiftY = calc2 - 127;
            int shiftZ = calc3 - 2047;
            //AppendLine(textBox3, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            AppendLine(textBox7, "Voxel: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX);
        }
        private void HexToVoxel(string basehx)
        {
            //PlanetNumber--SolarSystemIndex--GalaxyNumber--VoxelY--VoxelZ--VoxelX
            //4 bit--12 bit--8 bit--8 bit--12 bit--12 bit
            //"0x2 049 00 01 D37 652" 460 475 89 64 091 954   0 04A FB 9F6 C9D

            string b6 = basehx.Substring(basehx.Length - 3, 3);
            string b5 = basehx.Substring(basehx.Length - 6, 3);
            string b4 = basehx.Substring(basehx.Length - 8, 2);
            string b3 = basehx.Substring(basehx.Length - 10, 2);
            string b2 = basehx.Substring(basehx.Length - 13, 3);
            string b1 = basehx.Substring(basehx.Length - 16, 3);


            AppendLine(textBox7, "Base Hex Split: " + b1 + " " + b2 + " " + b3 + " " + b4 + " " + b5 + " " + b6);
            AppendLine(textBox7, "Base Hex id's: Planet #:" + b1 + " SSI:" + b2 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6);

            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            AppendLine(textBox7, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4);

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)
            int pidec = Convert.ToInt32(b1, 16); //HEX to DEC
            int ssidec = Convert.ToInt32(b2, 16);
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            AppendLine(textBox7, "Base Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX);

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            AppendLine(textBox7, "Base Voxel Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString());

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFFFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFFF); // Z[HEX] to Z[DEC] 3 digits
            //int ihexSSI = (Convert.ToInt32(ssidec, 16) & 0xFFFF); // SSI[HEX] to SSI[DEC] 3 digits

            //AppendLine(textBox14, "P: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + ssidec);
            GalacticCoord2 = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ihexX, ihexY, ihexZ, ssidec & 0xFFFF); //Format to 4 digit seperated by colon
            AppendLine(textBox7, "Galactic Coordinates: " + GalacticCoord2);

            int shiftX = calc1 - 2047;
            int shiftY = calc2 - 127;
            int shiftZ = calc3 - 2047;
            //AppendLine(textBox3, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            AppendLine(textBox7, "Base Voxel: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX);
            //voxel = "Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX;
        }

        private void GalacticToPortal(string X, string Y, string Z, string SSI)
        {
            //Galactic Coordinate to Portal Code

            int dec1 = Convert.ToInt32(X, 16); // X[HEX] to X[DEC]
            int dec2 = Convert.ToInt32(Y, 16); // Y[HEX] to X[DEC]
            int dec3 = Convert.ToInt32(Z, 16); // Z[HEX] to X[DEC]
            int dec4 = Convert.ToInt32(SSI, 16); // SSI[HEX] to SSI[DEC]
            AppendLine(textBox7, "Galactic HEX to DEC: " + dec1.ToString() + " " + dec2.ToString() + " " + dec3.ToString() + " " + dec4);

            //string g4 = SSI.ToString("X");

            int dec5 = Convert.ToInt32("801", 16); // 801[HEX] to 801[DEC]
            int dec6 = Convert.ToInt32("81", 16); // 81[HEX] to 81[DEC]
            int dec7 = Convert.ToInt32("1000", 16); // 100[HEX] to 1000[DEC]
            int dec8 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            AppendLine(textBox7, "Shift HEX to DEC: " + "801:" + dec5.ToString() + " 81:" + dec6.ToString() + " 1000:" + dec7.ToString() + " 100:" + dec8.ToString());

            int calc1 = (dec1 + dec5) % dec7; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (dec2 + dec6) % dec8; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (dec3 + dec5) % dec7; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            AppendLine(textBox7, "Calculate Portal DEC: " + "X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + dec4);

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            AppendLine(textBox7, "Portal HEX numbers: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + SSI);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFF); // Z[HEX] to Z[DEC] 3 digits
            int ihexSSI = (Convert.ToInt32(SSI, 16) & 0xFFF); // SSI[HEX] to SSI[DEC] 3 digits

            PortalCode = string.Format("0{0:X3}{1:X2}{2:X3}{3:X3}", ihexSSI, ihexY, ihexZ, ihexX); // Format digits 0 3 2 3 3
            //[SSI][Y][Z][X] Portal Code
            AppendLine(textBox7, "*** Portal Code: " + PortalCode + " ***");
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

            glyphDict.Add('a', Properties.Resources.A);
            glyphDict.Add('b', Properties.Resources.B);
            glyphDict.Add('c', Properties.Resources.C);
            glyphDict.Add('d', Properties.Resources.D);
            glyphDict.Add('e', Properties.Resources.E);
            glyphDict.Add('f', Properties.Resources.F);
        }
        private void PortalToVoxel(string portalcode)
        {
            //0 04A FB 9F6 C9D
            //Break apart Portal Code
            string b6 = portalcode.Substring(portalcode.Length - 3, 3);
            string b5 = portalcode.Substring(portalcode.Length - 6, 3);
            string b4 = portalcode.Substring(portalcode.Length - 8, 2);
            string b3 = portalcode.Substring(portalcode.Length - 11, 3);
            AppendLine(textBox7, "Hex Split: " + b3 + " " + b4 + " " + b5 + " " + b6);
            AppendLine(textBox7, "Hex id's: Planet #:" + 0 + " SSI:" + 0 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6);

            //HEX to DEC
            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            AppendLine(textBox7, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4);

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)            
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            AppendLine(textBox7, "Dec: Planet #:" + 0 + " SSI:" + 0 + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX);

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            AppendLine(textBox7, "Voxel Dec: Planet #:" + 0 + " SSI:" + 0 + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString());

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            AppendLine(textBox7, "P: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + galdec.ToString("X"));

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFFFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFFF); // Z[HEX] to Z[DEC] 3 digits            
            GalacticCoord2 = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ihexX, ihexY, ihexZ, galdec & 0xFFFF); //Format to 4 digit seperated by colon
            AppendLine(textBox7, "*** Galactic Coordinates: " + GalacticCoord2 + " ***");

            iX = calc1 - 2047;
            iY = calc2 - 127;
            iZ = calc3 - 2047;
            iSSI = galdec;

            //AppendLine(textBox7, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            AppendLine(textBox7, "*** Voxel Coordinates: Portal#:" + "0" + " SSI#:" + iSSI + " Y:" + iY + " Z:" + iZ + " X:" + iX + " ***");
            //voxel = "Portal#:" + "0" + " SSI:" + "0" + " Gal#:" + iSSI + " Y:" + iY + " Z:" + iZ + " X:" + iX;
        }
        private void VoxelToGalacticCoord(int X, int Y, int Z, int SSI)
        {
            //Voxel Coordinates to Galactic Coordinate
            //textBox1.Clear();
            textBox7.Clear();

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            AppendLine(textBox7, "DEC: " + iX + " " + iY + " " + iZ);

            int dd1 = X + 2047;
            int dd2 = Y + 127;
            int dd3 = Z + 2047;
            AppendLine(textBox7, "SHIFT: " + dd1 + " " + dd2 + " " + dd3);

            string g1 = dd1.ToString("X");
            string g2 = dd2.ToString("X");
            string g3 = dd3.ToString("X");
            string g4 = SSI.ToString("X");
            AppendLine(textBox7, "Galactic HEX numbers: " + g1 + " " + g2 + " " + g3 + " " + g4);

            int ig1 = Convert.ToInt32(g1, 16); // X[HEX] to X[DEC]
            int ig2 = Convert.ToInt32(g2, 16); // Y[HEX] to Y[DEC]
            int ig3 = Convert.ToInt32(g3, 16); // Z[HEX] to Z[DEC]
            int ig4 = Convert.ToInt32(g4, 16); // SSI[HEX] to SSI[DEC]
            AppendLine(textBox7, "Galactic DEC numbers: " + ig1 + " " + ig2 + " " + ig3 + " " + ig4);

            GalacticCoord = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ig1, ig2, ig3, ig4); //Format to 4 digit seperated by colon
            AppendLine(textBox7, "*** Galactic Coordinates: " + GalacticCoord + " ***");
        }
        private void GalacticToVoxel(string X, string Y, string Z, string SSI)
        {
            //Galactic Coordinate to Voxel Coordinates 
            textBox7.Clear();

            //HEX in
            AppendLine(textBox7, "Galactic Coordinates HEX: SSI:" + SSI + " Y:" + Y + " Z:" + Z + " X:" + X);

            //HEX to DEC
            int icX = Convert.ToInt32(X, 16);
            int icY = Convert.ToInt32(Y, 16);
            int icZ = Convert.ToInt32(Z, 16);
            int icSSI = Convert.ToInt32(SSI, 16);
            AppendLine(textBox7, "Galactic Coordinates DEC: SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX);

            //SHIFT DEC 2047 127 2047 ssi-same
            int vX = icX - 2047;
            int vY = icY - 127;
            int vZ = icZ - 2047;

            //voxel dec  x y z ssi-same
            AppendLine(textBox7, "*** Voxel Coordinates DEC: SSI:" + icSSI + " Y:" + vY + " Z:" + vZ + " X:" + vX + " ***");
            //voxel = "SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX;

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

                    string t1 = textBox1.Text.Replace(" ", "");

                    if(t1.Length == 12 && !textBox1.Text.Contains(":"))
                    {
                        //Gives both Galactic and Voxel
                        PortalToVoxel(t1);
                        textBox2.Text = GalacticCoord2;
                        PortalCode = textBox1.Text;
                    }
                    else
                    {
                        Clear();
                        AppendLine(textBox7, "Incorrect Coordinate Input!");
                    }
                        
                }
            }
            catch
            {
                Clear();
                AppendLine(textBox7, "Incorrect Coordinate Input!");
            }         
        }              

        private void Button2_Click(object sender, EventArgs e)
        {
            //Galactic coordinates to Voxel
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

                    string t2 = textBox2.Text.Replace(" ", "");

                    if (t2.Contains(":") && t2.Length == 19)
                    {
                        string[] value = t2.Replace(" ", "").Split(':');
                        GalacticToVoxel(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());
                        GalacticToPortal(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());

                        textBox1.Text = PortalCode;
                    }

                    if(t2.Length == 16 && !t2.Contains(":"))
                    {
                        //string gc = textBox2.Text.Replace(" ", "");  //0000 0000 0000 0000
                        string g1 = t2.Substring(t2.Length - 4, 4);
                        string g2 = t2.Substring(t2.Length - 8, 4);
                        string g3 = t2.Substring(t2.Length - 12, 4);
                        string g4 = t2.Substring(t2.Length - 16, 4);

                        GalacticToVoxel(g4, g3, g2, g1);
                        GalacticToPortal(g4, g3, g2, g1);

                        textBox1.Text = PortalCode;
                    }

                    if(t2.Replace(":", "").Length < 16 | t2.Replace(":", "").Length > 16 | t2.Length < 16)
                    {
                        Clear();
                        AppendLine(textBox7, "Incorrect Coordinate Input!");
                    }
                    
                }
            }
            catch
            {
                Clear();                
                AppendLine(textBox7, "Incorrect Coordinate Input!");
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //Voxel to Galactic Coordinates
            try
            {
                if (textBox3.Text != "")
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox7.Clear();

                    VoxelToGalacticCoord(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox6.Text));
                    textBox2.Text = GalacticCoord;
                    string[] value = GalacticCoord.Replace(" ", "").Split(':');
                    GalacticToPortal(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());
                    textBox1.Text = PortalCode;
                }
            }
            catch
            {
                Clear();
                AppendLine(textBox7, "Incorrect Coordinate Input!");
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox8.Text != "")
                {
                    HexToVoxel(textBox8.Text);
                    CalculateVoxelFromHex(textBox8);
                }
            }
            catch
            {
                Clear();
                AppendLine(textBox7, "Incorrect Coordinate Input!");
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
        private Form1 f1;
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                f1 = (Form1)Application.OpenForms["Form1"];
                if (!f1.TextBoxPerm == true)
                {
                    f1.TextBoxValue = textBox2.Text;

                    if (f1 == null)
                    {
                        f1 = new Form1();
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
                    AppendLine(textBox7, "Please UNLOCK Manual Travel!");
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
