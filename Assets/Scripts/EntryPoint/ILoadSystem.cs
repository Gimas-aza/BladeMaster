using Assets.DataStorageSystem;
using Cysharp.Threading.Tasks;

namespace Assets.EntryPoint
{
    public interface ILoadSystem 
    {
        UniTask<DataStorage> LoadAsync();
    }
}
