using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class GameTimer
    {
        private float duration = 99999;
        private bool _isRunning;
        private CancellationTokenSource _cts;
        
        public event Action<int> OnSecondChanged;
        public event Action OnTimerEnd;

        public async void StartTimer()
        {
            if (_isRunning) return;
            
            Debug.Log("Таймер запущен");

            _isRunning = true;
            _cts = new CancellationTokenSource();
            
            try
            {
                await RunTimerAsync(_cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Таймер был отменён");
            }
        }
        
        public async void StartTimer(float duration)
        {
            this.duration = duration;
            
            if (_isRunning) return;
            
            Debug.Log("Таймер запущен");

            _isRunning = true;
            _cts = new CancellationTokenSource();
            
            try
            {
                await RunTimerAsync(_cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Таймер был отменён");
            }
        }

        public void StopTimer()
        {
            if (_isRunning)
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;
                _isRunning = false;
            }
        }

        private async UniTask RunTimerAsync(CancellationToken token)
        {
            float elapsed = 0f;
            int lastSecond = -1;

            while (elapsed < duration)
            {
                token.ThrowIfCancellationRequested();
                
                elapsed += Time.deltaTime;
                
                int currentSecond = Mathf.FloorToInt(elapsed);
                if (currentSecond != lastSecond)
                {
                    lastSecond = currentSecond;
                    OnSecondChanged?.Invoke(currentSecond);
                }
                
                await UniTask.Yield(token);
            }

            _isRunning = false;
            OnTimerEnd?.Invoke();
            
            Debug.Log("Таймер закончился");
        }
    }
}