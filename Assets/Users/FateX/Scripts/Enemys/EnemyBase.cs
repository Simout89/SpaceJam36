using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Entity;

namespace Users.FateX.Scripts
{
    public class EnemyBase: MonoBehaviour, IEnemy, IDamageable, IPoolable
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [SerializeField] private XpEntity _xp;
        
        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action<EnemyBase> OnDie;

        private void Awake()
        {
            CurrentHealth = _enemyData.Health;
        }

        public void Move(Vector3 direction)
        {
            // transform.position = transform.position + direction;
            _rigidbody2D.linearVelocity = direction;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            CurrentHealth -= damageInfo.Amount;
            
            if(CurrentHealth <= 0)
                OnDie?.Invoke(this);
        }

        public void DropXp()
        {
            LeanPool.Spawn(_xp, transform.position, Quaternion.identity);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(other.gameObject.name);
            
            if(other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageInfo damageInfo = new DamageInfo(2);
                
                snakeBodyPartHealth.TakeDamage(damageInfo);
            }
        }

        public void OnSpawn()
        {
            CurrentHealth = _enemyData.Health;
        }

        public void OnDespawn()
        {
            
        }
    }

    public struct DamageInfo
    {
        public float Amount;
        public DamageInfo(float amount)
        {
            this.Amount = amount;
        }
    }

    public interface IDamageable
    {
        public float CurrentHealth { get; }
        public void TakeDamage(DamageInfo damageInfo);
        public event Action<float> OnHealthChanged;
    }

    public interface IEnemy
    {
        public void Move(Vector3 direction);
    }
}