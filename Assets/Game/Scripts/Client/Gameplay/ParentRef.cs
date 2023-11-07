using UnityEngine;

namespace Ball.Client.Gameplay
{
    public class ParentRef : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;

        public GameObject Parent => _parent;
    }
}
