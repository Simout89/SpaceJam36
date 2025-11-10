using System;
using UnityEngine;

namespace Users.FateX.Scripts.Segments
{
    public class DamageZoneSegment: MonoBehaviour
    {
        [SerializeField] private float damage = 3;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float delayBetweenShots = 1f;
        [SerializeField] private GameObject damageArea;
        
        private float timeToNextShot = 0;

        [SerializeField] private TriggerDetector _triggerDetector;
        private SnakeBodyPart snakeBodyPart;
        private float initialRadius;


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
        
        private void Awake()
        {
            initialRadius = radius;

            snakeBodyPart = GetComponent<SnakeBodyPart>();
        }

        
        public void FixedUpdate()
        {
            if (Time.time < timeToNextShot)
            {
                return;
            }

            timeToNextShot = Time.time + Mathf.Max(0.5f, delayBetweenShots - snakeBodyPart.SnakeStats.FireRate / 10);
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius + snakeBodyPart.SnakeStats.Range);
            
            float scaleMultiplier = (radius + snakeBodyPart.SnakeStats.Range) / initialRadius;
            damageArea.transform.localScale = Vector3.one * scaleMultiplier;


            if (colliders.Length == 0)
            {
                return;
            }

            foreach (var VARIABLE in colliders)
            {
                if (VARIABLE.TryGetComponent(out EnemyBase enemyBase) && !enemyBase.AlreadyDie)
                {
                    enemyBase.TakeDamage(new DamageInfo(damage + snakeBodyPart.SnakeStats.Damage));
                }
            }
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}