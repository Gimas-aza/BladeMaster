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
        private ISaveSystem _saveSystem;

        public event Func<int, bool> RequestToBuy;
        public event UnityAction<IItemSkin> BoughtSkin;

        public void Init(IResolver resolver)
        {
            var shopData = resolver.Resolve<IShopData>();
            _saveSystem = resolver.Resolve<ISaveSystem>();

            for (int i = shopData.Items.Count; i < _items.Count; i++)
            {
                shopData.Items.Add(new ItemData());
            }

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Init(shopData.Items[i]);
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

        private void BuyItem(IItem item)
        {
            if (RequestToBuy?.Invoke(item.Price) ?? false && !item.IsBought)
            {
                item.SetBought(true);
                SetEquippedItem(item);
                _saveSystem.SaveAsync();

                BoughtSkin?.Invoke(item as IItemSkin);
                _itemIsBought?.Invoke(item);
            }
        }

        private void EquipItem(IItem item)
        {
            SetEquippedItem(item);
            _saveSystem.SaveAsync();

            BoughtSkin?.Invoke(item as IItemSkin);
        }

        private void SetEquippedItem(IItem item)
        {
            _items.ForEach(x => x.SetEquipped(false));
            _items.First(x => ReferenceEquals(x, item)).SetEquipped(true);
        }

        public IItemSkin GetEquippedItem()
        {
            _equippedItem = _items.FirstOrDefault(x => x.IsEquipped);
            _equippedItem ??= GetDefaultItem();
            _saveSystem.SaveAsync();
            return _equippedItem;
        }

        private IItemSkin GetDefaultItem()
        {
            var defaultItem = _items[0];
            defaultItem.SetEquipped(true);
            defaultItem.SetBought(true);
            return defaultItem;
        }
    }
}
