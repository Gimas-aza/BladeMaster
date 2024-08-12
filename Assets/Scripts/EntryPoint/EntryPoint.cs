using UnityEngine;

namespace Assets.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        readonly IObject _objectFactory = new ObjectFactory.ObjectFactory();

        void Start()
        {
            var camera = _objectFactory.CreateObject<Camera>();
        }
    }
}
