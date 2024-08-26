using System;
using UnityEngine;

namespace Assets.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerComponent : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 acceleration = Input.acceleration;
            if (acceleration.x < 0.3f && acceleration.x > -0.3f) return;

            var currentValue = Mathf.Clamp(_rigidbody.rotation.y + acceleration.x * 100, -_radius, _radius);
            var targetRotation = Quaternion.Euler(0, currentValue, 0);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _speed * Time.fixedDeltaTime));
        }
    }
}
