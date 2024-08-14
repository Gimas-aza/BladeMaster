using UnityEngine;

namespace Assets.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        private readonly IObject _objectFactory = new ObjectFactory.ObjectFactory();
        private readonly IDataStorageSystem _dataStorageSystem =
            new DataStorageSystem.DataStorageSystem(new DataStorageSystem.StorageXML());
        private ILoadSystem _loadSystem;
        private ISaveSystem _saveSystem;

        private async void Awake()
        {
            _loadSystem = _dataStorageSystem;
            _saveSystem = _dataStorageSystem;
        }
    }
}
