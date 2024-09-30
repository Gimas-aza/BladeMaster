using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EntryPoint;
using Assets.MVP;
using Assets.EntryPoint.Model;
using Assets.Target;
using UnityEngine;
using Assets.GameProgression.Interfaces;

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
        private int _unlockedLevels = 1;
        private int _pointsPerStroke;
        private float _averageEnemyPointsMultiplier = 1.5f;
        private int _moneyPerHit = 10;
        private int _amountHits;
        private int _multiplier;
        private int _amountOfLevels;
        private List<int> _ratingScoreOfLevels;
        private List<IPointsPerActionProvider> _enemies;
        private List<IWeaponEvents> _knives;
        private IItemSkin _currentSkin;
        private ISaveSystem _saveSystem;
        private IPlayerProgressionData _dataStorage;
        private IUIEvents _uiEvents;

        public void Init(IResolver container)
        {
            var currentState = container.Resolve<StateGame>();

            if (currentState == StateGame.MainMenu)
                InitializeMainMenu(container);
            else if (currentState == StateGame.GameMenu)
                InitializeGameMenu(container);
        }

        public void SubscribeToEvents(IResolver container)
        {
            _uiEvents = container.Resolve<IUIEvents>();

            _uiEvents.UnregisterPlayerProgressionEvents();
            UpdateUI();
            SubscribeToUIEvents(_uiEvents);
        }

        ~PlayerProgression()
        {
            _uiEvents.UnregisterPlayerProgressionEvents();
        }

        private void InitializeMainMenu(IResolver container)
        {
            var shop = container.Resolve<IShop>();

            _amountOfLevels = container.Resolve<ILevelInfoProvider>().GetAmountLevels();
            _saveSystem = container.Resolve<ISaveSystem>();
            _dataStorage = container.Resolve<IPlayerProgressionData>();

            shop.RequestToBuy += TrySpendMoney;
            shop.BoughtSkin += (item) => _currentSkin = item;
            _currentSkin = shop.GetEquippedItem();

            LoadPlayerData();
            InitializeRatingScores();
        }

        private void InitializeGameMenu(IResolver container)
        {
            var spawnerEnemies = container.Resolve<IEnemySpawner>();
            var knivesPool = container.Resolve<IKnivesPool>();

            _currentLevel = container.Resolve<ILevelInfoProvider>().GetLevelIndex();
            _dataStorage = container.Resolve<IPlayerProgressionData>();
            _enemies = spawnerEnemies.GetEnemies();
            _knives = knivesPool.GetKnives();
            ResetGameState();

            _maxCounter = CalculateMaxCounter();
            LoadPlayerData();
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
            _maxCounter = 0;
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
            AddPoints(scoreProvider.GetPointsPerStroke(), _multiplier);
            AddMoney();
            UpdateUI();
            EvaluateLevelCompletion();
        }

        private void StartActionForNoHit()
        {
            SubtractPoints();
            _multiplier = 0;
            UpdateUI();
            EvaluateLevelCompletion();
        }

        private void UpdateUI()
        {
            _uiEvents.MonitorCounter?.Invoke(_counter);
            _uiEvents.MonitorMoney?.Invoke(_money);
            _uiEvents.MonitorBestScore?.Invoke(_bestScore);
        }

        private void EvaluateLevelCompletion()
        {
            if (_amountHits == _enemies.Count)
                CompleteLevel(_currentLevel > _finishedLevels);
            else if (AreAllKnivesThrown() && _amountHits != _enemies.Count)
                FailLevel();
        }

        private bool AreAllKnivesThrown() => _knives.All(knife => knife.IsThrow());

        private void AddPoints(int points, int multiplier = 1)
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
            _uiEvents.MonitorBestScore?.Invoke(_bestScore);
        }

        private void AddMoney()
        {
            _money += _moneyPerHit;
            _dataStorage.Money = _money;
            _saveSystem.SaveAsync();
        }

        private void SubtractPoints()
        {
            _counter -= _pointsPerStroke / 2;
        }

        private void SubtractMoney(int amount)
        {
            _money -= amount;
            _dataStorage.Money = _money;
            _saveSystem.SaveAsync();
        }

        private void CompleteLevel(bool isFirstWin)
        {
            if (isFirstWin)
            {
                _finishedLevels++;
                _unlockedLevels++;
                _dataStorage.FinishedLevels = _finishedLevels;
                _dataStorage.UnlockedLevels = _unlockedLevels;
            }

            UpdateRatingScore();
            _uiEvents.FinishedLevel?.Invoke(true);
        }

        private void FailLevel()
        {
            _uiEvents.FinishedLevel?.Invoke(false);
        }

        private void UpdateRatingScore()
        {
            var currentRatingScore = CalculateRatingScore();
            _ratingScoreOfLevels[_currentLevel - 1] = Math.Max(_ratingScoreOfLevels[_currentLevel - 1], currentRatingScore);
            _dataStorage.RatingScoreOfLevels = _ratingScoreOfLevels;
            _saveSystem.SaveAsync();
            _uiEvents.DisplayRatingScore?.Invoke(currentRatingScore);
        }

        private int CalculateRatingScore()
        {
            var percentage = (float)_counter / _maxCounter * 100;
            var sector = 100 / 3;
            return Mathf.Clamp((int)Mathf.Ceil(percentage / sector), 0, 3);
        }

        private bool TrySpendMoney(int price)
        {
            if (_money >= price)
            {
                SubtractMoney(price);
                _uiEvents.MonitorMoney?.Invoke(_money);
                return true;
            }
            return false;
        }

        private int CalculateMaxCounter() => _enemies.Sum(enemy => Mathf.CeilToInt(enemy.GetPointsPerStroke() * _averageEnemyPointsMultiplier));
    }
}
