using UnityEngine;
using DG.Tweening;
using Lean.Pool;

namespace Users.FateX.Scripts
{
    public class BottleProjectile: MonoBehaviour
    {
        [SerializeField] private BottleDamageZone _damageZone;
        [SerializeField] private float flyDuration = 2f;
        [SerializeField] private float rotationSpeed = 360f; // градусов в секунду

        private float damage;
        private float radius;
        
        public void Init(Vector3 randomWorldPoint, float damage, float radius)
        {
            transform.position = randomWorldPoint + Vector3.up * 30;

            this.damage = damage;
            this.radius = radius;
    
            // Полет по дуге
            transform.DOJump(randomWorldPoint, 2f, 1, flyDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(OnComplete);
    
            // Вращение
            transform.DORotate(new Vector3(0, 0, 720f), flyDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear);
        }
        
        private void OnComplete()
        {
            Debug.Log("Снаряд долетел!");
            
            // Здесь можно добавить эффекты, урон и т.д.
            var newDamageZone = LeanPool.Spawn(_damageZone, transform.position, Quaternion.identity);
            
            newDamageZone.Init(damage, radius);
            
            LeanPool.Despawn(gameObject);
        }
    }
}