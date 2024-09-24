using System;
using Assets.DI;
using Assets.ShopManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StateShopMenu : IStateView
    {
        private VisualTreeAsset _templateItemShop;
        private UIElements _uiElements;
        private UIEvents _uiEvents;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _uiEvents = events;
            _uiElements = elements;
            _templateItemShop = container.Resolve<VisualTreeAsset>("templateItemShop");

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

        private async void SetMoney(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            foreach (var money in _uiElements.Money)
                money.text = $"{amount}";
        }

        private void ShowMenu()
        {
            _uiElements.Menus[StateMenu.ShopMenu].style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.Menus[StateMenu.ShopMenu].style.display = DisplayStyle.None;
        }
    }
}