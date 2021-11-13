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

        #region JsonSerialize
        /// <summary>
        /// Serialize a garage
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public void JsonSerialize( object data, string filePath)
        {

            // https://www.newtonsoft.com/json/help/html/preserveobjectreferences.htm
            // https://stackoverflow.com/questions/8513042/json-net-serialize-deserialize-derived-types

            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented,
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.All }));
        }
        #endregion

        #region JsonDeserialize
        /// <summary>
        /// Deserialize a park from /parks. Remembers references and derived class types
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public object JsonDeserialize(Type dataType, string filePath)
        {
            // https://www.newtonsoft.com/json/help/html/preserveobjectreferences.htm
            // https://stackoverflow.com/questions/8513042/json-net-serialize-deserialize-derived-types

            MyGarage Garage = (MyGarage)JsonConvert.DeserializeObject(File.ReadAllText(filePath),
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.All });

            return Garage;
        }
        #endregion

        #region JsonDeserializeTemplate
        /// <summary>
        /// Deserialize a template garage from GarageMaker/templates/
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="filePath"></param>
        /// <returns>A garage object with contents</returns>
        public object JsonDeserializeTemplate(Type dataType, string filePath)
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
            return obj.ToObject(dataType);
        }
        #endregion
    }
}