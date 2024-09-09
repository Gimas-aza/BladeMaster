using System;
using Assets.Knife;
using UnityEngine.Events;

namespace Assets.GameProgression
{
    public interface IShop
    {
        event Func<int, bool> RequestToBuy;
        event UnityAction<IItemSkin> BoughtSkin;
    }
}