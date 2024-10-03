using Assets.DI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.MVP.State
{
    public class StateGameMenu : IStateView
    {
        private float _tiltRadius = 0.25f;
        private bool _isClampingTouch = false;
        private bool _isActiveInput = true;
        private IForceOfThrowingKnife _forceOfThrowing;
        private StateMachine _stateMachine;
        private UIElements _uiElements;
        private UIEvents _uiEvents;

        public void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container)
        {
            _stateMachine = stateMachine;
            _uiElements = elements;
            _uiEvents = events;

            InitializeUI();
            SubscribeToEvents();

            stateMachine.RegisterEvents();

            OnMonitorInputInFixedUpdate();
            OnMonitorInputInUpdate();
            AdjustLayout();
        }

        public void Enter()
        {
            ShowMenu();
            ActiveInput();
        }

        public void Exit()
        {
            HideMenu();
            ClearPressureForce();
        }

        private void InitializeUI()
        {
            _uiElements.ButtonPause.clicked += SetPause;
        }

        private void SubscribeToEvents()
        {
            _uiEvents.MonitorCounter += SetCount;
            _uiEvents.MonitorMoney += SetMoney;
            _uiEvents.FinishedLevel += SetFinishedLevel;
            _uiEvents.DisplayRatingScore += SetRating;
            _uiEvents.DisplayAmountKnives += SetAmountKnives;
        }

        private void SetPause()
        {
            DeactivateInput();
            _stateMachine.ChangeState<StatePauseMenu>();
        }

        private async void OnMonitorInputInFixedUpdate()
        {
            while (true)
            {
                OnMonitorInputAcceleration();
                await UniTask.WaitForFixedUpdate();
            }
        }

        private void OnMonitorInputAcceleration()
        {
            Vector3 acceleration = Input.acceleration;
            if (Mathf.Abs(acceleration.x) > _tiltRadius && _isActiveInput)
            {
                _uiEvents.OnMonitorInputRotation(acceleration.x);
            }
        }

        private async void OnMonitorInputInUpdate()
        {
            float time = 0;
            while (true)
            {
                if (Input.touchCount > 0 && _isActiveInput)
                {
                    var touch = Input.GetTouch(0);
                    HandleTouchPhase(touch, ref time);
                }
                await UniTask.WaitForEndOfFrame();
            }
        }

        private void HandleTouchPhase(Touch touch, ref float time)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    BeginTouchPhase(touch, ref time);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    MovedOrStationaryPhase(touch, ref time);
                    break;
                case TouchPhase.Ended:
                    EndedPhase();
                    break;
            }
        }

        private void BeginTouchPhase(Touch touch, ref float time)
        {
            _isClampingTouch = true;
            time = Time.time;
            _forceOfThrowing = _uiEvents.OnMonitorInputTouchBegin();
        }

        private void MovedOrStationaryPhase(Touch touch, ref float time)
        {
            if (!_isClampingTouch) return;

            var force = _forceOfThrowing.GetPercentOfForce(Time.time - time);
            _uiElements.PressureForce.value = force;
        }

        private void EndedPhase()
        {
            if (!_isClampingTouch) return;

            ClearPressureForce();
            _uiEvents.OnMonitorInputTouchEnded();
        }

        private void SetCount(int amount)
        {
            _uiElements.Counter.ForEach(counter => counter.text = $"{amount}");
            AdjustLayout();
        }

        private void SetMoney(int amount)
        {
            _uiElements.Money.ForEach(money => money.text = $"{amount}");
            AdjustLayout();
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

        private void SetFinishedLevel(bool isWin)
        {
            DeactivateInput();
            _uiElements.Win.style.display = isWin ? DisplayStyle.Flex : DisplayStyle.None;
            _uiElements.Lose.style.display = isWin ? DisplayStyle.None : DisplayStyle.Flex;
            _stateMachine.ChangeState<StateFinishedMenu>();
        }

        private void SetRating(int score)
        {
            for (int i = 0; i < _uiElements.RatingScoreEmpty.Count; i++)
            {
                bool isFull = i < score;
                _uiElements.RatingScoreFull[i].style.display = isFull ? DisplayStyle.Flex : DisplayStyle.None;
                _uiElements.RatingScoreEmpty[i].style.display = isFull ? DisplayStyle.None : DisplayStyle.Flex;
            }
        }

        private void SetAmountKnives(IAmountOfKnives amountOfKnives)
        {
            _uiElements.AmountKnives.text = $"{amountOfKnives.Amount}/{amountOfKnives.MaxAmount}";
        }

        private void ClearPressureForce() 
        {
            _isClampingTouch = false;
            _uiElements.PressureForce.value = 0;
        }

        private async void ActiveInput()
        {
            await UniTask.Delay(200);
            _isActiveInput = true;
        }

        private void DeactivateInput()
        {
            _isActiveInput = false;
        }

        private void ShowMenu()
        {
            _uiElements.GameMenu.style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            _uiElements.GameMenu.style.display = DisplayStyle.None;
        }
    }
}
