using Assets.EntryPoint;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.LevelManager
{
    public abstract class LevelManager : ILevelManager, IModel
    {
        private static readonly int _mainSceneIndex = -1;

        protected int CurrentLevelIndex { get; set; } = _mainSceneIndex;
        protected int NextLevelIndex { get => (CurrentLevelIndex + 1 <= MaxLevelIndex) ? CurrentLevelIndex + 1 : CurrentLevelIndex; }
        protected int PreviousLevelIndex { get => (CurrentLevelIndex - 1 >= 0) ? CurrentLevelIndex - 1 : CurrentLevelIndex; }
        protected abstract int MaxLevelIndex { get; set; }

        public event UnityAction LevelStartedToLoad;
        public event UnityAction LevelLoaded;

        public abstract void LoadLevel(int sceneIndex);
        public abstract void LoadNextLevel();
        public abstract void LoadPreviousLevel();

        protected void OnLevelLoaded() => LevelLoaded?.Invoke();
        protected void OnLevelStartedToLoad() => LevelStartedToLoad?.Invoke();
    }
}
