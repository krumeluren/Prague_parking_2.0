using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    class GarageSerializer
    {
        public MyGarage Garage { get; set; }

        #region BinarySerialize(object, filePath)
        public void BinarySerialize(object data, string filePath)
        {
            FileStream fileStream;
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(filePath)) File.Delete(filePath);
            fileStream = File.Create(filePath);
            bf.Serialize(fileStream, data);
            fileStream.Close();
        }
        #endregion
        #region BinaryDeserialize(filePath)
        public object BinaryDeserialize(string filePath)
        {
            object obj = null;
            FileStream fileStream;
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(filePath))
            {
                fileStream = File.OpenRead(filePath);
                obj = bf.Deserialize(fileStream);
                fileStream.Close();
            }
            return obj;
        }
        #endregion

        #region XmlSerialize(Type, object, filepath)
        public void XmlSerialize(Type dataType, object data, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(dataType);
            if (File.Exists(filePath)) File.Delete(filePath);
            TextWriter writer = new StreamWriter(filePath);
            xmlSerializer.Serialize(writer, data);
            writer.Close();
        }
        #endregion
        #region XmlDeserialize(Type, filepath)
        public object XmlDeserialize(Type dataType,  string filePath)
        {
            object obj = null;

            XmlSerializer xmlSerializer = new XmlSerializer(dataType);
            if (File.Exists(filePath))
            {
                TextReader textReader = new StreamReader(filePath);
                obj = xmlSerializer.Deserialize(textReader);
                textReader.Close();
            }
            return obj;
        }
        #endregion

        #region JsonSerialize
        public void JsonSerialize( object data, string filePath)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
        }
        #endregion
        #region JsonDeserialize
        public object JsonDeserialize(Type dataType, string filePath)
        {
            JObject obj = null;
            JsonSerializer jsonSerializer = new JsonSerializer();
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                JsonReader jsonReader = new JsonTextReader(sr);
                
                obj = jsonSerializer.Deserialize(jsonReader) as JObject;
                
                jsonReader.Close();
                sr.Close();
            }
            Garage = (MyGarage)obj.ToObject(dataType);
            // JSON load child class as parent class. This code goes through all json vehicles objects and recreates them with correct child class
            for (int i = 0; i < Garage.Locations.Count; i++)
            {
                for (int ii = 0; ii < Garage.Locations[i].Rows.Count; ii++)
                {
                    for (int iii = 0; iii < Garage.Locations[i].Rows[ii].Lots.Length; iii++)
                    {
                        for (int iv = 0; iv < Garage.Locations[i].Rows[ii].Lots[iii].Vehicles.Count; iv++)
                        {
                            if(Garage.Locations[i].Rows[ii].Lots[iii].Vehicles[iv].Type == "Car")
                            {
                                List<Vehicle> vehicles = Garage.Locations[i].Rows[ii].Lots[iii].Vehicles;
                                for (int v = 0; v < vehicles.Count; v++)
                                {
                                    vehicles[v] = new Car(vehicles[v].Arrival, vehicles[v].Heigth, vehicles[v].Id, vehicles[v].Color, vehicles[v].Electric);
                                }
                            }
                            if (Garage.Locations[i].Rows[ii].Lots[iii].Vehicles[iv].Type == "MC")
                            {
                                List<Vehicle> vehicles = Garage.Locations[i].Rows[ii].Lots[iii].Vehicles;
                                for (int v = 0; v < vehicles.Count; v++)
                                {
                                    vehicles[v] = new MC(vehicles[v].Arrival, vehicles[v].Heigth, vehicles[v].Id, vehicles[v].Color, vehicles[v].Electric);
                                }
                            }
                            if (Garage.Locations[i].Rows[ii].Lots[iii].Vehicles[iv].Type == "Truck")
                            {
                                List<Vehicle> vehicles = Garage.Locations[i].Rows[ii].Lots[iii].Vehicles;
                                for (int v = 0; v < vehicles.Count; v++)
                                {
                                    vehicles[v] = new Truck(vehicles[v].Arrival, vehicles[v].Heigth, vehicles[v].Id, vehicles[v].Color, vehicles[v].Electric);
                                }
                            }
                            if (Garage.Locations[i].Rows[ii].Lots[iii].Vehicles[iv].Type == "Bike")
                            {
                                List<Vehicle> vehicles = Garage.Locations[i].Rows[ii].Lots[iii].Vehicles;
                                for (int v = 0; v < vehicles.Count; v++)
                                {
                                    vehicles[v] = new Bike(vehicles[v].Arrival, vehicles[v].Heigth, vehicles[v].Color);
                                }
                            }
                        }
                    }
                }
            }
            return Garage;
        }
        #endregion
    }
}