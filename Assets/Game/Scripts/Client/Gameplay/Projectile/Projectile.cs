using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileData _data;

        [Header("Tech")] 
        [SerializeField] private LayerMask _enemyMask;

        private float _energy;
        private Vector3 _defScale;

        private bool _isShoot;
        private Vector3 _dir;

        private Player _player;

        public void SetPlayer(Player player) =>
            _player = player;

        private void Awake()
        {
            _defScale = transform.localScale;
            _isShoot = false;
        }

        private void Update()
        {
            if (_isShoot)
            {
                transform.position += _dir * _data.Speed * Time.deltaTime;
            }
        }

        public void Grow(float addEnergy)
        {
            _energy += addEnergy;

            var addScale = _energy * _data.ScaleChangeByOneEnergy;
            var newScale = _defScale + new Vector3(addScale, addScale, addScale);

            transform.localScale = newScale;
        }

        public void Shoot(Vector3 dir)
        {
            _isShoot = true;
            _dir = dir;
        }

        private void OnCollisionEnter(Collision collision)
        {
            float radius = _data.MinExplosionRadius + _energy * _data.RadiusExplosionByOneEnergy;

            Ray ray = new Ray(transform.position, _dir);
            List<RaycastHit> raycastHits = Physics.SphereCastAll(ray, radius, radius, _enemyMask).ToList();
            if (raycastHits.Count > 0)
            {
                foreach (var hit in raycastHits)
                {
                    Enemy enemy = hit.collider.GetComponent<ParentRef>().Parent.GetComponent<Enemy>();
                    if (enemy)
                    {
                        enemy.Die();
                    }
                }
            
                _player.TryMove();
            }

            Destroy(gameObject);
        }
    }
}
