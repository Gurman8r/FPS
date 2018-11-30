using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class Damage
    {
        public float amount;

        public static implicit operator bool(Damage value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }
}

