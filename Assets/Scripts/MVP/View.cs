using System;
using System.Collections.Generic;
using Assets.EntryPoint;
using UnityEngine;
using UnityEngine.Events;
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
        private StateMainMenu _stateMainMenu;

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
                    _stateMainMenu = new StateMainMenu(_root, _templateButtonStartLevel, _presenter);
                    break;
                case StateView.GameMenu:
                    break;
            }
        }
    }
}