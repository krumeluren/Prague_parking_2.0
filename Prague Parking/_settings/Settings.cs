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
        public double Bike_Price { get; set; }
        public double Car_Price { get; set; }
        public double MC_Price {get; set; }
        public double Truck_Price { get; set; }
        public double Buss_Price { get; set; }
        public int Free_Time { get; set; }

        public int Size_Per_Lot { get; set; }
        public int Bike_Size { get; set; }
        public int MC_Size { get; set; }
        public int Car_Size { get; set; }
        public int Truck_Size { get; set; }
        public int Buss_Size { get; set; }

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
