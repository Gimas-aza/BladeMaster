using System.IO;
using System.Threading;
using System.Xml.Serialization;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.DataManagement
{
    public class StorageXML : IStorage
    {
        private DataStorage _dataStorage = new();
        private readonly string _filePath;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public StorageXML()
        {
#if UNITY_EDITOR
            _filePath = Path.Combine(Application.dataPath, "Saves", "DataStorage.xml");
#else
            _filePath = Path.Combine(Application.persistentDataPath, "DataStorage.xml");
#endif
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }

        public async UniTask<DataStorage> LoadAsync()
        {
            var serializer = new XmlSerializer(typeof(DataStorage));

            using (
                var reader = new FileStream(
                    _filePath,
                    FileMode.OpenOrCreate,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    FileOptions.Asynchronous
                )
            )
            {
                return await UniTask.RunOnThreadPool(() => DeserializeData(serializer, reader));
            }
        }

        public async void SaveAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var serializer = new XmlSerializer(typeof(DataStorage));

                using (
                    var stream = new FileStream(
                        _filePath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        4096,
                        FileOptions.Asynchronous
                    )
                )
                {
                    await UniTask.RunOnThreadPool(() => serializer.Serialize(stream, _dataStorage));
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private DataStorage DeserializeData(XmlSerializer serializer, FileStream reader)
        {
            try
            {
                var data = serializer.Deserialize(reader) as DataStorage;
                _dataStorage = data ?? new DataStorage();
                return _dataStorage;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Deserialization error: {ex.Message}");
                return _dataStorage;
            }
        }
    }
}
