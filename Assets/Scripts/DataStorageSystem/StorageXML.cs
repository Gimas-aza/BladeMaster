using System.IO;
using System.Xml.Serialization;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.DataStorageSystem
{
    public class StorageXML : IStorage
    {
        private DataStorage _dataStorage = new();
        private readonly string _filePath;

        public StorageXML()
        {
#if UNITY_EDITOR
            _filePath = Path.Combine(Application.dataPath, "Saves", "DataStorage.xml");
#else 
            _filePath = Path.Combine(Application.dataPath, "DataStorage.xml");
#endif
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }

        public async UniTask<DataStorage> LoadAsync()
        {
            var serializer = new XmlSerializer(typeof(DataStorage));

            using (var reader = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
            {
                return await UniTask.RunOnThreadPool(() => TryDeserialize(serializer, reader));
            }
        }

        public async void SaveAsync()
        {
            var serializer = new XmlSerializer(typeof(DataStorage));

            using (var reader = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                await UniTask.RunOnThreadPool(() => serializer.Serialize(reader, _dataStorage)); 
            }
        }

        private DataStorage TryDeserialize(XmlSerializer serializer, FileStream reader)
        {
            try
            {
                var data = serializer.Deserialize(reader) as DataStorage;
                _dataStorage = data;
                return data;
            }
            catch (System.Exception)
            {
                return _dataStorage;
            }
        }
    }
}
