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

        public void RegisterEventsForView(
            ref Func<int> levelAmountRequestedForDisplay,
            ref UnityAction<int> pressingTheSelectedLevel,
            ref Func<int> unlockedLevels
        )
        {
            foreach (var model in _models)
            {
                model.SubscribeToEvents(
                    ref levelAmountRequestedForDisplay,
                    ref pressingTheSelectedLevel
                );
                model.SubscribeToEvents(
                    ref unlockedLevels
                );
            }
        }

        public void RegisterEventsForView(
            ref UnityAction<float> monitorInputRotation,
            ref UnityAction monitorInputTouchBegin,
            ref UnityAction monitorInputTouchEnded,
            ref UnityAction clickedButtonBackMainMenu,
            ref UnityAction<int> monitorCounter,
            ref UnityAction<int> monitorMoney,
            ref UnityAction<bool> finishedLevel,
            ref UnityAction clickedButtonAgainLevel
        )
        {
            foreach (var model in _models)
            {
                model.SubscribeToEvents(
                    ref monitorInputRotation,
                    ref monitorInputTouchBegin,
                    ref monitorInputTouchEnded
                );
                model.SubscribeToEvents(
                    ref clickedButtonBackMainMenu,
                    ref clickedButtonAgainLevel
                );
                model.SubscribeToEvents(
                    ref monitorCounter,
                    ref monitorMoney,
                    ref finishedLevel
                );
            }
        }
    }
}
