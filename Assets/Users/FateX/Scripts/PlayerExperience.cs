using System;
using UnityEngine;
using Users.FateX.Scripts.View;
using Zenject;

namespace Users.FateX.Scripts
{
    public class PlayerExperience
    {
        [Inject] private ChoiceCardMenu _choiceCardMenu;

        public float NextLevelXp { get; private set; } = 5;
        public float CurrentXp { get; private set; }
        public float CurrentLevel { get; private set; } = 1;

        public event Action OnGetLevel;
        public event Action OnChangeXp;

        public void AddXp(float value) // добавить модификаторы
        {
            CurrentXp += value;
            
            OnChangeXp?.Invoke();

            if (CurrentXp >= NextLevelXp)
            {
                UpLevel();
            }
        }

        private void UpLevel()
        {
            _choiceCardMenu.SpawnCards(3);
            UpdateLevelXpRequirement();
            CurrentXp = 0;
            CurrentLevel++;
            OnChangeXp?.Invoke();
        }

        private void UpdateLevelXpRequirement()
        {
            NextLevelXp *= 1.5f;
        }
    }
}