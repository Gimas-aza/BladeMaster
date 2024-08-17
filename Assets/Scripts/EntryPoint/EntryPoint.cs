using Assets.LevelManager;
using Assets.MVP.Model;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        private IObject _objectFactory;
        private ILoadSystem _loadSystem; 
        private ISaveSystem _saveSystem;
        private ILevelManager _levelManager;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _objectFactory = new ObjectFactory.ObjectFactory();
            _saveSystem = new DataStorageSystem.DataStorageSystem(new DataStorageSystem.StorageXML());
            _saveSystem = _loadSystem as ISaveSystem;
            _levelManager = new GameSceneManager();

            _levelManager.LoadLevel(1);
        }
    }
}
