using System;
using System.Collections.Generic;
using Npgsql;
using UnityEngine;

namespace Users.FateX.Scripts.LeaderBoard
{
    public class LeaderboardManager
    {
        private string connectionString = "Host=95.170.184.22;Port=5432;Username=user;Password=1234;Database=mydb";
    
        // Добавить счёт в базу данных
        public void AddScore(string playerName, int score)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("INSERT INTO leaderboard (player_name, score) VALUES (@name, @score)", conn);
                cmd.Parameters.AddWithValue("name", playerName);
                cmd.Parameters.AddWithValue("score", score);
                cmd.ExecuteNonQuery();
            }
        }

        // Получить топ игроков
        public List<LeaderboardEntry> GetTopScores(int limit = 10)
        {
            var entries = new List<LeaderboardEntry>();
            
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT player_name, score FROM leaderboard ORDER BY score DESC LIMIT @limit", conn);
                cmd.Parameters.AddWithValue("limit", limit);
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(new LeaderboardEntry
                        {
                            PlayerName = reader.GetString(0),
                            Score = reader.GetInt32(1)
                        });
                    }
                }
            }
            
            return entries;
        }
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string PlayerName;
        public int Score;
    }
}