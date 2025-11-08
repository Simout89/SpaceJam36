using System;
using TMPro;
using UnityEngine;
using Users.FateX.Scripts.Enemys;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class DifficultyView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        [Inject] private EnemySpawnDirector _enemySpawnDirector;

        private float lastDif = 0;

        private void OnEnable()
        {
            _enemySpawnDirector.UpdateDifficulty += HandleUpdate;
        }

        private void OnDisable()
        {
            _enemySpawnDirector.UpdateDifficulty -= HandleUpdate;
        }

        private void HandleUpdate(float obj)
        {
            _text.text = $"{obj.ToString("F2")} (+{(obj-lastDif).ToString("F2")})";

            lastDif = obj;
        }
    }
}