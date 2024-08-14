using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.EntryPoint
{
    public interface IObject 
    {
        T CreateObject<T>() where T : Behaviour;
        UniTask<T> CreateObjectAsync<T>() where T : Behaviour;
    }
}
