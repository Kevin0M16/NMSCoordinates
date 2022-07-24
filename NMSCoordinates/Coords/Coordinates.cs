using NMSCoordinates.SaveData;
using System;
using System.Windows.Forms;

namespace NMSCoordinates
{
    public class Coordinates
    {
        public struct Player
        {
            internal string LongHex { get; set; }
            internal string GalacticCoordinate { get; set; }
            internal string PortalCode { get; set; }
            internal string DistanceToCenter { get; set; }
            internal string GalaxyName { get; set; }
            internal string Galaxy { get; set; }
            internal string X { get; set; }
            internal string Y { get; set; }
            internal string Z { get; set; }
            internal string SSI { get; set; }
            internal string PI { get; set; }

            internal int iGalaxy { get; set; }
            internal int iX { get; set; }
            internal int iY { get; set; }
            internal int iZ { get; set; }
            internal int iSSI { get; set; }
            internal int iPI { get; set; }
        }
        public struct Destination
        {
            internal string LongHex { get; set; }
            internal string GalacticCoordinate { get; set; }
            internal string PortalCode { get; set; }
            internal string DistanceToCenter { get; set; }
            internal string GalaxyName { get; set; }
            internal string Galaxy { get; set; }
            internal string X { get; set; }
            internal string Y { get; set; }
            internal string Z { get; set; }
            internal string SSI { get; set; }
            internal string PI { get; set; }

