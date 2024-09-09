using Assets.Target;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Player
{
    public interface IKnife 
    {
        event UnityAction<ITarget> Hit;
        event UnityAction NoHit;

        void SetTransform(Transform transform);
        void SetActive(bool active);
        void SetGlobalParent();
        bool IsActive();
        bool IsThrow();
        void Throw(float force);
        void SwitchSkin(GameObject skin);
    }
}
