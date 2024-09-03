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
        private VisualElement _gameMenu;
        private VisualElement _gameMenuInfo;
        private VisualElement _gameMenuStatistic;
        private VisualElement _pauseMenu;
        private VisualElement _pauseMenuStatistic;
        private List<Label> _counter;
        private List<Label> _money;

        // ==========================================================
        private Button _buttonPause;
        private Button _buttonContinue;
        private Button _buttonSettings;
        private Button _buttonBackMainMenu;

        public event UnityAction<float> MonitorInputRotation;
        public event UnityAction MonitorInputTouchBegin;
        public event UnityAction MonitorInputTouchEnded;
        public event UnityAction ClickedButtonBackMainMenu;
        // ==========================================================
        public UnityAction<int> MonitorCounter;
        public UnityAction<int> MonitorMoney;

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
                ref MonitorMoney
            );
            OnMonitorInputInFixedUpdate();
            OnMonitorInputInUpdate();
            Start();
        }

        private void Start()
        {
            _gameMenu = _root.Q<VisualElement>("GameMenu");
            _gameMenuInfo = _root.Q<VisualElement>("GameMenu-Info");
            _gameMenuStatistic = _root.Q<VisualElement>("GameMenu__Statistic");
            _pauseMenu = _root.Q<VisualElement>("PauseMenu");
            _pauseMenuStatistic = _root.Q<VisualElement>("PauseMenu__Statistic");
            _counter = _root.Query<Label>("Counter").ToList();
            _money = _root.Query<Label>("Money").ToList();
            _buttonPause = _root.Q<Button>("ButtonPause");
            _buttonContinue = _root.Q<Button>("ButtonContinue");
            _buttonSettings = _root.Q<Button>("ButtonSettings");
            _buttonBackMainMenu = _root.Q<Button>("ButtonBackMainMenu");

            _buttonPause.clicked += OnButtonPauseClick;
            _buttonContinue.clicked += OnButtonContinueClick;
            _buttonSettings.clicked += OnButtonSettingsClick;
            _buttonBackMainMenu.clicked += OnButtonBackMainMenuClick;

            _gameMenu.style.display = DisplayStyle.Flex;
            AdjustLayout();
        }

        private void SubscribeToMonitorUpdate()
        {
            MonitorCounter += SetCount;
            MonitorMoney += SetMoney;
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
            throw new NotImplementedException();
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
            while (true)
            {
                if (Input.touchCount > 0 && _isGameActive)
                {
                    var touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        _isClampingTouch = true;
                        MonitorInputTouchBegin?.Invoke();
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (!_isClampingTouch)
                            return;

                        _isClampingTouch = false;
                        MonitorInputTouchEnded?.Invoke();
                    }
                }

                await UniTask.WaitForEndOfFrame();
            }
        }

        private void SetCount(int amount)
        {
            foreach (var counter in _counter)
                counter.text = $"{amount}";
            AdjustLayout();
        }

        private void SetMoney(int amount)
        {
            foreach (var money in _money)
                money.text = $"{amount}";
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            for (int i = 0; i < _counter.Count; i++)
            {
                int totalLength = _counter[i].text.Length + _money[i].text.Length;
                _pauseMenuStatistic.style.flexDirection =
                    totalLength > 6 ? FlexDirection.Column : FlexDirection.Row;
                _gameMenuStatistic.style.flexDirection =
                    totalLength > 7 ? FlexDirection.Column : FlexDirection.Row;
            }
        }
    }
}
