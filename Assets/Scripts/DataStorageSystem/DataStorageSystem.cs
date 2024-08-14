using Assets.EntryPoint;
using Cysharp.Threading.Tasks;

namespace Assets.DataStorageSystem
{
    public class DataStorageSystem : IDataStorageSystem
    {
        private IStorage _storage;

        public DataStorageSystem(IStorage storage)
        {
            _storage = storage;
        }

        public async UniTask<DataStorage> LoadAsync()
        {
            return await _storage.LoadAsync();
        }

        public void SaveAsync()
        {
            _storage.SaveAsync();
        }
    }
}