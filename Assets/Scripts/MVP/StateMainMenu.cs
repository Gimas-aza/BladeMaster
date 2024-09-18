using System;
using System.Collections.Generic;
using Assets.ShopManagement;
using Cysharp.Threading.Tasks;
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

        private Dictionary<string, VisualElement> _menus;
        private VisualElement _containerButtonsStartLevel;
        private VisualElement _viewItem;
        private VisualElement _containerItemsShop;
        // ==========================================================
        private List<Label> _money;
        private Label _bestScore;
        private DropdownField _dropdownQuality;
        private Slider _sliderVolume;
        private Button _buttonPlay;
        private Button _buttonShop;
        private Button _buttonSettings;
        private Button _buttonExit;
        private List<Button> _buttonBack;
        private List<Button> _buttonsStartLevel;
        private Button _buttonBuyItem;
        private Button _currentButtonItem;
        private Action _preventButtonBuyItem;

        public event Func<int> LevelAmountRequestedForDisplay;
        public event UnityAction<int> PressingTheSelectedLevel;
        public event Func<int> UnlockedLevels;
        public event Func<List<IItem>> ItemsRequestedForDisplay;
        public event UnityAction<IItem> ItemRequestedForBuy;
        public event UnityAction<IItem> EquipItem;
        public event Func<int, int> RatingScoreReceived;
        public event UnityAction<int> ChangeQuality;
        public event UnityAction<float> ChangeVolume;
        // ==========================================================
        public UnityAction<IItem> ItemIsBought;
        public UnityAction<int> MonitorMoney;
        public UnityAction<int> MonitorBestScore;
        public UnityAction<int> CurrentQuality;
        public UnityAction<float> CurrentVolume;

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
            Start();
            SubscribeToMonitorUpdate();
            presenter.RegisterEventsForView(
                ref LevelAmountRequestedForDisplay,
                ref PressingTheSelectedLevel,
                ref UnlockedLevels,
                ref ItemsRequestedForDisplay,
                ref ItemRequestedForBuy,
                ref EquipItem,
                ref ItemIsBought,
                ref MonitorMoney,
                ref MonitorBestScore,
                ref RatingScoreReceived,
                ref ChangeQuality,
                ref CurrentQuality,
                ref ChangeVolume,
                ref CurrentVolume
            );

            ShowMenu("MainMenu");
            AddButtonsStartLevel();
            AddButtonsItemsShop();
        }

        private void Start()
        {
            _menus = new Dictionary<string, VisualElement>
            {
                { "MainMenu", _root.Q<VisualElement>("MainMenu") },
                { "LevelsMenu", _root.Q<VisualElement>("LevelsMenu") },
                { "ShopMenu", _root.Q<VisualElement>("ShopMenu") },
                { "SettingsMenu", _root.Q<VisualElement>("SettingsMenu") }
            };

            _containerButtonsStartLevel = _root.Q<VisualElement>("ContainerButtonsStartLevel");
            _viewItem = _root.Q<VisualElement>("ViewItem");
            _containerItemsShop = _root.Q<VisualElement>("GroupBoxItemShop");
            _money = _root.Query<Label>("Money").ToList();
            _bestScore = _root.Q<Label>("LabelBestScore");
            _dropdownQuality = _root.Q<DropdownField>("DropdownQuality");
            _sliderVolume = _root.Q<Slider>("SliderVolume");

            _buttonPlay = _root.Q<Button>("ButtonPlay");
            _buttonShop = _root.Q<Button>("ButtonShop");
            _buttonSettings = _root.Q<Button>("ButtonSettings");
            _buttonExit = _root.Q<Button>("ButtonExit");
            _buttonBack = _root.Query<Button>("ButtonBack").ToList();
            _buttonsStartLevel = new List<Button>();
            _buttonBuyItem = _root.Q<Button>("ButtonBuy");

            _buttonPlay.clicked += () => ShowMenu("LevelsMenu");
            _buttonShop.clicked += () => ShowMenu("ShopMenu");
            _buttonSettings.clicked += () => ShowMenu("SettingsMenu");
            _buttonExit.clicked += Application.Quit;

            _dropdownQuality.RegisterValueChangedCallback(OnChangeQuality);
            _sliderVolume.RegisterValueChangedCallback(OnChangeVolume);

            foreach (var button in _buttonBack)
            {
                button.clicked += () => ShowMenu("MainMenu");
            }
        }

        private void SubscribeToMonitorUpdate()
        {
            ItemIsBought += (item) => SetButtonItem(item);
            MonitorMoney += SetMoney;
            MonitorBestScore += SetBestScore;
            CurrentQuality += (value) => _dropdownQuality.index = value;
            CurrentVolume += (value) => _sliderVolume.value = value * 100;
        }

        private void OnChangeQuality(ChangeEvent<string> evt)
        {
            ChangeQuality?.Invoke(_dropdownQuality.index);
        }

        private void OnChangeVolume(ChangeEvent<float> evt)
        {
            ChangeVolume?.Invoke(_sliderVolume.value / 100);
        }

        private void ShowMenu(string menuName)
        {
            foreach (var menu in _menus)
            {
                menu.Value.style.display =
                    menu.Key == menuName ? DisplayStyle.Flex : DisplayStyle.None;
            }
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
                SetRatingScoreForButtonLevel(i, RatingScoreReceived?.Invoke(i) ?? 0);
            }
            for (var i = 0; i < unlockedLevels; i++)
            {
                if (i >= levelAmount)
                    break;
                _buttonsStartLevel[i].enabledSelf = true;
            }
        }

        private void SetRatingScoreForButtonLevel(int index, int ratingScore)
        {
            var ratingScoreFull = _buttonsStartLevel[index].Query<VisualElement>("RatingScoreFull").ToList();
            var ratingScoreEmpty = _buttonsStartLevel[index].Query<VisualElement>("RatingScoreEmpty").ToList();

            for (int i = 0; i < ratingScoreEmpty.Count; i++)
            {
                if (i < ratingScore)
                {
                    ratingScoreFull[i].style.display = DisplayStyle.Flex;
                    ratingScoreEmpty[i].style.display = DisplayStyle.None;
                }
            }
        }

        private void AddButtonsItemsShop()
        {
            var items = ItemsRequestedForDisplay?.Invoke() ?? new List<IItem>();

            foreach (var item in items)
            {
                var newTemplateButtonItemShop = _templateItemShop.CloneTree();
                var newButtonItem = newTemplateButtonItemShop.Q<Button>("ButtonItem");

                newButtonItem.style.backgroundImage = new StyleBackground(item.Icon);
                newButtonItem.clicked += () => SetButtonItem(item, newButtonItem);
                _currentButtonItem = newButtonItem;
                if (item.IsBought)
                    _currentButtonItem.AddToClassList("ShopMenu__Item-Bought");

                _containerItemsShop.Add(newButtonItem);
            }
        }

        private void SetButtonItem(IItem item, Button buttonItem = null)
        {
            if (_preventButtonBuyItem != null)
                UnsetButtonBuyItem();

            buttonItem ??= _currentButtonItem;
            _viewItem.style.backgroundImage = new StyleBackground(item.Icon);
            if (!item.IsBought)
                SetItemForBuy(item);
            else
                SetItemForEquip(item, buttonItem);

            _buttonBuyItem.enabledSelf = true;
        }

        private void UnsetButtonBuyItem() => _buttonBuyItem.clicked -= _preventButtonBuyItem;

        private void SetItemForBuy(IItem item)
        {
            _buttonBuyItem.text = $"Купить за {item.Price}";
            _preventButtonBuyItem = () => ItemRequestedForBuy?.Invoke(item);
            _buttonBuyItem.clicked += _preventButtonBuyItem;
        }

        private void SetItemForEquip(IItem item, Button buttonItem)
        {
            _buttonBuyItem.text = "Экипировать";
            buttonItem.AddToClassList("ShopMenu__Item-Bought");
            _preventButtonBuyItem = () => EquipItem?.Invoke(item);
            _buttonBuyItem.clicked += _preventButtonBuyItem;
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

        private async void SetMoney(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            foreach (var money in _money)
                money.text = $"{amount}";
        }

        private async void SetBestScore(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            _bestScore.text = $"{amount}";
        }
    }
}
