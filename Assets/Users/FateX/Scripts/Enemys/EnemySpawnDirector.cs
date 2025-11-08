using System;
using UnityEngine;
using Users.FateX.Scripts.Data.WaveData;
using Zenject;

namespace Users.FateX.Scripts.Enemys
{
    public class EnemySpawnDirector: IInitializable, IDisposable
    {
        [Inject] private GameTimer _gameTimer;
        [Inject] private EnemySpawner _enemySpawner;

        public event Action<float> UpdateDifficulty;

        private WaveData _waveData;
        public void SetWaveData(WaveData waveData)
        {
            _waveData = waveData;
        }

        public void Initialize()
        {
            _gameTimer.OnSecondChanged += HandleSecondTick;
        }

        public void Dispose()
        {
            _gameTimer.OnSecondChanged -= HandleSecondTick;
        }

        private void HandleSecondTick(int obj)
        {
    
            int currentEnemyIndex = 0;

            float dif = GetDifficultyMultiplier(obj);
            
            UpdateDifficulty?.Invoke(dif);
    
            for (int i = 0; i < _waveData.WaveChangeSpawns.Length; i++)
            {
                if (dif >= _waveData.WaveChangeSpawns[i].DiffMarker)
                {
                    currentEnemyIndex = i;
                }
                else
                {
                    break;
                }
            }
    
            _enemySpawner.SpawnEnemy(_waveData.WaveChangeSpawns[currentEnemyIndex].Enemy);
        }
        
        private float GetDifficultyMultiplier(float t)
        {
            float linearPart = 1f + t * 0.1f; 
            float logPart = Mathf.Log(t + 1f, 2f);
            float curve = linearPart * (1f + 0.3f * logPart); 
            return curve;
        }
    }
}