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
        [field: SerializeField] public Transform body { get; private set; }
        
        [Header("Damage Indication")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _damageDuration = 0.2f;
        [SerializeField] private float _damageScalePunch = 0.15f;
        
        private Color _originalColor;
        private Sequence _damageSequence;
        
        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action<float> OnTakeDamage;
        public event Action<EnemyBase> OnDie;

        public bool AlreadyDie { get; private set; }

        public bool Visible { get; private set; } = false;

        private void Awake()
        {
            CurrentHealth = _enemyData.Health;
            
            if (_spriteRenderer != null)
            {
                _originalColor = _spriteRenderer.color;
            }
        }

        public void AddHealth(float value)
        {
            CurrentHealth += value;
        }

        private void OnBecameVisible()
        {
            Visible = true;
        }

        private void OnBecameInvisible()
        {
            Visible = false;
        }

        public void Move(Vector3 direction)
        {
            if(AlreadyDie)
                return;
            
            _rigidbody2D.linearVelocity = direction;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if(!Visible)
                return;
            
            CurrentHealth -= damageInfo.Amount;
            
            // Визуальная индикация урона
            PlayDamageIndication();
            
            RuntimeManager.PlayOneShot("event:/SFX/Enemy/e_Death");

            OnTakeDamage?.Invoke(damageInfo.Amount);
            OnHealthChanged?.Invoke(CurrentHealth);
            
            if(CurrentHealth <= 0)
            {
                if(AlreadyDie)
                    return;

                AlreadyDie = true;

                DropXp();
                
                _spriteRenderer.material.DOFloat(1f, "_DissolveAmount", 0.5f).OnComplete((() =>
                {
                    OnDie?.Invoke(this);
                }));
            }
        }

        private void PlayDamageIndication()
        {
            // Останавливаем предыдущую анимацию, если она была
            _damageSequence?.Complete();

            _damageSequence = DOTween.Sequence();

            // Изменение цвета (мигание)
            if (_spriteRenderer != null)
            {
                _spriteRenderer.material.SetFloat("_FlashAmount", 1);
                _damageSequence.Append(
                    DOTween.To(() => _spriteRenderer.material.GetFloat("_FlashAmount"), 
                        x => _spriteRenderer.material.SetFloat("_FlashAmount", x), 
                        0f, 
                        _damageDuration)
                );
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
            if(AlreadyDie)
                return;
            
            if(other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageInfo damageInfo = new DamageInfo(2);
                
                snakeBodyPartHealth.TakeDamage(damageInfo);
            }
        }

        public void OnSpawn()
        {
            Visible = false;
            
            AlreadyDie = false;
            
            CurrentHealth = _enemyData.Health;

            _spriteRenderer.material.SetFloat("_FlashAmount", 0);
            _spriteRenderer.material.SetFloat("_DissolveAmount", 0);
            
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