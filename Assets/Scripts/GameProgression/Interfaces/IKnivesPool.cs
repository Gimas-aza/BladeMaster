using System.Collections.Generic;

namespace Assets.GameProgression.Interfaces
{
    public interface IKnivesPool
    {
        List<IWeaponEvents> GetKnives();
    }
}