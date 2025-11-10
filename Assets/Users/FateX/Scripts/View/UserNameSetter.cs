using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.LeaderBoard;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class UserNameSetter: MonoBehaviour
    {
        [Inject] private UserInfo _userInfo;
        [Inject] private LeaderboardManager _leaderboardManager;
        [Inject] private GameTimer _gameTimer;
        [Inject] private LeaderBoardView _leaderBoardView;

        [SerializeField] private TMP_InputField _tmpInputField;
        [SerializeField] private Button _button;

        private void Awake()
        {
            if (_userInfo.UserName != "null")
            {
                gameObject.SetActive(false);
                return;
            }
            
            _button.onClick.AddListener(HandleClick);
        }

        private async void HandleClick()
        {
            _userInfo.SetName(_tmpInputField.text);
            
            await _leaderboardManager.AddScore(_userInfo.UserName,(int) _gameTimer.CurrentTime);
            _leaderBoardView.ShowLeaderBoard();
            
            gameObject.SetActive(false);
        }
    }
}