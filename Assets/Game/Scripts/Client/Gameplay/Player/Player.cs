using System;
using System.Collections;
using System.Collections.Generic;
using Ball.Client.Placements;
using Ball.Core.Binding;
using Ball.Core.Placements;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        [Header("Tech")] 
        [SerializeField] private Transform _visual;
        [SerializeField] private Transform _posSpawnProjectile;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _door;

        private float _curEnergy;
        private float _curEnergySpend;

        private Vector3 _defScale;

        private bool _isReadyShoot;
        private bool _isPrepareShoot;
        private Projectile _curProjectile;

        private bool _isMoving;
        private Vector3 _newPos;
        
        private void Awake()
        {
            _isReadyShoot = true;
            _isPrepareShoot = false;
            _isMoving = false;
            
            _curProjectile = null;

            _curEnergy = _data.Energy;
            _defScale = _visual.localScale;
        }

        private void Update()
        {
            UpdateMoving();
            
            UpdateInput();
        }

        public void TryMove()
        {
            StartCoroutine(DelayMove(2.0f));
        }

        private IEnumerator DelayMove(float time)
        {
            yield return new WaitForSeconds(time);
            
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, transform.localScale.x, out hit))
            {
                _isMoving = true;
                float dist = (hit.point - transform.position).magnitude;
                dist -= 2;
                _newPos = transform.position + transform.forward * dist;
            }
        }

        private void UpdateMoving()
        {
            if (_isMoving)
            {
                float distToDoor = (_door.position - transform.position).magnitude;
                if (distToDoor <= _data.DistToDoor)
                {
                    // todoV: win
                    var placementsModule = ServiceLocator.Resolve<IPlacementsModule>();
                    placementsModule.InvokePlacement(new RestartLevelPlacement());
                }
                
                float delta = _data.Speed * Time.deltaTime;

                float dist = (_newPos - transform.position).magnitude;
                if (dist <= delta)
                {
                    transform.position = _newPos;
                    _isMoving = false;
                    _isReadyShoot = true;
                }
                else
                {
                    transform.position += transform.forward * delta;
                }
            }
        }

        private void UpdateInput()
        {
            if (!_isReadyShoot)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                _isPrepareShoot = true;

                _curProjectile = Instantiate(_projectilePrefab, transform);
                _curProjectile.transform.position = _posSpawnProjectile.position;
                
                _curProjectile.SetPlayer(this);
            }
            else if (Input.GetMouseButton(0))
            {
                if (_isPrepareShoot)
                {
                    var deltaEnergy = _data.SpeedSpendEnergyInSec * Time.deltaTime;

                    _curEnergySpend += deltaEnergy;

                    if (_curEnergySpend >= _data.MaxSpendEnergyOneShoot)
                    {
                        Shoot();
                    }
                    else
                    {
                        
                        _curEnergy -= deltaEnergy;
                        if (_curEnergy <= 0)
                        {
                            // todoV: Lose
                            var placementsModule = ServiceLocator.Resolve<IPlacementsModule>();
                            placementsModule.InvokePlacement(new RestartLevelPlacement());
                        }
                    
                        UpdateScale();
                    
                        _curProjectile.Grow(deltaEnergy);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Shoot();
            }
        }

        private void UpdateScale()
        {
            var minusScale = (_data.Energy - _curEnergy) * _data.ScaleChangeByOneEnergy;
            var newScale = _defScale - new Vector3(minusScale, minusScale, minusScale);
            _visual.localScale = newScale;
        }

        private void Shoot()
        {
            _isReadyShoot = false;
            _isPrepareShoot = false;
            _curEnergySpend = 0;
                
            _curProjectile.Shoot(transform.forward);
        }
    }
}
