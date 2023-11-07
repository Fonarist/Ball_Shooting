using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class CameraParent : MonoBehaviour
    {
        private Vector3 _offset;
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            if (_target)
            {
                transform.position = _target.position + _offset;
            }
        }
    }
}
