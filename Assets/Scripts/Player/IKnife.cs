using UnityEngine;

namespace Assets.Player
{
    public interface IKnife 
    {
        void SetTransform(Transform transform);
        void Throw(float force);
    }
}
