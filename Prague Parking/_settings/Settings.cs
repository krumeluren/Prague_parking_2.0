using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prague_Parking_2_0_beta.Garage;

namespace Prague_Parking
{
   
    public class Settings
    {
        public string Currency { get; set; }
        public double BikePrice { get; set; }
        public double CarPrice { get; set; }
        public double MCPrice {get; set; }
        public double TruckPrice { get; set; }
        public double BussPrice { get; set; }
        public int FreeTime { get; set; }
        public Settings(){}
        #region Load
        /// <summary>
        /// Deserialize a settings object from settings.json
        /// </summary>
        static public Settings Load()
        {
            GarageSerializer g = new GarageSerializer();
            Settings settings =  g.JsonDeserializeSimple(typeof(Settings), "../../../_settings/settings.json") as Settings;
            return settings;
        }
        #endregion
        #region Save
        public void Save()
        {
            File.WriteAllText("../../../settings.json", JsonConvert.SerializeObject(this));
        }
        #endregion
    }
}
