using System.Collections.Generic;
using Assets.EntryPoint;
using Assets.GameProgression;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Enemy
{
    public class EnemySpawnerComponent : MonoBehaviour, IInitializer, ISpawnerEnemies
    {
        [SerializeField] private List<EnemyLocatorList> _transformOfEnemiesOnLevels;

        private int _currentLevel;
        private List<EnemyComponent> _enemies;

        public void Init(int levelIndex)
        {
            if (!CheckLevelIndexValidity(levelIndex, _transformOfEnemiesOnLevels.Count - 1))
                return;
            _currentLevel = levelIndex;
            _enemies = new List<EnemyComponent>();

            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            var enemyLocators = _transformOfEnemiesOnLevels[_currentLevel].enemyLocators;
            foreach (var enemyLocator in enemyLocators)
            {
                _enemies.Add(
                    Instantiate(
                        enemyLocator.enemyPrefab,
                        enemyLocator.position,
                        enemyLocator.rotation,
                        transform
                    )
                );
            }
        }

        private bool CheckLevelIndexValidity(int levelIndex, int maxIndexEnemyLocators)
        {
            if (levelIndex > maxIndexEnemyLocators)
            {
                Debug.LogWarning(
                    $"Level index out of range. Level index: {levelIndex}. Max index of enemy locators: {maxIndexEnemyLocators}"
                );
                return false;
            }
            else if (levelIndex < 0)
            {
                Debug.LogError($"Not correct level index: {levelIndex}");
                return false;
            }
            return true;
        }

        public List<EnemyComponent> GetEnemies()
        {
            return _enemies;
        }
    }
}
