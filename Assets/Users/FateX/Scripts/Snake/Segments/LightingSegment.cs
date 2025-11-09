using Lean.Pool;
using UnityEngine;
using System.Collections.Generic;
using FMODUnity;

namespace Users.FateX.Scripts.Segments
{
    public class LightingSegment: MonoBehaviour
    {
        [SerializeField] private GameObject lightingPrefab;
        
        [SerializeField] private float radius = 5f;
        [SerializeField] private float damage = 3;
        [SerializeField] private int shotsPerAttack = 3; // Количество выстрелов за раз
        [SerializeField] private float delayBetweenShots = 1f;
        private float timeToNextShot = 0;

        public void FixedUpdate()
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
                if (collider.TryGetComponent(out EnemyBase enemy) && enemy.Visible && !enemy.AlreadyDie)
                {
                    enemies.Add(enemy);
                }
            }

            if (enemies.Count == 0)
            {
                return;
            }

            // Определяем количество выстрелов (не больше чем врагов)
            int actualShots = Mathf.Min(shotsPerAttack, enemies.Count);

            // Стреляем в случайных врагов без повторений
            for (int i = 0; i < actualShots; i++)
            {
                int randomIndex = Random.Range(0, enemies.Count);
                EnemyBase targetEnemy = enemies[randomIndex];
                
                var newProjectile = LeanPool.Spawn(lightingPrefab, targetEnemy.transform.position, Quaternion.identity);
                targetEnemy.TakeDamage(new DamageInfo(damage));
                
                RuntimeManager.PlayOneShot("event:/SFX/Player/p_Lightning");
                
                LeanPool.Despawn(newProjectile, 0.5f);
                
                // Удаляем врага из списка, чтобы не попасть в него повторно
                enemies.RemoveAt(randomIndex);
            }
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}