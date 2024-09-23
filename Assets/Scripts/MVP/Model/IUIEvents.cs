using System;
using System.Collections.Generic;
using Assets.ShopManagement;
using UnityEngine.Events;

namespace Assets.MVP.Model
{
    public interface IUIEvents
    {
        public event Func<int> LevelAmountRequestedForDisplay;
        public event UnityAction<int> PressingTheSelectedLevel;
        public event Func<int> UnlockedLevels;
        public event Func<List<IItem>> ItemsRequestedForDisplay;
        public event UnityAction<IItem> ItemRequestedForBuy;
        public event UnityAction<IItem> EquipItem;
        public event Func<int, int> RatingScoreReceived;
        public event UnityAction<float> MonitorInputRotation;
        public event Func<IForceOfThrowingKnife> MonitorInputTouchBegin;
        public event UnityAction MonitorInputTouchEnded;
        public event UnityAction ClickedButtonBackMainMenu;
        public event UnityAction ClickedButtonAgainLevel;
        public event UnityAction<int> ChangeQuality;
        public event UnityAction<float> ChangeVolume;

        public UnityAction<IItem> ItemIsBought { get; set; }
        public UnityAction<int> MonitorMoney { get; set; }
        public UnityAction<int> MonitorBestScore { get; set; }
        public UnityAction<int> MonitorCounter { get; set; }
        public UnityAction<bool> FinishedLevel { get; set; }
        public UnityAction<IAmountOfKnives> DisplayAmountKnives { get; set; }
        public UnityAction<int> DisplayRatingScore { get; set; }
        public UnityAction<int> CurrentQuality { get; set; }
        public UnityAction<float> CurrentVolume { get; set; }
    }
}