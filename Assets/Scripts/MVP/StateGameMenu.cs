using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateGameMenu : IStateView
    {
        private VisualElement _root;
        private Presenter _presenter;
        private bool _isClampingTouch = false;
        private bool _isGameActive = true;
        private IForceOfThrowingKnife _forceOfThrowing;
        private VisualElement _gameMenu;
        private VisualElement _gameMenuInfo;
        private List<VisualElement> _gameStatistic;
        private VisualElement _pauseMenu;
        private VisualElement _finishedLevelMenu;
        private VisualElement _pauseMenuStatistic;
        private List<VisualElement> _ratingScoreEmpty;
        private List<VisualElement> _ratingScoreFull;
        // ==========================================================
        private List<Label> _counter;
        private List<Label> _money;
        private Label _win;
        private Label _lose;
        private Label _amountKnives;
        private ProgressBar _pressureForce;
        // ==========================================================
        private Button _buttonPause;
        private Button _buttonContinue;
        private List<Button> _buttonsSettings;
        private List<Button> _buttonsBackMainMenu;
        private Button _buttonButtonAgain;

        public event UnityAction<float> MonitorInputRotation;
        public event Func<IForceOfThrowingKnife> MonitorInputTouchBegin;
        public event UnityAction MonitorInputTouchEnded;
        public event UnityAction ClickedButtonBackMainMenu;
        public event UnityAction ClickedButtonAgainLevel;
        // ==========================================================
        public UnityAction<int> MonitorCounter;
        public UnityAction<int> MonitorMoney;
        public UnityAction<bool> FinishedLevel;
        public UnityAction<IAmountOfKnives> DisplayAmountKnives;
        public UnityAction<int> DisplayRatingScore;

        public StateGameMenu(VisualElement root, Presenter presenter)
        {
            _root = root;
            SubscribeToMonitorUpdate();
            presenter.RegisterEventsForView(
                ref MonitorInputRotation,
                ref MonitorInputTouchBegin,
                ref MonitorInputTouchEnded,
                ref ClickedButtonBackMainMenu,
                ref MonitorCounter,
                ref MonitorMoney,
                ref FinishedLevel,
                ref DisplayAmountKnives,
                ref ClickedButtonAgainLevel,
                ref DisplayRatingScore
            );
            OnMonitorInputInFixedUpdate();
            OnMonitorInputInUpdate();
            Start();
        }

        private void Start()
        {
            _gameMenu = _root.Q<VisualElement>("GameMenu");
            _gameMenuInfo = _root.Q<VisualElement>("GameMenu-Info");
            _gameStatistic = _root.Query<VisualElement>("Statistic").ToList();
            _pauseMenu = _root.Q<VisualElement>("PauseMenu");
            _finishedLevelMenu = _root.Q<VisualElement>("FinishedLevelMenu");
            _ratingScoreEmpty = _finishedLevelMenu.Query<VisualElement>("RatingScoreEmpty").ToList();
            _ratingScoreFull = _finishedLevelMenu.Query<VisualElement>("RatingScoreFull").ToList();
            _counter = _root.Query<Label>("Counter").ToList();
            _money = _root.Query<Label>("Money").ToList();
            _buttonPause = _root.Q<Button>("ButtonPause");
            _buttonContinue = _root.Q<Button>("ButtonContinue");
            _buttonsSettings = _root.Query<Button>("ButtonSettings").ToList();
            _buttonsBackMainMenu = _root.Query<Button>("ButtonBackMainMenu").ToList();
            _win = _root.Q<Label>("LabelWin");
            _lose = _root.Q<Label>("LabelLose");
            _amountKnives = _root.Q<Label>("LabelAmountKnives");
            _pressureForce = _root.Q<ProgressBar>("ProgressBarPressureForce");
            _buttonButtonAgain = _root.Q<Button>("ButtonAgain");

            _buttonPause.clicked += OnButtonPauseClick;
            _buttonContinue.clicked += OnButtonContinueClick;
            _buttonButtonAgain.clicked += OnButtonButtonAgainClick;

            foreach (var buttonSettings in _buttonsSettings)
                buttonSettings.clicked += OnButtonSettingsClick;
            foreach (var buttonBackMainMenu in _buttonsBackMainMenu)
                buttonBackMainMenu.clicked += OnButtonBackMainMenuClick;

            _gameMenu.style.display = DisplayStyle.Flex;
            AdjustLayout();
        }

        private void OnButtonButtonAgainClick()
        {
            ClickedButtonAgainLevel?.Invoke();
        }

        private void SubscribeToMonitorUpdate()
        {
            MonitorCounter += SetCount;
            MonitorMoney += SetMoney;
            FinishedLevel += SetFinishedLevel;
            DisplayAmountKnives += SetAmountKnives;
            DisplayRatingScore += SetRating;
        }

        private void OnButtonPauseClick()
        {
            _pauseMenu.style.display = DisplayStyle.Flex;
            _gameMenuInfo.style.display = DisplayStyle.None;
            _isGameActive = false;
        }

        private async void OnButtonContinueClick()
        {
            _pauseMenu.style.display = DisplayStyle.None;
            _gameMenuInfo.style.display = DisplayStyle.Flex;
            await UniTask.Delay(200);
            _isGameActive = true;
        }

        private void OnButtonSettingsClick()
        {
            Debug.Log("OnButtonSettingsClick");
        }

        private void OnButtonBackMainMenuClick()
        {
            ClickedButtonBackMainMenu?.Invoke();
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
            if ((acceleration.x > 0.3f || acceleration.x < -0.3f) && _isGameActive)
            {
                MonitorInputRotation?.Invoke(acceleration.x);
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
            _forceOfThrowing = MonitorInputTouchBegin?.Invoke();
        }

        private void MovedOrStationaryPhase(Touch touch, ref float time)
        {
            if (!_isClampingTouch)
                return;

            var force = _forceOfThrowing.GetPercentOfForce(Time.time - time);
            _pressureForce.value = force;
        }

        private void EndedPhase(Touch touch)
        {
            if (!_isClampingTouch)
                return;

            _isClampingTouch = false;
            _pressureForce.value = 0;
            MonitorInputTouchEnded?.Invoke();
        }

        private async void SetCount(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            foreach (var counter in _counter)
                counter.text = $"{amount}";
            AdjustLayout();
        }

        private async void SetMoney(int amount)
        {
            await UniTask.WaitForEndOfFrame();
            foreach (var money in _money)
                money.text = $"{amount}";
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            for (int i = 0; i < _gameStatistic.Count; i++)
            {
                int totalLength = _counter[i].text.Length + _money[i].text.Length;
                _gameStatistic[i].style.flexDirection =
                    totalLength > 6 ? FlexDirection.Column : FlexDirection.Row;
            }
        }

        private void SetFinishedLevel(bool isWin)
        {
            _finishedLevelMenu.style.display = DisplayStyle.Flex;
            _pauseMenu.style.display = DisplayStyle.None;
            _gameMenuInfo.style.display = DisplayStyle.None;

            if (isWin)
            {
                _win.style.display = DisplayStyle.Flex;
                _lose.style.display = DisplayStyle.None;
            }
            else
            {
                _win.style.display = DisplayStyle.None;
                _lose.style.display = DisplayStyle.Flex;
            }
            _isGameActive = false;
        }

        private async void SetAmountKnives(IAmountOfKnives amountOfKnives)
        {
            await UniTask.WaitForEndOfFrame();
            _amountKnives.text = $"{amountOfKnives.Amount}/{amountOfKnives.MaxAmount}";
        }

        private void SetRating(int score)
        {
            for (int i = 0; i < _ratingScoreEmpty.Count; i++)
            {
                if (i < score)
                {
                    _ratingScoreFull[i].style.display = DisplayStyle.Flex;
                    _ratingScoreEmpty[i].style.display = DisplayStyle.None;
                }
            }
        }
    }
}
