using UnityEngine;
using Zenject;
using IPoolable = Lean.Pool.IPoolable;

namespace Users.FateX.Scripts
{
    public class LightingProjectile: MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject[] lightingVariant;

        public void OnSpawn()
        {
            foreach (var VARIABLE in lightingVariant)
            {
                VARIABLE.SetActive(false);
            }
            
            lightingVariant[Random.Range(0, lightingVariant.Length)].SetActive(true);
        }

        public void OnDespawn()
        {
            
        }
    }
}