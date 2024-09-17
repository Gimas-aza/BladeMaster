using System.Collections.Generic;
using Assets.DataStorageSystem;
using Assets.Enemy;
using Assets.GameProgression;
using Assets.Knife;
using Assets.LevelManager;
using Assets.MVP;
using Assets.MVP.Model;
using Assets.Player;
using Assets.ShopManagement;
using Assets.GameSettings;
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
        private IInitializer _playerProgression;
        private IInitializer _settings;
        private List<IModel> _models;
        private DataStorage _dataStorage;

        private async void Awake()
        {
            DontDestroyOnLoad(this);

            _models = new List<IModel>();
            _objectFactory = new ObjectFactory.ObjectFactory();
            _loadSystem = new DataStorageSystem.DataStorageSystem(
                new DataStorageSystem.StorageXML()
            );
            _saveSystem = _loadSystem as ISaveSystem;
            _levelManager = new GameSceneManager();
            _presenter = new Presenter();
            _playerProgression = new PlayerProgression();
            _settings = new Settings();

            _dataStorage = await _loadSystem.LoadAsync();
            _settings.Init(_saveSystem, _dataStorage as ISettingsData);

            _models.Add(_levelManager as IModel);
            _models.Add(_playerProgression as IModel);
            _models.Add(_settings as IModel);

            _levelManager.LevelLoaded += OnLevelLoaded;
            _levelManager.LoadLevel(0);
        }

        private void OnLevelLoaded(int levelIndex)
        {
            var view = _objectFactory.CreateObject<View>() as IInitializer;
            var currentState = (levelIndex == 0) ? StateView.MainMenu : StateView.GameMenu;

            AddObjects(currentState, levelIndex);
            ClearList(_models);

            _presenter.Init(_models);
            view.Init(_presenter as Presenter, currentState);
        }

        private void AddObjects(StateView stateView, int levelIndex)
        {
            var dataStoragePlayer = _dataStorage as IPlayerProgressionData;
            switch (stateView)
            {
                case StateView.MainMenu:
                    var shop = _objectFactory.CreateObject<ShopComponent>() as IInitializer;

                    shop.Init(_saveSystem, _dataStorage as IShopData);
                    _playerProgression.Init(
                        shop as IShop,
                        _saveSystem,
                        ref dataStoragePlayer,
                        _levelManager as ILevelManager
                    );

                    _models.Add(shop as IModel);
                    break;
                case StateView.GameMenu:
                    var player = _objectFactory.CreateObject<PlayerComponent>() as IInitializer;
                    var knife = _objectFactory.CreateObject<KnifeComponent>();
                    var enemySpawner =
                        _objectFactory.CreateObject<EnemySpawnerComponent>() as IInitializer;

                    player.Init(knife.gameObject, _levelManager.GetLevelIndex() - 1);
                    enemySpawner.Init(levelIndex - 1);
                    _playerProgression.Init(
                        enemySpawner as ISpawnerEnemies,
                        player as IKnivesPool,
                        _levelManager as ILevelManager
                    );

                    _models.Add(player as IModel);
                    break;
            }
        }

        private void ClearList(List<IModel> list)
        {
            list.RemoveAll(x => x.ToString() == "null");
        }
    }
}
