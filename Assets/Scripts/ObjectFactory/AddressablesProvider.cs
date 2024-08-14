using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace Assets.ObjectFactory
{
    public class AddressablesProvider : IObjectProvider 
    {
        private readonly string _assetsPath = "Assets/";
        private AsyncOperationHandle<GameObject> _handle;

        public GameObject LoadResource<T>() where T : Behaviour
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(_assetsPath + typeof(T).Name);
            _handle = handle;
            handle.WaitForCompletion();

            TestingLoadResource<T>(handle);
            return handle.Result;
        }

        public async UniTask<GameObject> LoadResourceAsync<T>() where T : Behaviour
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(_assetsPath + typeof(T).Name);
            _handle = handle;
            await handle.Task;

            TestingLoadResource<T>(handle);
            return handle.Result;
        }

        public void UnloadResource()
        {
            if (_handle.IsValid())
                Addressables.Release(_handle);
        }

        private void TestingLoadResource<T>(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load resource: " + _assetsPath + typeof(T).Name);
            }
        }
    }
}
