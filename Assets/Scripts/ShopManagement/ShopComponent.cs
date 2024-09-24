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
    public class ShopComponent : MonoBehaviour, IInitializer, IModel, IShop
    {
        [SerializeField] private List<ItemComponent> _items;

        private UnityAction<IItem> _itemIsBought;
        private IItemSkin _equippedItem;
        private ISaveSystem _saveSystem;

        public event Func<int, bool> RequestToBuy;
        public event UnityAction<IItemSkin> BoughtSkin;

        public void Init(IResolver container)
        {
            var shopData = container.Resolve<IShopData>();
            _saveSystem = container.Resolve<ISaveSystem>();

            int missingItemsCount = _items.Count - shopData.Items.Count;
            if (missingItemsCount > 0)
            {
                var newItems = Enumerable.Repeat(new ItemData { IsBought = false, IsEquipped = false }, missingItemsCount);
                shopData.Items.AddRange(newItems);
            }

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Init(shopData.Items[i]);
            }
        }

        public void SubscribeToEvents(IResolver container)
        {
            var uiEvents = container.Resolve<IUIEvents>();

            uiEvents.ItemsRequestedForDisplay += GetItems;
            uiEvents.ItemRequestedForBuy += BuyItem;
            uiEvents.EquipItem += EquipItem;
            _itemIsBought = uiEvents.ItemIsBought;
        }

        public IItemSkin GetEquippedItem()
        {
            _equippedItem = _items.FirstOrDefault(x => x.IsEquipped) ?? GetDefaultItem();

            _saveSystem.SaveAsync();
            return _equippedItem;
        }

        private List<IItem> GetItems() => _items.Cast<IItem>().ToList();

        private void BuyItem(IItem item)
        {
            if (RequestToBuy?.Invoke(item.Price) == true && !item.IsBought)
            {
                item.SetBought(true);
                EquipItem(item);

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
            _items.ForEach(i => i.SetEquipped(ReferenceEquals(i, item)));
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
