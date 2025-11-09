using System;
using UnityEngine;
using Users.FateX.Scripts.Data.WaveData;
using Users.FateX.Scripts.Enemys;
using Users.FateX.Scripts.LeaderBoard;
using Users.FateX.Scripts.View;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class GamePlaySceneEntryPoint: IInitializable
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

        public void Initialize()
        {
            Debug.Log("W");
            

            Snake snake = _snakeSpawner.SpawnSnake();
            
            _enemyManager.SetSnake(snake);
            
            _healthView.Init(snake.GetComponent<SnakeHealth>());
            _deathHandler.Init(snake.GetComponent<SnakeHealth>());

            _lootManager.Init(snake.GetComponent<SnakeInteraction>());

            WaveData waveData = Resources.LoadAll<WaveData>("Data/Waves")[0];
            
            _enemySpawnDirector.SetWaveData(waveData);
            
            _gameTimer.StartTimer();
        }
    }
}