using TMPro;
using UnityEngine;
using Users.FateX.Scripts.LeaderBoard;

namespace Users.FateX.Scripts.View
{
    public class LeaderBoardEntryView: MonoBehaviour
    {
        [SerializeField] private TMP_Text nickName;
        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text position;

        public void Init(int position, LeaderboardEntry leaderboardEntry)
        {
            this.position.text = $"{position.ToString()}.";
            score.text = leaderboardEntry.score.ToString();
            nickName.text = leaderboardEntry.player_name;
        }
    }
}