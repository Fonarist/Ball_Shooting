using System;
using System.Collections;
using Ball.Client.UI;
using Ball.Core.Binding;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        [Header("Tech")] 
        [SerializeField] private Transform _visual;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _posSpawnProjectile;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private LayerMask _enemyMask;
        [SerializeField] private Transform _finish;
        [SerializeField] private Doors _door;
        [SerializeField] private Transform _road;
        [SerializeField] private CameraParent _camera;

        private static readonly int IsJumpKey = Animator.StringToHash("IsJump");
        
        private float _curEnergy;
        private float _curEnergySpend;

        private Vector3 _defScale;

        private bool _isReadyShoot;
        private bool _isPrepareShoot;
        private Projectile _curProjectile;

        private bool _isMoving;
        private Vector3 _newPos;

        private EndScreenFeature _endScreenFeature;
        private ProgressFeature _progressFeature;
        
        private void Awake()
        {
            _isReadyShoot = true;
            _isPrepareShoot = false;
            _isMoving = false;
            
            _curProjectile = null;

            _curEnergy = _data.Energy;
            _defScale = _visual.localScale;
            
            _camera.SetTarget(transform);
        }

        private void Start()
        {
            _endScreenFeature = ServiceLocator.Resolve<EndScreenFeature>();
            _progressFeature = ServiceLocator.Resolve<ProgressFeature>();
        }

        private void Update()
        {
            UpdateInput();
        }

        private void FixedUpdate()
        {
            UpdateMoving();
        }

        public void TryMove()
        {
            StartCoroutine(DelayMove(1.75f));
        }

        private IEnumerator DelayMove(float time)
        {
            yield return new WaitForSeconds(time);
            
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.1f, out hit, 100.0f,_enemyMask))
            {
                float dist = (hit.point - transform.position).magnitude;
                dist -= 1.6f;
                _newPos = transform.position + transform.forward * dist;
            }
            else
            {
                _newPos = _finish.position;
                _newPos.y = transform.position.y;
            }
            
            _isMoving = true;
            _animator.SetBool(IsJumpKey, true);
        }

        private void UpdateMoving()
        {
            if (_isMoving)
            {
                if (!_door.IsOpened)
                {
                    float distToDoor = (_door.transform.position - transform.position).magnitude;
                    if (distToDoor <= _data.DistToDoor)
                    {
                        _door.Open();
                    }
                }
                
                var vecToFinish = (_finish.position - transform.position);
                vecToFinish.y = 0;
                if (vecToFinish.magnitude < 0.2f)
                {
                    _animator.SetBool(IsJumpKey, false);
                    _isMoving = false;
                    _endScreenFeature.Show(true);
                }

                float delta = _data.Speed * Time.fixedDeltaTime;

                float dist = (_newPos - transform.position).magnitude;
                if (dist <= delta * 2.0f)
                {
                    transform.position = _newPos;
                    _isMoving = false;
                    _isReadyShoot = true;
                    _animator.SetBool(IsJumpKey, false);
                    
                    _progressFeature.ShowTextHold();
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

                    var percent = _curEnergySpend / _data.MaxSpendEnergyOneShoot;
                    _progressFeature.UpdateProgress(percent);

                    if (_curEnergySpend >= _data.MaxSpendEnergyOneShoot)
                    {
                        Shoot();
                    }
                    else
                    {
                        _curEnergy -= deltaEnergy;
                        if (_curEnergy <= 0)
                        {
                            _isReadyShoot = false;
                            _endScreenFeature.Show(false);
                        }
                    
                        UpdateScale();
                    
                        _curProjectile.Grow(deltaEnergy);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_isPrepareShoot)
                {
                    Shoot();
                }
            }
        }

        private void UpdateScale()
        {
            var minusScale = (_data.Energy - _curEnergy) * _data.ScaleChangeByOneEnergy;
            var newScale = _defScale - new Vector3(minusScale, minusScale, minusScale);
            _visual.localScale = newScale;

            _road.localScale = new Vector3(_visual.localScale.x, _road.localScale.y, _road.localScale.z);
        }

        private void Shoot()
        {
            _isReadyShoot = false;
            _isPrepareShoot = false;
            _curEnergySpend = 0;
            
            _progressFeature.Reset();
                
            _curProjectile.Shoot(transform.forward);
        }
    }
}
