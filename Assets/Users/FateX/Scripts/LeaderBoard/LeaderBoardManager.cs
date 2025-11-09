using System;
using System.Collections.Generic;
using Npgsql;
using UnityEngine;

namespace Users.FateX.Scripts.LeaderBoard
{
    public class LeaderboardManager
    {
        private string connectionString = "Host=95.170.184.22;Port=5432;Username=user;Password=1234;Database=mydb";
    
        public void AddScore(string playerName, int score)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                
                    string sql = "INSERT INTO leaderboard (player_name, score) VALUES (@name, @score)";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("name", playerName);
                        cmd.Parameters.AddWithValue("score", score);
                        cmd.ExecuteNonQuery();
                    }
                
                    Debug.Log($"✓ Счёт добавлен: {playerName} - {score}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"✗ Ошибка при добавлении счёта: {ex.Message}");
            }
        }

        public List<LeaderboardEntry> GetTopScores(int limit = 10)
        {
            List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                
                    string sql = "SELECT player_name, score FROM leaderboard ORDER BY score DESC LIMIT @limit";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
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
                
                    Debug.Log($"✓ Получено {entries.Count} записей из таблицы лидеров");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"✗ Ошибка при получении счётов: {ex.Message}");
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