using NMSCoordinates.LocationData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NMSCoordinates
{
    public partial class Globals
    {
        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        public static SavedLocationData CreateNewLocationJson(int version, int bs, int ss)
        {
            // Create new locbackup json file
            SavedLocationData newSDL = new SavedLocationData();
            newSDL.Version = version;
            newSDL.Locations = new Locations();
            newSDL.Locations.Bases = new Basis[bs];
            newSDL.Locations.Spacestations = new Basis[ss];
            return newSDL;
        }
        public static string GameModeLookupInt(int mode)
        {
            //Looks up game mode in ranges to prevent "not found"            
            if (mode > 4600 & mode < 4700)
            {
                return "Normal";
            }
            else if (mode > 5600 & mode < 5700)
            {
                return "Survival";
            }
            else if (mode > 6600 & mode < 6700)
            {
                return "Permadeath";
            }
            else if (mode > 5100 & mode < 5200)
            {
                return "Creative";
            }
            else
            {
                return "Unknown";
            }
        }
        public static string GalaxyLookup(string galaxy)
        {
            //lookup the galaxy and if not in galaxy dict, display the number
            IDictionary<string, string> galaxyDict = new Dictionary<string, string>();
            galaxyDict = GIndex();

            try
            {
                string value;
                if (galaxyDict.TryGetValue(galaxy, out value))
                {
                    return value;
                }
                else
                {
                    int g = Convert.ToInt32(galaxy);
                    if (g >= 0)
                    {
                        return (g + 1).ToString();
                    }
                    else
                    {
                        return g.ToString();
                    }
                }
                //source.Text = galaxyDict[galaxy];
            }
            catch
            {
                return "unknown";
                //source.Text = (Convert.ToInt32(galaxy) + 1).ToString();
                //Globals.AppendLine(textBox17, "Galaxy Not Found, update needed.");
            }
        }
        public static string CheckGalaxyType(string igalaxy)
        {
            int galaxy = Convert.ToInt32(igalaxy);
            var lushlist = new List<int>
            {
                9, 18, 29, 38, 49, 58, 69, 78, 89, 98, 109, 118,
                129, 138, 149, 158, 169, 178, 189, 198, 209, 218,
                229, 238, 249
            };

            var harshlist = new List<int>
            {
                2, 14, 22, 34, 42, 54, 62, 74, 82, 94, 102, 114, 122,
                134, 142, 154, 162, 174, 182, 194, 202, 214, 222, 234,
                242, 254
            };

            var emptylist = new List<int>
            {
                6, 11, 26, 31, 46, 51, 66, 71, 86, 91, 106, 111, 126,
                131, 146, 151, 166, 171, 186, 191, 206, 211, 226, 231,
                246, 251
            };

            if (lushlist.Contains(galaxy))
            {
                return "LUSH";
            }
            else if (harshlist.Contains(galaxy))
            {
                return "HARSH";
            }
            else if (emptylist.Contains(galaxy))
            {
                return "EMPTY";
            }
            else
            {
                return "NORMAL";
            }
        }
        public static string MakeUniqueSave(string path, int saveslot)
        {
            //Makes path in \backup\saves unique by date.time
            path = String.Format("{0}{1}{2}{3}{4}", @".\backup\saves\", Path.GetFileNameWithoutExtension(path), "_" + saveslot + "_", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"), Path.GetExtension(path));
            return path;
        }
        public static string MakeUniqueLoc(string path, int saveslot)
        {
            //Makes path in \backup\saves unique by date.time
            path = String.Format("{0}{1}{2}{3}{4}", @".\backup\locations\", Path.GetFileNameWithoutExtension(path), "_" + saveslot + "_", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"), Path.GetExtension(path));
            return path;
        }
        public static int Find(ListBox lb, string searchString, int startIndex)
        {
            //Find method for search bars on top of listboxes
            for (int i = startIndex; i < lb.Items.Count; ++i)
            {
                //string lbString = lb.Items[i].ToString();
                if (lb.Items[i].ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    return i;
            }
            return -1; //Find(lb, searchString, 0);
        }
        public static List<string> Contains(List<string> list1, List<string> list2)
        {
            //Compare two lists and send back differences
            List<string> result = new List<string>();

            result.AddRange(list1.Except(list2, StringComparer.OrdinalIgnoreCase));
            result.AddRange(list2.Except(list1, StringComparer.OrdinalIgnoreCase));

            return result;
        }
        public static Dictionary<char, Bitmap> Glyphs()
        {
            //Set the dictionary to find glyphs
            Dictionary<char, Bitmap> gldict = new Dictionary<char, Bitmap>();
            gldict.Add('0', Properties.Resources._0);
            gldict.Add('1', Properties.Resources._1);
            gldict.Add('2', Properties.Resources._2);
            gldict.Add('3', Properties.Resources._3);
            gldict.Add('4', Properties.Resources._4);
            gldict.Add('5', Properties.Resources._5);
            gldict.Add('6', Properties.Resources._6);
            gldict.Add('7', Properties.Resources._7);
            gldict.Add('8', Properties.Resources._8);
            gldict.Add('9', Properties.Resources._9);
            gldict.Add('A', Properties.Resources.A);
            gldict.Add('B', Properties.Resources.B);
            gldict.Add('C', Properties.Resources.C);
            gldict.Add('D', Properties.Resources.D);
            gldict.Add('E', Properties.Resources.E);
            gldict.Add('F', Properties.Resources.F);
            return gldict;
        }
        public static IDictionary<string, string> GMode()
        {
            //Set the dictionary for game modes
            IDictionary<string, string> gameMode = new Dictionary<string, string>();
            gameMode.Add(new KeyValuePair<string, string>("4631", "Normal"));
            gameMode.Add(new KeyValuePair<string, string>("5655", "Survival"));
            gameMode.Add(new KeyValuePair<string, string>("6679", "Permadeath"));
            gameMode.Add(new KeyValuePair<string, string>("5143", "Creative"));

            gameMode.Add(new KeyValuePair<string, string>("4628", "Normal"));
            gameMode.Add(new KeyValuePair<string, string>("5652", "Survival"));
            gameMode.Add(new KeyValuePair<string, string>("6676", "Permadeath"));
            //gameMode.Add(new KeyValuePair<string, string>("", "Creative"));

            gameMode.Add(new KeyValuePair<string, string>("4634", "Normal"));
            gameMode.Add(new KeyValuePair<string, string>("5658", "Survival"));
            gameMode.Add(new KeyValuePair<string, string>("6682", "Permadeath"));
            gameMode.Add(new KeyValuePair<string, string>("5146", "Creative"));

            return gameMode;
        }
        public static IDictionary<string, string> GIndex()
        {
            //Main dictionary for galaxies
            IDictionary<string, string> galaxyDict = new Dictionary<string, string>();
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
            galaxyDict.Add(new KeyValuePair<string, string>("26", "Eajerandal"));
            galaxyDict.Add(new KeyValuePair<string, string>("27", "Aldukesci"));
            galaxyDict.Add(new KeyValuePair<string, string>("28", "Wotyarogii"));
            galaxyDict.Add(new KeyValuePair<string, string>("29", "Sudzerbal"));
            galaxyDict.Add(new KeyValuePair<string, string>("30", "Maupenzhay"));
            galaxyDict.Add(new KeyValuePair<string, string>("31", "Sugueziume"));
            galaxyDict.Add(new KeyValuePair<string, string>("32", "Brogoweldian"));
            galaxyDict.Add(new KeyValuePair<string, string>("33", "Ehbogdenbu"));
            galaxyDict.Add(new KeyValuePair<string, string>("34", "Ijsenufryos"));
            galaxyDict.Add(new KeyValuePair<string, string>("35", "Nipikulha"));
            galaxyDict.Add(new KeyValuePair<string, string>("36", "Autsurabin"));
            galaxyDict.Add(new KeyValuePair<string, string>("37", "Lusontrygiamh"));
            galaxyDict.Add(new KeyValuePair<string, string>("38", "Rewmanawa"));
            galaxyDict.Add(new KeyValuePair<string, string>("39", "Ethiophodhe"));
            galaxyDict.Add(new KeyValuePair<string, string>("40", "Urastrykle"));
            galaxyDict.Add(new KeyValuePair<string, string>("41", "Xobeurindj"));
            galaxyDict.Add(new KeyValuePair<string, string>("42", "Oniijialdu"));
            galaxyDict.Add(new KeyValuePair<string, string>("43", "Wucetosucc"));
            galaxyDict.Add(new KeyValuePair<string, string>("44", "Ebyeloof"));
            galaxyDict.Add(new KeyValuePair<string, string>("45", "Odyavanta"));
            galaxyDict.Add(new KeyValuePair<string, string>("46", "Milekistri"));
            galaxyDict.Add(new KeyValuePair<string, string>("47", "Waferganh"));
            galaxyDict.Add(new KeyValuePair<string, string>("48", "Agnusopwit"));
            galaxyDict.Add(new KeyValuePair<string, string>("49", "Teyaypilny"));
            galaxyDict.Add(new KeyValuePair<string, string>("50", "Zalienkosm"));
            galaxyDict.Add(new KeyValuePair<string, string>("51", "Ladgudiraf"));
            galaxyDict.Add(new KeyValuePair<string, string>("52", "Mushonponte"));
            galaxyDict.Add(new KeyValuePair<string, string>("53", "Amsentisz"));
            galaxyDict.Add(new KeyValuePair<string, string>("54", "Fladiselm"));
            galaxyDict.Add(new KeyValuePair<string, string>("55", "Laanawemb"));
            galaxyDict.Add(new KeyValuePair<string, string>("56", "Ilkerloor"));
            galaxyDict.Add(new KeyValuePair<string, string>("57", "Davanossi"));
            galaxyDict.Add(new KeyValuePair<string, string>("58", "Ploehrliou"));
            galaxyDict.Add(new KeyValuePair<string, string>("59", "Corpinyaya"));
            galaxyDict.Add(new KeyValuePair<string, string>("60", "Leckandmeram"));
            galaxyDict.Add(new KeyValuePair<string, string>("61", "Quulngais"));
            galaxyDict.Add(new KeyValuePair<string, string>("62", "Nokokipsechl"));
            galaxyDict.Add(new KeyValuePair<string, string>("63", "Rinblodesa"));
            galaxyDict.Add(new KeyValuePair<string, string>("64", "Loydporpen"));
            galaxyDict.Add(new KeyValuePair<string, string>("65", "Ibtrevskip"));
            galaxyDict.Add(new KeyValuePair<string, string>("66", "Elkowaldb"));
            galaxyDict.Add(new KeyValuePair<string, string>("67", "Heholhofsko"));
            galaxyDict.Add(new KeyValuePair<string, string>("68", "Yebrilowisod"));
            galaxyDict.Add(new KeyValuePair<string, string>("69", "Husalvangewi"));
            galaxyDict.Add(new KeyValuePair<string, string>("70", "Ovna'uesed"));
            galaxyDict.Add(new KeyValuePair<string, string>("71", "Bahibusey"));
            galaxyDict.Add(new KeyValuePair<string, string>("72", "Nuybeliaure"));
            galaxyDict.Add(new KeyValuePair<string, string>("73", "Doshawchuc"));
            galaxyDict.Add(new KeyValuePair<string, string>("74", "Ruckinarkh"));
            galaxyDict.Add(new KeyValuePair<string, string>("75", "Thorettac"));
            galaxyDict.Add(new KeyValuePair<string, string>("76", "Nuponoparau"));
            galaxyDict.Add(new KeyValuePair<string, string>("77", "Moglaschil"));
            galaxyDict.Add(new KeyValuePair<string, string>("78", "Uiweupose"));
            galaxyDict.Add(new KeyValuePair<string, string>("79", "Nasmilete"));
            galaxyDict.Add(new KeyValuePair<string, string>("80", "Ekdaluskin"));
            galaxyDict.Add(new KeyValuePair<string, string>("81", "Hakapanasy"));
            galaxyDict.Add(new KeyValuePair<string, string>("82", "Dimonimba"));
            galaxyDict.Add(new KeyValuePair<string, string>("83", "Cajaccari"));
            galaxyDict.Add(new KeyValuePair<string, string>("84", "Olonerovo"));
            galaxyDict.Add(new KeyValuePair<string, string>("85", "Umlanswick"));
            galaxyDict.Add(new KeyValuePair<string, string>("86", "Henayliszm"));
            galaxyDict.Add(new KeyValuePair<string, string>("87", "Utzenmate"));
            galaxyDict.Add(new KeyValuePair<string, string>("88", "Umirpaiya"));
            galaxyDict.Add(new KeyValuePair<string, string>("89", "Paholiang"));
            galaxyDict.Add(new KeyValuePair<string, string>("90", "Iaereznika"));
            galaxyDict.Add(new KeyValuePair<string, string>("91", "Yudukagath"));
            galaxyDict.Add(new KeyValuePair<string, string>("92", "Boealalosnj"));
            galaxyDict.Add(new KeyValuePair<string, string>("93", "Yaevarcko"));
            galaxyDict.Add(new KeyValuePair<string, string>("94", "Coellosipp"));
            galaxyDict.Add(new KeyValuePair<string, string>("95", "Wayndohalou"));
            galaxyDict.Add(new KeyValuePair<string, string>("96", "Smoduraykl"));
            galaxyDict.Add(new KeyValuePair<string, string>("97", "Apmaneessu"));
            galaxyDict.Add(new KeyValuePair<string, string>("98", "Hicanpaav"));
            galaxyDict.Add(new KeyValuePair<string, string>("99", "Akvasanta"));
            galaxyDict.Add(new KeyValuePair<string, string>("100", "Tuychelisaor"));
            galaxyDict.Add(new KeyValuePair<string, string>("109", "Nudquathsenfe"));
            galaxyDict.Add(new KeyValuePair<string, string>("118", "Torweierf"));
            galaxyDict.Add(new KeyValuePair<string, string>("129", "Broomerrai"));
            galaxyDict.Add(new KeyValuePair<string, string>("138", "Emiekereks"));
            galaxyDict.Add(new KeyValuePair<string, string>("140", "Kimycuristh"));
            galaxyDict.Add(new KeyValuePair<string, string>("149", "Zavainlani"));
            galaxyDict.Add(new KeyValuePair<string, string>("158", "Rycempler"));
            galaxyDict.Add(new KeyValuePair<string, string>("169", "Ezdaranit"));
            galaxyDict.Add(new KeyValuePair<string, string>("178", "Wepaitvas"));
            galaxyDict.Add(new KeyValuePair<string, string>("189", "Cugnatachh"));
            galaxyDict.Add(new KeyValuePair<string, string>("198", "Horeroedsh"));
            galaxyDict.Add(new KeyValuePair<string, string>("209", "Digarlewena"));
            galaxyDict.Add(new KeyValuePair<string, string>("218", "Chmageaki"));
            galaxyDict.Add(new KeyValuePair<string, string>("229", "Raldwicarn"));
            galaxyDict.Add(new KeyValuePair<string, string>("238", "Yuwarugha"));
            galaxyDict.Add(new KeyValuePair<string, string>("249", "Nepitzaspru"));
            galaxyDict.Add(new KeyValuePair<string, string>("254", "Iousongola"));
            galaxyDict.Add(new KeyValuePair<string, string>("255", "Odyalutai"));

            //galaxyDict.Add(new KeyValuePair<string, string>("256", "Yilsrussimil"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-1", "Pequibanu"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-2", "Uewamoisow"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-3", "Hiteshamij"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-4", "Usgraikik"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-5", "Helqvishap"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-6", "Enyokudohkiw"));
            //galaxyDict.Add(new KeyValuePair<string, string>("-7", "Loqvishess"));

            //galaxyDict.Add(new KeyValuePair<string, string>("", ""));

            return galaxyDict;
        }
    }
}
