using System;
using UnityEngine;
using UnityEngine.UI;

namespace Users.FateX.Scripts.View
{
    public class ChoiceCard: MonoBehaviour
    {
        [field: SerializeField] public SnakeBodyPart SnakeBodyPart { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
    }
}