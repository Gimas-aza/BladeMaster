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
using Assets.Sounds;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Assets.DI;

namespace Assets.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        private DIContainer _container;
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

            InitializeFields();
            await LoadDataAsync();
            InitializeSettings();
            SubscribeToLevelManagerEvents();
            LoadMainMenuLevel();
        }

        private void InitializeFields()
        {
            _container = new DIContainer();
            _models = new List<IModel>();
            _objectFactory = new ObjectFactory.ObjectFactory();
            _loadSystem = new DataStorageSystem.DataStorageSystem(new DataStorageSystem.StorageXML());
            _saveSystem = _loadSystem as ISaveSystem;
            _levelManager = new GameSceneManager();
            _presenter = new Presenter();
            _playerProgression = new PlayerProgression();
            _settings = new Settings();
        }

        private async UniTask LoadDataAsync()
        {
            _dataStorage = await _loadSystem.LoadAsync();
        }

        private void InitializeSettings()
        {
            _settings.Init(_saveSystem, _dataStorage as ISettingsData);
            AddModels();
        }

        private void AddModels()
        {
            _models.Add(_levelManager as IModel);
            _models.Add(_playerProgression as IModel);
            _models.Add(_settings as IModel);
        }

        private void SubscribeToLevelManagerEvents()
        {
            _levelManager.LevelLoaded += OnLevelLoaded;
        }

        private void LoadMainMenuLevel()
        {
            _levelManager.LoadLevel(0);
        }

        private void OnLevelLoaded(int levelIndex)
        {
            var view = _objectFactory.CreateObject<View>() as IInitializer;
            var currentState = (levelIndex == 0) ? StateView.MainMenu : StateView.GameMenu;

            AddObjects(currentState, levelIndex);
            ClearModels();

            _presenter.Init(_models);
            view.Init(_presenter as Presenter, currentState);
        }

        private void AddObjects(StateView stateView, int levelIndex)
        {
            var dataStoragePlayer = _dataStorage as IPlayerProgressionData;
            var audio = _objectFactory.CreateObject<AudioComponent>() as IInitializer;

            audio.Init(stateView);

            switch (stateView)
            {
                case StateView.MainMenu:
                    AddMainMenuObjects(dataStoragePlayer, levelIndex);
                    break;
                case StateView.GameMenu:
                    AddGameMenuObjects(levelIndex);
                    break;
            }

            _settings.Init(audio as AudioComponent);
        }

        private void AddMainMenuObjects(IPlayerProgressionData dataStoragePlayer, int levelIndex)
        {
            var shop = _objectFactory.CreateObject<ShopComponent>() as IInitializer;
            shop.Init(_saveSystem, _dataStorage as IShopData);

            _playerProgression.Init(
                shop as IShop,
                _saveSystem,
                ref dataStoragePlayer,
                levelIndex
            );

            _models.Add(shop as IModel);
        }

        private void AddGameMenuObjects(int levelIndex)
        {
            var player = _objectFactory.CreateObject<PlayerComponent>() as IInitializer;
            var knife = _objectFactory.CreateObject<KnifeComponent>();
            var enemySpawner = _objectFactory.CreateObject<EnemySpawnerComponent>() as IInitializer;

            player.Init(knife.gameObject, levelIndex - 1);
            enemySpawner.Init(levelIndex - 1);

            _playerProgression.Init(
                enemySpawner as ISpawnerEnemies,
                player as IKnivesPool,
                levelIndex
            );

            _models.Add(player as IModel);
        }

        private void ClearModels()
        {
            _models.RemoveAll(x => x.ToString() == "null");
        }
    }
}
