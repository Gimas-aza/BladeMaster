using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EntryPoint;
using Assets.GameProgression;
using Assets.MVP;
using Assets.MVP.Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerComponent : MonoBehaviour, IInitializer, IModel, IKnivesPool
    {
        [Header("Player input")]
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _rotationRadius;

        [Header("Knife")]
        [SerializeField] private KnivesPoolComponent _knivesPool;
        [SerializeField] private List<int> _amountOfKnivesOnLevels;
        [SerializeField] private ThrowingForceSettings _knifeSettings;

        private Rigidbody _rigidbody;
        private float _touchStartTime;
        private UnityAction<IAmountOfKnives> _displayAmountKnives;

        public void Init(IResolver container)
        {
            var knife = container.Resolve<IKnifeObject>();
            int levelIndex = container.Resolve<ILevelInfoProvider>().GetLevelIndex();

            CheckKnivesPoolInitialization();
            _knivesPool.CreateKnife(knife.GetGameObject(), _amountOfKnivesOnLevels[levelIndex - 1]);
        }

        public void SubscribeToEvents(IResolver container)
        {
            var uiEvents = container.Resolve<IUIEvents>();

            uiEvents.MonitorInputRotation += InputRotation;
            uiEvents.MonitorInputTouchBegin += InputTouchBegin;
            uiEvents.MonitorInputTouchEnded += InputTouchEnded;

            _displayAmountKnives = uiEvents.DisplayAmountKnives;
            _displayAmountKnives?.Invoke(_knivesPool);
        }

        public List<IWeaponEvents> GetKnives() => _knivesPool.GetKnives().Cast<IWeaponEvents>().ToList();

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void InputRotation(float acceleration)
        {
            RotatePlayer(acceleration);
        }

        private ThrowingForceSettings InputTouchBegin()
        {
            _touchStartTime = Time.time;
            return _knifeSettings;
        }

        private void InputTouchEnded()
        {
            var force = _knifeSettings.GetForce(Time.time - _touchStartTime);
            ThrowKnife(force);
        }

        private void RotatePlayer(float acceleration)
        {
            float currentRotationY = Mathf.Clamp(_rigidbody.rotation.y + acceleration * 100, -_rotationRadius, _rotationRadius);
            Quaternion targetRotation = Quaternion.Euler(0, currentRotationY, 0);

            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
        }

        private void ThrowKnife(float force)
        {
            _knivesPool.ThrowKnife(force);
            _displayAmountKnives?.Invoke(_knivesPool);
        }

        private void CheckKnivesPoolInitialization()
        {
            if (_knivesPool == null)
                Debug.LogError($"{nameof(Init)}: {nameof(KnivesPoolComponent)} is null");
        }
    }
}
