using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeInteraction: MonoBehaviour
    {
        [SerializeField] private Snake snake;

        public event Action<GameObject> OnCollect;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                OnCollect?.Invoke(other.gameObject);
            }
        }
    }
}