            internal int iGalaxy { get; set; }
            internal int iX { get; set; }
            internal int iY { get; set; }
            internal int iZ { get; set; }
            internal int iSSI { get; set; }
            internal int iPI { get; set; }
        }
        public struct GalacticCoordinates
        {
            internal string HexX { get; set; }
            internal string HexY { get; set; }
            internal string HexZ { get; set; }
            internal string HexSSI { get; set; }
        }
        public static Destination TeleportEndpoints(int i, string json, TextBox tb)
        {
            //lookup info from the Json hg file
            var nms = GameSaveData.FromJson(json);
            Destination dest = new Destination
            {
                Galaxy = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.RealityIndex.ToString(),
                X = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelX.ToString(),
                Y = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelY.ToString(),
                Z = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelZ.ToString(),
                SSI = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.SolarSystemIndex.ToString(),
                PI = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.PlanetIndex.ToString(),

                iGalaxy = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.RealityIndex),
                iX = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelX),
                iY = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelY),
                iZ = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelZ),
                iSSI = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.SolarSystemIndex),
                iPI = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.PlanetIndex)
            };

            dest.LongHex = CoordCalculations.VoxelToHex(dest.iGalaxy, dest.iPI, dest.iX, dest.iY, dest.iZ, dest.iSSI);
            dest.GalacticCoordinate = CoordCalculations.VoxelToGalacticCoord(dest.iX, dest.iY, dest.iZ, dest.iSSI, tb);
            dest.PortalCode = CoordCalculations.VoxelToPortal(dest.iPI, dest.iX, dest.iY, dest.iZ, dest.iSSI, tb);
            dest.DistanceToCenter = CoordCalculations.DistanceToCenter(dest.iX, dest.iY, dest.iZ); 
            dest.GalaxyName = Globals.GalaxyLookup(dest.Galaxy);

            return dest;
        }
        public static Destination TeleportEndpoints(int i, string json)
        {
            // 
            var nms = GameSaveData.FromJson(json);
            Destination dest = new Destination
            {
                Galaxy = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.RealityIndex.ToString(),
                X = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelX.ToString(),
                Y = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelY.ToString(),
                Z = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelZ.ToString(),
                SSI = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.SolarSystemIndex.ToString(),
                PI = nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.PlanetIndex.ToString(),

                iGalaxy = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.RealityIndex),
                iX = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelX),
                iY = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelY),
                iZ = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.VoxelZ),
                iSSI = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.SolarSystemIndex),
                iPI = Convert.ToInt32(nms.PlayerStateData.TeleportEndpoints[i].UniverseAddress.GalacticAddress.PlanetIndex)
            };

            dest.LongHex = CoordCalculations.VoxelToHex(dest.iGalaxy, dest.iPI, dest.iX, dest.iY, dest.iZ, dest.iSSI);
            dest.GalacticCoordinate = CoordCalculations.VoxelToGalacticCoord(dest.iX, dest.iY, dest.iZ, dest.iSSI);
            dest.PortalCode = CoordCalculations.VoxelToPortal(dest.iPI, dest.iX, dest.iY, dest.iZ, dest.iSSI);
            dest.DistanceToCenter = CoordCalculations.DistanceToCenter(dest.iX, dest.iY, dest.iZ);
            dest.GalaxyName = Globals.GalaxyLookup(dest.Galaxy);

            return dest;
        }
        public static Destination PersistentBases(int i, string json)
        {
            //lookup info from the Json hg file            
            var nms = GameSaveData.FromJson(json);
            string basehex = nms.PlayerStateData.PersistentPlayerBases[i].GalacticAddress;
            Destination dest = HexToAll(basehex);

            return dest;            
        }
        public static Player PlayerGalaxy(string json)
        {
            // 
            var nms = GameSaveData.FromJson(json);
            Player player = new Player
            {
                Galaxy = nms.PlayerStateData.UniverseAddress.RealityIndex.ToString(),
                iGalaxy = Convert.ToInt32(nms.PlayerStateData.UniverseAddress.RealityIndex)
            };

            return player;
        }
        public static Player PlayerLocation(string json)
        {
            // 
            var nms = GameSaveData.FromJson(json);
            Player player = new Player
            {
                Galaxy = nms.PlayerStateData.UniverseAddress.RealityIndex.ToString(),
                X = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelX.ToString(),
                Y = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelY.ToString(),
                Z = nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelZ.ToString(),
                SSI = nms.PlayerStateData.UniverseAddress.GalacticAddress.SolarSystemIndex.ToString(),
                PI = nms.PlayerStateData.UniverseAddress.GalacticAddress.PlanetIndex.ToString(),

                iGalaxy = Convert.ToInt32(nms.PlayerStateData.UniverseAddress.RealityIndex),
                iX = Convert.ToInt32(nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelX),
                iY = Convert.ToInt32(nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelY),
                iZ = Convert.ToInt32(nms.PlayerStateData.UniverseAddress.GalacticAddress.VoxelZ),
                iSSI = Convert.ToInt32(nms.PlayerStateData.UniverseAddress.GalacticAddress.SolarSystemIndex),
                iPI = Convert.ToInt32(nms.PlayerStateData.UniverseAddress.GalacticAddress.PlanetIndex)
            };

            player.LongHex = CoordCalculations.VoxelToHex(player.iGalaxy, player.iPI, player.iX, player.iY, player.iZ, player.iSSI);
            player.GalacticCoordinate = CoordCalculations.VoxelToGalacticCoord(player.iX, player.iY, player.iZ, player.iSSI);
            player.PortalCode = CoordCalculations.VoxelToPortal(player.iPI, player.iX, player.iY, player.iZ, player.iSSI);
            player.DistanceToCenter = CoordCalculations.DistanceToCenter(player.iX, player.iY, player.iZ);
            player.GalaxyName = Globals.GalaxyLookup(player.Galaxy);

            return player;
        }
        public static Destination GalacticToVoxel(int galaxy, string X, string Y, string Z, string SSI, TextBox tb)
        {
            //Galactic Coordinate to Voxel Coordinates

            //HEX in
            Globals.AppendLine(tb, "Galactic Coordinates HEX: SSI:" + SSI + " Y:" + Y + " Z:" + Z + " X:" + X);

            //HEX to DEC
            int icX = Convert.ToInt32(X, 16);
            int icY = Convert.ToInt32(Y, 16);
            int icZ = Convert.ToInt32(Z, 16);
            int icSSI = Convert.ToInt32(SSI, 16);
            Globals.AppendLine(tb, "Galactic Coordinates DEC: SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX);

            //SHIFT DEC 2047 127 2047 ssi-same
            int vX = icX - 2047;
            int vY = icY - 127;
            int vZ = icZ - 2047;

            //voxel dec  x y z ssi-same
            Globals.AppendLine(tb, "*** Voxel Coordinates DEC: SSI:" + icSSI + " Y:" + vY + " Z:" + vZ + " X:" + vX + " ***");
            //voxel = "SSI:" + icSSI + " Y:" + icY + " Z:" + icZ + " X:" + icX;

            Destination dest = new Destination
            {
                iGalaxy = galaxy,
                iX = vX,
                iY = vY,
                iZ = vZ,
                iSSI = icSSI,
                
                Galaxy = galaxy.ToString(),
                X = vX.ToString(),
                Y = vY.ToString(),
                Z = vZ.ToString(),
                SSI = icSSI.ToString()
            };

            return dest;
        }
        public static GalacticCoordinates GetGalacticCoordHex(string GalacticCoord)
        {
            //if format 0000:0000:0000:0000 A:B:C:D
            if (GalacticCoord.Contains(":") && GalacticCoord.Length == 19)
            {
                string[] value = GalacticCoord.Replace(" ", "").Split(':');

                //Only take 4 digits from the last array so can add notes GC: 0000:0000:0000:[0000] A:B:C:D
                string A = value[0].Trim();
                string B = value[1].Trim();
                string C = value[2].Trim();
                string D = value[3].Trim().Substring(0, 4);

                GalacticCoordinates gac = new GalacticCoordinates
                {
                    HexX = A,
                    HexY = B,
                    HexZ = C,
                    HexSSI = D
                };
                return gac;
            }

            //if format 0000000000000000
            if (GalacticCoord.Length == 16 && !GalacticCoord.Contains(":"))
            {
                //0000 0000 0000 0000  XXXX:YYYY:ZZZZ:SSIX  A B C D
                string A = GalacticCoord.Substring(GalacticCoord.Length - 16, 4);
                string B = GalacticCoord.Substring(GalacticCoord.Length - 12, 4);
                string C = GalacticCoord.Substring(GalacticCoord.Length - 8, 4);
                string D = GalacticCoord.Substring(GalacticCoord.Length - 4, 4);

                GalacticCoordinates gac = new GalacticCoordinates
                {
                    HexX = A,
                    HexY = B,
                    HexZ = C,
                    HexSSI = D
                };
                return gac;
            }

            return new GalacticCoordinates();
        }
        public static Destination HexToAll(string basehx)
        {
            //PlanetNumber--SolarSystemIndex--GalaxyNumber--VoxelY--VoxelZ--VoxelX
            //4 bit--12 bit--8 bit--8 bit--12 bit--12 bit
            //"0x2 049 00 01 D37 652" 460 475 89 64 091 954   0 04A FB 9F6 C9D

            if (!basehx.StartsWith("0x"))
            {
                long hxe = Convert.ToInt64(basehx);
                //Globals.AppendLine(tb, "Long DEC: " + hxe); // Display Long DEC

                basehx = "0x" + hxe.ToString("X"); // Convert Long DEC to Long HEX
                //Globals.AppendLine(tb, "Long HEX: " + basehx);// Display Long HEX
            }
            else
            {
                long hxe = Convert.ToInt64(basehx, 16); // Convert Long HEX to DEC
                //Globals.AppendLine(tb, "Long DEC: " + hxe); // Display Long DEC
                //Globals.AppendLine(tb, "Long HEX: " + basehx); // Display Long HEX
            }

            string b6 = basehx.Substring(basehx.Length - 3, 3);
            string b5 = basehx.Substring(basehx.Length - 6, 3);
            string b4 = basehx.Substring(basehx.Length - 8, 2);
            string b3 = basehx.Substring(basehx.Length - 10, 2);
            string b2 = basehx.Substring(basehx.Length - 13, 3);
            string b1 = basehx.Substring(basehx.Length - 16, 3);

            /*Globals.AppendLine(tb, "Base Hex Split: " + b1 + " " + b2 + " " + b3 + " " + b4 + " " + b5 + " " + b6);*/
            /*Globals.AppendLine(tb, "Base Hex id's: Planet #:" + b1 + " SSI:" + b2 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6);*/

            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            /*Globals.AppendLine(tb, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4);*/

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)
            int pidec = Convert.ToInt32(b1, 16); //HEX to DEC
            int ssidec = Convert.ToInt32(b2, 16);
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            /*Globals.AppendLine(tb, "Base Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX);*/

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //Globals.AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            /*Globals.AppendLine(tb, "Base Voxel Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString());*/

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            //Globals.AppendLine(tb, "X:" + hexX + " Y:" + hexY + " Z:" + hexZ);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFFFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFFF); // Z[HEX] to Z[DEC] 3 digits
            //int ihexSSI = (Convert.ToInt32(ssidec, 16) & 0xFFFF); // SSI[HEX] to SSI[DEC] 3 digits

            //Globals.AppendLine(textBox14, "P: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + ssidec);
            string GalacticCoord2 = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ihexX, ihexY, ihexZ, ssidec & 0xFFFF); //Format to 4 digit seperated by colon
            /*Globals.AppendLine(tb, "Galactic Coordinates: " + GalacticCoord2);*/

            int shiftX = calc1 - 2047;
            int shiftY = calc2 - 127;
            int shiftZ = calc3 - 2047;
            //Globals.AppendLine(textBox3, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            /*Globals.AppendLine(tb, "Base Voxel: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX);*/
            //voxel = "Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX;

            GalacticCoordinates gac = GetGalacticCoordHex(GalacticCoord2);
            string pc = CoordCalculations.GalacticToPortal(pidec, gac.HexX, gac.HexY, gac.HexZ, gac.HexSSI);

            Destination dest = new Destination
            {
                iGalaxy = galdec,
                iX = shiftX,
                iY = shiftY,
                iZ = shiftZ,
                iSSI = ssidec,
                iPI = pidec,

                Galaxy = galdec.ToString(),
                X = shiftX.ToString(),
                Y = shiftY.ToString(),
                Z = shiftZ.ToString(),
                SSI = ssidec.ToString(),
                PI = pidec.ToString(),

                GalacticCoordinate = GalacticCoord2,
                PortalCode = pc
            };

            dest.LongHex = basehx;
            dest.DistanceToCenter = CoordCalculations.DistanceToCenter(dest.iX, dest.iY, dest.iZ);
            dest.GalaxyName = Globals.GalaxyLookup(dest.Galaxy);

            return dest;
        }
        public static Destination HexToAll(string basehx, TextBox tb)
        {
            //PlanetNumber--SolarSystemIndex--GalaxyNumber--VoxelY--VoxelZ--VoxelX
            //4 bit--12 bit--8 bit--8 bit--12 bit--12 bit
            //"0x2 049 00 01 D37 652" 460 475 89 64 091 954   0 04A FB 9F6 C9D

            if (!basehx.StartsWith("0x"))
            {
                long hxe = Convert.ToInt64(basehx);
                Globals.AppendLine(tb, "Long DEC: " + hxe); // Display Long DEC

                basehx = "0x" + hxe.ToString("X"); // Convert Long DEC to Long HEX
                Globals.AppendLine(tb, "Long HEX: " + basehx);// Display Long HEX
            }
            else
            {
                long hxe = Convert.ToInt64(basehx, 16); // Convert Long HEX to DEC
                Globals.AppendLine(tb, "Long DEC: " + hxe); // Display Long DEC
                Globals.AppendLine(tb, "Long HEX: " + basehx); // Display Long HEX
            }

            string b6 = basehx.Substring(basehx.Length - 3, 3);
            string b5 = basehx.Substring(basehx.Length - 6, 3);
            string b4 = basehx.Substring(basehx.Length - 8, 2);
            string b3 = basehx.Substring(basehx.Length - 10, 2);
            string b2 = basehx.Substring(basehx.Length - 13, 3);
            string b1 = basehx.Substring(basehx.Length - 16, 3);

            Globals.AppendLine(tb, "Base Hex Split: " + b1 + " " + b2 + " " + b3 + " " + b4 + " " + b5 + " " + b6);
            Globals.AppendLine(tb, "Base Hex id's: Planet #:" + b1 + " SSI:" + b2 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6);

            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            Globals.AppendLine(tb, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4);

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)
            int pidec = Convert.ToInt32(b1, 16); //HEX to DEC
            int ssidec = Convert.ToInt32(b2, 16);
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            Globals.AppendLine(tb, "Base Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX);

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //Globals.AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            Globals.AppendLine(tb, "Base Voxel Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString());

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            //Globals.AppendLine(tb, "X:" + hexX + " Y:" + hexY + " Z:" + hexZ);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFFFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFFF); // Z[HEX] to Z[DEC] 3 digits
            //int ihexSSI = (Convert.ToInt32(ssidec, 16) & 0xFFFF); // SSI[HEX] to SSI[DEC] 3 digits

            //Globals.AppendLine(textBox14, "P: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + ssidec);
            string GalacticCoord2 = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ihexX, ihexY, ihexZ, ssidec & 0xFFFF); //Format to 4 digit seperated by colon
            Globals.AppendLine(tb, "Galactic Coordinates: " + GalacticCoord2);

            int shiftX = calc1 - 2047;
            int shiftY = calc2 - 127;
            int shiftZ = calc3 - 2047;
            //Globals.AppendLine(textBox3, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            Globals.AppendLine(tb, "Base Voxel: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX);
            //voxel = "Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX;

            GalacticCoordinates gac = GetGalacticCoordHex(GalacticCoord2);
            string pc = CoordCalculations.GalacticToPortal(pidec, gac.HexX, gac.HexY, gac.HexZ, gac.HexSSI);

            Destination dest = new Destination
            {
                iGalaxy = galdec,
                iX = shiftX,
                iY = shiftY,
                iZ = shiftZ,
                iSSI = ssidec,
                iPI = pidec,

                Galaxy = galdec.ToString(),
                X = shiftX.ToString(),
                Y = shiftY.ToString(),
                Z = shiftZ.ToString(),
                SSI = ssidec.ToString(),
                PI = pidec.ToString(),

                GalacticCoordinate = GalacticCoord2,
                PortalCode = pc
            };

            dest.LongHex = basehx;
            dest.DistanceToCenter = CoordCalculations.DistanceToCenter(dest.iX, dest.iY, dest.iZ);
            dest.GalaxyName = Globals.GalaxyLookup(dest.Galaxy);

            return dest;
        }
    }
}
