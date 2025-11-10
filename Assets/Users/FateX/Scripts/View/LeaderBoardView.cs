using UnityEngine;
using Users.FateX.Scripts.LeaderBoard;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class LeaderBoardView: MonoBehaviour
    {
        [Inject] private LeaderboardManager _leaderboardManager;
        
        [SerializeField] private Transform linesContainer;
        [SerializeField] private LeaderBoardEntryView _leaderBoardEntryViewPrefab;

        public void ShowLeaderBoard()
        {
            foreach (Transform child in linesContainer)
            {
                Destroy(child.gameObject);
            }
            
            _leaderboardManager.GetTopScores(10, list =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var newEntryView = Instantiate(_leaderBoardEntryViewPrefab, linesContainer);
                    newEntryView.Init(i + 1, list[i]);
                }
            });
        }
    }
}