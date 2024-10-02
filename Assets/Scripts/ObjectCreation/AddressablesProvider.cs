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
        private Dictionary<string, int> _referenceCounts = new();

        public GameObject LoadResource<T>() where T : Behaviour
        {
            var resourceName = typeof(T).Name;
            if (_referenceCounts.ContainsKey(resourceName))
            {
                _referenceCounts[resourceName]++;
                return _handles[resourceName].Result;
            }

            UnloadResource();

            var handle = Addressables.LoadAssetAsync<GameObject>(resourceName);
            _handles.Add(resourceName,handle);
            handle.WaitForCompletion();

            TestingLoadResource<T>(handle);
            _referenceCounts[resourceName] = 1;
            return handle.Result;
        }

        public async UniTask<GameObject> LoadResourceAsync<T>() where T : Behaviour
        {
            var resourceName = typeof(T).Name;
            if (_referenceCounts.ContainsKey(resourceName))
            {
                _referenceCounts[resourceName]++;
                return _handles[resourceName].Result;
            }

            UnloadResource();

            var handle = Addressables.LoadAssetAsync<GameObject>(typeof(T).Name);
            _handles.Add(resourceName,handle);
            await handle.Task;

            TestingLoadResource<T>(handle);
            _referenceCounts[resourceName] = 1;
            return handle.Result;
        }

        public void UnloadResource()
        {
            foreach (var (resourceName, handle) in _handles)
            {
                if (!handle.IsValid())
                {
                    if (_referenceCounts.ContainsKey(resourceName))
                    {
                        _referenceCounts[resourceName]--;
                        if (_referenceCounts[resourceName] <= 0)
                        {
                            Addressables.Release(handle);
                            _handles.Remove(resourceName);
                            _referenceCounts.Remove(resourceName);
                        }
                    }
                }
            }
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
