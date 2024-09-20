using Assets.Target;
using UnityEngine;

namespace Assets.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class EnemyComponent : MonoBehaviour, ITarget, IScoreProvider
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _pointsPerStroke;

        private Vector3 _targetPosition = Vector3.zero;
        private bool _isHit;

        private void Update()
        {
            if (_targetPosition != Vector3.zero)
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        }

        public void SetHit(bool isHit) => _isHit = isHit;

        public bool IsHit() => _isHit;

        public int GetPointsPerStroke() => _pointsPerStroke;
    }
}
