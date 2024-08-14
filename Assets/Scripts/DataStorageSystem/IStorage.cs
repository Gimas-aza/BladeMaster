using Cysharp.Threading.Tasks;

namespace Assets.DataStorageSystem
{
    public interface IStorage
    {
        UniTask<DataStorage> LoadAsync();
        void SaveAsync();
    }
}