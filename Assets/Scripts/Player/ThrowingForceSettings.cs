using Assets.MVP;
using UnityEngine;

namespace Assets.Player
{
    [System.Serializable]
    public class ThrowingForceSettings : IForceOfThrowingKnife
    {
        [SerializeField] private float _minForce = 20;
        [SerializeField] private float _maxForce = 50;
        [SerializeField] private float _forceScalingFactor = 2;

        public float GetForce(float time)
        {
            float normalizedTime = (time % _forceScalingFactor) / _forceScalingFactor;

            float force = Mathf.Lerp(_minForce, _maxForce, Mathf.PingPong(normalizedTime * 2, 1));

            return Mathf.Clamp(force, _minForce, _maxForce);
        }

        public float GetPercentOfForce(float time) =>
            (GetForce(time) - _minForce) / (_maxForce - _minForce) * 100;
    }
}
