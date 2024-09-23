using Assets.DI;
using Assets.ShopManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateMainMenu : IStateView
    {
        private VisualTreeAsset _templateButtonStartLevel;
        private VisualTreeAsset _templateItemShop;
        private Presenter _presenter;
        private UIElements _uiElements;
        private UIEvents _uiEvents;

        public StateMainMenu(UIElements elements, UIEvents events, DIContainer container, Presenter presenter)
        {
            _templateButtonStartLevel = container.Resolve<VisualTreeAsset>(
                "templateButtonStartLevel"
            );
            _templateItemShop = container.Resolve<VisualTreeAsset>("templateItemShop");
            _presenter = presenter;
            _uiEvents = events;
            _uiElements = elements;

            InitializeUI();
            SubscribeToMonitorUpdate();

            _presenter.RegisterEventsForView(container);

            ShowMenu(StateMenu.MainMenu);
            AddButtonsStartLevel();
            AddButtonsItemsShop();
        }

        private void InitializeUI()
        {
            _uiElements.ButtonPlay.clicked += () => ShowMenu(StateMenu.LevelsMenu);
            _uiElements.ButtonShop.clicked += () => ShowMenu(StateMenu.ShopMenu);
            _uiElements.ButtonSettings.clicked += () => ShowMenu(StateMenu.SettingsMenu);
            _uiElements.ButtonExit.clicked += Application.Quit;

            _uiElements.DropdownQuality.RegisterValueChangedCallback(OnChangeQuality);
            _uiElements.SliderVolume.RegisterValueChangedCallback(OnChangeVolume);

            foreach (var button in _uiElements.ButtonBack)
            {
                button.clicked += () => ShowMenu(StateMenu.MainMenu);
            }
        }

        private void SubscribeToMonitorUpdate()
        {
            _uiEvents.ItemIsBought += (item) => SetButtonItem(item);
            _uiEvents.MonitorMoney += SetMoney;
            _uiEvents.MonitorBestScore += SetBestScore;
            _uiEvents.CurrentQuality += (value) => _uiElements.DropdownQuality.index = value;
            _uiEvents.CurrentVolume += (value) => _uiElements.SliderVolume.value = value * 100;
        }

        private void OnChangeQuality(ChangeEvent<string> evt)
        {
            _uiEvents.OnChangeQuality(_uiElements.DropdownQuality.index);
        }

        private void OnChangeVolume(ChangeEvent<float> evt)
        {
            _uiEvents.OnChangeVolume(_uiElements.SliderVolume.value / 100);
        }

        private void ShowMenu(StateMenu menuName)
        {
            foreach (var menu in _uiElements.Menus)
            {
                menu.Value.style.display =
                    menu.Key == menuName ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void AddButtonsStartLevel()
        {
            var levelAmount = _uiEvents.OnLevelAmountRequestedForDisplay();
            var unlockedLevels = _uiEvents.OnUnlockedLevels();
            if (levelAmount == 0)
                return;

            CreateButtonsStartLevel(levelAmount);
            for (var i = 0; i < levelAmount; i++)
            {
                int index = i;
                _uiElements.ButtonsStartLevel[i].style.display = DisplayStyle.Flex;
                _uiElements.ButtonsStartLevel[i].enabledSelf = false;
                _uiElements.ButtonsStartLevel[i].clicked += () =>
                    _uiEvents.OnPressingTheSelectedLevel(index + 1);
                SetRatingScoreForButtonLevel(i, _uiEvents.OnRatingScoreReceived(i));
            }
            for (var i = 0; i < unlockedLevels; i++)
            {
                if (i >= levelAmount)
                    break;
                _uiElements.ButtonsStartLevel[i].enabledSelf = true;
            }
        }

        private void SetRatingScoreForButtonLevel(int index, int ratingScore)
        {
            var ratingScoreFull = _uiElements
                .ButtonsStartLevel[index]
                .Query<VisualElement>("RatingScoreFull")
                .ToList();
            var ratingScoreEmpty = _uiElements
                .ButtonsStartLevel[index]
                .Query<VisualElement>("RatingScoreEmpty")
                .ToList();

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
            var items = _uiEvents.OnItemsRequestedForDisplay();

            foreach (var item in items)
            {
                var newTemplateButtonItemShop = _templateItemShop.CloneTree();
                var newButtonItem = newTemplateButtonItemShop.Q<Button>("ButtonItem");

                newButtonItem.style.backgroundImage = new StyleBackground(item.Icon);
                newButtonItem.clicked += () => SetButtonItem(item, newButtonItem);
                _uiElements.CurrentButtonItem = newButtonItem;

                if (item.IsBought)
                    _uiElements.CurrentButtonItem.AddToClassList("ShopMenu__Item-Bought");

                _uiElements.ContainerItemsShop.Add(newButtonItem);
            }
        }

        private void SetButtonItem(IItem item, Button buttonItem = null)
        {
            if (_uiElements.PreventButtonBuyItem != null)
                UnsetButtonBuyItem();

            buttonItem ??= _uiElements.CurrentButtonItem;
            _uiElements.ViewItem.style.backgroundImage = new StyleBackground(item.Icon);
            if (!item.IsBought)
                SetItemForBuy(item);
            else
                SetItemForEquip(item, buttonItem);

            _uiElements.ButtonBuyItem.enabledSelf = true;
        }

        private void UnsetButtonBuyItem() =>
            _uiElements.ButtonBuyItem.clicked -= _uiElements.PreventButtonBuyItem;

        private void SetItemForBuy(IItem item)
        {
            _uiElements.ButtonBuyItem.text = $"Купить за {item.Price}";
            _uiElements.PreventButtonBuyItem = () => _uiEvents.OnItemRequestedForBuy(item);
            _uiElements.ButtonBuyItem.clicked += _uiElements.PreventButtonBuyItem;
        }

        private void SetItemForEquip(IItem item, Button buttonItem)
        {
            _uiElements.ButtonBuyItem.text = "Экипировать";
            buttonItem.AddToClassList("ShopMenu__Item-Bought");
            _uiElements.PreventButtonBuyItem = () => _uiEvents.OnEquipItem(item);
            _uiElements.ButtonBuyItem.clicked += _uiElements.PreventButtonBuyItem;
        }

        private void CreateButtonsStartLevel(int levelAmount)
        {
            for (var i = 0; i < levelAmount; i++)
            {
                var newTemplateButtonStartLevel = _templateButtonStartLevel.CloneTree();
                var newButtonStartLevel = newTemplateButtonStartLevel.Q<Button>("ButtonStartLevel");
                newButtonStartLevel.text = $"Уровень {i + 1}";

                _uiElements.ContainerButtonsStartLevel.Add(newButtonStartLevel);
                _uiElements.ButtonsStartLevel.Add(newButtonStartLevel);
            }
        }

        private async void SetMoney(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            foreach (var money in _uiElements.Money)
                money.text = $"{amount}";
        }

        private async void SetBestScore(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            _uiElements.BestScore.text = $"{amount}";
        }
    }
}
