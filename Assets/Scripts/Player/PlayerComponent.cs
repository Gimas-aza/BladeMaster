using System.Collections.Generic;
using System.Linq;
using Assets.EntryPoint;
using Assets.GameProgression;
using Assets.MVP.Model;
using UnityEngine;

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
        private IUIEvents _uiEvents;

        public void Init(IResolver container)
        {
            var knife = container.Resolve<IKnifeObject>();
            int levelIndex = container.Resolve<ILevelInfoProvider>().GetLevelIndex();

            CheckKnivesPoolInitialization();
            _knivesPool.CreateKnife(knife.GetGameObject(), _amountOfKnivesOnLevels[levelIndex - 1]);
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SubscribeToEvents(IResolver container)
        {
            _uiEvents = container.Resolve<IUIEvents>();

            _uiEvents.UnregisterPlayerEvents();
            _uiEvents.MonitorInputRotation += InputRotation;
            _uiEvents.MonitorInputTouchBegin += InputTouchBegin;
            _uiEvents.MonitorInputTouchEnded += InputTouchEnded;

            _uiEvents.DisplayAmountKnives?.Invoke(_knivesPool);
        }

        public List<IWeaponEvents> GetKnives() => _knivesPool.GetKnives().Cast<IWeaponEvents>().ToList();

        private void OnDestroy()
        {
            _uiEvents.UnregisterPlayerEvents();
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
            _uiEvents.DisplayAmountKnives?.Invoke(_knivesPool);
        }

        private void CheckKnivesPoolInitialization()
        {
            if (_knivesPool == null)
                Debug.LogError($"{nameof(Init)}: {nameof(KnivesPoolComponent)} is null");
        }
    }
}
