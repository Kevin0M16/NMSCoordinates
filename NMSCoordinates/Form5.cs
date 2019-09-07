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
                    //Gives both Galactic and Voxel
                    PortalToVoxel((textBox1.Text).Replace(" ", ""));
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

                    string[] value = textBox2.Text.Replace(" ", "").Split(':');
                    GalacticToVoxel(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());
                    GalacticToPortal(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());
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
                    string[] value = GalacticCoord.Replace(" ", "").Split(':');
                    GalacticToPortal(value[0].Trim(), value[1].Trim(), value[2].Trim(), value[3].Trim());
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
    }
}
