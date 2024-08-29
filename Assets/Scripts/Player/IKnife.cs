using UnityEngine;

namespace Assets.Player
{
    public interface IKnife 
    {
        void SetTransform(Transform transform);
        void SetActive(bool active);
        bool IsActive();
        void Throw(float force);
        void SwitchSkin();
    }
}
