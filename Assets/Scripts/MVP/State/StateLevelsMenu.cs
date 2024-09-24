using System;
using Assets.DI;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StateLevelsMenu : IStateView
    {
        private VisualTreeAsset _templateButtonStartLevel;
        private UIEvents _uiEvents;
        private UIElements _uiElements;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _uiElements = elements;
            _uiEvents = events;
            _templateButtonStartLevel = container.Resolve<VisualTreeAsset>(
                "templateButtonStartLevel"
            );

            stateMachine.RegisterEvents();

            AddButtonsStartLevel();
        }

        public void Enter()
        {
            ShowMenu();
        }

        public void Exit()
        {
            HideMenu();
        }

        private void AddButtonsStartLevel()
        {
            var levelAmount = _uiEvents.OnLevelAmountRequestedForDisplay();
            var unlockedLevels = _uiEvents.OnUnlockedLevels();
            if (levelAmount == 0)
                return;

            CreateButtonsStartLevel(levelAmount);
            for (var i = 0; i < levelAmount; i++)
            {
                int index = i;
                _uiElements.ButtonsStartLevel[i].style.display = DisplayStyle.Flex;
                _uiElements.ButtonsStartLevel[i].enabledSelf = false;
                _uiElements.ButtonsStartLevel[i].clicked += () =>
                    _uiEvents.OnPressingTheSelectedLevel(index + 1);
                SetRatingScoreForButtonLevel(i, _uiEvents.OnRatingScoreReceived(i));
            }
            for (var i = 0; i < unlockedLevels; i++)
            {
                if (i >= levelAmount)
                    break;
                _uiElements.ButtonsStartLevel[i].enabledSelf = true;
            }
        }

        private void CreateButtonsStartLevel(int levelAmount)
        {
            for (var i = 0; i < levelAmount; i++)
            {
                var newTemplateButtonStartLevel = _templateButtonStartLevel.CloneTree();
                var newButtonStartLevel = newTemplateButtonStartLevel.Q<Button>("ButtonStartLevel");
                newButtonStartLevel.text = $"Уровень {i + 1}";

                _uiElements.ContainerButtonsStartLevel.Add(newButtonStartLevel);
                _uiElements.ButtonsStartLevel.Add(newButtonStartLevel);
            }
        }

        private void SetRatingScoreForButtonLevel(int index, int ratingScore)
        {
            var ratingScoreFull = _uiElements
                .ButtonsStartLevel[index]
                .Query<VisualElement>("RatingScoreFull")
                .ToList();
            var ratingScoreEmpty = _uiElements
                .ButtonsStartLevel[index]
                .Query<VisualElement>("RatingScoreEmpty")
                .ToList();

            for (int i = 0; i < ratingScoreEmpty.Count; i++)
            {
                if (i < ratingScore)
                {
                    ratingScoreFull[i].style.display = DisplayStyle.Flex;
                    ratingScoreEmpty[i].style.display = DisplayStyle.None;
                }
            }
        }

        private void ShowMenu() => _uiElements.Menus[StateMenu.LevelsMenu].style.display = DisplayStyle.Flex;

        private void HideMenu() => _uiElements.Menus[StateMenu.LevelsMenu].style.display = DisplayStyle.None;
    }
}