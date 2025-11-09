using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Entity;
using DG.Tweening;
using FMODUnity;

namespace Users.FateX.Scripts
{
    public class EnemyBase: MonoBehaviour, IEnemy, IDamageable, IPoolable
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private XpEntity _xp;
        
        [Header("Damage Indication")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _damageColor = Color.red;
        [SerializeField] private float _damageDuration = 0.2f;
        [SerializeField] private float _damageScalePunch = 0.15f;
        
        private Color _originalColor;
        private Sequence _damageSequence;
        
        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action<EnemyBase> OnDie;

        private void Awake()
        {
            CurrentHealth = _enemyData.Health;
            
            if (_spriteRenderer != null)
            {
                _originalColor = _spriteRenderer.color;
            }
        }

        public void Move(Vector3 direction)
        {
            _rigidbody2D.linearVelocity = direction;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            CurrentHealth -= damageInfo.Amount;
            
            // Визуальная индикация урона
            PlayDamageIndication();
            
            RuntimeManager.PlayOneShot("event:/SFX/Enemy/e_Death");

            OnHealthChanged?.Invoke(CurrentHealth);
            
            if(CurrentHealth <= 0)
                OnDie?.Invoke(this);
        }

        private void PlayDamageIndication()
        {
            // Останавливаем предыдущую анимацию, если она была
            _damageSequence?.Kill();
            
            _damageSequence = DOTween.Sequence();
            
            // Изменение цвета
            if (_spriteRenderer != null)
            {
                _damageSequence.Append(_spriteRenderer.DOColor(_damageColor, _damageDuration / 2f));
                _damageSequence.Append(_spriteRenderer.DOColor(_originalColor, _damageDuration / 2f));
            }
            
            // Эффект "удара" (небольшое увеличение и уменьшение размера)
            _damageSequence.Join(transform.DOPunchScale(Vector3.one * _damageScalePunch, _damageDuration, 1, 0.5f));
        }

        public void DropXp()
        {
            LeanPool.Spawn(_xp, transform.position, Quaternion.identity);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageInfo damageInfo = new DamageInfo(2);
                
                snakeBodyPartHealth.TakeDamage(damageInfo);
            }
        }

        public void OnSpawn()
        {
            CurrentHealth = _enemyData.Health;
            
            // Сбрасываем цвет и масштаб при спавне
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _originalColor;
            }
            transform.localScale = Vector3.one;
            
            _damageSequence?.Kill();
        }

        public void OnDespawn()
        {
            _damageSequence?.Kill();
        }

        private void OnDestroy()
        {
            _damageSequence?.Kill();
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