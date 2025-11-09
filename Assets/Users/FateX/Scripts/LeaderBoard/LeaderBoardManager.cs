using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Users.FateX.Scripts.LeaderBoard
{
    public class LeaderboardManager
    {
        private string supabaseUrl = "https://phmiaappvpzdofykywyi.supabase.co";
        private string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InBobWlhYXBwdnB6ZG9meWt5d3lpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjI2OTI5OTcsImV4cCI6MjA3ODI2ODk5N30.qZVLQg0FLLbh8EEAluPSDrIoos2bVf1x-GCgYNoCylo";

        // Добавить счёт
        public async void AddScore(string playerName, int score, Action<bool> onComplete = null)
        {
            var entry = new LeaderboardEntry { player_name = playerName, score = score };
            string json = JsonUtility.ToJson(entry);
            
            using (UnityWebRequest request = new UnityWebRequest(supabaseUrl + "/rest/v1/leaderboard", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("apikey", supabaseKey);
                request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

                var operation = request.SendWebRequest();
                while (!operation.isDone) await System.Threading.Tasks.Task.Yield();

                bool success = request.result == UnityWebRequest.Result.Success;
                if (success) Debug.Log($"✓ Добавлено: {playerName} - {score}");
                else Debug.LogError($"✗ Ошибка: {request.error}");
                
                onComplete?.Invoke(success);
            }
        }

        // Получить топ игроков
        public async void GetTopScores(int limit, Action<List<LeaderboardEntry>> onComplete)
        {
            string url = supabaseUrl + $"/rest/v1/leaderboard?select=player_name,score&order=score.desc&limit={limit}";
            
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("apikey", supabaseKey);
                request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

                var operation = request.SendWebRequest();
                while (!operation.isDone) await System.Threading.Tasks.Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string json = "{\"entries\":" + request.downloadHandler.text + "}";
                    var response = JsonUtility.FromJson<LeaderboardResponse>(json);
                    Debug.Log($"✓ Получено записей: {response.entries.Count}");
                    onComplete?.Invoke(response.entries);
                }
                else
                {
                    Debug.LogError($"✗ Ошибка: {request.error}");
                    onComplete?.Invoke(new List<LeaderboardEntry>());
                }
            }
        }
    }

    [Serializable]
    public class LeaderboardEntry
    {
        public string player_name;
        public int score;
    }

    [Serializable]
    public class LeaderboardResponse
    {
        public List<LeaderboardEntry> entries;
    }
}