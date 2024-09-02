using System;
using System.Collections.Generic;
using Assets.Enemy;
using Assets.EntryPoint;
using Assets.MVP.Model;
using Assets.Player;
using Assets.Target;
using UnityEngine;

namespace Assets.GameProgression
{
    public class PlayerProgression : IInitializer, IModel
    {
        private int _counter;
        private int _money;
        private int _finishedLevels = 0;
        private int _currentLevel;
        private int _unlockedLevels = 1;
        private int _pointsPerStroke = 50;
        private int _moneyPerHit = 10;
        private int _moneyPerNoHit = 5;
        private int _amountHits;
        private int _multiplier = 0;
        private List<EnemyComponent> _enemies;
        private List<IKnife> _knives;

        public void Init(ISpawnerEnemies spawnerEnemies, IKnivesPool knivesPool, ILevelManager levelManager)
        {
            _enemies = spawnerEnemies.GetEnemies();
            _knives = knivesPool.GetKnives();
            _currentLevel = levelManager.GetIndexOfLevel();

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

        private void StartActionForHit(ITarget target)
        {
            _amountHits++;
            _multiplier++;
            AddCounter(_multiplier);
            AddMoney();
            Debug.Log("Counter: " + _counter + " Money: " + _money);

            if (_amountHits == _enemies.Count && _currentLevel != _finishedLevels)
            {
                FinishLevel();
            }
        }

        private void StartActionForNoHit()
        {
            SubtractMoney();
            _multiplier = 0;
            Debug.Log("NoHit. Counter: " + _counter + " Money: " + _money);
        }

        private void AddCounter(int multiplier = 1)
        {
            _counter += _pointsPerStroke * multiplier;
        }

        private void AddMoney()
        {
            _money += _moneyPerHit;
        }

        private void SubtractMoney()
        {
            _money -= _moneyPerNoHit;
        }

        private void FinishLevel()
        {
            _finishedLevels++;
            _currentLevel++;
            _unlockedLevels++;
        }
    }
}
