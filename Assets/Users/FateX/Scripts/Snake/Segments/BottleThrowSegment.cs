using System;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;

namespace Users.FateX.Scripts.Segments
{
    public class BottleThrowSegment: MonoBehaviour
    {
        [SerializeField] private BottleProjectile _bottleProjectile;
        [SerializeField] private float damage = 3;
        [SerializeField] private float delayBetweenShots = 1f;
        private float timeToNextShot = 0;
        private Camera camera;

        private void Awake()
        {
            camera = Camera.main;
        }

        public void FixedUpdate()
        {
            if (Time.time < timeToNextShot)
            {
                return;
            }

            timeToNextShot = Time.time + delayBetweenShots;
    
            // Viewport координаты от 0 до 1
            float randomX = UnityEngine.Random.Range(0.1f, 0.9f);
            float randomY = UnityEngine.Random.Range(0.1f, 0.9f);
            float distance = 10f;
    
            Vector3 randomViewportPoint = new Vector3(randomX, randomY, distance);
            Vector3 randomWorldPoint = camera.ViewportToWorldPoint(randomViewportPoint);

            var newProjectile = LeanPool.Spawn(_bottleProjectile, randomWorldPoint, Quaternion.identity);
            newProjectile.Init(randomWorldPoint);
        }
    }
}