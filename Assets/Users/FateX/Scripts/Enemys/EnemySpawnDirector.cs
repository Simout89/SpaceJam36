using System;
using FMODUnity;
using UnityEngine;
using Users.FateX.Scripts.Data.WaveData;
using Zenject;

namespace Users.FateX.Scripts.Enemys
{
    public class EnemySpawnDirector : IInitializable, IDisposable
    {
        [Inject] private GameTimer _gameTimer;
        [Inject] private EnemySpawner _enemySpawner;
        [Inject] private EnemyManager _enemyManager;

        public event Action<float> UpdateDifficulty;

        private WaveData _waveData;

        public void SetWaveData(WaveData waveData)
        {
            _waveData = waveData;
        }

        public void Initialize()
        {
            RuntimeManager.StudioSystem.setParameterByName("PlayTime", 0);
            _gameTimer.OnSecondChanged += HandleSecondTick;
        }

        public void Dispose()
        {
            _gameTimer.OnSecondChanged -= HandleSecondTick;
        }

        private void HandleSecondTick(int seconds)
        {
            int currentEnemyIndex = 0;
            float dif = GetDifficultyMultiplier(seconds);

            RuntimeManager.StudioSystem.setParameterByName("PlayTime", dif);
            UpdateDifficulty?.Invoke(dif);

            // Определяем текущий индекс врага
            for (int i = 0; i < _waveData.WaveChangeSpawns.Length; i++)
            {
                if (dif >= _waveData.WaveChangeSpawns[i].DiffMarker)
                    currentEnemyIndex = i;
                else
                    break;
            }

            float poolProgress = GetPoolProgress(dif, currentEnemyIndex);
            float intensityMultiplier = GetIntensityCurve(poolProgress);
            float baseSpawnCount = dif * 0.2f;
            int spawnCount = Mathf.Max(1, Mathf.RoundToInt(baseSpawnCount * intensityMultiplier));

            bool isLastEnemyType = currentEnemyIndex >= _waveData.WaveChangeSpawns.Length - 1;

            for (int i = 0; i < spawnCount; i++)
            {
                // Проверяем лимит врагов
                if (!_enemyManager.CanSpawn)
                    break;

                var enemy = _enemySpawner.SpawnEnemy(_waveData.WaveChangeSpawns[currentEnemyIndex].Enemy);

                if (isLastEnemyType)
                {
                    // Ограничиваем рост здоровья, чтобы не ушёл в бесконечность
                    float hpMultiplier = 1f + dif - _waveData.WaveChangeSpawns[currentEnemyIndex].DiffMarker * 0.25f;
                    enemy.AddHealth(hpMultiplier);
                }
            }
        }

        private float GetDifficultyMultiplier(float t)
        {
            float linearPart = 1f + t * 0.1f;
            float logPart = Mathf.Log(t + 1f, 2f);
            return linearPart * (1f + 0.1f * logPart);
        }

        private float GetPoolProgress(float currentDifficulty, int currentIndex)
        {
            if (currentIndex >= _waveData.WaveChangeSpawns.Length - 1)
            {
                float lastMarker = _waveData.WaveChangeSpawns[currentIndex].DiffMarker;
                float nextMarker = lastMarker + 50f;
                return Mathf.Clamp01((currentDifficulty - lastMarker) / (nextMarker - lastMarker));
            }

            float currentMarker = _waveData.WaveChangeSpawns[currentIndex].DiffMarker;
            float nextPoolMarker = _waveData.WaveChangeSpawns[currentIndex + 1].DiffMarker;
            return Mathf.Clamp01((currentDifficulty - currentMarker) / (nextPoolMarker - currentMarker));
        }

        private float GetIntensityCurve(float progress)
        {
            if (progress < 0.6f)
            {
                float normalizedProgress = progress / 0.6f;
                return 0.3f + 0.7f * (normalizedProgress * normalizedProgress);
            }
            else
            {
                float normalizedProgress = (progress - 0.6f) / 0.4f;
                return 1.0f * (1f - normalizedProgress * normalizedProgress * 0.8f);
            }
        }
    }
}
