using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EntryPoint;
using Assets.Knife;
using Assets.MVP;
using Assets.MVP.Model;
using Assets.Player;
using Assets.Target;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameProgression
{
    public class PlayerProgression : IInitializer, IModel
    {
        private int _counter;
        private int _maxCounter;
        private int _bestScore;
        private int _money;
        private int _finishedLevels;
        private int _currentLevel;
        private int _amountOfLevels;
        private int _unlockedLevels = 1;
        private int _pointsPerStroke;
        private int _moneyPerHit = 10;
        private int _amountHits;
        private int _multiplier;
        private List<int> _ratingScoreOfLevels;
        private List<IPointsPerActionProvider> _enemies;
        private List<IWeaponEvents> _knives;
        private IItemSkin _currentSkin;
        private ISaveSystem _saveSystem;
        private IPlayerProgressionData _dataStorage;
        private UnityAction<int> _monitorCounter;
        private UnityAction<int> _monitorMoney;
        private UnityAction<int> _monitorBestScore;
        private UnityAction<bool> _finishedLevel;
        private UnityAction<int> _displayRatingScore;

        public void Init(IResolver container)
        {
            var currentState = container.Resolve<StateView>();

            if (currentState == StateView.MainMenu)
                InitializeDependenciesForMainMenu(container);
            else if (currentState == StateView.GameMenu)
                InitializeDependenciesForGameMenu(container);
        }

        public void SubscribeToEvents(IResolver container)
        {
            var uiEvents = container.Resolve<IUIEvents>();

            _monitorMoney = uiEvents.MonitorMoney;
            _monitorCounter = uiEvents.MonitorCounter;
            _finishedLevel = uiEvents.FinishedLevel;
            _displayRatingScore = uiEvents.DisplayRatingScore;
            _monitorBestScore = uiEvents.MonitorBestScore;

            UpdateUI();
            SubscribeToUIEvents(uiEvents);
        }

        private void InitializeDependenciesForMainMenu(IResolver resolver)
        {
            var shop = resolver.Resolve<IShop>();
            _amountOfLevels = resolver.Resolve<ILevelInfoProvider>().GetAmountLevels();
            _saveSystem = resolver.Resolve<ISaveSystem>();
            _dataStorage = resolver.Resolve<IPlayerProgressionData>();

            shop.RequestToBuy += TrySpendMoney;
            shop.BoughtSkin += (item) => _currentSkin = item;
            _currentSkin = shop.GetEquippedItem();

            LoadPlayerData();
            InitializeRatingScores();
        }

        private void InitializeDependenciesForGameMenu(IResolver resolver)
        {
            var spawnerEnemies = resolver.Resolve<IEnemySpawner>();
            var knivesPool = resolver.Resolve<IKnivesPool>();
            _currentLevel = resolver.Resolve<ILevelInfoProvider>().GetLevelIndex();

            _enemies = spawnerEnemies.GetEnemies();
            _knives = knivesPool.GetKnives();
            ResetGameState();

            foreach (var enemy in _enemies)
                _maxCounter += enemy.GetPointsPerStroke() * 2;

            InitializeKnives();
        }

        private void LoadPlayerData()
        {
            _money = _dataStorage.Money;
            _bestScore = _dataStorage.BestScore;
            _finishedLevels = _dataStorage.FinishedLevels;
            _unlockedLevels = _dataStorage.UnlockedLevels;
            _ratingScoreOfLevels = _dataStorage.RatingScoreOfLevels;
        }

        private void InitializeRatingScores()
        {
            while (_ratingScoreOfLevels.Count < _amountOfLevels)
                _ratingScoreOfLevels.Add(0);

            _dataStorage.RatingScoreOfLevels = _ratingScoreOfLevels;
        }

        private void ResetGameState()
        {
            _counter = 0;
            _pointsPerStroke = 20;
            _multiplier = 0;
            _amountHits = 0;
        }

        private void InitializeKnives()
        {
            foreach (var knife in _knives)
            {
                knife.Hit += StartActionForHit;
                knife.NoHit += StartActionForNoHit;

                if (_currentSkin != null)
                    knife.SwitchSkin(_currentSkin.GetSkin());
            }
        }

        private void SubscribeToUIEvents(IUIEvents uiEvents)
        {
            uiEvents.UnlockedLevels += () => _unlockedLevels;
            uiEvents.RatingScoreReceived += index => _ratingScoreOfLevels[index];
        }

        private void StartActionForHit(ITarget target)
        {
            if (target is not IPointsPerActionProvider scoreProvider)
            {
                Debug.LogError("Target does not implement IPointsPerActionProvider.");
                return;
            }

            _amountHits++;
            _multiplier++;
            AddCounter(scoreProvider.GetPointsPerStroke(), _multiplier);
            AddMoney();
            UpdateUI();
            EvaluateLevelCompletionStatus();
        }

        private void StartActionForNoHit()
        {
            SubtractCounter();
            _multiplier = 0;
            UpdateUI();
            EvaluateLevelCompletionStatus();
        }

        private void UpdateUI()
        {
            _monitorCounter?.Invoke(_counter);
            _monitorMoney?.Invoke(_money);
            _monitorBestScore?.Invoke(_bestScore);
        }

        private void EvaluateLevelCompletionStatus()
        {
            if (_amountHits == _enemies.Count)
                CompletedLevel(_currentLevel != _finishedLevels);
            else if (IsKnivesAllThrown() && _amountHits != _enemies.Count)
                FailedLevel();
        }

        private bool IsKnivesAllThrown() => _knives.All(knife => knife.IsThrow());

        private void AddCounter(int points, int multiplier = 1)
        {
            _pointsPerStroke = points;
            _counter += points * multiplier;
            _bestScore = Math.Max(_counter, _bestScore);
            SaveBestScore();
        }

        private void SaveBestScore()
        {
            _dataStorage.BestScore = _bestScore;
            _saveSystem.SaveAsync();
            _monitorBestScore?.Invoke(_bestScore);
        }

        private void AddMoney()
        {
            _money += _moneyPerHit;
            _dataStorage.Money = _money;
            _saveSystem.SaveAsync();
        }

        private void SubtractCounter()
        {
            _counter -= _pointsPerStroke / 2;
        }

        private void SubtractMoney(int amount)
        {
            _money -= amount;
            _dataStorage.Money = _money;
            _saveSystem.SaveAsync();
        }

        private void CompletedLevel(bool isFirstWin)
        {
            if (isFirstWin)
            {
                _finishedLevels++;
                _unlockedLevels++;
                _dataStorage.FinishedLevels = _finishedLevels;
                _dataStorage.UnlockedLevels = _unlockedLevels;
            }

            UpdateRatingScore();
            _amountHits = 0;
            _finishedLevel?.Invoke(true);
        }

        private void FailedLevel()
        {
            _amountHits = 0;
            _finishedLevel?.Invoke(false);
        }

        private void UpdateRatingScore()
        {
            var currentRatingScore = CalculateRatingScore();
            _ratingScoreOfLevels[_currentLevel - 1] = Math.Max(_ratingScoreOfLevels[_currentLevel - 1], currentRatingScore);
            _dataStorage.RatingScoreOfLevels = _ratingScoreOfLevels;
            _saveSystem.SaveAsync();
            _displayRatingScore?.Invoke(currentRatingScore);
        }

        private int CalculateRatingScore()
        {
            var percent = (float)_counter / _maxCounter * 100;
            var sector = 100 / 3;
            var result = (int)Math.Ceiling(percent / sector);
            return Math.Clamp(result, 0, 3);
        }

        private bool TrySpendMoney(int price)
        {
            if (_money >= price)
            {
                SubtractMoney(price);
                _monitorMoney?.Invoke(_money);
                return true;
            }
            return false;
        }
    }
}
