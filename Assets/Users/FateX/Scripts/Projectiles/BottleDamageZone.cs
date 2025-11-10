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

        [SerializeField] private Transform _transform;
        
        private float timeToNextShot = 0;
        
        private float initialRadius; // Сохраняем начальный радиус
        private float initialDamage; // Сохраняем начальный радиус

        private void Awake()
        {
            initialRadius = radius;
            initialDamage = damage;
        }

        public void Init(float damage, float radius)
        {
            this.damage += damage;
            this.radius += radius;
            
            // Рассчитываем коэффициент изменения радиуса и применяем к scale
            float scaleMultiplier = this.radius / initialRadius;
            _transform.localScale = Vector3.one * scaleMultiplier;
        }
        
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
            damage = initialDamage;
            
            radius = initialRadius;
            _transform.localScale = Vector3.one;
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}