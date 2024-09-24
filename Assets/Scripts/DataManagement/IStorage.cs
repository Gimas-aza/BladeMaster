using Cysharp.Threading.Tasks;

namespace Assets.DataManagement
{
    public interface IStorage
    {
        UniTask<DataStorage> LoadAsync();
        void SaveAsync();
    }
}