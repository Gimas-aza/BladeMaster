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

            AdjustLayout();
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
            _uiElements.ButtonContinue.clicked += OnButtonContinueClick;
            _uiElements.ButtonBackGameMenu.clicked += OnButtonBackGameMenuClick;

            foreach (var buttonSettings in _uiElements.ButtonsSettings)
                buttonSettings.clicked += OnButtonSettingsClick;
            foreach (var buttonBackMainMenu in _uiElements.ButtonsBackMainMenu)
                buttonBackMainMenu.clicked += _uiEvents.OnButtonBackMainMenuClick;
        }

        private void OnButtonContinueClick()
        {
            _stateMachine.ChangeState<StateGameMenu>();
        }

        private void OnButtonSettingsClick()
        {
            _stateMachine.ChangeState<StateSettingsMenu>();
        }

        private void OnButtonBackGameMenuClick()
        {
            _stateMachine.BackToPreviousState();
        }

        private void AdjustLayout()
        {
            for (int i = 0; i < _uiElements.GameStatistic.Count; i++)
            {
                int totalLength = _uiElements.Counter[i].text.Length + _uiElements.Money[i].text.Length;
                _uiElements.GameStatistic[i].style.flexDirection =
                    totalLength > 6 ? FlexDirection.Column : FlexDirection.Row;
            }
        }

        private void ShowMenu()
        {
            _uiElements.PauseMenu.style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.PauseMenu.style.display = DisplayStyle.None;
        }
    }
}