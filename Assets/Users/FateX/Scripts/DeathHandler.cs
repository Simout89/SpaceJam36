using System;
using DG.Tweening;
using FMODUnity;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.LeaderBoard;
using Users.FateX.Scripts.View;
using Zenject;
using Скриптерсы;

namespace Users.FateX.Scripts
{
    public class DeathHandler: IDisposable
    {
        [Inject] private LeaderboardManager _leaderboardManager;
        [Inject] private LeaderBoardView _leaderBoardView;
        
        [Inject] private UserInfo _userInfo;
        [Inject] private GameTimer _gameTimer;
        [Inject] private DeathView _deathView;
        [Inject] private GameStateManager _gameStateManager;
        
        private SnakeHealth _snakeHealth;

        public void Init(SnakeHealth snakeHealth)
        {
            _snakeHealth = snakeHealth;
            _snakeHealth.OnDeath += HandleDeath;
        }

        public void Dispose()
        {
            if (_snakeHealth != null)
            {
                _snakeHealth.OnDeath -= HandleDeath;
            }
        }

        private void HandleDeath()
        {
            RuntimeManager.PlayOneShot("event:/SFX/Player/p_Death");
            DOTween.KillAll();
            
            
            if(_userInfo.UserName != "null")
            {
                _leaderboardManager.AddScore(_userInfo.UserName, (int)_gameTimer.CurrentTime);
                _leaderBoardView.ShowLeaderBoard();
            }

            RuntimeManager.StudioSystem.setParameterByName("PlayTime", 0);
            
            _userInfo.firstPlay = false;
            
            _deathView.ShowDeathView();
            
            _gameStateManager.ChangeState(GameStates.Death);
            
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}