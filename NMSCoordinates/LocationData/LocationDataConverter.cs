using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSCoordinates.LocationData
{
    public class LocationDataConverter
    {
        public static string ConvertOldLocationFile(string locjson, int curversion, int latversion, string path)
        {
            // Version handling of save files make edits and change version
            if (curversion == 1)
            {
                locjson = locjson.Replace("\"name\"", "\"Name\"");
                locjson = locjson.Replace("\"locations\"", "\"Locations\"");
                locjson = locjson.Replace("\"bases\"", "\"TeleportEndpoints\"");
                locjson = locjson.Replace("\"name\"", "\"Name\"");
                locjson = locjson.Replace("\"details\"", "\"Details\"");
                locjson = locjson.Replace("\"datetime\"", "\"DateTime\"");
                locjson = locjson.Replace("\"saveslot\"", "\"SaveSlot\"");
                locjson = locjson.Replace("\"galaxy\"", "\"Galaxy\"");
                locjson = locjson.Replace("\"portalcode\"", "\"PortalCode\"");
                locjson = locjson.Replace("\"galacticcoords\"", "\"GalacticCoords\"");
                locjson = locjson.Replace("\"notes\"", "\"Notes\"");
                locjson = locjson.Replace("\"spacestations\"", "\"FutureUse\"");

                SavedLocationData loc = SavedLocationData.FromJson(locjson);
                loc.Version = latversion;

                for (int i = 0; i < loc.Locations.TeleportEndpoints.Length; i++)
                {
                    if (string.IsNullOrEmpty(loc.Locations.TeleportEndpoints[i].Details.DateTime))
                    {
                        loc.Locations.TeleportEndpoints[i].Details.DateTime = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
                    }
                    if (string.IsNullOrEmpty(loc.Locations.TeleportEndpoints[i].Details.LongHex))
                    {
                        int gal = loc.Locations.TeleportEndpoints[i].Details.Galaxy;
                        string pc = loc.Locations.TeleportEndpoints[i].Details.PortalCode;
                        loc.Locations.TeleportEndpoints[i].Details.LongHex = CoordCalculations.PortalToHex(gal, pc);
                    }
                }

                string newjson = Serialize.ToJson(loc);
                File.WriteAllText(path, newjson);

                return newjson;
            }

            return locjson;
        }
    }
}
