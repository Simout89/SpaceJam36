using FMODUnity;
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
        private SnakeBodyPart snakeBodyPart;

        private void Awake()
        {
            snakeBodyPart = GetComponent<SnakeBodyPart>();
        }

        public void FixedUpdate()
        {
            if (Time.time < timeToNextShot)
            {
                return;
            }

            timeToNextShot = Time.time + (Mathf.Max(delayBetweenShots - snakeBodyPart.SnakeStats.FireRate / 10, 0.5f));
            
            RuntimeManager.PlayOneShot("event:/SFX/Player/p_Shoot");
            
            var newProjectile = LeanPool.Spawn(projectilePrefab, gunPivot.position, Quaternion.identity);
            newProjectile.Initialize(gunPivot.rotation, 10, damage + snakeBodyPart.SnakeStats.Damage);
        }
    }
}