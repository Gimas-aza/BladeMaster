using System.Collections.Generic;

namespace Assets.ShopManagement
{
    public interface IShopData
    {
        List<ItemData> Items { get; set; }
    }
}