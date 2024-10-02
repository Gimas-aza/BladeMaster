using Assets.DI;
using Assets.EntryPoint;
using Assets.EntryPoint.Model;
using Assets.MVP.State;
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
        private StateMachine _stateMachine;
        private StateGame _currentState;
        private Presenter _presenter;
        private VisualElement _root;

        public void Init(IResolver container)
        {
            _currentState = container.Resolve<StateGame>();
            _presenter = container.Resolve<Presenter>();
            _root = GetComponent<UIDocument>().rootVisualElement;
            _container = new DIContainer();
            _uiEvents = new UIEvents();
            _uiElements = new UIElements(_root);
            _stateMachine = new StateMachine(_uiElements, _uiEvents, _presenter, _container);
            CheckFieldsForNull();

            RegisterFields();
            StartState();
        }

        private void StartState()
        {
            switch (_currentState)
            {
                case StateGame.MainMenu:
                    _stateMachine.ChangeState<StateMainMenu>();
                    break;
                case StateGame.GameMenu:
                    _stateMachine.ChangeState<StateGameMenu>();
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

        private void CheckFieldsForNull()
        {
            if (_templateButtonStartLevel == null)
                Debug.LogError("templateButtonStartLevel is null");
            if (_templateItemShop == null)
                Debug.LogError("templateItemShop is null");
        }
    }
}