using UnityEngine; 
using Assets.EntryPoint;
using Cysharp.Threading.Tasks;

namespace Assets.ObjectFactory
{
    public class ObjectFactory : IObject
    {
        private IObjectProvider _objectProvider;

        public ObjectFactory()
        {
            _objectProvider = new AddressablesProvider();
        }
        
        public async UniTask<T> CreateObjectAsync<T>() where T : Behaviour
        {
            var newObject = GameObject.Instantiate(await _objectProvider.LoadResourceAsync<T>());
            TestingIsComponent<T>(newObject.TryGetComponent(out T result));
            _objectProvider.UnloadResource();

            return result;
        }

        public T CreateObject<T>() where T : Behaviour
        {
            var newObject = GameObject.Instantiate(_objectProvider.LoadResource<T>());
            TestingIsComponent<T>(newObject.TryGetComponent(out T result));
            _objectProvider.UnloadResource();

            return result;
        }

        private void TestingIsComponent<T>(bool isSuccess)
        {
            if (!isSuccess)
            {
                Debug.LogError("Not found component: " + typeof(T).Name);
            }
        }
    }
}
