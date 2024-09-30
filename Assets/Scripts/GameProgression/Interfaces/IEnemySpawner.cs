using System.Collections.Generic;

namespace Assets.GameProgression.Interfaces
{
    public interface IEnemySpawner
    {
        List<IPointsPerActionProvider> GetEnemies();
    }
}