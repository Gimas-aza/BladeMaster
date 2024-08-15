using UnityEngine;
using UnityEngine.Events;

namespace Assets.EntryPoint
{
    public interface ILevelManager
    {
        UnityAction OnLevelLoaded { get; set; }
    }
}