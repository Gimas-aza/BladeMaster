using System.Collections.Generic;
using Assets.Enemy;
using Assets.Target;

namespace Assets.GameProgression
{
    public interface ISpawnerEnemies
    {
        List<IScoreProvider> GetEnemies();
    }
}