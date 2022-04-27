using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data
{
    public class JsonManager<T>
    {
        public List<T> GetCollection()
        {
            string collectionPath = $"{Directory.GetCurrentDirectory()}\\Data\\{typeof(T)}.json";
            var myCollection = new List<T>();

            if (File.Exists(collectionPath))
            {
                var streamReader = new StreamReader(collectionPath);
                string currentContent = streamReader.ReadToEnd();
                myCollection = JsonConvert.DeserializeObject<List<T>>(currentContent);
                streamReader.Close();
            }
            else
            {
                var jsonCollection = JsonConvert.SerializeObject(myCollection, Formatting.Indented);
                var streamWriter = new StreamWriter(collectionPath);
                streamWriter.WriteLine(jsonCollection);
                streamWriter.Close();
            }

            return myCollection;
        }

        public bool SaveCollection(List<T> collection)
        {
            string collectionPath = $"{Directory.GetCurrentDirectory()}\\Data\\{typeof(T)}.json";

            try
            {
                var jsonCollection = JsonConvert.SerializeObject(collection, Formatting.Indented);
                var streamWriter = new StreamWriter(collectionPath);
                streamWriter.WriteLine(jsonCollection);
                streamWriter.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
