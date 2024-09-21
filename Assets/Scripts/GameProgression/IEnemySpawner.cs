using System.Collections.Generic;
using Assets.Enemy;
using Assets.Target;

namespace Assets.GameProgression
{
    public interface IEnemySpawner
    {
        List<IScoreProvider> GetEnemies();
    }
}