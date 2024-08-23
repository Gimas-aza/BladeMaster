using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.LevelManager
{
    public class GameSceneManager : LevelManager
    {
        protected override int MaxLevelIndex { get; set; }

        public GameSceneManager()
        {
            MaxLevelIndex = SceneManager.sceneCountInBuildSettings - 2;
        }

        public override async void LoadNextLevel()
        {
            if (CurrentLevelIndex == MaxLevelIndex) return;

            OnLevelStartedToLoad();

            CurrentLevelIndex = NextLevelIndex;
            await SceneManager.LoadSceneAsync(NextLevelIndex);

            OnLevelLoaded();
        }

        public override async void LoadPreviousLevel()
        {
            if (CurrentLevelIndex == PreviousLevelIndex) return;

            OnLevelStartedToLoad();

            CurrentLevelIndex = PreviousLevelIndex;
            await SceneManager.LoadSceneAsync(PreviousLevelIndex);

            OnLevelLoaded();
        }

        public override async void LoadLevel(int sceneIndex)
        {
            if (CurrentLevelIndex == sceneIndex || sceneIndex == MainSceneIndex) return;

            OnLevelStartedToLoad();

            CurrentLevelIndex = sceneIndex;
            await SceneManager.LoadSceneAsync(sceneIndex);

            OnLevelLoaded();
        }
    }
}
