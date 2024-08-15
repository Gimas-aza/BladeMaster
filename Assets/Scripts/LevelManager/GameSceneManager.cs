using UnityEngine;
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

        public override void LoadNextLevel()
        {
            if (CurrentLevelIndex == MaxLevelIndex) return;

            SceneManager.LoadScene(NextLevelIndex);
            CurrentLevelIndex = NextLevelIndex;
            OnLevelLoaded?.Invoke();
        }

        public override void LoadPreviousLevel()
        {
            if (CurrentLevelIndex == PreviousLevelIndex) return;

            SceneManager.LoadScene(PreviousLevelIndex);
            CurrentLevelIndex = PreviousLevelIndex;
            OnLevelLoaded?.Invoke();
        }

        public override void LoadLevel(int sceneIndex)
        {
            if (CurrentLevelIndex == sceneIndex) return;

            SceneManager.LoadScene(sceneIndex);
            CurrentLevelIndex = sceneIndex;
            OnLevelLoaded?.Invoke();
        }
    }
}
