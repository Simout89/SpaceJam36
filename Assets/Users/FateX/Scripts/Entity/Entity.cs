using System;
using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Entity
{
    public class Entity: MonoBehaviour, ICollectable, IEntity
    {
        public virtual void Collect()
        {
            OnCollect?.Invoke();
            LeanPool.Despawn(gameObject);
        }

        public event Action OnCollect;
    }
}