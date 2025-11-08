using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Segments
{
    public class CannonSegment: MonoBehaviour
    {
        [SerializeField] private Transform gunPivot;
        [SerializeField] private Projectile projectilePrefab;
        
        [SerializeField] private float damage = 3;
        private float delayBetweenShots = 1f;
        private float timeToNextShot = 0;

        public void FixedUpdate()
        {
            if (Time.time < timeToNextShot)
            {
                return;
            }

            timeToNextShot = Time.time + delayBetweenShots;
            
            var newProjectile = LeanPool.Spawn(projectilePrefab, gunPivot.position, Quaternion.identity);
            newProjectile.Initialize(gunPivot.rotation, 10, damage);
        }
    }
}