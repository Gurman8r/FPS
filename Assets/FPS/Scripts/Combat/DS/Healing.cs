using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class Healing
    {
        [SerializeField] float m_amount;

        public float amount
        {
            get { return Mathf.Abs(m_amount); }
            set { m_amount = value; }
        }

        public static implicit operator bool(Healing value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }
}
