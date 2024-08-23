using System;
using UnityEngine.Events;

namespace Assets.MVP.Model
{
    public interface IModel
    {
        void SubscribeToEvents(ref Func<int> levelAmountRequestedForDisplay, ref UnityAction<int> pressingTheSelectedLevel) {}
    }
}
