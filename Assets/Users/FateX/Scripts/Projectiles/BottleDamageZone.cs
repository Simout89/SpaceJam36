using System;
using System.Collections.Generic;
using FMODUnity;
using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class BottleDamageZone: MonoBehaviour, IPoolable
    {
        [SerializeField] private float damage = 3;
        [SerializeField] private float delayBetweenShots = 1f;
        [SerializeField] private float radius = 5f;
        private float timeToNextShot = 0;
        
        private void FixedUpdate()
        {
            if (Time.time < timeToNextShot)
            {
                return;
            }

            timeToNextShot = Time.time + delayBetweenShots;
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

            if (colliders.Length == 0)
            {
                return;
            }

            // Собираем всех врагов в список
            List<EnemyBase> enemies = new List<EnemyBase>();
            
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyBase enemy) && !enemy.AlreadyDie)
                {
                    enemy.TakeDamage(new DamageInfo(damage));
                }
            }
        }

        public void OnSpawn()
        {
            RuntimeManager.PlayOneShot("event:/SFX/Player/WaterBOOM");
            LeanPool.Despawn(gameObject, 3f);
        }

        public void OnDespawn()
        {
            
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}