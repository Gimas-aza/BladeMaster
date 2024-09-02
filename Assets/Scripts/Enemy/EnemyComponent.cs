using Assets.Target;
using UnityEngine;

namespace Assets.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class EnemyComponent : MonoBehaviour, ITarget
    {
        [SerializeField] private Vector3 _targetPosition = Vector3.zero;
        [SerializeField] private float _speed;

        public bool _isHit;

        private void Update()
        {
            if (_targetPosition != Vector3.zero)
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        }

        public void SetHit(bool isHit) => _isHit = isHit;

        public bool IsHit() => _isHit;
    }
}
