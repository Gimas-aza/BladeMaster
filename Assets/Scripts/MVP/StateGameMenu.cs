using Assets.DI;
using Assets.MVP.Model;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateGameMenu : IStateView
    {
        private Presenter _presenter;
        private bool _isClampingTouch = false;
        private bool _isGameActive = true;
        private IForceOfThrowingKnife _forceOfThrowing;
        private UIElements _uiElements;
        private UIEvents _uiEvents;

        public StateGameMenu(UIElements elements, UIEvents events, DIContainer container, Presenter presenter)
        {
            _presenter = presenter;
            _uiElements = elements;
            _uiEvents = events;

            InitializeUI();
            SubscribeToEvents();

            _presenter.RegisterEventsForView(container);

            OnMonitorInputInFixedUpdate();
            OnMonitorInputInUpdate();
            AdjustLayout();
        }

        private void InitializeUI()
        {
            _uiElements.ButtonPause.clicked += OnButtonPauseClick;
            _uiElements.ButtonContinue.clicked += OnButtonContinueClick;
            _uiElements.ButtonAgain.clicked += _uiEvents.OnButtonButtonAgainClick;
            _uiElements.ButtonBackGameMenu.clicked += OnButtonBackGameMenuClick;
            _uiElements.DropdownQuality.RegisterValueChangedCallback(OnChangeQuality);
            _uiElements.SliderVolume.RegisterValueChangedCallback(OnChangeVolume);

            foreach (var buttonSettings in _uiElements.ButtonsSettings)
                buttonSettings.clicked += OnButtonSettingsClick;
            foreach (var buttonBackMainMenu in _uiElements.ButtonsBackMainMenu)
                buttonBackMainMenu.clicked += _uiEvents.OnButtonBackMainMenuClick;

            _uiElements.GameMenu.style.display = DisplayStyle.Flex;
        }

        private void OnChangeQuality(ChangeEvent<string> evt)
        {
            _uiEvents.OnChangeQuality(_uiElements.DropdownQuality.index);
        }

        private void OnChangeVolume(ChangeEvent<float> evt)
        {
            _uiEvents.OnChangeVolume(_uiElements.SliderVolume.value / 100);
        }

        private void SubscribeToEvents()
        {
            _uiEvents.MonitorCounter += SetCount;
            _uiEvents.MonitorMoney += SetMoney;
            _uiEvents.FinishedLevel += SetFinishedLevel;
            _uiEvents.DisplayAmountKnives += SetAmountKnives;
            _uiEvents.DisplayRatingScore += SetRating;
            _uiEvents.CurrentQuality += (quality) => _uiElements.DropdownQuality.index = quality;
            _uiEvents.CurrentVolume += (volume) => _uiElements.SliderVolume.value = volume * 100;
        }

        private void OnButtonPauseClick()
        {
            _uiElements.PauseMenu.style.display = DisplayStyle.Flex;
            _uiElements.GameMenuInfo.style.display = DisplayStyle.None;
            _isGameActive = false;
        }

        private async void OnButtonContinueClick()
        {
            _uiElements.PauseMenu.style.display = DisplayStyle.None;
            _uiElements.GameMenuInfo.style.display = DisplayStyle.Flex;
            await UniTask.Delay(200);
            _isGameActive = true;
        }

        private void OnButtonSettingsClick()
        {
            _uiElements.SettingsMenu.style.display = DisplayStyle.Flex;
            _uiElements.GameMenu.style.display = DisplayStyle.None;
        }

        private void OnButtonBackGameMenuClick()
        {
            _uiElements.SettingsMenu.style.display = DisplayStyle.None;
            _uiElements.GameMenu.style.display = DisplayStyle.Flex;
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
            if (Mathf.Abs(acceleration.x) > 0.3f && _isGameActive)
            {
                _uiEvents.OnMonitorInputRotation(acceleration.x);
            }
        }

        private async void OnMonitorInputInUpdate()
        {
            float time = 0;
            while (true)
            {
                if (Input.touchCount > 0 && _isGameActive)
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
                    EndedPhase(touch);
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

        private void EndedPhase(Touch touch)
        {
            if (!_isClampingTouch) return;

            _isClampingTouch = false;
            _uiElements.PressureForce.value = 0;
            _uiEvents.OnMonitorInputTouchEnded();
        }

        private async void SetCount(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            _uiElements.Counter.ForEach(counter => counter.text = $"{amount}");
            AdjustLayout();
        }

        private async void SetMoney(int amount)
        {
            await UniTask.WaitForEndOfFrame();
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
            _uiElements.FinishedLevelMenu.style.display = DisplayStyle.Flex;
            _uiElements.PauseMenu.style.display = DisplayStyle.None;
            _uiElements.GameMenuInfo.style.display = DisplayStyle.None;

            _uiElements.Win.style.display = isWin ? DisplayStyle.Flex : DisplayStyle.None;
            _uiElements.Lose.style.display = isWin ? DisplayStyle.None : DisplayStyle.Flex;

            _isGameActive = false;
        }

        private async void SetAmountKnives(IAmountOfKnives amountOfKnives)
        {
            await UniTask.WaitForEndOfFrame();
            _uiElements.AmountKnives.text = $"{amountOfKnives.Amount}/{amountOfKnives.MaxAmount}";
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
    }
}
