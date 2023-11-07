using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _energy;
        [SerializeField] private float _maxSpendEnergyOneShoot;
        [SerializeField] private float _speedSpendEnergyInSec;
        [SerializeField] private float _scaleChangeByOneEnergy;
        [SerializeField] private float _distToDoor;

        public float Speed => _speed;
        public float Energy => _energy;
        public float MaxSpendEnergyOneShoot => _maxSpendEnergyOneShoot;
        public float SpeedSpendEnergyInSec => _speedSpendEnergyInSec;
        public float ScaleChangeByOneEnergy => _scaleChangeByOneEnergy;
        public float DistToDoor => _distToDoor;
    }
}
