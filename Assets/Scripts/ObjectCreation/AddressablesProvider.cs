using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.ObjectCreation
{
    public class AddressablesProvider : IObjectProvider 
    {
        private Dictionary<string, AsyncOperationHandle<GameObject>> _handles = new();

        public GameObject LoadResource<T>() where T : Behaviour
        {
            var resourceName = typeof(T).Name;
            if (_handles.ContainsKey(resourceName))
                return _handles[resourceName].Result;

            var handle = Addressables.LoadAssetAsync<GameObject>(resourceName);
            _handles.Add(resourceName,handle);
            handle.WaitForCompletion();

            TestingLoadResource<T>(handle);
            return handle.Result;
        }

        public async UniTask<GameObject> LoadResourceAsync<T>() where T : Behaviour
        {
            var resourceName = typeof(T).Name;
            if (_handles.ContainsKey(resourceName))
                return _handles[resourceName].Result;

            var handle = Addressables.LoadAssetAsync<GameObject>(resourceName);
            _handles.Add(resourceName,handle);
            await handle.Task;

            TestingLoadResource<T>(handle);
            return handle.Result;
        }

        private void TestingLoadResource<T>(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load resource: " + typeof(T).Name);
            }
        }
    }
}
