using NMSCoordinates.SaveData;
using System;
using System.Windows.Forms;

namespace NMSCoordinates
{
    public class Coordinates
    {
        public struct Player
        {
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

            dest.GalacticCoordinate = CoordCalculations.VoxelToGalacticCoord(dest.iX, dest.iY, dest.iZ, dest.iSSI);
            dest.PortalCode = CoordCalculations.VoxelToPortal(dest.iPI, dest.iX, dest.iY, dest.iZ, dest.iSSI);
            dest.DistanceToCenter = CoordCalculations.DistanceToCenter(dest.iX, dest.iY, dest.iZ);
            dest.GalaxyName = Globals.GalaxyLookup(dest.Galaxy);

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
                Galaxy = galaxy.ToString(),
                iX = vX,
                iY = vY,
                iZ = vZ,
                iSSI = icSSI,

                iGalaxy = galaxy,
                X = vX.ToString(),
                Y = vY.ToString(),
                Z = vZ.ToString(),
                SSI = icSSI.ToString()
            };

            return dest;
        }
    }
}
