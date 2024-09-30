using Assets.GameProgression.Interfaces;
using Assets.Target;
using UnityEngine;

namespace Assets.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class EnemyComponent : MonoBehaviour, ITarget, IPointsPerActionProvider
    {
        [SerializeField] private int _pointsPerStroke;

        private bool _isHit;

        public void SetHit(bool isHit) => _isHit = isHit;
        public bool IsHit() => _isHit;
        public int GetPointsPerStroke() => _pointsPerStroke;
    }
}
