using System;
using System.Collections.Generic;
using Assets.EntryPoint;
using Assets.MVP.Model;
using Assets.ShopManagement;
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
            ref Func<int> unlockedLevels,
            ref Func<List<IItem>> itemsRequestedForDisplay,
            ref UnityAction<IItem> itemRequestedForBuy,
            ref UnityAction<IItem> equipItem,
            ref UnityAction<IItem> itemIsBought,
            ref UnityAction<int> monitorMoney,
            ref UnityAction<int> monitorBestScore,
            ref Func<int, int> ratingScoreReceived
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
                model.SubscribeToEvents(
                    ref itemsRequestedForDisplay,
                    ref itemRequestedForBuy,
                    ref equipItem,
                    ref itemIsBought
                );
                model.SubscribeToEvents(
                    ref monitorMoney,
                    ref monitorBestScore,
                    ref ratingScoreReceived
                );
            }
        }

        public void RegisterEventsForView(
            ref UnityAction<float> monitorInputRotation,
            ref Func<IForceOfThrowingKnife> monitorInputTouchBegin,
            ref UnityAction monitorInputTouchEnded,
            ref UnityAction clickedButtonBackMainMenu,
            ref UnityAction<int> monitorCounter,
            ref UnityAction<int> monitorMoney,
            ref UnityAction<bool> finishedLevel,
            ref UnityAction<IAmountOfKnives> displayAmountKnives,
            ref UnityAction clickedButtonAgainLevel,
            ref UnityAction<int> displayRatingScore
        )
        {
            foreach (var model in _models)
            {
                model.SubscribeToEvents(
                    ref monitorInputRotation,
                    ref monitorInputTouchBegin,
                    ref monitorInputTouchEnded,
                    ref displayAmountKnives
                );
                model.SubscribeToEvents(
                    ref clickedButtonBackMainMenu,
                    ref clickedButtonAgainLevel
                );
                model.SubscribeToEvents(
                    ref monitorCounter,
                    ref monitorMoney,
                    ref finishedLevel,
                    ref displayRatingScore
                );
            }
        }
    }
}
