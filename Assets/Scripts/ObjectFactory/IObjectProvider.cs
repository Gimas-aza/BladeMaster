using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.ObjectFactory
{
    public interface IObjectProvider
    {
        GameObject LoadResource<T>() where T : Behaviour;
        UniTask<GameObject> LoadResourceAsync<T>() where T : Behaviour;
        void UnloadResource();
    }
}
