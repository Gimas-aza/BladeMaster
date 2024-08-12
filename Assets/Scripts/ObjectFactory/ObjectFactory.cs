using UnityEngine; 
using Assets.EntryPoint;

namespace Assets.ObjectFactory
{
    public class ObjectFactory : IObject
    {
        public T CreateObject<T>() where T : Behaviour
        {
            IObjectProvider objectProvider = new AddressablesProvider();
            var newObject = GameObject.Instantiate(objectProvider.LoadResource<T>());

            return newObject.GetComponent<T>();
        }
    }
}
