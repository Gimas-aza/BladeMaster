using System;
using UnityEngine;
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
            ref UnityAction<float> monitorInputRotation,
            ref UnityAction monitorInputTouchBegin,
            ref UnityAction monitorInputTouchEnded
        ) { }
        void SubscribeToEvents(
            ref UnityAction clickedButtonBackMainMenu,
            ref UnityAction clickedButtonAgainLevel
        ) { }
        void SubscribeToEvents(
            ref UnityAction<int> monitorCounter,
            ref UnityAction<int> monitorMoney,
            ref UnityAction<bool> finishedLevel
        ) { }
    }
}
