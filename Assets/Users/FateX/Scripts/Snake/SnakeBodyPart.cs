using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeBodyPart: MonoBehaviour
    {
        [field: SerializeField] public SnakeBodyPartHealth SnakeBodyPartHealth { get; private set; }
    }
}