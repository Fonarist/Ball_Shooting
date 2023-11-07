using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int BlinkKey = Animator.StringToHash("Blink");

        public void Die()
        {
            _animator.SetTrigger(BlinkKey);

            StartCoroutine(DelayDestroy(1.5f));
        }

        private IEnumerator DelayDestroy(float time)
        {
            yield return new WaitForSeconds(time);
            
            Destroy(gameObject);
        }
    }
}
