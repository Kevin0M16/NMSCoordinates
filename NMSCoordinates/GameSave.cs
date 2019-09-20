using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NMSCoordinates
{
    public class GameSave
    {
        public enum PlayerStates
        {
            Unknown,
            InShip,
            OnFoot
                // TODO: What are the other states?
        }

        public enum GameModes
        {
            Unknown,
            Normal,
            Creative,
            Survival,
            Permadeath
        }

        #region Public Constants
        public const int NumContainers = 10;
        public const int MaxHealth = 8;
        public const int MaxLifeSupport = 100;
        public const int MaxExosuitShield = 100;
        public const int MaxShipShield = 200;
        public const int MaxShipHealth = 8;
        public const int MaxUnits = int.MaxValue;
        #endregion

        #region Private Member Variables
        private dynamic _json;        
        private Random _random = new Random();
        #endregion

        #region Public Constructors

        public GameSave(string jsonStr)
        {
            _json = JObject.Parse(jsonStr);
        }

        #endregion

        #region Public Properties

        public int Version
        {
            get
            {
                return (int)_json.Version;
            }
        }

        public GameModes GameMode
        {
            get
            {
                return VersionToGameMode(Version);
            }
        }

        public string Platform
        {
            get
            {
                return _json.Platform;
            }
        }

        public int PlayerHealth
        {
            get
            {
                return _json.PlayerStateData.Health;
            }

            set
            {
                if (value < 1 || value > MaxHealth)
                {
                    throw new ArgumentException(string.Format("Invalid value for player health: {0}. Health must be in the range [{1},{2}]", value, 1, MaxHealth));
                }

                _json.PlayerStateData.Health = value;
            }
        }

        public int LifeSupport
        {
            get
            {
                return _json.PlayerStateData.Energy;
            }

            set
            {
                if (value < 1 || value > MaxLifeSupport)
                {
                    throw new ArgumentException(string.Format("Invalid value for life support: {0}. Life support must be in the range [{1},{2}]", value, 1, MaxLifeSupport));
                }

                _json.PlayerStateData.Energy = value;
            }
        }

        public int ExosuitShield
        {
            get
            {
                return _json.PlayerStateData.Shield;
            }

            set
            {
                if (value < 1 || value > MaxLifeSupport)
                {
                    throw new ArgumentException(string.Format("Invalid value for exosuit shield: {0}. Exosuit shield level must be in the range [{1},{2}]", value, 1, MaxExosuitShield));
                }

                _json.PlayerStateData.Shield = value;
            }
        }

        public int ShipHealth
        {
            get
            {
                return _json.PlayerStateData.ShipHealth;
            }

            set
            {
                if (value < 1 || value > MaxShipHealth)
                {
                    throw new ArgumentException(string.Format("Invalid value for ship health: {0}. Ship health must be in the range [{1},{2}]", value, 1, MaxShipHealth));
                }
            }
        }

        public int ShipShield
        {
            get
            {
                return _json.PlayerStateData.ShipShield;
            }

            set
            {
                if (value < 1 || value > MaxShipShield)
                {
                    throw new ArgumentException(string.Format("Invalid value for ship shield: {0}. Ship shield must be in the range [{1},{2}]", value, 1, MaxShipShield));
                }
            }
        }

        public int Units
        {
            get
            {
                return _json.PlayerStateData.Units;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(string.Format("Invalid value for units: {0}. Units must be in the range [{1},{2}]", value, 0, MaxUnits));
                }
                _json.PlayerStateData.Units = value;
            }
        }

        

        public int PlayerGalaxy
        {
            get
            {
                return _json.PlayerStateData.UniverseAddress.RealityIndex;
            }

            set
            {
                if (value != (int)_json.PlayerStateData.UniverseAddress.RealityIndex)
                {
                    _json.PlayerStateData.UniverseAddress.RealityIndex = value;
                }                
            }
        }

        public int PlayerPlanet
        {
            get
            {
                return _json.PlayerStateData.UniverseAddress.GalacticAddress.PlanetIndex;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(string.Format("Invalid planet index: {0}", value));
                }

                _json.PlayerStateData.UniverseAddress.GalacticAddress.PlanetIndex = value;
            }
            //_json.SpawnStateData.LastKnownPlayerState = "InShip";
        }

        public PlayerStates PlayerState
        {
            get
            {
                PlayerStates result;
                if (Enum.TryParse<PlayerStates>(_json.SpawnStateData.LastKnownPlayerState, true, out result))
                {
                    return result;
                }

                return PlayerStates.Unknown;
            }

            set
            {
                _json.SpawnStateData.LastKnownPlayerState = value.ToString();
            }            
        }


        public ulong ShipSeed
        {
            get
            {
                return Convert.ToUInt64((string)PrimaryShipNode.Resource.Seed[1], 16);
            }

            set
            {
                PrimaryShipNode.Resource.Seed[1] = string.Format("0x{0:X16}", value);
            }
        }

        public ulong MultitoolSeed
        {
            get
            {
                return Convert.ToUInt64((string)_json.PlayerStateData.CurrentWeapon.GenerationSeed[1], 16);
            }

            set
            {                
                _json.PlayerStateData.CurrentWeapon.GenerationSeed[1] = string.Format("0x{0:X16}", value);
            }
        }

        public ulong FreighterSeed
        {
            get
            {
                return Convert.ToUInt64((string)_json.PlayerStateData.CurrentFreighter.Seed[1], 16);
            }

            set
            {
                _json.PlayerStateData.CurrentFreighter.Seed[1] = string.Format("0x{0:X16}", value);
            }
        }


        #endregion

        #region Public Methods

        public void SetMaxPlayerHealth()
        {
            PlayerHealth = MaxHealth;
        }

        public void SetMaxLifeSupport()
        {
            LifeSupport = MaxLifeSupport;
        }

        public void SetMaxExosuitShield()
        {
            ExosuitShield = MaxExosuitShield;
        }

        public void SetMaxShipHealth()
        {
            ShipHealth = MaxShipHealth;
        }

        public void SetMaxShipShield()
        {
            ShipShield = MaxShipShield;
        }

        public void SetMaxUnits()
        {
            Units = MaxUnits;
        }

        public void AddUnits(int unitsDelta)
        {
            long newUnits = Units + unitsDelta;
            newUnits = Math.Max(0, Math.Min(int.MaxValue, newUnits));
            Units = (int)newUnits;
        }

        public ulong RandomizeShipSeed()
        {
            byte[] randBytes = new byte[8];
            _random.NextBytes(randBytes);
            ulong seed = BitConverter.ToUInt64(randBytes, 0);
            ShipSeed = seed;
            return seed;
        }

        public ulong RandomizeMultitoolSeed()
        {
            byte[] randBytes = new byte[8];
            _random.NextBytes(randBytes);
            ulong seed = BitConverter.ToUInt64(randBytes, 0);
            MultitoolSeed = seed;
            return seed;
        }

        public ulong RandomizeFreighterSeed()
        {
            byte[] randBytes = new byte[8];
            _random.NextBytes(randBytes);
            ulong seed = BitConverter.ToUInt64(randBytes, 0);
            FreighterSeed = seed;
            return seed;
        }

        public void RepairAll()
        {
            SetMaxPlayerHealth();
            SetMaxExosuitShield();
            SetMaxShipHealth();
            SetMaxShipShield();
        }

        public void RechargeAll()
        {
            SetMaxLifeSupport();
        }


        public string ToFormattedJsonString()
        {
            string json = JsonConvert.SerializeObject(_json, Formatting.Indented);
            return json;
        }

        public string ToUnformattedJsonString()
        {
            string json = JsonConvert.SerializeObject(_json, Formatting.None);
            return json;
        }

        #endregion

        #region Private Methods and Properties

        private dynamic PrimaryShipNode
        {
            get
            {
                int primaryShipIndex = _json.PlayerStateData.PrimaryShip;
                return _json.PlayerStateData.ShipOwnership[primaryShipIndex];
            }
        }

        private dynamic PrimaryVehicleNode
        {
            get
            {
                int primaryVehicleIndex = _json.PlayerStateData.PrimaryVehicle;
                return _json.PlayerStateData.VehicleOwnership[primaryVehicleIndex];
            }
        }

        private static GameModes VersionToGameMode(int version)
        {
            switch (version)
            {
                case 4616: return GameModes.Normal;
                case 5128: return GameModes.Creative;
                case 5640: return GameModes.Survival;
                case 6664: return GameModes.Permadeath;
                default: return GameModes.Unknown;
            }
        }

        private static int GameModeToVersion(GameModes gameMode)
        {
            switch(gameMode)
            {
                case GameModes.Normal: return 4616;
                case GameModes.Creative: return 5128;
                case GameModes.Survival: return 5640;
                case GameModes.Permadeath: return 6664;
                default: throw new ArgumentException("Unknown game mode");
            }
        }

        #endregion

    }
}
