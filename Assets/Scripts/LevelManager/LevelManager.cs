using System;
using Assets.EntryPoint;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.LevelManager
{
    public abstract class LevelManager : ILevelManager, IModel
    {
        protected static readonly int MainSceneIndex = -1;
        protected int CurrentLevelIndex { get; set; } = MainSceneIndex;
        protected int NextLevelIndex { get => (CurrentLevelIndex + 1 <= MaxLevelIndex) ? CurrentLevelIndex + 1 : CurrentLevelIndex; }
        protected int PreviousLevelIndex { get => (CurrentLevelIndex - 1 >= 0) ? CurrentLevelIndex - 1 : CurrentLevelIndex; }
        protected abstract int MaxLevelIndex { get; set; }

        public event UnityAction LevelStartedToLoad;
        public event UnityAction<int> LevelLoaded;

        public abstract void LoadLevel(int sceneIndex);
        public abstract void LoadNextLevel();
        public abstract void LoadPreviousLevel();

        protected void OnLevelLoaded() => LevelLoaded?.Invoke(CurrentLevelIndex);
        protected void OnLevelStartedToLoad() => LevelStartedToLoad?.Invoke();

        public void SubscribeToEvents(ref Func<int> levelAmountRequestedForDisplay, ref UnityAction<int> pressingTheSelectedLevel)
        {
            levelAmountRequestedForDisplay += () => MaxLevelIndex;
            pressingTheSelectedLevel += LoadLevel;
        }
    }
}
