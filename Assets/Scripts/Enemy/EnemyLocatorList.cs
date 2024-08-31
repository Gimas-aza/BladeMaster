using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemy
{
    [CreateAssetMenu(fileName = "EnemyLocatorList", menuName = "Scriptable Objects/EnemyLocatorList", order = 1)]
    public class EnemyLocatorList : ScriptableObject
    {
        public List<EnemyLocator> enemyLocators;
    }
}
