using System;
using System.Collections.Generic;
using Assets.ShopManagement;
using UnityEngine.Events;

namespace Assets.MVP.Model
{
    public interface IModel
    {
        void SubscribeToEvents(
            ref Func<int> levelAmountRequestedForDisplay,
            ref UnityAction<int> pressingTheSelectedLevel
        ) { }
        void SubscribeToEvents(
            ref Func<int> unlockedLevels
        ) { }
        void SubscribeToEvents(
            ref Func<List<IItem>> itemsRequestedForDisplay,
            ref UnityAction<IItem> itemRequestedForBuy,
            ref UnityAction<IItem> equipItem,
            ref UnityAction<IItem> itemIsBought
        ) { }
        void SubscribeToEvents(
            ref UnityAction<float> monitorInputRotation,
            ref Func<IForceOfThrowingKnife> monitorInputTouchBegin,
            ref UnityAction monitorInputTouchEnded,
            ref UnityAction<IAmountOfKnives> displayAmountKnives
        ) { }
        void SubscribeToEvents(
            ref UnityAction clickedButtonBackMainMenu,
            ref UnityAction clickedButtonAgainLevel
        ) { }
        void SubscribeToEvents(
            ref UnityAction<int> monitorCounter,
            ref UnityAction<int> monitorMoney,
            ref UnityAction<bool> finishedLevel,
            ref UnityAction<int> displayRatingScore
        ) { }
        void SubscribeToEvents(
            ref UnityAction<int> monitorMoney,
            ref UnityAction<int> monitorBestScore,
            ref Func<int, int> ratingScoreReceived
        ) { }
        void SubscribeToEvents(
            ref UnityAction<int> changeQuality,
            ref Func<int> currentQuality
        ) { }
    }
}
