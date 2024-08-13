using UnityEngine;

namespace Assets.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        private readonly IObject _objectFactory = new ObjectFactory.ObjectFactory();

        void Start()
        {
            
        }
    }
}
