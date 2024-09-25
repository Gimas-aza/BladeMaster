using Assets.DI;
using Assets.MVP.State;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateMainMenu : IStateView
    {
        private UIElements _uiElements;
        private StateMachine _stateMachine;
        private UIEvents _uiEvents;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _stateMachine = stateMachine;
            _uiEvents = events;
            _uiElements = elements;

            InitializeUI();
            SubscribeToMonitorUpdate();

            _stateMachine.RegisterEvents();
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
            _uiElements.ButtonPlay.clicked += () => _stateMachine.ChangeState<StateLevelsMenu>();
            _uiElements.ButtonShop.clicked += () => _stateMachine.ChangeState<StateShopMenu>();
            _uiElements.ButtonsSettings[typeof(StateMainMenu)].clicked += () => _stateMachine.ChangeState<StateSettingsMenu>();
            _uiElements.ButtonExit.clicked += Application.Quit;
        }

        private void SubscribeToMonitorUpdate()
        {
            _uiEvents.MonitorBestScore += SetBestScore;
        }

        private async void SetBestScore(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            _uiElements.BestScore.text = $"{amount}";
        }

        private void ShowMenu() => _uiElements.Menus[typeof(StateMainMenu)].style.display = DisplayStyle.Flex;

        private void HideMenu() => _uiElements.Menus[typeof(StateMainMenu)].style.display = DisplayStyle.None;
    }
}
