using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _scaleChangeByOneEnergy;
        [SerializeField] private float _minExplosionRadius;
        [SerializeField] private float _radiusExplosionByOneEnergy;

        public float Speed => _speed;
        public float ScaleChangeByOneEnergy => _scaleChangeByOneEnergy;

        public float MinExplosionRadius => _minExplosionRadius;
        public float RadiusExplosionByOneEnergy => _radiusExplosionByOneEnergy;
    }
}
