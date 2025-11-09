using System;
using DG.Tweening;
using FMODUnity;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.LeaderBoard;
using Zenject;

namespace Users.FateX.Scripts
{
    public class DeathHandler: IDisposable
    {
        [Inject] private LeaderboardManager _leaderboardManager;
        [Inject] private UserInfo _userInfo;
        [Inject] private GameTimer _gameTimer;
        
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
            
            _leaderboardManager.AddScore(_userInfo.UserName,(int) _gameTimer.CurrentTime);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}