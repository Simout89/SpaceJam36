using System;
using FMODUnity;
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
            RuntimeManager.StudioSystem.setParameterByName("PlayTime", 0);
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

            RuntimeManager.StudioSystem.setParameterByName("PlayTime", dif);
            
            UpdateDifficulty?.Invoke(dif);
    
            // Определяем текущий индекс врага
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

            // Вычисляем прогресс в текущем пуле врагов (0 = начало, 1 = конец)
            float poolProgress = GetPoolProgress(dif, currentEnemyIndex);
            
            // Получаем множитель интенсивности на основе прогресса в пуле
            float intensityMultiplier = GetIntensityCurve(poolProgress);
            
            // Базовое количество врагов зависит от сложности
            float baseSpawnCount = dif * 0.2f; // Можно настроить коэффициент (0.1-0.5)
            
            // Итоговое количество с учетом кривой интенсивности
            int spawnCount = Mathf.Max(1, Mathf.RoundToInt(baseSpawnCount * intensityMultiplier));

            // Спавним врагов
            for (int i = 0; i < spawnCount; i++)
            {
                _enemySpawner.SpawnEnemy(_waveData.WaveChangeSpawns[currentEnemyIndex].Enemy);
            }
        }
        
        private float GetDifficultyMultiplier(float t)
        {
            float linearPart = 1f + t * 0.1f; 
            float logPart = Mathf.Log(t + 1f, 2f);
            float curve = linearPart * (1f + 0.1f * logPart); 
            return curve;
        }

        // Вычисляет прогресс в текущем пуле врагов (от 0 до 1)
        private float GetPoolProgress(float currentDifficulty, int currentIndex)
        {
            if (currentIndex >= _waveData.WaveChangeSpawns.Length - 1)
            {
                // Последний пул - считаем прогресс от последнего маркера
                float lastMarker = _waveData.WaveChangeSpawns[currentIndex].DiffMarker;
                float nextMarker = lastMarker + 50f; // Условная длина последнего пула
                return Mathf.Clamp01((currentDifficulty - lastMarker) / (nextMarker - lastMarker));
            }
            
            float currentMarker = _waveData.WaveChangeSpawns[currentIndex].DiffMarker;
            float nextPoolMarker = _waveData.WaveChangeSpawns[currentIndex + 1].DiffMarker;
            
            return Mathf.Clamp01((currentDifficulty - currentMarker) / (nextPoolMarker - currentMarker));
        }

        // Кривая интенсивности: начало низкая, середина высокая, конец спад
        private float GetIntensityCurve(float progress)
        {
            // progress: 0 -> 1
            
            if (progress < 0.6f)
            {
                // От 0 до 0.6: плавный рост от минимума до максимума
                float normalizedProgress = progress / 0.6f;
                return 0.3f + 0.7f * (normalizedProgress * normalizedProgress);
            }
            else
            {
                // От 0.6 до 1.0: более плавный спад
                float normalizedProgress = (progress - 0.6f) / 0.4f;
                // Линейный спад с небольшим ускорением
                return 1.0f * (1f - normalizedProgress * normalizedProgress * 0.8f);
            }
        }
    }
}