using System;
using UnityEngine;
using Users.FateX.Scripts.Entity;
using Zenject;

namespace Users.FateX.Scripts
{
    public class LootManager: IDisposable
    {
        [Inject] private PlayerExperience _playerExperience;
        
        private SnakeInteraction snakeInteraction;
        
        public void Init(SnakeInteraction snakeInteraction)
        {
            this.snakeInteraction = snakeInteraction;
            snakeInteraction.OnCollect += HandleCollect;
        }

        private void HandleCollect(GameObject obj)
        {
            if (obj.TryGetComponent(out XpEntity xpEntity))
            {
                _playerExperience.AddXp(xpEntity.XpEntityData.XpAmount);
            }
        }

        public void Dispose()
        {
            snakeInteraction.OnCollect += HandleCollect;
        }
    }
}