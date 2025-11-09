using System;
using UnityEngine;
using UnityEngine.UI;

namespace Users.FateX.Scripts.View
{
    public class HealthView: MonoBehaviour
    {
        [SerializeField] private Image _image;
        private SnakeHealth _snakeHealth;
        
        public void Init(SnakeHealth snakeHealth)
        {
            _snakeHealth = snakeHealth;
            
            snakeHealth.OnHealthChanged += HandleChanged;
        }

        private void OnDisable()
        {
            // _snakeHealth.OnHealthChanged -= HandleChanged;
        }

        private void HandleChanged()
        {
            _image.fillAmount = _snakeHealth.CurrentHealth / _snakeHealth.MaxHealth;
        }
    }
}