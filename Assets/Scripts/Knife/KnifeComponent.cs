using System.Collections.Generic;
using Assets.Player;
using Assets.Target;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Knife
{
    public class KnifeComponent : MonoBehaviour, IKnife, IKnifeObject
    {
        [SerializeField] private List<Rigidbody> _rigidbodies;
        [SerializeField] private TriggerHandler _triggerHandler;

        private bool _isThrown = false;
        private GameObject _skin;

        public event UnityAction<ITarget> Hit;
        public event UnityAction NoHit;

        private void Awake()
        {
            if (!IsFieldValid())
                return;

            _triggerHandler.TriggerEntered += OnTriggerEnter;
            EnsureRigidbodiesAreInitialized();
        }

        private void OnDestroy()
        {
            if (_triggerHandler != null)
            {
                _triggerHandler.TriggerEntered -= OnTriggerEnter;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            SetRigidbodiesState(isKinematic: true, useGravity: false);

            if (!_isThrown && other.TryGetComponent(out ITarget target) && !target.IsHit())
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

        public void SetTransform(Transform parentTransform)
        {
            transform.SetParent(parentTransform);
            transform.position = parentTransform.position;
            transform.rotation = parentTransform.rotation;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);

            if (!active)
            {
                SetRigidbodiesState(isKinematic: true, useGravity: false);
            }
        }

        public void Throw(float force)
        {
            SetRigidbodiesState(isKinematic: false, useGravity: true);

            if (_rigidbodies.Count > 0)
            {
                _rigidbodies[0].AddForce(transform.forward * force, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("No rigidbodies available to apply force");
            }
        }

        public void SwitchSkin(GameObject newSkin)
        {
            if (_skin != null)
            {
                Destroy(_skin);
            }

            _skin = Instantiate(newSkin, _rigidbodies[0].transform);
        }

        public void SetGlobalParent() => transform.SetParent(null);
        public bool IsActive() => gameObject.activeSelf;
        public bool IsThrow() => _isThrown;
        public GameObject GetGameObject() => gameObject;

        private void EnsureRigidbodiesAreInitialized()
        {
            SetRigidbodiesState(isKinematic: true, useGravity: false);
        }

        private void SetRigidbodiesState(bool isKinematic, bool useGravity)
        {
            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = isKinematic;
                rb.useGravity = useGravity;
            }
        }

        private bool IsFieldValid() => _rigidbodies.Count > 0 && _triggerHandler != null;
    }
}
