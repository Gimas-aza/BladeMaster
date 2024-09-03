using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateMainMenu : IStateView
    {
        private VisualTreeAsset _templateButtonStartLevel;
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
        private List<Button> _buttonsStartLevel;

        public event Func<int> LevelAmountRequestedForDisplay;
        public event UnityAction<int> PressingTheSelectedLevel;
        public event Func<int> UnlockedLevels;

        public StateMainMenu(
            VisualElement root,
            VisualTreeAsset templateButtonStartLevel,
            Presenter presenter
        )
        {
            _root = root;
            _templateButtonStartLevel = templateButtonStartLevel;
            presenter.RegisterEventsForView(
                ref LevelAmountRequestedForDisplay,
                ref PressingTheSelectedLevel,
                ref UnlockedLevels
            );
            Start();
        }

        private void Start()
        {
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
            _buttonsStartLevel = new List<Button>();

            _buttonPlay.clicked += OnButtonPlayClick;
            _buttonShop.clicked += OnButtonShopClick;
            _buttonSettings.clicked += OnButtonSettingsClick;
            _buttonExit.clicked += OnButtonExitClick;

            foreach (var button in _buttonBack)
            {
                button.clicked += OnButtonBackClick;
            }

            _mainMenu.style.display = DisplayStyle.Flex;
            AddButtonsStartLevel();
        }

        private void AddButtonsStartLevel()
        {
            var levelAmount = LevelAmountRequestedForDisplay?.Invoke() ?? 0;
            var unlockedLevels = UnlockedLevels?.Invoke() ?? 0;
            if (levelAmount == 0)
                return;

            CreateButtonsStartLevel(levelAmount);
            for (var i = 0; i < levelAmount; i++)
            {
                int index = i;
                _buttonsStartLevel[i].style.display = DisplayStyle.Flex;
                _buttonsStartLevel[i].enabledSelf = false;
                _buttonsStartLevel[i].clicked += () => PressingTheSelectedLevel?.Invoke(index + 1);
            }
            for (var i = 0; i < unlockedLevels; i++)
            {
                _buttonsStartLevel[i].enabledSelf = true;
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
        }

        private void CreateButtonsStartLevel(int levelAmount)
        {
            for (var i = 0; i < levelAmount; i++)
            {
                var newTemplateButtonStartLevel = _templateButtonStartLevel.CloneTree();
                var newButtonStartLevel = newTemplateButtonStartLevel.Q<Button>("ButtonStartLevel");
                newButtonStartLevel.text = $"Уровень {i + 1}";

                _containerButtonsStartLevel.Add(newButtonStartLevel);
                _buttonsStartLevel.Add(newButtonStartLevel);
            }
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
