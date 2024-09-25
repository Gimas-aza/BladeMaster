using Assets.DI;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StateSettingsMenu : IStateView
    {
        private UIElements _uiElements;
        private StateMachine _stateMachine;
        private UIEvents _uiEvents;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _stateMachine = stateMachine;
            _uiEvents = events;
            _uiElements = elements;

            InitializeUIEvents();
            SubscribeToMonitorUpdate();

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
            _uiElements.ButtonsBack[typeof(StateSettingsMenu)].clicked += BackToPreviousState;
            _uiElements.DropdownQuality.RegisterValueChangedCallback(ChangeQuality);
            _uiElements.SliderVolume.RegisterValueChangedCallback(ChangeVolume);
        }

        private void SubscribeToMonitorUpdate()
        {
            _uiEvents.UnregisterSettingsMenuEvents();
            _uiEvents.CurrentQuality += (value) => _uiElements.DropdownQuality.index = value;
            _uiEvents.CurrentVolume += (value) => _uiElements.SliderVolume.value = value * 100;
        }

        private void BackToPreviousState()
        {
            _stateMachine.BackToPreviousState();
        }

        private void ChangeQuality(ChangeEvent<string> evt)
        {
            _uiEvents.OnChangeQuality(_uiElements.DropdownQuality.index);
        }

        private void ChangeVolume(ChangeEvent<float> evt)
        {
            _uiEvents.OnChangeVolume(_uiElements.SliderVolume.value / 100);
        }

        private void ShowMenu()
        {
            _uiElements.Menus[typeof(StateSettingsMenu)].style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.Menus[typeof(StateSettingsMenu)].style.display = DisplayStyle.None;
        }
    }
}