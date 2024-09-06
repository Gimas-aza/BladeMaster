using System;
using System.Collections.Generic;
using Assets.Enemy;
using Assets.EntryPoint;
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
        private int _money;
        private int _finishedLevels = 0;
        private int _currentLevel;
        private int _unlockedLevels = 1;
        private int _pointsPerStroke;
        private int _moneyPerHit = 10;
        private int _moneyPerNoHit = 5;
        private int _amountHits;
        private int _multiplier;
        private List<ITarget> _enemies;
        private List<IKnife> _knives;
        private UnityAction<int> _monitorCounter;
        private UnityAction<int> _monitorMoney;
        private UnityAction<bool> _finishedLevel;
        private UnityAction<int> _displayRatingScore;

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
        }

        private void AddMoney()
        {
            _money += _moneyPerHit;
        }

        private void SubtractCounter()
        {
            _counter -= _pointsPerStroke / 2;
        }

        private void SubtractMoney()
        {
            _money -= _moneyPerNoHit;
        }

        private void CompletedLevel(bool isFirstWin)
        {
            if (isFirstWin)
            {
                _finishedLevels++;
                _unlockedLevels++;
            }

            _displayRatingScore?.Invoke(CalculateRatingScore());
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
    }
}
