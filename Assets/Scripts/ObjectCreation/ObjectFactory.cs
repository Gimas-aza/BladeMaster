using UnityEngine; 
using Assets.EntryPoint;
using Cysharp.Threading.Tasks;

namespace Assets.ObjectCreation
{
    public class ObjectFactory : IObject
    {
        private IObjectProvider _objectProvider;

        public ObjectFactory(IObjectProvider objectProvider)
        {
            _objectProvider = objectProvider;
        }
        
        public async UniTask<T> CreateObjectAsync<T>() where T : Behaviour
        {
            var newObject = Object.Instantiate(await _objectProvider.LoadResourceAsync<T>());
            TestingIsComponent<T>(newObject.TryGetComponent(out T result));

            return result;
        }

        public T CreateObject<T>() where T : Behaviour
        {
            var newObject = Object.Instantiate(_objectProvider.LoadResource<T>());
            TestingIsComponent<T>(newObject.TryGetComponent(out T result));

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
