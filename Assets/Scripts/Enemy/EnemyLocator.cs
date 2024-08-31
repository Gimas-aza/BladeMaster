using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemy
{
    [Serializable]
    public class EnemyLocator
    {
        public EnemyComponent enemyPrefab; 
        public Vector3 position;
        public Quaternion rotation;
    }
}