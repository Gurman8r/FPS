using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class CastingSource
    {
        public Transform transform;
        public Item item;

        public static implicit operator bool(CastingSource value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }
}
