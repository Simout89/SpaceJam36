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
        [SerializeField] private SnakeBodyPartHealth headHealth;
        
        public float MaxHealth { get; private set; } = 100f;
        public float CurrentHealth { get; private set; }
        public event Action OnHealthChanged;

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

        public void Add(SnakeBodyPartHealth snakeBodyPartHealth)
        {
            snakeBodyPartHealths.Add(snakeBodyPartHealth);

            snakeBodyPartHealth.OnHealthChanged += HandleHealthChanged;
        }


        public void Remove(SnakeBodyPartHealth snakeBodyPartHealth)
        {
            snakeBodyPartHealth.OnHealthChanged -= HandleHealthChanged;
            
            snakeBodyPartHealths.Remove(snakeBodyPartHealth);
        }
        private void HandleHealthChanged(float obj)
        {
            Debug.Log("Змея получила урон");
            CurrentHealth -= obj;

            if (CurrentHealth <= 0)
            {
                RuntimeManager.PlayOneShot("event:/SFX/Player/p_Death");
                DOTween.KillAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            OnHealthChanged?.Invoke();
        }
    }
}