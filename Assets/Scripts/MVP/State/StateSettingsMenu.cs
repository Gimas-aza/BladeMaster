using System;
using Assets.DI;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StateSettingsMenu : IStateView
    {
        private UIElements _uiElements;
        private UIEvents _uiEvents;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _uiEvents = events;
            _uiElements = elements;

            InitializeUIEvents();

            stateMachine.RegisterEvents();
        }

        public void Enter()
        {
            ShowMenu();
        }

        public void Exit()
        {
            HideMenu();
        }

        private void InitializeUIEvents()
        {
            _uiElements.DropdownQuality.RegisterValueChangedCallback(OnChangeQuality);
            _uiElements.SliderVolume.RegisterValueChangedCallback(OnChangeVolume);
        }

        private void OnChangeQuality(ChangeEvent<string> evt)
        {
            _uiEvents.OnChangeQuality(_uiElements.DropdownQuality.index);
        }

        private void OnChangeVolume(ChangeEvent<float> evt)
        {
            _uiEvents.OnChangeVolume(_uiElements.SliderVolume.value / 100);
        }

        private void ShowMenu()
        {
            _uiElements.Menus[StateMenu.SettingsMenu].style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.Menus[StateMenu.SettingsMenu].style.display = DisplayStyle.None;
        }
    }
}