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

        public event Func<int, bool> RequestToBuy;
        public event UnityAction<IItemSkin> BoughtSkin;

        public void SubscribeToEvents(ref Func<List<IItem>> itemsRequestedForDisplay, ref UnityAction<IItem> itemRequestedForBuy)
        {
            itemRequestedForBuy += BuyItem;
            itemsRequestedForDisplay += GetItems;
        }

        public List<IItem> GetItems() => _items.Cast<IItem>().ToList(); 

        public void BuyItem(IItem item)
        {
            if (RequestToBuy?.Invoke(item.Price) ?? false)
            {
                BoughtSkin?.Invoke(item as IItemSkin);
            }
        }
    }
}
