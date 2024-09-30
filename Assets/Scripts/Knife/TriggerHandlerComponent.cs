using UnityEngine;
using UnityEngine.Events;

namespace Assets.Knife
{
    [RequireComponent(typeof(Collider))]
    public class TriggerHandlerComponent : MonoBehaviour
    {
        public event UnityAction<Collider> TriggerEntered;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(other);
        }
    }
}