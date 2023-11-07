using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class Enemy : MonoBehaviour
    {
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
