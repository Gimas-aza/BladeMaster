using System;
using System.Collections.Generic;
using Assets.MVP.Model;
using Assets.ShopManagement;
using UnityEngine.Events;

namespace Assets.MVP
{
    public class UIEvents : IUIEvents
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

        public int OnLevelAmountRequestedForDisplay() => LevelAmountRequestedForDisplay?.Invoke() ?? 0;
        public void OnPressingTheSelectedLevel(int level) => PressingTheSelectedLevel?.Invoke(level);
        public int OnUnlockedLevels() => UnlockedLevels?.Invoke() ?? 0;
        public List<IItem> OnItemsRequestedForDisplay() => ItemsRequestedForDisplay?.Invoke() ?? new List<IItem>();
        public void OnItemRequestedForBuy(IItem item) => ItemRequestedForBuy?.Invoke(item);
        public void OnEquipItem(IItem item) => EquipItem?.Invoke(item);
        public int OnRatingScoreReceived(int level) => RatingScoreReceived?.Invoke(level) ?? 0;
        public void OnChangeQuality(int quality) => ChangeQuality?.Invoke(quality);
        public void OnChangeVolume(float volume) => ChangeVolume?.Invoke(volume);
        public void OnButtonButtonAgainClick() => ClickedButtonAgainLevel?.Invoke();
        public void OnButtonBackMainMenuClick() => ClickedButtonBackMainMenu?.Invoke();
        public void OnMonitorInputRotation(float value) => MonitorInputRotation?.Invoke(value);
        public IForceOfThrowingKnife OnMonitorInputTouchBegin() => MonitorInputTouchBegin?.Invoke();
        public void OnMonitorInputTouchEnded() => MonitorInputTouchEnded?.Invoke();

        public void UnregisterLevelManagerEvents()
        {
            LevelAmountRequestedForDisplay = null;
            PressingTheSelectedLevel = null;
            ClickedButtonBackMainMenu = null;
            ClickedButtonAgainLevel = null;
        }

        public void UnregisterPlayerProgressionEvents()
        {
            UnlockedLevels = null;
            RatingScoreReceived = null;
        }

        public void UnregisterPlayerEvents()
        {
            MonitorInputRotation = null;
            MonitorInputTouchBegin = null;
            MonitorInputTouchEnded = null;
        }

        public void UnregisterShopEvents()
        {
            ItemsRequestedForDisplay = null;
            ItemRequestedForBuy = null;
            EquipItem = null;
        }

        public void UnregisterSettingsEvents()
        {
            ChangeQuality = null;
            ChangeVolume = null;
        }

        public void UnregisterSettingsMenuEvents()
        {
            CurrentQuality = null;
            CurrentVolume = null;
        }
    }
}