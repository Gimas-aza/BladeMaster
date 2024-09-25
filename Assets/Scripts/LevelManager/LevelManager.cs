using Assets.EntryPoint;
using Assets.MVP.Model;
using UnityEngine.Events;

namespace Assets.LevelManager
{
    public abstract class LevelManager : ILevelManager, ILevelInfoProvider, IModel
    {
        protected static readonly int _entryPointIndex = -1;
        protected static readonly int _mainMenuIndex = 0;
        private IUIEvents _uiEvents;

        protected int _currentLevelIndex { get; set; } = _entryPointIndex;
        protected int _nextLevelIndex { get => (_currentLevelIndex + 1 <= _maxLevelIndex) ? _currentLevelIndex + 1 : _currentLevelIndex; }
        protected int _previousLevelIndex { get => (_currentLevelIndex - 1 >= 0) ? _currentLevelIndex - 1 : _currentLevelIndex; }
        protected abstract int _maxLevelIndex { get; set; }

        public event UnityAction LevelStartedToLoad;
        public event UnityAction<int> LevelLoaded;

        public abstract void LoadLevel(int sceneIndex);
        public abstract void LoadNextLevel();
        public abstract void LoadPreviousLevel();
        public abstract void ReloadingCurrentLevel();

        protected void OnLevelLoaded() => LevelLoaded?.Invoke(_currentLevelIndex);
        protected void OnLevelStartedToLoad() => LevelStartedToLoad?.Invoke();

        public void SubscribeToEvents(IResolver container)
        {
            _uiEvents = container.Resolve<IUIEvents>();
            _uiEvents.UnregisterLevelManagerEvents();

            _uiEvents.LevelAmountRequestedForDisplay += () => _maxLevelIndex;
            _uiEvents.PressingTheSelectedLevel += LoadLevel;
            _uiEvents.ClickedButtonBackMainMenu += () => LoadLevel(_mainMenuIndex);
            _uiEvents.ClickedButtonAgainLevel += ReloadingCurrentLevel;
        }

        ~LevelManager()
        {
            _uiEvents.UnregisterLevelManagerEvents();
        }

        public int GetLevelIndex() => _currentLevelIndex;
        public int GetAmountLevels() => _maxLevelIndex;
    }
}
