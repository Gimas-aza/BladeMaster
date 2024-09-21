using System.Collections.Generic;
using Assets.Player;
using Assets.Target;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Knife
{
    public class KnifeComponent : MonoBehaviour, IKnife, IKnifeObject
    {
        [SerializeField] private List<Rigidbody> _rigidbodyList;
        [SerializeField] private TriggerHandler _triggerHandler;

        private bool _isThrown = false;
        private GameObject _skin;

        public event UnityAction<ITarget> Hit;
        public event UnityAction NoHit;

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

            if (other.TryGetComponent(out ITarget target) && !target.IsHit() && !_isThrown)
            {
                _isThrown = true;
                target.SetHit(true);
                Hit?.Invoke(target);
            }
            else if (!_isThrown)
            {
                _isThrown = true;
                NoHit?.Invoke();
            }
        }

        public void SetTransform(Transform transform)
        {
            this.transform.SetParent(transform);
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

        public void SetGlobalParent() => transform.SetParent(null);

        public bool IsActive() => gameObject.activeSelf;
        public bool IsThrow() => _isThrown;

        public void Throw(float force)
        {
            foreach (var rigidbody in _rigidbodyList)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }

            _rigidbodyList[0].AddForce(transform.forward * force, ForceMode.Impulse);
        }

        public void SwitchSkin(GameObject skin)
        {
            _skin = Instantiate(skin, _rigidbodyList[0].transform);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
