using Assets.DI;
using Assets.EntryPoint;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    [RequireComponent(typeof(UIDocument))]
    public class View : MonoBehaviour, IInitializer
    {
        [Header("Templates UI")]
        [SerializeField] private VisualTreeAsset _templateButtonStartLevel;
        [SerializeField] private VisualTreeAsset _templateItemShop;

        private DIContainer _container;
        private UIEvents _uiEvents;
        private UIElements _uiElements;
        private StateView _currentState;
        private Presenter _presenter;
        private VisualElement _root;
        private IStateView _stateView;

        public void Init(IResolver container)
        {
            _currentState = container.Resolve<StateView>();
            _presenter = container.Resolve<Presenter>();
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _container = new DIContainer();
            _uiEvents = new UIEvents();
            _uiElements = new UIElements(_root);
            RegisterFields();
        }

        private void Start()
        {
            switch (_currentState)
            {
                case StateView.MainMenu:
                    _stateView = new StateMainMenu(_uiElements, _uiEvents, _container, _presenter);
                    break;
                case StateView.GameMenu:
                    _stateView = new StateGameMenu(_uiElements, _uiEvents, _container, _presenter);
                    break;
            }
        }

        private void RegisterFields()
        {
            VisualTreeAsset templateButtonStartLevel;
            _container.RegisterInstance(nameof(templateButtonStartLevel), _templateButtonStartLevel);
            VisualTreeAsset templateItemShop;
            _container.RegisterInstance(nameof(templateItemShop), _templateItemShop);
            _container.RegisterInstance(_uiEvents as IUIEvents);
        }
    }
}