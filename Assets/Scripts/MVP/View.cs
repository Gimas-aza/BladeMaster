using Assets.EntryPoint;
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

        private StateView _currentState;
        private Presenter _presenter;
        private VisualElement _root;
        private IStateView _stateView;

        public void Init(IResolver resolver)
        {
            _currentState = resolver.Resolve<StateView>();
            _presenter = resolver.Resolve<Presenter>();
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }

        private void Start()
        {
            switch (_currentState)
            {
                case StateView.MainMenu:
                    _stateView = new StateMainMenu(_root, _templateButtonStartLevel, _templateItemShop, _presenter);
                    break;
                case StateView.GameMenu:
                    _stateView = new StateGameMenu(_root, _presenter);
                    break;
            }
        }
    }
}