using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class ObjectData
    {
        [Header("Settings")]
        public LayerMask    solidLayer;
        public LayerMask    unitLayer;
        public float        speed;
        public float        lifeSpan;
        public Damage       damage;
        public Healing      healing;
        
        [Header("Runtime")]
        public Unit         owner;
        public Vector3      pos;
        public Vector3      dir;
        public Vector3      dest;

        public static implicit operator bool(ObjectData value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }

}
