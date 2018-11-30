using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class Healing
    {
        public float amount;

        public static implicit operator bool(Healing value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }
}
