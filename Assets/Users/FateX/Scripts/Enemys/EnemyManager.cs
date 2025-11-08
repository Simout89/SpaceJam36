using System;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts;

public class EnemyManager : MonoBehaviour
{
    private Snake _snake;
    private float enemySpeed = 100f;
    
    private List<EnemyBase> _enemies = new List<EnemyBase>();

    public void SetSnake(Snake snake)
    {
        _snake = snake;
    }
    
    public void AddEnemy(EnemyBase enemyBase)
    {
        _enemies.Add(enemyBase);

        enemyBase.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnDie -= HandleDie;
        }
    }

    private void HandleDie(EnemyBase enemyBase)
    {
        _enemies.Remove(enemyBase);
        enemyBase.OnDie -= HandleDie;
        enemyBase.DropXp();
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
            
            _enemies[i].Move((nearestSegment.position - _enemies[i].transform.position).normalized * (Time.deltaTime * enemySpeed));
        }
    }
}
