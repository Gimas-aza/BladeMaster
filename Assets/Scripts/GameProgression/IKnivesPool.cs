using System.Collections.Generic;
using Assets.Player;

namespace Assets.GameProgression
{
    public interface IKnivesPool
    {
        List<IKnife> GetKnives();
    }
}