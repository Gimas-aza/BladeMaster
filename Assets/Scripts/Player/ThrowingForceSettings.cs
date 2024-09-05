using Assets.MVP;
using UnityEngine;

namespace Assets.Player
{
    [System.Serializable]
    public class ThrowingForceSettings : IForceOfThrowingKnife
    {
        public float MinForce = 20;
        public float MaxForce = 50;
        public float ForceScalingFactor = 2;

        public float GetForce(float time)
        {
            float normalizedTime = (time % ForceScalingFactor) / ForceScalingFactor;

            float force = Mathf.Lerp(MinForce, MaxForce, Mathf.PingPong(normalizedTime * 2, 1));

            return Mathf.Clamp(force, MinForce, MaxForce);
        }

        public float GetPercentOfForce(float time) =>
            (GetForce(time) - MinForce) / (MaxForce - MinForce) * 100;
    }
}
