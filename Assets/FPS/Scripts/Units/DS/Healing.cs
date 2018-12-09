using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class Healing
    {
        [SerializeField] float m_value;

        public float value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public Healing(float value)
        {
            this.value = value;
        }

        public Healing()
            : this(0f)
        {
        }

        public Healing(Healing copy)
            : this(copy.value)
        {
        }

        public static Healing operator*(Healing lhs, float rhs)
        {
            return new Healing(lhs.value * rhs);
        }
    }
}
