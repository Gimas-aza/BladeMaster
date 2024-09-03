using System;
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

            OnLevelStartedToLoad();

            CurrentLevelIndex = NextLevelIndex;
            await SceneManager.LoadSceneAsync(NextLevelIndex + _sceneIndexOffset);

            OnLevelLoaded();
        }

        public override async void LoadPreviousLevel()
        {
            if (CurrentLevelIndex == PreviousLevelIndex)
                return;

            OnLevelStartedToLoad();

            CurrentLevelIndex = PreviousLevelIndex;
            await SceneManager.LoadSceneAsync(PreviousLevelIndex + _sceneIndexOffset);

            OnLevelLoaded();
        }

        public override async void LoadLevel(int sceneIndex)
        {
            if (
                CurrentLevelIndex == sceneIndex
                || sceneIndex > MaxLevelIndex
                || sceneIndex == EntryPointIndex
            )
                return;

            OnLevelStartedToLoad();

            CurrentLevelIndex = sceneIndex;
            await SceneManager.LoadSceneAsync(sceneIndex + _sceneIndexOffset);

            OnLevelLoaded();
        }
    }
}
