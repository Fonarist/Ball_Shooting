using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class Doors : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public bool IsOpened => _isOpened;
        
        private static readonly int OpenKey = Animator.StringToHash("Open");
        
        private bool _isOpened;

        public void Open()
        {
            _isOpened = true;
            _animator.SetTrigger(OpenKey);
        }
    }
}
