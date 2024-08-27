using System;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.MVP.Model
{
    public interface IModel
    {
        void SubscribeToEvents(ref Func<int> levelAmountRequestedForDisplay, ref UnityAction<int> pressingTheSelectedLevel) {}
        void SubscribeToEvents(ref UnityAction<float> monitorInputRotation) {}
    }
}
