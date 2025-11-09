using System;
using DG.Tweening;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeBodyPart: MonoBehaviour
    {
        [field: SerializeField] public SnakeBodyPartHealth SnakeBodyPartHealth { get; private set; }
        [field: SerializeField] public TriggerDetector TriggerDetector { get; private set; }
        [field: SerializeField] public SnakeBodyVariant[] SnakeBodyVariants { get; private set; }
        [field: SerializeField] public SpriteRenderer[] SnakeBodySpriteRenderers { get; private set; }

        public void DamageEffect()
        {
            foreach (var VARIABLE in SnakeBodySpriteRenderers)
            {
                if (VARIABLE.enabled)
                {
                    VARIABLE.DOComplete();
                    
                    VARIABLE.material.SetFloat("_FlashAmount", 1f);
                    VARIABLE.material.DOFloat(0f, "_FlashAmount", 0.2f);
                }
            }
        }
    }

    [Serializable]
    public class SnakeBodyVariant
    {
        [field: SerializeField] public GameObject[] BodyParts { get; private set; }
    }
}