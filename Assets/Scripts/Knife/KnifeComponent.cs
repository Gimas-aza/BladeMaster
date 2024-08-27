using UnityEngine;

namespace Assets.Knife
{
    [RequireComponent(typeof(Collider))]
    public class KnifeComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        private float _force = 10;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (_force < 40)
                    _force += 0.1f;
                Debug.Log("Force: " + _force);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _rigidbody.useGravity = true;
                _rigidbody.AddForce(Vector3.forward * _force, ForceMode.Impulse);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }
    }
}
