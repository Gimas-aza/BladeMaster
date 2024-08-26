using Assets.EntryPoint;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    [RequireComponent(typeof(UIDocument))]
    public class View : MonoBehaviour, IInitializer
    {
        [SerializeField] private VisualTreeAsset _templateButtonStartLevel;

        private StateView _currentState;
        private Presenter _presenter;
        private VisualElement _root;
        private IStateView _stateView;

        public void Init(Presenter presenter, StateView currentState)
        {
            _currentState = currentState;
            _presenter = presenter;
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
                    _stateView = new StateMainMenu(_root, _templateButtonStartLevel, _presenter);
                    break;
                case StateView.GameMenu:
                    _stateView = new StateGameMenu(_root);
                    break;
            }
        }
    }
}