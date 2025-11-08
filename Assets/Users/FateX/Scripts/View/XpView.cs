using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class XpView: MonoBehaviour
    {
        [SerializeField] private Image _image;
        [Inject] private PlayerExperience _playerExperience;

        private void OnEnable()
        {
            _playerExperience.OnChangeXp += HandleChangeXp;
        }

        private void OnDisable()
        {
            _playerExperience.OnChangeXp -= HandleChangeXp;
        }

        private void HandleChangeXp()
        {
            _image.fillAmount = _playerExperience.CurrentXp / _playerExperience.NextLevelXp;
        }
    }
}