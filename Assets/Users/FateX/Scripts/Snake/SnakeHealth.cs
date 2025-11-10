using System;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Action = System.Action;

namespace Users.FateX.Scripts
{
    public class SnakeHealth: MonoBehaviour
    {
        [SerializeField] private List<SnakeBodyPartHealth> snakeBodyPartHealths = new List<SnakeBodyPartHealth>();
        [SerializeField] private List<SnakeBodyPart> snakeBodyParts = new List<SnakeBodyPart>();
        [SerializeField] private SnakeBodyPartHealth headHealth;
        
        [SerializeField] private float delayBetweenShots = 1f;
        private float timeToNextShot = 0;
        
        public float MaxHealth { get; private set; } = 100;
        public float CurrentHealth { get; private set; }
        public event Action OnHealthChanged;

        public event Action OnDeath;

        private void OnEnable()
        {
            headHealth.OnHealthChanged += HandleHealthChanged;
        }

        private void OnDisable()
        {
            headHealth.OnHealthChanged -= HandleHealthChanged;

        }

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public void Heal()
        {
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke();
        }

        public void Add(SnakeBodyPartHealth snakeBodyPartHealth, SnakeBodyPart snakeBodyPart)
        {
            snakeBodyPartHealths.Add(snakeBodyPartHealth);
            snakeBodyParts.Add(snakeBodyPart);

            snakeBodyPartHealth.OnHealthChanged += HandleHealthChanged;
        }


        public void Remove(SnakeBodyPartHealth snakeBodyPartHealth, SnakeBodyPart snakeBodyPart)
        {
            snakeBodyPartHealth.OnHealthChanged -= HandleHealthChanged;
            
            snakeBodyPartHealths.Remove(snakeBodyPartHealth);
            snakeBodyParts.Remove(snakeBodyPart);
        }
        private void HandleHealthChanged(float obj)
        {
            if(Time.time < timeToNextShot)
                return;

            timeToNextShot = delayBetweenShots + Time.time;
            
            Debug.Log("Змея получила урон");
            CurrentHealth -= obj;
            
            headHealth.GetComponent<SnakeBodyPart>().DamageEffect();

            foreach (var VARIABLE in snakeBodyParts)
            {
                VARIABLE.DamageEffect();
            }

            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
            
            RuntimeManager.PlayOneShot("event:/SFX/Player/p_TakeDamage");
            
            OnHealthChanged?.Invoke();
        }
    }
}