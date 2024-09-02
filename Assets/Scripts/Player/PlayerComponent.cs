using System;
using System.Collections.Generic;
using Assets.EntryPoint;
using Assets.GameProgression;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerComponent : MonoBehaviour, IModel, IInitializer, IKnivesPool
    {
        [Header("Player")]
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;
        [Header("Knife")]
        [SerializeField] private KnivesPoolComponent _knivesPool;
        [SerializeField] private int _amountOfKnives = 5;
        [SerializeField] private float _minForce = 20;
        [SerializeField] private float _maxForce = 50;

        private Rigidbody _rigidbody;
        private float _time;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(GameObject knife)
        {
            _knivesPool.CreateKnife(knife, _amountOfKnives);
        }

        public void SubscribeToEvents(ref UnityAction<float> monitorInputRotation, ref UnityAction monitorInputTouchBegin, ref UnityAction monitorInputTouchEnded)
        {
            monitorInputRotation += InputRotation;
            monitorInputTouchBegin += InputTouchBegin;
            monitorInputTouchEnded += InputTouchEnded;
        }

        private void InputRotation(float acceleration)
        {
            var currentValue = Mathf.Clamp(_rigidbody.rotation.y + acceleration * 100, -_radius, _radius);
            var targetRotation = Quaternion.Euler(0, currentValue, 0);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _speed * Time.fixedDeltaTime));
        }

        private void InputTouchBegin()
        {
            _time = Time.time;
        }

        private void InputTouchEnded()
        {
            var force = _minForce + (Time.time - _time) * (_maxForce - _minForce) / 5;
            force = Mathf.Clamp(force, _minForce, _maxForce);
            _knivesPool.ThrowKnife(force);
        }

        public List<IKnife> GetKnives()
        {
            return _knivesPool.GetKnives();
        }
    }
}
