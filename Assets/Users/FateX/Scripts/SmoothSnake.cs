using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Snake2D : MonoBehaviour
{
    [Header("Движение")]
    public float speed = 5f;
    public float rotationSpeed = 150f;
    
    [Header("Тело змеи")]
    public GameObject bodyPrefab;
    public int startBodyParts = 3;
    public float bodyPartDistance = 0.5f;
    
    [Header("Еда")]
    public GameObject foodPrefab;
    
    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionsHistory = new List<Vector3>();
    private Vector2 moveDirection = Vector2.right;
    private float angle = 0f;

    void Start()
    {
        // Создаем начальное тело
        for (int i = 0; i < startBodyParts; i++)
        {
            GrowSnake();
        }
        
        // Создаем еду
        SpawnFood();
    }

    void Update()
    {
        HandleInput();
        MoveSnake();
        UpdateBodyPositions();
    }

    void HandleInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Поворот влево
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            angle += rotationSpeed * Time.deltaTime;
        }
        
        // Поворот вправо
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            angle -= rotationSpeed * Time.deltaTime;
        }

        // Обновляем направление движения
        float radians = angle * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    void MoveSnake()
    {
        // Двигаем голову
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // Записываем позицию в историю
        positionsHistory.Insert(0, transform.position);
        
        // Ограничиваем историю позиций
        if (positionsHistory.Count > bodyParts.Count * 50)
        {
            positionsHistory.RemoveAt(positionsHistory.Count - 1);
        }
    }

    void UpdateBodyPositions()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            // Вычисляем нужное расстояние для этой части тела
            float targetDistance = bodyPartDistance * (i + 1);
            
            // Ищем позицию в истории на нужном расстоянии
            Vector3 targetPosition = GetPositionAtDistance(targetDistance);
            bodyParts[i].transform.position = targetPosition;
            
            // Направление взгляда части тела
            if (i < bodyParts.Count - 1)
            {
                Vector3 direction = bodyParts[i + 1].transform.position - bodyParts[i].transform.position;
                float bodyAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bodyParts[i].transform.rotation = Quaternion.Euler(0, 0, bodyAngle);
            }
        }
    }

    Vector3 GetPositionAtDistance(float distance)
    {
        float currentDistance = 0f;
        
        for (int i = 0; i < positionsHistory.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positionsHistory[i], positionsHistory[i + 1]);
            
            if (currentDistance + segmentDistance >= distance)
            {
                float remainingDistance = distance - currentDistance;
                float t = remainingDistance / segmentDistance;
                return Vector3.Lerp(positionsHistory[i], positionsHistory[i + 1], t);
            }
            
            currentDistance += segmentDistance;
        }
        
        return positionsHistory[positionsHistory.Count - 1];
    }

    void GrowSnake()
    {
        GameObject newPart = Instantiate(bodyPrefab);
        
        if (bodyParts.Count == 0)
        {
            newPart.transform.position = transform.position - (Vector3)moveDirection * bodyPartDistance;
        }
        else
        {
            newPart.transform.position = bodyParts[bodyParts.Count - 1].transform.position;
        }
        
        bodyParts.Add(newPart);
    }

    void SpawnFood()
    {
        Vector2 randomPosition = Random.insideUnitCircle * 8f;
        Instantiate(foodPrefab, randomPosition, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            GrowSnake();
            Destroy(collision.gameObject);
            SpawnFood();
            Debug.Log("Съедена еда! Длина: " + (bodyParts.Count + 1));
        }
        
        if (collision.CompareTag("Body"))
        {
            Debug.Log("Столкновение с телом! Перезапуск...");
            RestartGame();
        }
    }

    void RestartGame()
    {
        // Удаляем все части тела
        foreach (var part in bodyParts)
        {
            Destroy(part);
        }
        bodyParts.Clear();
        positionsHistory.Clear();
        
        // Сбрасываем позицию
        transform.position = Vector3.zero;
        angle = 0f;
        moveDirection = Vector2.right;
        
        // Создаем новое тело
        for (int i = 0; i < startBodyParts; i++)
        {
            GrowSnake();
        }
        
        // Удаляем старую еду
        foreach (var food in GameObject.FindGameObjectsWithTag("Food"))
        {
            Destroy(food);
        }
        SpawnFood();
    }
}