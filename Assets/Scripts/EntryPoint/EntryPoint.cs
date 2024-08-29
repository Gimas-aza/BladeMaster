using System.Collections.Generic;
using Assets.Knife;
using Assets.LevelManager;
using Assets.MVP;
using Assets.MVP.Model;
using Assets.Player;
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

            _levelManager.LevelLoaded += OnLevelLoaded;
            _levelManager.LoadLevel(2);
        }

        private void OnLevelLoaded(int levelIndex)
        {
            var view = _objectFactory.CreateObject<View>() as IInitializer;
            var currentState = (levelIndex == 1) ? StateView.MainMenu : StateView.GameMenu;

            AddObjects(currentState);
            ClearList(_models);

            _presenter.Init(_models);
            view.Init(_presenter as Presenter, currentState);
        }

        private void AddObjects(StateView stateView)
        {
            switch (stateView)
            {
                case StateView.GameMenu:
                    var player = _objectFactory.CreateObject<PlayerComponent>() as IModel;
                    var playerInit = player as IInitializer;
                    var knife = _objectFactory.CreateObject<KnifeComponent>();
                    playerInit.Init(knife.gameObject);
                    
                    _models.Add(player);
                    break;
            }
        }

        private void ClearList(List<IModel> list)
        {
            list.RemoveAll(x => x.ToString() == "null");
        }
    }
}
