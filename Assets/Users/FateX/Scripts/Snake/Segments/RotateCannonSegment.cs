using System;
using FMODUnity;
using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Segments
{
    public class RotateCannonSegment: MonoBehaviour
    {
        [SerializeField] private Transform gunPivot;
        [SerializeField] private Projectile projectilePrefab;
        
        [SerializeField] private float radius = 5f;
        [SerializeField] private float damage = 3;
        private float delayBetweenShots = 1f;
        private float timeToNextShot = 0;

        public void FixedUpdate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(gunPivot.position, radius);

            if (colliders.Length == 0)
            {
                return;
            }

            Collider2D nearestCollider = null;
            float nearestDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyBase enemy) && enemy.Visible && !enemy.AlreadyDie)
                {
                    float distance = Vector3.Distance(collider.transform.position, gunPivot.transform.position);
            
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestCollider = collider;
                    }
                }
            }

            if (nearestCollider != null && nearestCollider.TryGetComponent(out EnemyBase nearestEnemy)  && nearestEnemy.Visible && !nearestEnemy.AlreadyDie)
            {
                Vector2 direction = nearestEnemy.transform.position - gunPivot.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                gunPivot.rotation = Quaternion.Euler(0, 0, angle);
                
                if (Time.time < timeToNextShot)
                {
                    return;
                }

                timeToNextShot = Time.time + delayBetweenShots;

                RuntimeManager.PlayOneShot("event:/SFX/Player/p_Shoot");
                
                var newProjectile = LeanPool.Spawn(projectilePrefab, gunPivot.position, Quaternion.identity);
                newProjectile.Initialize(direction.normalized, 10, damage);
            }
        }
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(gunPivot.position, radius);
        }
    }
}