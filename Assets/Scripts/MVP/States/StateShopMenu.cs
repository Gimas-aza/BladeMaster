using System.Collections.Generic;
using System.Linq;
using Assets.DI;
using Assets.ShopManagement;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StateShopMenu : IStateView
    {
        private VisualTreeAsset _templateItemShop;
        private UIElements _uiElements;
        private StateMachine _stateMachine;
        private UIEvents _uiEvents;
        private Dictionary<IItem, Button> _buttonsItems;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _stateMachine = stateMachine;
            _uiEvents = events;
            _uiElements = elements;
            _templateItemShop = container.Resolve<VisualTreeAsset>("templateItemShop");
            _buttonsItems = new Dictionary<IItem, Button>();

            InitializeUI();
            SubscribeToMonitorUpdate();

            stateMachine.RegisterEvents();

            AddButtonsItemsShop();
        }

        public void Enter()
        {
            ShowMenu();
        }

        public void Exit()
        {
            HideMenu();
        }

        private void InitializeUI()
        {
            _uiElements.ButtonsBack[typeof(StateShopMenu)].clicked += BackToPreviousState;
        }

        private void BackToPreviousState()
        {
            _stateMachine.BackToPreviousState();
        }

        private void SubscribeToMonitorUpdate()
        {
            _uiEvents.ItemIsBought += (item) => SetButtonItem(item);
            _uiEvents.MonitorMoney += SetMoney;
        }

        private void AddButtonsItemsShop()
        {
            var items = _uiEvents.OnItemsRequestedForDisplay();

            foreach (var item in items)
            {
                var newTemplateButtonItemShop = _templateItemShop.CloneTree();
                var newButtonItem = newTemplateButtonItemShop.Q<Button>("ButtonItem");

                newButtonItem.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(item.Icon);
                newButtonItem.clicked += () => SetButtonItem(item, newButtonItem);
                _uiElements.CurrentButtonItem = newButtonItem;
                _buttonsItems.Add(item, newButtonItem);

                if (item.IsEquipped)
                    _uiElements.CurrentButtonItem.AddToClassList(_uiElements.ClassItemEquip);

                _uiElements.ContainerItemsShop.Add(newButtonItem);
            }
        }

        private void SetButtonItem(IItem item, Button buttonItem = null)
        {
            if (_uiElements.TemporaryBuyItemButton != null)
                UnsetButtonBuyItem();

            buttonItem ??= _uiElements.CurrentButtonItem;
            _uiElements.ViewItem.style.backgroundImage = new StyleBackground(item.Icon);
            _uiElements.ButtonBuyItem.enabledSelf = true;
            if (!item.IsBought)
                SetItemForBuy(item);
            else
                SetItemForEquip(item, buttonItem);
        }

        private void UnsetButtonBuyItem() =>
            _uiElements.ButtonBuyItem.clicked -= _uiElements.TemporaryBuyItemButton;

        private void SetItemForBuy(IItem item)
        {
            _uiElements.ButtonBuyItem.text = $"Купить за {item.Price}";
            _uiElements.TemporaryBuyItemButton = () => _uiEvents.OnItemRequestedForBuy(item);
            _uiElements.ButtonBuyItem.clicked += _uiElements.TemporaryBuyItemButton;
        }

        private void SetItemForEquip(IItem item, Button buttonItem)
        {
            _uiElements.ButtonBuyItem.text = "Экипировать";
            _uiElements.TemporaryBuyItemButton = () => OnEquipItem(item);
            _uiElements.ButtonBuyItem.clicked += _uiElements.TemporaryBuyItemButton;
            if (item.IsEquipped)
                OnEquipItem(item);
        }

        private void SetMoney(int amount)
        {
            _uiElements.Money.ForEach(money => money.text = $"{amount}");
        }

        private void OnEquipItem(IItem item)
        {
            _uiEvents.OnEquipItem(item);
            _buttonsItems.Values.ToList().ForEach(i => i.RemoveFromClassList(_uiElements.ClassItemEquip));
            _buttonsItems[item].AddToClassList(_uiElements.ClassItemEquip);
            _uiElements.ButtonBuyItem.text = "Предмет экипирован";
            _uiElements.ButtonBuyItem.enabledSelf = false;
        }

        private void ShowMenu()
        {
            _uiElements.Menus[typeof(StateShopMenu)].style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.Menus[typeof(StateShopMenu)].style.display = DisplayStyle.None;
        }
    }
}