using UnityEngine;

namespace Assets.ShopManagement
{
    public interface IItem
    {
        int Price { get; }
        Sprite Icon { get; }
    }
}