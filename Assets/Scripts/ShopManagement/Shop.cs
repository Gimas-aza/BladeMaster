using System.Collections.Generic;
using UnityEngine;

namespace Assets.ShopManagement
{
    public class Shop
    {
        private List<IItem> _items;

        public List<IItem> GetItems() => _items; // todo: give items to shop menu and set it in BuyItem

        public void BuyItem(IItem item)
        {
            
        }
    }
}
