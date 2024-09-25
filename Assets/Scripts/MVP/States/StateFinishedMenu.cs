using Assets.DI;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StateFinishedMenu : IStateView
    {
        private StateMachine _stateMachine;
        private UIElements _uiElements;
        private UIEvents _uiEvents;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _stateMachine = stateMachine;
            _uiElements = elements;
            _uiEvents = events;

            InitializeUI();

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

        private void InitializeUI()
        {
            _uiElements.ButtonAgain.clicked += _uiEvents.OnButtonButtonAgainClick;
            _uiElements.ButtonsSettings[typeof(StateFinishedMenu)].clicked += OpenSettingsMenu;
            _uiElements.ButtonsBackMainMenu[typeof(StateFinishedMenu)].clicked += _uiEvents.OnButtonBackMainMenuClick;
        }

        private void OpenSettingsMenu()
        {
            _stateMachine.ChangeState<StateSettingsMenu>();
        }

        private void ShowMenu()
        {
            _uiElements.Menus[typeof(StateFinishedMenu)].style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.Menus[typeof(StateFinishedMenu)].style.display = DisplayStyle.None;
        }
    }
}