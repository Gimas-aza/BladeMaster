using System;
using System.Collections.Generic;
using Assets.EntryPoint;
using Assets.MVP.Model;
using UnityEngine.Events;

namespace Assets.MVP
{
    public class Presenter : IInitializer 
    {
        private List<IModel> _models;
        private Func<int> _levelAmountRequestedForDisplay;
        private UnityAction<int> _pressingTheSelectedLevel;

        public void Init(List<IModel> model)
        {
            _models = model;
        }

        public void RegisterEventsForView(ref Func<int> levelAmountRequestedForDisplay, ref UnityAction<int> pressingTheSelectedLevel)
        {
            _levelAmountRequestedForDisplay = levelAmountRequestedForDisplay;
            _pressingTheSelectedLevel = pressingTheSelectedLevel;

            foreach (var model in _models)
            {
                model.SubscribeToEvents(ref levelAmountRequestedForDisplay, ref pressingTheSelectedLevel);
            }
        }
    }
}