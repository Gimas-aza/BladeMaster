using System;
using System.Collections.Generic;
using Assets.Enemy;
using Assets.EntryPoint;
using Assets.Knife;
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
        private int _finishedLevels = 0;
        private int _currentLevel;
        private int _amountLevels;
        private int _unlockedLevels = 1;
        private int _pointsPerStroke;
        private int _moneyPerHit = 10;
        private int _amountHits;
        private int _multiplier;
        private List<int> _ratingScoreOfLevels;
        private List<ITarget> _enemies;
        private List<IKnife> _knives;
        private IItemSkin _currentSkin;
        private ISaveSystem _saveSystem;
        private IPlayerProgressionData _dataStorage;
        private UnityAction<int> _monitorCounter;
        private UnityAction<int> _monitorMoney;
        private UnityAction<int> _monitorBestScore;
        private UnityAction<bool> _finishedLevel;
        private UnityAction<int> _displayRatingScore;

        public void Init(
            IShop shop,
            ISaveSystem saveSystem,
            ref IPlayerProgressionData dataStorage,
            ILevelManager levelManager
        )
        {
            shop.RequestToBuy += TrySpentMoney;
            shop.BoughtSkin += (item) => _currentSkin = item;
            _currentSkin = shop.GetEquippedItem();

            _amountLevels = levelManager.GetAmountLevels();
            _saveSystem = saveSystem;
            _dataStorage = dataStorage;
            _money = dataStorage.Money;
            _bestScore = dataStorage.BestScore;
            _finishedLevels = dataStorage.FinishedLevels;
            _unlockedLevels = dataStorage.UnlockedLevels;
            _ratingScoreOfLevels = dataStorage.RatingScoreOfLevels;

            for (int i = _ratingScoreOfLevels.Count; i < _amountLevels; i++)
                _ratingScoreOfLevels.Add(0);

            dataStorage.RatingScoreOfLevels = _ratingScoreOfLevels;
        }

        public void Init(
            ISpawnerEnemies spawnerEnemies,
            IKnivesPool knivesPool,
            ILevelManager levelManager
        )
        {
            _enemies = spawnerEnemies.GetEnemies();
            _knives = knivesPool.GetKnives();
            _currentLevel = levelManager.GetLevelIndex();
            _counter = 0;
            _pointsPerStroke = 20;
            _multiplier = 0;
            _maxCounter = 0;

            foreach (var enemy in _enemies)
            {
                _maxCounter += enemy.GetPointsPerStroke() * 2;
            }

            foreach (var knife in _knives)
            {
                knife.Hit += StartActionForHit;
                knife.NoHit += StartActionForNoHit;

                if (_currentSkin != null)
                    knife.SwitchSkin(_currentSkin.GetSkin());
            }
        }

        public void SubscribeToEvents(ref Func<int> unlockedLevels)
        {
            unlockedLevels += () => _unlockedLevels;
        }

        public void SubscribeToEvents(
            ref UnityAction<int> monitorCounter,
            ref UnityAction<int> monitorMoney,
            ref UnityAction<bool> finishedLevel,
            ref UnityAction<int> displayRatingScore
        )
        {
            _monitorCounter = monitorCounter;
            _monitorMoney = monitorMoney;
            _finishedLevel = finishedLevel;
            _displayRatingScore = displayRatingScore;

            _monitorCounter?.Invoke(_counter);
            _monitorMoney?.Invoke(_money);
        }

        public void SubscribeToEvents(
            ref UnityAction<int> monitorMoney,
            ref UnityAction<int> monitorBestScore,
            ref Func<int, int> ratingScoreReceived
        )
        {
            _monitorMoney = monitorMoney;
            _monitorBestScore = monitorBestScore;

            _monitorMoney?.Invoke(_money);
            _monitorBestScore?.Invoke(_bestScore);

            ratingScoreReceived += (index) => _ratingScoreOfLevels[index];
        }

        private void StartActionForHit(ITarget target)
        {
            _amountHits++;
            _multiplier++;
            AddCounter(target.GetPointsPerStroke(), _multiplier);
            AddMoney();
            _monitorCounter?.Invoke(_counter);
            _monitorMoney?.Invoke(_money);

            EvaluateLevelCompletionStatus();
        }

        private void StartActionForNoHit()
        {
            SubtractCounter();
            _multiplier = 0;
            _monitorCounter?.Invoke(_counter);
            _monitorMoney?.Invoke(_money);

            EvaluateLevelCompletionStatus();
        }

        private void EvaluateLevelCompletionStatus()
        {
            var firstWin = _currentLevel != _finishedLevels;

            if (_amountHits == _enemies.Count)
                CompletedLevel(firstWin);
            else if (IsKnivesAllThrown() && _amountHits != _enemies.Count)
                FailedLevel();
        }

        private bool IsKnivesAllThrown()
        {
            foreach (var knife in _knives)
            {
                if (!knife.IsThrow())
                    return false;
            }
            return true;
        }

        private void AddCounter(int points, int multiplier = 1)
        {
            _pointsPerStroke = points;
            _counter += points * multiplier;
            _bestScore = _counter > _bestScore ? _counter : _bestScore;
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

            var currentRatingScore = CalculateRatingScore();
            _ratingScoreOfLevels[_currentLevel - 1] =
                _ratingScoreOfLevels[_currentLevel - 1] < currentRatingScore
                    ? currentRatingScore
                    : _ratingScoreOfLevels[_currentLevel - 1];
            _dataStorage.RatingScoreOfLevels = _ratingScoreOfLevels;
            _saveSystem.SaveAsync();

            _displayRatingScore?.Invoke(currentRatingScore);
            _amountHits = 0;
            _finishedLevel?.Invoke(true);
        }

        private void FailedLevel()
        {
            _amountHits = 0;
            _finishedLevel?.Invoke(false);
        }

        private int CalculateRatingScore()
        {
            var percent = (float)_counter / _maxCounter * 100;
            var sector = 100 / 3;
            var result = (int)Math.Ceiling(percent / sector);
            return Math.Clamp(result, 0, 3);
        }

        private bool TrySpentMoney(int price)
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
