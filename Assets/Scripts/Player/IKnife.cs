using UnityEngine;

namespace Assets.Player
{
    public interface IKnife 
    {
        void SetTransform(Transform transform);
        void SetActive(bool active);
        void SetGlobalParent();
        bool IsActive();
        void Throw(float force);
        void SwitchSkin();
    }
}
