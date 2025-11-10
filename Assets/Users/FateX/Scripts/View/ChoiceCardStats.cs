using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.View
{
    public class ChoiceCardStats: ChoiceCard
    {
        [SerializeField] private TMP_Text _text;
        
        private System.Action<SnakeStats, float> _apply;
        private string _statName;
        private float _statValue;

        private SnakeStats _snakeStats;

        public void Init(SnakeStats snakeStats)
        {
            _snakeStats = snakeStats;
            
            int random = Random.Range(0, 4);
            _statValue = Mathf.Round(Random.Range(0.1f, 0.5f) * 10f) / 10f;

            
            switch (random)
            {
                case 0:
                    _statName = "Damage";
                    _apply = (s, v) => s.Damage += v;
                    break;
                case 1:
                    _statName = "Fire rate";
                    _apply = (s, v) => s.FireRate += v;
                    break;
                case 2:
                    _statName = "Projectile";
                    _apply = (s, v) => s.ProjectileCount += v;
                    break;
                case 3:
                    _statName = "Range";
                    _apply = (s, v) => s.Range += v;
                    break;  
            }

            if (_statName != "Damage")
            {
                _text.text = $"{_statName}: <color=#DB8536>+{_statValue:F1}";
            }
            else
            {
                _text.text = $"{_statName}: <color=#DB8536>+{_statValue * 10}";
            }
            
            Button.onClick.AddListener(ApplyStat);
        }

        public void ApplyStat()
        {
            _apply(_snakeStats, _statValue);
        }
    }
}