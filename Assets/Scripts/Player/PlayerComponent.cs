using System;
using System.Collections.Generic;
using Assets.EntryPoint;
using Assets.GameProgression;
using Assets.MVP;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerComponent : MonoBehaviour, IModel, IInitializer, IKnivesPool
    {
        [Header("Player input")]
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;
        [Header("Knife")]
        [SerializeField] private KnivesPoolComponent _knivesPool;
        [SerializeField] private List<int> _amountOfKnivesOnLevels;
        [SerializeField] private ThrowingForceSettings _knifeSettings;

        private Rigidbody _rigidbody;
        private float _time;
        private UnityAction<IAmountOfKnives> _displayAmountKnives;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(GameObject knife, int levelIndex)
        {
            _knivesPool.CreateKnife(knife, _amountOfKnivesOnLevels[levelIndex]);
        }

        public void SubscribeToEvents(
            ref UnityAction<float> monitorInputRotation,
            ref Func<IForceOfThrowingKnife> monitorInputTouchBegin,
            ref UnityAction monitorInputTouchEnded,
            ref UnityAction<IAmountOfKnives> displayAmountKnives
        )
        {
            monitorInputRotation += InputRotation;
            monitorInputTouchBegin += InputTouchBegin;
            monitorInputTouchEnded += InputTouchEnded;

            _displayAmountKnives = displayAmountKnives;
            _displayAmountKnives?.Invoke(_knivesPool);
        }

        private void InputRotation(float acceleration)
        {
            var currentValue = Mathf.Clamp(
                _rigidbody.rotation.y + acceleration * 100,
                -_radius,
                _radius
            );
            var targetRotation = Quaternion.Euler(0, currentValue, 0);
            _rigidbody.MoveRotation(
                Quaternion.RotateTowards(
                    _rigidbody.rotation,
                    targetRotation,
                    _speed * Time.fixedDeltaTime
                )
            );
        }

        private ThrowingForceSettings InputTouchBegin()
        {
            _time = Time.time;
            return _knifeSettings;
        }

        private void InputTouchEnded()
        {
            var force = _knifeSettings.GetForce(Time.time - _time);
            _knivesPool.ThrowKnife(force);
            _displayAmountKnives?.Invoke(_knivesPool);
        }

        public List<IKnife> GetKnives()
        {
            return _knivesPool.GetKnives();
        }
    }
}
