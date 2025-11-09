using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Users.FateX.Scripts;
using Zenject;
using Скриптерсы.Services;

public class Snake : MonoBehaviour
{
    private IInputService _inputService;
    
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int startSize = 3;
    [SerializeField] private float segmentDistance = 0.5f;
    
    [Header("References")]
    [SerializeField] private SnakeBodyPart segmentPrefab;
    [SerializeField] private SnakeHealth snakeHealth;
    [SerializeField] private SnakeInteraction snakeInteraction;
    
    private List<Transform> segments = new List<Transform>();
    public List<Transform> Segments => segments;

    private int currentColor = 0;
    
    public void Init(IInputService inputService)
    {
        _inputService = inputService;
        
        segments.Add(transform);
        for (int i = 0; i < startSize; i++)
        {
            Grow();
        }
    }

    private void Update()
    {
        float horizontal = _inputService.InputSystemActions.Player.Move.ReadValue<Vector2>().x;
        Vector2 joyStickInput = new Vector2(SimpleInput.GetAxisRaw("Horizontal"), -SimpleInput.GetAxisRaw("Vertical"));
        
        float snakeAngle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedJoystick = new Vector2(
            joyStickInput.x * Mathf.Cos(snakeAngle) - joyStickInput.y * Mathf.Sin(snakeAngle),
            joyStickInput.x * Mathf.Sin(snakeAngle) + joyStickInput.y * Mathf.Cos(snakeAngle)
        );
        
        float totalHorizontal = horizontal + rotatedJoystick.x;
        
        transform.Rotate(Vector3.forward * (-totalHorizontal) * rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
        
        for (int i = 1; i < segments.Count; i++)
        {
            Transform prev = segments[i - 1];
            Transform curr = segments[i];

            Vector3 targetPosition = prev.position - prev.up * segmentDistance;
            curr.position = Vector3.Lerp(curr.position, targetPosition, Time.deltaTime * 10f);
            
            Vector3 direction = prev.position - curr.position;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                curr.rotation = Quaternion.Lerp(curr.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10f);
            }
        }
    }

    public void Grow()
    {
        var newSegment = Instantiate(segmentPrefab);
        Transform last = segments[segments.Count - 1];
        newSegment.transform.position = last.position;
        segments.Add(newSegment.transform);
        snakeHealth.Add(newSegment.SnakeBodyPartHealth, newSegment);
        snakeInteraction.AddTrigger(newSegment.TriggerDetector);

        ChangeBodyVariants(newSegment);
    }

    private void ChangeBodyVariants(SnakeBodyPart newSegment)
    {
        foreach (var bodyVariant in newSegment.SnakeBodyVariants)
        {
            foreach (var bodyParts in bodyVariant.BodyParts)
            {
                bodyParts.SetActive(false);
            }
        }
        
        foreach (var bodyParts in newSegment.SnakeBodyVariants[currentColor].BodyParts)
        {
            bodyParts.SetActive(true);
        }
        
        currentColor++;
        if (currentColor >= 3)
            currentColor = 0;
    }

    public void Grow(SnakeBodyPart snakeBodyPart)
    {
        var newSegment = Instantiate(snakeBodyPart);
        Transform last = segments[segments.Count - 1];
        newSegment.transform.position = last.position;
        segments.Add(newSegment.transform);
        snakeHealth.Add(newSegment.SnakeBodyPartHealth, newSegment);
        snakeInteraction.AddTrigger(newSegment.TriggerDetector);

        
        ChangeBodyVariants(newSegment);
    }
    
#if UNITY_EDITOR
    [ContextMenu("GrowDebug")]
    public void GrowDebug()
    {
        for (int i = 0; i < 50; i++)
        {
            Grow();
        }
    }
#endif

}