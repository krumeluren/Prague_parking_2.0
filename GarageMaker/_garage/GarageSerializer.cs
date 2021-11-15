using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Prague_Parking_2_0_beta.Garage
{
    class GarageSerializer
    {

        #region JsonSerialize
        public void JsonSerialize(object data, string filePath)
        {

            // https://www.newtonsoft.com/json/help/html/preserveobjectreferences.htm
            // https://stackoverflow.com/questions/8513042/json-net-serialize-deserialize-derived-types

            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented,
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.All }));
        }
        #endregion

        #region JsonDeserialize
        public object JsonDeserialize(Type dataType, string filePath)
        {
            // https://www.newtonsoft.com/json/help/html/preserveobjectreferences.htm
            // https://stackoverflow.com/questions/8513042/json-net-serialize-deserialize-derived-types

            Garage Garage = (Garage)JsonConvert.DeserializeObject(File.ReadAllText(filePath),
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.All });

            return Garage;
        }
        #endregion
    }
}