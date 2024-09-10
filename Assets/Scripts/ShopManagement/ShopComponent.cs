using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EntryPoint;
using Assets.GameProgression;
using Assets.Knife;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.ShopManagement
{
    public class ShopComponent : MonoBehaviour, IModel, IInitializer, IShop
    {
        [SerializeField] private List<ItemComponent> _items;

        private UnityAction<IItem> _itemIsBought;

        public event Func<int, bool> RequestToBuy;
        public event UnityAction<IItemSkin> BoughtSkin;

        private void Awake()
        {
            foreach (var item in _items)
            {
                item.IsBought = false;
            }
        }

        public void SubscribeToEvents(
            ref Func<List<IItem>> itemsRequestedForDisplay,
            ref UnityAction<IItem> itemRequestedForBuy,
            ref UnityAction<IItem> equipItem,
            ref UnityAction<IItem> itemIsBought
        )
        {
            itemsRequestedForDisplay += GetItems;
            itemRequestedForBuy += BuyItem;
            equipItem += EquipItem;
            _itemIsBought = itemIsBought;
        }

        public List<IItem> GetItems() => _items.Cast<IItem>().ToList();

        public void BuyItem(IItem item)
        {
            if (RequestToBuy?.Invoke(item.Price) ?? false && !item.IsBought)
            {
                item.IsBought = true;
                BoughtSkin?.Invoke(item as IItemSkin);
                _itemIsBought?.Invoke(item);
            }
        }

        private void EquipItem(IItem item)
        {
            Debug.Log("EquipItem");
        }
    }
}
