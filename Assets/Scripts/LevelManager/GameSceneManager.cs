using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.LevelManager
{
    public class GameSceneManager : LevelManager
    {
        protected override int MaxLevelIndex { get; set; }

        private readonly int _ignoredSceneIndex = 2;
        private readonly int _sceneIndexOffset = 1;

        public GameSceneManager()
        {
            MaxLevelIndex = SceneManager.sceneCountInBuildSettings - _ignoredSceneIndex;
        }

        public override async void LoadNextLevel()
        {
            if (CurrentLevelIndex == MaxLevelIndex)
                return;

            await LoadSceneAsync(NextLevelIndex);
        }

        public override async void LoadPreviousLevel()
        {
            if (CurrentLevelIndex == PreviousLevelIndex)
                return;

            await LoadSceneAsync(PreviousLevelIndex);
        }

        public override async void LoadLevel(int sceneIndex)
        {
            if (!IsValidSceneIndex(sceneIndex))
                return;

            await LoadSceneAsync(sceneIndex);
        }

        public override async void ReloadingCurrentLevel()
        {
            await LoadSceneAsync(CurrentLevelIndex);
        }

        private async UniTask LoadSceneAsync(int sceneIndex)
        {
            OnLevelStartedToLoad();

            CurrentLevelIndex = sceneIndex;
            await SceneManager.LoadSceneAsync(sceneIndex + _sceneIndexOffset);

            OnLevelLoaded();
        }

        private bool IsValidSceneIndex(int sceneIndex)
        {
            return sceneIndex >= 0 && sceneIndex <= MaxLevelIndex && sceneIndex != EntryPointIndex;
        }
    }
}
