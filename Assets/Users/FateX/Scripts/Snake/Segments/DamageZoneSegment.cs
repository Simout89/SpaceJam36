using System;
using UnityEngine;

namespace Users.FateX.Scripts.Segments
{
    public class DamageZoneSegment: MonoBehaviour
    {
        [SerializeField] private float damage = 3;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float delayBetweenShots = 1f;
        private float timeToNextShot = 0;

        [SerializeField] private TriggerDetector _triggerDetector;

        private void OnEnable()
        {
            _triggerDetector.onTriggerEntered += HandleEntered;
        }

        private void OnDisable()
        {
            _triggerDetector.onTriggerEntered -= HandleEntered;
        }

        private void HandleEntered(Collider2D obj)
        {
            Debug.Log(obj);
            
            //if (obj.gameObject.TryGetComponent(out EnemyBase enemyBase))
            //{
            //    enemyBase.TakeDamage(new DamageInfo(damage));
            //}
        }

        
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

            foreach (var VARIABLE in colliders)
            {
                if (VARIABLE.TryGetComponent(out EnemyBase enemyBase) && !enemyBase.AlreadyDie)
                {
                    enemyBase.TakeDamage(new DamageInfo(damage));
                }
            }
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}