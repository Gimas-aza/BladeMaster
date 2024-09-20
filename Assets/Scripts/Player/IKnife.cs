using Assets.Target;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Player
{
    public interface IKnife : IWeaponEvents
    {
        void SetTransform(Transform transform);
        void SetActive(bool active);
        void SetGlobalParent();
        bool IsActive();
        void Throw(float force);
    }
}
