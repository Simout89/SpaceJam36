using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        
        private float _damage;
        private float _speed;
        private Vector2 _direction;

        public void Initialize(Vector2 direction, float speed, float damage)
        {
            _direction = direction.normalized;
            _speed = speed;
            _damage = damage;
            
            // Применяем скорость к снаряду
            _rigidbody2D.linearVelocity = _direction * _speed;
            
            // Поворачиваем снаряд в направлении движения
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            LeanPool.Despawn(gameObject, 10f);
        }
        
        public void Initialize(Quaternion rotation, float speed, float damage)
        {
            // Преобразуем Quaternion в направление (вперед по оси X для 2D)
            _direction = rotation * Vector2.right;
            _speed = speed;
            _damage = damage;
            
            // Применяем скорость к снаряду
            _rigidbody2D.linearVelocity = _direction * _speed;
            
            // Устанавливаем вращение снаряда
            transform.rotation = rotation;
            
            LeanPool.Despawn(gameObject, 10f);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Проверяем столкновение с врагом или целью
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(new DamageInfo(_damage));
            }
            
            LeanPool.Despawn(gameObject);
        }
    }
}