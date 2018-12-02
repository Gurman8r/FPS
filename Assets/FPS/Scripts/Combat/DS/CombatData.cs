using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class CombatData
    {
        public Unit         owner;
        public LayerMask    layerMask;
        public Vector3      position;
        public Vector3      direction;
        public float        speed;
        public float        lifeSpan;
        public Damage       damage;
        public Healing      healing;

        public static implicit operator bool(CombatData value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }

}
