using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeBodyPart: MonoBehaviour
    {
        [field: SerializeField] public SnakeBodyPartHealth SnakeBodyPartHealth { get; private set; }
        [field: SerializeField] public TriggerDetector TriggerDetector { get; private set; }
        [field: SerializeField] public SnakeBodyVariant[] SnakeBodyVariants { get; private set; }
    }

    [Serializable]
    public class SnakeBodyVariant
    {
        [field: SerializeField] public GameObject[] BodyParts { get; private set; }
    }
}