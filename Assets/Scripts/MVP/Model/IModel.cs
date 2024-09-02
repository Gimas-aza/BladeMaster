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
    }
}
