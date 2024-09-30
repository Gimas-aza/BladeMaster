using System.Collections.Generic;
using System.Linq;
using Assets.EntryPoint;
using Assets.GameProgression.Interfaces;
using UnityEngine;

namespace Assets.Enemy
{
    public class EnemySpawnerComponent : MonoBehaviour, IInitializer, IEnemySpawner
    {
        [SerializeField] private List<EnemyLocatorList> _transformOfEnemiesOnLevels;

        private int _currentLevel;
        private List<EnemyComponent> _enemies;

        public void Init(IResolver container)
        {
            int levelIndex = container.Resolve<ILevelInfoProvider>().GetLevelIndex();

            if (!IsLevelIndexValid(levelIndex, _transformOfEnemiesOnLevels.Count))
                return;

            _currentLevel = levelIndex - 1;
            _enemies = new List<EnemyComponent>();

            SpawnEnemies();
        }

        public List<IPointsPerActionProvider> GetEnemies() => _enemies.Cast<IPointsPerActionProvider>().ToList();

        private void SpawnEnemies()
        {
            var enemyLocators = _transformOfEnemiesOnLevels[_currentLevel].enemyLocators;
            foreach (var enemyLocator in enemyLocators)
            {
                _enemies.Add(
                    Instantiate(
                        enemyLocator.EnemyPrefab,
                        enemyLocator.Position,
                        enemyLocator.Rotation,
                        transform
                    )
                );
            }
        }

        private bool IsLevelIndexValid(int levelIndex, int maxIndex)
        {
            if (levelIndex <= 0)
            {
                Debug.LogError($"Invalid level index: {levelIndex - 1}. Level index cannot be negative.");
                return false;
            }

            if (levelIndex > maxIndex)
            {
                Debug.LogWarning($"Level index out of range. Level index: {levelIndex - 1}. Max index: {maxIndex - 1}");
                return false;
            }

            return true;
        }
    }
}
