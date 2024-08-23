using System.Collections.Generic;
using Assets.LevelManager;
using Assets.MVP;
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
        private IInitializer _presenter;
        private List<IModel> _models;
        private IModel _modelLevelManager;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _models = new List<IModel>();
            _objectFactory = new ObjectFactory.ObjectFactory();
            _saveSystem = new DataStorageSystem.DataStorageSystem(new DataStorageSystem.StorageXML());
            _saveSystem = _loadSystem as ISaveSystem;
            _levelManager = new GameSceneManager();
            _presenter = new Presenter();

            _models.Add(_levelManager as IModel);

            _levelManager.LoadLevel(1);
            _levelManager.LevelLoaded += OnLevelLoaded;
        }

        private void OnLevelLoaded()
        {
            var view = _objectFactory.CreateObject<View>() as IInitializer;
            _presenter.Init(_models);
            view.Init(_presenter as Presenter);
        }
    }
}
