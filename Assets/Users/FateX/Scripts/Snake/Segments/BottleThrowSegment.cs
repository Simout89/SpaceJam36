using System;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Segments
{
    public class BottleThrowSegment: MonoBehaviour
    {
        [SerializeField] private BottleProjectile _bottleProjectile;
        [SerializeField] private float damage = 3;
        [SerializeField] private float delayBetweenShots = 1f;
        private SnakeBodyPart snakeBodyPart;
        
        private float timeToNextShot = 0;
        private Camera camera;

        private void Awake()
        {
            camera = Camera.main;

            snakeBodyPart = GetComponent<SnakeBodyPart>();
        }

        public void FixedUpdate()
        {
            if (Time.time < timeToNextShot)
            {
                return;
            }

            for (int i = 0; i < Mathf.FloorToInt(snakeBodyPart.SnakeStats.ProjectileCount); i++)
            {
                timeToNextShot = Time.time + delayBetweenShots;
    
                // Viewport координаты от 0 до 1
                float randomX = Random.Range(0.1f, 0.9f);
                float randomY = Random.Range(0.1f, 0.9f);
                float distance = 10f;
    
                Vector3 randomViewportPoint = new Vector3(randomX, randomY, distance);
                Vector3 randomWorldPoint = camera.ViewportToWorldPoint(randomViewportPoint);

                var newProjectile = LeanPool.Spawn(_bottleProjectile, randomWorldPoint, Quaternion.identity);
                newProjectile.Init(randomWorldPoint, snakeBodyPart.SnakeStats.Damage, snakeBodyPart.SnakeStats.Range);
            }
        }
    }
}