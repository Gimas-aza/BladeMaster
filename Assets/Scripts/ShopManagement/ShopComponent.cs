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
        private IItemSkin _equippedItem;

        public event Func<int, bool> RequestToBuy;
        public event UnityAction<IItemSkin> BoughtSkin;

        // private void Awake()
        // {
        //     foreach (var item in _items)
        //     {
        //         item.IsBought = false;
        //     }
        // }

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

        private void BuyItem(IItem item)
        {
            if (RequestToBuy?.Invoke(item.Price) ?? false && !item.IsBought)
            {
                item.IsBought = true;
                SetEquippedItem(item);
                BoughtSkin?.Invoke(item as IItemSkin);
                _itemIsBought?.Invoke(item);
            }
        }

        private void EquipItem(IItem item)
        {
            SetEquippedItem(item);
            BoughtSkin?.Invoke(item as IItemSkin);
        }

        private void SetEquippedItem(IItem item)
        {
            _items.ForEach(x => x.IsEquipped = false);
            _items.First(x => ReferenceEquals(x, item)).IsEquipped = true;
        }

        public IItemSkin GetEquippedItem()
        {
            _equippedItem = _items.First(x => x.IsEquipped);
            return _equippedItem;
        }
    }
}
