using Assets.Target;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameProgression.Interfaces
{
    public interface IWeaponEvents
    {
        event UnityAction<ITarget> Hit;
        event UnityAction NoHit;

        bool IsThrow();
        void SwitchSkin(GameObject skin);
    }
}