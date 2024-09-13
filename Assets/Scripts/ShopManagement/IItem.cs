using UnityEngine;

namespace Assets.ShopManagement
{
    public interface IItem
    {
        int Price { get; }
        Sprite Icon { get; }
        bool IsBought { get; }
        bool IsEquipped { get; }

        void SetBought(bool isBought);
        void SetEquipped(bool isEquipped);
    }
}