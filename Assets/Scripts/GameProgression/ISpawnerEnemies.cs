using System.Collections.Generic;
using Assets.Enemy;

namespace Assets.GameProgression
{
    public interface ISpawnerEnemies
    {
        List<EnemyComponent> GetEnemies();
    }
}