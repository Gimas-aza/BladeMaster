using Assets.DI;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StatePauseMenu : IStateView
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
            _uiElements.ButtonContinue.clicked += ChangeStateToGameMenu;
            _uiElements.ButtonsSettings[typeof(StatePauseMenu)].clicked += OpenSettingsMenu;
            _uiElements.ButtonsBackMainMenu[typeof(StatePauseMenu)].clicked += _uiEvents.OnButtonBackMainMenuClick;
        }

        private void ChangeStateToGameMenu()
        {
            _stateMachine.ChangeState<StateGameMenu>();
        }

        private void OpenSettingsMenu()
        {
            _stateMachine.ChangeState<StateSettingsMenu>();
        }

        private void ShowMenu()
        {
            _uiElements.Menus[typeof(StatePauseMenu)].style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.Menus[typeof(StatePauseMenu)].style.display = DisplayStyle.None;
        }
    }
}