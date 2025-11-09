using System;
using System.Collections.Generic;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeInteraction: MonoBehaviour
    {
        [SerializeField] private Snake snake;
        public event Action<GameObject> OnCollect;

        public void AddTrigger(TriggerDetector triggerDetector)
        {
            triggerDetector.onTriggerEntered += OnTriggerEnter2D;
        }

        public void RemoveTrigger(TriggerDetector triggerDetector)
        {
            triggerDetector.onTriggerEntered -= OnTriggerEnter2D;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect(transform);
                OnCollect?.Invoke(other.gameObject);
            }
        }
    }

    public interface ICollectable
    {
        public void Collect(Transform pos);
    }
}