using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.DataStorageSystem
{
    public class StorageXML : IStorage
    {
        private string _filePath = Path.Combine(Application.streamingAssetsPath, "DataStorage.xml");

        public DataStorage Load()
        {
            var serializer = new XmlSerializer(typeof(DataStorage));

            using (var reader = new StreamReader(_filePath))
            {
                var data = serializer.Deserialize(reader) as DataStorage;
                return data;
            }
        }

        public void Save()
        {
            var serializer = new XmlSerializer(typeof(DataStorage));

            using (var reader = new StreamWriter(_filePath))
            {
                serializer.Serialize(reader, new DataStorage());
            }
        }
    }
}