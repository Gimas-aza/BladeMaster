using System;
using System.Collections.Generic;
using Assets.EntryPoint;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.MVP
{
    public class Presenter : IInitializer 
    {
        private List<IModel> _models;

        public void Init(List<IModel> model)
        {
            _models = model;
        }

        public void RegisterEventsForView(ref Func<int> levelAmountRequestedForDisplay, ref UnityAction<int> pressingTheSelectedLevel)
        {
            foreach (var model in _models)
            {
                model.SubscribeToEvents(ref levelAmountRequestedForDisplay, ref pressingTheSelectedLevel);
            }
        }

        public void RegisterEventsForView(ref UnityAction<float> monitorInputRotation)
        {
            foreach (var model in _models)
            {
                model.SubscribeToEvents(ref monitorInputRotation);
            }
        }
    }
}