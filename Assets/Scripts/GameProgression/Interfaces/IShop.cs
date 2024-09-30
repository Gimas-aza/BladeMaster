using System;
using UnityEngine.Events;

namespace Assets.GameProgression.Interfaces
{
    public interface IShop
    {
        event Func<int, bool> RequestToBuy;
        event UnityAction<IItemSkin> BoughtSkin;

        IItemSkin GetEquippedItem();
    }
}