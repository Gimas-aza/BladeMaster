using System.Collections.Generic;
using Assets.Player;
using UnityEngine;

namespace Assets.Knife
{
    public class KnifeComponent : MonoBehaviour, IKnife
    {
        [SerializeField] private List<Rigidbody> _rigidbodyList;
        [SerializeField] private TriggerHandler _triggerHandler;

        private void Awake()
        {
            if (_triggerHandler == null) Debug.LogError("TriggerHandler is null"); 
            _triggerHandler.TriggerEntered += OnTriggerEnter;
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (var rigidbody in _rigidbodyList)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }
        }

        public void SetTransform(Transform transform)
        {
            this.transform.position = transform.position;
            this.transform.rotation = transform.rotation;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);

            if (!active)
            {
                foreach (var rigidbody in _rigidbodyList)
                {
                    rigidbody.isKinematic = true;
                    rigidbody.useGravity = false;
                }
            }
        }

        public bool IsActive() => gameObject.activeSelf;

        public void Throw(float force)
        {
            foreach (var rigidbody in _rigidbodyList)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }

            _rigidbodyList[0].AddForce(Vector3.forward * force, ForceMode.Impulse);
        }

        public void SwitchSkin()
        {
            throw new System.NotImplementedException();
        }
    }
}
