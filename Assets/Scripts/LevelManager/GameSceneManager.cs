using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.LevelManager
{
    public class GameSceneManager : LevelManager
    {
        protected override int _maxLevelIndex { get; set; }

        private readonly int _ignoredSceneIndex = 2;
        private readonly int _sceneIndexOffset = 1;

        public GameSceneManager()
        {
            _maxLevelIndex = SceneManager.sceneCountInBuildSettings - _ignoredSceneIndex;
        }

        public override async void LoadNextLevel()
        {
            if (_currentLevelIndex == _maxLevelIndex)
                return;

            await LoadSceneAsync(_nextLevelIndex);
        }

        public override async void LoadPreviousLevel()
        {
            if (_currentLevelIndex == _previousLevelIndex)
                return;

            await LoadSceneAsync(_previousLevelIndex);
        }

        public override async void LoadLevel(int sceneIndex)
        {
            if (!IsValidSceneIndex(sceneIndex))
                return;

            await LoadSceneAsync(sceneIndex);
        }

        public override async void ReloadingCurrentLevel()
        {
            await LoadSceneAsync(_currentLevelIndex);
        }

        private async UniTask LoadSceneAsync(int sceneIndex)
        {
            OnLevelStartedToLoad();

            _currentLevelIndex = sceneIndex;
            await SceneManager.LoadSceneAsync(sceneIndex + _sceneIndexOffset);

            OnLevelLoaded();
        }

        private bool IsValidSceneIndex(int sceneIndex)
        {
            return sceneIndex >= 0 && sceneIndex <= _maxLevelIndex && sceneIndex != _entryPointIndex;
        }
    }
}
