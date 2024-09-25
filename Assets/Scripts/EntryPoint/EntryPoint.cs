using Assets.DataManagement;
using Assets.Enemy;
using Assets.GameProgression;
using Assets.Knife;
using Assets.LevelManager;
using Assets.MVP;
using Assets.MVP.Model;
using Assets.Player;
using Assets.ShopManagement;
using Assets.GameSettings;
using Assets.Audio;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Assets.DI;
using Assets.ObjectCreation;

namespace Assets.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        private IDIContainer _container;
        private IObject _objectFactory;
        private ILoadSystem _loadSystem;
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
            AddModels();
            SubscribeToLevelManagerEvents();
            LoadMainMenuLevel();
        }

        private void OnDestroy()
        {
            UnsubscribeFromLevelManagerEvents();
        }

        private void InitializeFields()
        {
            _container = new DIContainer();
            _models = new List<IModel>();
            _objectFactory = new ObjectFactory();
            _loadSystem = new DataStorageSystem(new StorageXML());
            _levelManager = new GameSceneManager();
            _presenter = new Presenter();
            _playerProgression = new PlayerProgression();
            _settings = new Settings();

            RegisterSingletons();
        }

        private void RegisterSingletons()
        {
            _container.RegisterSingleton(_ => _loadSystem as ISaveSystem);
            _container.RegisterSingleton(_ => _presenter as Presenter);
            _container.RegisterSingleton(_ => _levelManager as ILevelInfoProvider);
            _container.RegisterTransient(c => 
                c.Resolve<ILevelInfoProvider>().GetLevelIndex() == 0 ? StateGame.MainMenu : StateGame.GameMenu);
        }

        private async UniTask LoadDataAsync()
        {
            _dataStorage = await _loadSystem.LoadAsync();
            RegisterDataStorage();
        }

        private void RegisterDataStorage()
        {
            _container.RegisterSingleton(_ => _dataStorage as ISettingsData);
            _container.RegisterSingleton(_ => _dataStorage as IPlayerProgressionData);
            _container.RegisterSingleton(_ => _dataStorage as IShopData);
        }

        private void AddModels()
        {
            _models.AddRange(new IModel[] { _levelManager as IModel, _playerProgression as IModel, _settings as IModel });
        }

        private void SubscribeToLevelManagerEvents()
        {
            _levelManager.LevelLoaded += OnLevelLoaded;
        }

        private void UnsubscribeFromLevelManagerEvents()
        {
            _levelManager.LevelLoaded -= OnLevelLoaded;
        }

        private void LoadMainMenuLevel()
        {
            _levelManager.LoadLevel(0);
        }

        private void OnLevelLoaded(int levelIndex)
        {
            var containerChildren = new DIContainer(_container as DIContainer);
            var view = _objectFactory.CreateObject<View>() as IInitializer;

            RegisterFields(containerChildren);
            InitializeComponents(containerChildren);

            _settings.Init(containerChildren);
            _presenter.Init(containerChildren);
            view.Init(containerChildren);
        }

        private void InitializeComponents(DIContainer containerChildren)
        {
            var stateView = containerChildren.Resolve<StateGame>();

            switch (stateView)
            {
                case StateGame.MainMenu:
                    InitializeMainMenuComponents(containerChildren);
                    break;
                case StateGame.GameMenu:
                    InitializeGameMenuComponents(containerChildren);
                    break;
            }
        }

        private void InitializeMainMenuComponents(DIContainer containerChildren)
        {
            var models = containerChildren.Resolve<List<IModel>>();
            _playerProgression.Init(containerChildren);

            models.Add(containerChildren.Resolve<IShop>() as IModel);
        }

        private void InitializeGameMenuComponents(DIContainer containerChildren)
        {
            var models = containerChildren.Resolve<List<IModel>>();
            _playerProgression.Init(containerChildren);

            models.Add(containerChildren.Resolve<IKnivesPool>() as IModel);
        }

        private void RegisterFields(DIContainer containerChildren)
        {
            containerChildren.RegisterSingleton(_ => new List<IModel>(_models));
            containerChildren.RegisterSingleton((c) => {
                var audio = _objectFactory.CreateObject<AudioComponent>() as IInitializer;
                audio.Init(c);
                return audio as IAudioSettings;
            });
            containerChildren.RegisterSingleton((c) => {
                var shop = _objectFactory.CreateObject<ShopComponent>() as IInitializer;
                shop.Init(c);
                return shop as IShop;
            });
            containerChildren.RegisterSingleton((c) => {
                var knife = _objectFactory.CreateObject<KnifeComponent>();
                return knife as IKnifeObject;
            });
            containerChildren.RegisterSingleton((c) => {
                var player = _objectFactory.CreateObject<PlayerComponent>() as IInitializer;
                player.Init(c);
                return player as IKnivesPool;
            });
            containerChildren.RegisterSingleton((c) => {
                var enemySpawner = _objectFactory.CreateObject<EnemySpawnerComponent>() as IInitializer;
                enemySpawner.Init(c);
                return enemySpawner as IEnemySpawner;
            });
        }
    }
}
