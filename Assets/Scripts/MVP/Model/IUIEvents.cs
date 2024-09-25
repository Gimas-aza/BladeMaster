using System;
using System.Collections.Generic;
using Assets.ShopManagement;
using UnityEngine.Events;

namespace Assets.MVP.Model
{
    public interface IUIEvents
    {
        event Func<int> LevelAmountRequestedForDisplay;
        event UnityAction<int> PressingTheSelectedLevel;
        event Func<int> UnlockedLevels;
        event Func<List<IItem>> ItemsRequestedForDisplay;
        event UnityAction<IItem> ItemRequestedForBuy;
        event UnityAction<IItem> EquipItem;
        event Func<int, int> RatingScoreReceived;
        event UnityAction<float> MonitorInputRotation;
        event Func<IForceOfThrowingKnife> MonitorInputTouchBegin;
        event UnityAction MonitorInputTouchEnded;
        event UnityAction ClickedButtonBackMainMenu;
        event UnityAction ClickedButtonAgainLevel;
        event UnityAction<int> ChangeQuality;
        event UnityAction<float> ChangeVolume;

        UnityAction<IItem> ItemIsBought { get; set; }
        UnityAction<int> MonitorMoney { get; set; }
        UnityAction<int> MonitorBestScore { get; set; }
        UnityAction<int> MonitorCounter { get; set; }
        UnityAction<bool> FinishedLevel { get; set; }
        UnityAction<IAmountOfKnives> DisplayAmountKnives { get; set; }
        UnityAction<int> DisplayRatingScore { get; set; }
        UnityAction<int> CurrentQuality { get; set; }
        UnityAction<float> CurrentVolume { get; set; }

        void UnregisterLevelManagerEvents();
        void UnregisterPlayerProgressionEvents();
        void UnregisterPlayerEvents();
        void UnregisterShopEvents();
        void UnregisterSettingsEvents();
    }
}