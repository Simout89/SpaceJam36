using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class TimeView: MonoBehaviour
    {

        [Inject] private GameTimer _gameTimer;
        [SerializeField] private TMP_Text _tmpText;

        private void OnEnable()
        {
            _gameTimer.OnSecondChanged += HandleTimeChanged;
        }

        private void OnDisable()
        {
            _gameTimer.OnSecondChanged -= HandleTimeChanged;

        }

        private void HandleTimeChanged(int obj)
        {
            _tmpText.text = $"Life time: <color=#FE8714>{obj.ToString()}";
        }
    }
}