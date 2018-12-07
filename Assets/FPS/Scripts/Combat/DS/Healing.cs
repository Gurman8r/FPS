using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class Healing
    {
        [SerializeField] float  m_amount;
        [SerializeField] float  m_duration;

        public float amount
        {
            get { return Mathf.Abs(m_amount); }
            set { m_amount = value; }
        }

        public float duration
        {
            get { return m_duration; }
        }

        public static implicit operator bool(Healing value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }
}
