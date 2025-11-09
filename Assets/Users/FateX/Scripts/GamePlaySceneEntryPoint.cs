using System;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Users.FateX.Scripts.Data.WaveData;
using Users.FateX.Scripts.Enemys;
using Users.FateX.Scripts.LeaderBoard;
using Users.FateX.Scripts.View;
using Zenject;
using Скриптерсы.Services;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Users.FateX.Scripts
{
    public class GamePlaySceneEntryPoint: IInitializable, IDisposable
    {
        [Inject] private IInputService _inputService;

        [Inject] private EnemySpawnDirector _enemySpawnDirector;
        [Inject] private GameTimer _gameTimer;
        [Inject] private SnakeSpawner _snakeSpawner;
        [Inject] private EnemyManager _enemyManager;
        [Inject] private HealthView _healthView;
        [Inject] private LootManager _lootManager;

        [Inject] private LeaderboardManager _leaderboardManager;
        [Inject] private DeathHandler _deathHandler;

        private EventInstance _eventInstance;

        public async void Initialize()
        {
            Debug.Log("W");
            
            await LoadBanksAsync();

            _eventInstance = RuntimeManager.CreateInstance("event:/Music/MainMusic");
            _eventInstance.start();

            Snake snake = _snakeSpawner.SpawnSnake();
            
            _enemyManager.SetSnake(snake);
            
            _healthView.Init(snake.GetComponent<SnakeHealth>());
            _deathHandler.Init(snake.GetComponent<SnakeHealth>());

            _lootManager.Init(snake.GetComponent<SnakeInteraction>());

            WaveData waveData = Resources.LoadAll<WaveData>("Data/Waves")[0];
            
            _enemySpawnDirector.SetWaveData(waveData);
            
            _gameTimer.StartTimer();
        }

        public void Dispose()
        {
            _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            
            RuntimeManager.UnloadBank("Master");
            
            Debug.Log("выгружено");
        }
        
        private async UniTask LoadBanksAsync()
        {
            // Загружаем Master и Music банки
            RuntimeManager.LoadBank("Master", true);

            // Ждём полной загрузки
            while (!RuntimeManager.HasBankLoaded("Master"))
            {
                await UniTask.Yield();
            }
            

            RuntimeManager.StudioSystem.flushCommands();
            Debug.Log("Банки загружены");
        }
    }
}