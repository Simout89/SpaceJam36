using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Entity
{
    public class Entity: MonoBehaviour, ICollectable, IEntity, IPoolable
    {
        private bool alreadyCollect = false;
        private Tween moveTween;
        
        public virtual void Collect(Transform target)
        {
            if(alreadyCollect)
                return;

            alreadyCollect = true;

            GetComponent<Collider2D>().enabled = false;

            float duration = 0.5f;
            float elapsed = 0f;
            
            Vector3 startPos = transform.position;
            Vector3 lastTargetPos = target.position;
            
            // Вычисляем начальную точку дуги
            Vector3 midPoint = (startPos + target.position) / 2f;
            midPoint.y += Vector3.Distance(startPos, target.position) * 0.25f;
            
            moveTween = DOVirtual.Float(0f, 1f, duration, (t) =>
            {
                elapsed = t;
                
                // Если цель сдвинулась, пересчитываем путь
                if (Vector3.Distance(lastTargetPos, target.position) > 0.01f)
                {
                    lastTargetPos = target.position;
                    Vector3 currentPos = transform.position;
                    midPoint = (currentPos + target.position) / 2f;
                    midPoint.y += Vector3.Distance(currentPos, target.position) * 0.25f;
                }
                
                // Вычисляем позицию на кривой Безье
                Vector3 p0 = Vector3.Lerp(startPos, midPoint, t);
                Vector3 p1 = Vector3.Lerp(midPoint, target.position, t);
                transform.position = Vector3.Lerp(p0, p1, t);
            })
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                transform.position = target.position;
                OnCollect?.Invoke();
                LeanPool.Despawn(gameObject);
            });
        }

        public event Action OnCollect;
        
        public void OnSpawn()
        {
            GetComponent<Collider2D>().enabled = true;

            alreadyCollect = false;
        }

        public void OnDespawn()
        {
            // Убиваем все твины при деспавне
            moveTween?.Kill();
            transform.DOKill();
        }
    }
}