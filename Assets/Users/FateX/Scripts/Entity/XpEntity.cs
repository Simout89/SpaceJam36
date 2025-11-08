using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Data.Enity;

namespace Users.FateX.Scripts.Entity
{
    public class XpEntity : Entity
    {
        [field: SerializeField] public XpEntityData XpEntityData { get; private set; }
    }
}