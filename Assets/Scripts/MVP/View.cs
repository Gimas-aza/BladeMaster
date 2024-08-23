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
        private Presenter _presenter;
        private VisualElement _root;
        // ==========================================
        private VisualElement _mainMenu;
        private VisualElement _levelsMenu;
        private VisualElement _shopMenu;
        private VisualElement _settingsMenu;
        private VisualElement _containerButtonsStartLevel;
        // ==========================================
        private Button _buttonPlay;
        private Button _buttonShop;
        private Button _buttonSettings;
        private Button _buttonExit;
        private List<Button> _buttonBack;
        private List<Button> _buttonStartLevel;

        public event Func<int> LevelAmountRequestedForDisplay;
        public event UnityAction<int> PressingTheSelectedLevel;

        public void Init(Presenter presenter)
        {
            _presenter = presenter;
            _presenter.RegisterEventsForView(ref LevelAmountRequestedForDisplay, ref PressingTheSelectedLevel);
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _mainMenu = _root.Q<VisualElement>("MainMenu");
            _levelsMenu = _root.Q<VisualElement>("LevelsMenu");
            _shopMenu = _root.Q<VisualElement>("ShopMenu");
            _settingsMenu = _root.Q<VisualElement>("SettingsMenu");
            _containerButtonsStartLevel = _root.Q<VisualElement>("ContainerButtonsStartLevel");

            _buttonPlay = _root.Q<Button>("ButtonPlay");
            _buttonShop = _root.Q<Button>("ButtonShop");
            _buttonSettings = _root.Q<Button>("ButtonSettings");
            _buttonExit = _root.Q<Button>("ButtonExit");
            _buttonBack = _root.Query<Button>("ButtonBack").ToList();
            _buttonStartLevel = _root.Query<Button>("ButtonStartLevel").ToList();
        }

        private void Start()
        {
            _buttonPlay.clicked += OnButtonPlayClick;
            _buttonShop.clicked += OnButtonShopClick;
            _buttonSettings.clicked += OnButtonSettingsClick;
            _buttonExit.clicked += OnButtonExitClick;
            
            foreach (var button in _buttonBack)
            {
                button.clicked += OnButtonBackClick;
            }
        }

        private void OnButtonBackClick()
        {
            _mainMenu.style.display = DisplayStyle.Flex;
            _levelsMenu.style.display = DisplayStyle.None;
            _shopMenu.style.display = DisplayStyle.None;
            _settingsMenu.style.display = DisplayStyle.None;
        }

        private void OnButtonPlayClick()
        {
            _mainMenu.style.display = DisplayStyle.None;
            _levelsMenu.style.display = DisplayStyle.Flex;
            _shopMenu.style.display = DisplayStyle.None;
            _settingsMenu.style.display = DisplayStyle.None;

            Debug.Log(LevelAmountRequestedForDisplay?.Invoke());
        }

        private void OnButtonShopClick()
        {
            _mainMenu.style.display = DisplayStyle.None;
            _levelsMenu.style.display = DisplayStyle.None;
            _shopMenu.style.display = DisplayStyle.Flex;
            _settingsMenu.style.display = DisplayStyle.None;
        }

        private void OnButtonSettingsClick()
        {
            _mainMenu.style.display = DisplayStyle.None;
            _levelsMenu.style.display = DisplayStyle.None;
            _shopMenu.style.display = DisplayStyle.None;
            _settingsMenu.style.display = DisplayStyle.Flex;
        }

        private void OnButtonExitClick()
        {
            Debug.Log("Exit");
        }
    }
}