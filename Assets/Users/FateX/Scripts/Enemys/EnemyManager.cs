using System;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using DG.Tweening;
using Users.FateX.Scripts;

public class EnemyManager : MonoBehaviour
{
    private Snake _snake;
    private float enemySpeed = 100f;
    
    [Header("Настройки покачивания")]
    [SerializeField] private float swingAngle = 10f;
    [SerializeField] private float swingDuration = 0.8f;
    
    private List<EnemyBase> _enemies = new List<EnemyBase>();
    private Dictionary<EnemyBase, Vector3> _originalScales = new Dictionary<EnemyBase, Vector3>();

    public void SetSnake(Snake snake)
    {
        _snake = snake;
    }
    
    public void AddEnemy(EnemyBase enemyBase)
    {
        _enemies.Add(enemyBase);
        _originalScales[enemyBase] = enemyBase.transform.localScale;

        enemyBase.OnDie += HandleDie;
        
        // Запускаем покачивание
        StartSwing(enemyBase);
    }

    private void StartSwing(EnemyBase enemy)
    {
        enemy.body.DORotate(new Vector3(0, 0, -swingAngle), swingDuration)
            .From(new Vector3(0, 0, swingAngle))
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetId(enemy);
    }

    private void OnDisable()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnDie -= HandleDie;
            DOTween.Kill(enemy);
        }
    }

    private void HandleDie(EnemyBase enemyBase)
    {
        _enemies.Remove(enemyBase);
        _originalScales.Remove(enemyBase);
        enemyBase.OnDie -= HandleDie;
        
        // Останавливаем анимацию покачивания
        DOTween.Kill(enemyBase);
        
        LeanPool.Despawn(enemyBase.gameObject);
    }

    public void FixedUpdate()
    {
        if (_snake == null || _snake.Segments.Count == 0) return;
        
        for (int i = 0; i < _enemies.Count; i++)
        {
            Transform nearestSegment = _snake.Segments[0];
            
            for (int j = 0; j < _snake.Segments.Count; j++)
            {
                if (Vector3.Distance(_snake.Segments[j].position, _enemies[i].transform.position) < 
                    Vector3.Distance(nearestSegment.position, _enemies[i].transform.position))
                {
                    nearestSegment = _snake.Segments[j];
                }
            }
            
            Vector3 direction = (nearestSegment.position - _enemies[i].transform.position).normalized;
            
            // Поворот врага лицом к змейке через scale.x
            if (_originalScales.ContainsKey(_enemies[i]))
            {
                Vector3 scale = _originalScales[_enemies[i]];
                scale.x = direction.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                _enemies[i].body.localScale = scale;
            }
            
            _enemies[i].Move(direction * (Time.deltaTime * enemySpeed));
        }
    }
}