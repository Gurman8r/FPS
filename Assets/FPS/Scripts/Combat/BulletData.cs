using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    [Serializable]
    public sealed class BulletData
    {
        public Unit         owner;
        public LayerMask    layerMask;
        public Vector3      position;
        public Vector3      direction;
        public float        speed;
        public float        lifeSpan;
        [Space]
        public Damage       damage;
        public Healing      healing;
    }

}
