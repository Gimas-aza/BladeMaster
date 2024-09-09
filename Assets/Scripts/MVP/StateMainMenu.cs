using System;
using System.Collections.Generic;
using Assets.ShopManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateMainMenu : IStateView
    {
        private VisualTreeAsset _templateButtonStartLevel;
        private VisualTreeAsset _templateItemShop;
        private VisualElement _root;

        // ==========================================
        private VisualElement _mainMenu;
        private VisualElement _levelsMenu;
        private VisualElement _shopMenu;
        private VisualElement _settingsMenu;
        private VisualElement _containerButtonsStartLevel;
        private VisualElement _viewItem;
        private VisualElement _containerItemsShop;

        // ==========================================
        private Button _buttonPlay;
        private Button _buttonShop;
        private Button _buttonSettings;
        private Button _buttonExit;
        private List<Button> _buttonBack;
        private List<Button> _buttonsStartLevel;
        private Button _buttonBuyItem;
        private Action _preventButtonBuyItem;

        public event Func<int> LevelAmountRequestedForDisplay;
        public event UnityAction<int> PressingTheSelectedLevel;
        public event Func<int> UnlockedLevels;
        public event Func<List<IItem>> ItemsRequestedForDisplay;
        public event UnityAction<IItem> ItemRequestedForBuy;

        public StateMainMenu(
            VisualElement root,
            VisualTreeAsset templateButtonStartLevel,
            VisualTreeAsset templateItemShop,
            Presenter presenter
        )
        {
            _root = root;
            _templateButtonStartLevel = templateButtonStartLevel;
            _templateItemShop = templateItemShop;
            presenter.RegisterEventsForView(
                ref LevelAmountRequestedForDisplay,
                ref PressingTheSelectedLevel,
                ref UnlockedLevels,
                ref ItemsRequestedForDisplay,
                ref ItemRequestedForBuy
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

            _viewItem = _root.Q<VisualElement>("ViewItem");
            _containerItemsShop = _root.Q<VisualElement>("GroupBoxItemShop");

            _buttonPlay = _root.Q<Button>("ButtonPlay");
            _buttonShop = _root.Q<Button>("ButtonShop");
            _buttonSettings = _root.Q<Button>("ButtonSettings");
            _buttonExit = _root.Q<Button>("ButtonExit");
            _buttonBack = _root.Query<Button>("ButtonBack").ToList();
            _buttonsStartLevel = new List<Button>();
            _buttonBuyItem = _root.Q<Button>("ButtonBuy");

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
            AddButtonsItemsShop();
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
                if (i >= levelAmount)
                    break;
                _buttonsStartLevel[i].enabledSelf = true;
            }
        }

        private void AddButtonsItemsShop()
        {
            var items = ItemsRequestedForDisplay?.Invoke() ?? new List<IItem>();

            for (var i = 0; i < items.Count; i++)
            {
                var index = i;
                var newTemplateButtonItemShop = _templateItemShop.CloneTree();
                var newButtonItem = newTemplateButtonItemShop.Q<Button>("ButtonItem");

                newButtonItem.style.backgroundImage = new StyleBackground(items[index].Icon);
                newButtonItem.clicked += () => SetButtonBuyItem(items[index]);

                _containerItemsShop.Add(newButtonItem);
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

        private void SetButtonBuyItem(IItem item)
        {
            _viewItem.style.backgroundImage = new StyleBackground(item.Icon);
            _buttonBuyItem.text = $"Купить за {item.Price}";

            if (_preventButtonBuyItem != null)
                UnsetButtonBuyItem();

            _preventButtonBuyItem = () => ItemRequestedForBuy?.Invoke(item);

            _buttonBuyItem.clicked += _preventButtonBuyItem;
            _buttonBuyItem.enabledSelf = true;
        }

        private void UnsetButtonBuyItem() => _buttonBuyItem.clicked -= _preventButtonBuyItem;
    }
}
