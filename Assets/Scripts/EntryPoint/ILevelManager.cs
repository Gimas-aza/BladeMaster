using UnityEngine;
using UnityEngine.Events;

namespace Assets.EntryPoint
{
    public interface ILevelManager
    {
        event UnityAction LevelStartedToLoad;
        event UnityAction<int> LevelLoaded;
        void LoadLevel(int sceneIndex);
    }
}