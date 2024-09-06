using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemy
{
    [Serializable]
    public class EnemyLocator
    {
        public EnemyComponent EnemyPrefab; 
        public Vector3 Position;
        public Quaternion Rotation;
    }
}