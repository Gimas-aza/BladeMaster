using System;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerComponent : MonoBehaviour, IModel
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SubscribeToEvents(ref UnityAction<float> monitorInputRotation)
        {
            monitorInputRotation += InputRotation;
        }

        private void InputRotation(float acceleration)
        {
            var currentValue = Mathf.Clamp(_rigidbody.rotation.y + acceleration * 100, -_radius, _radius);
            var targetRotation = Quaternion.Euler(0, currentValue, 0);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _speed * Time.fixedDeltaTime));
        }
    }
}
