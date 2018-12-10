using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class Damage
    {
        [SerializeField] float m_value;

        public float value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public Damage(float value)
        {
            this.value = value;
        }

        public Damage()
            : this(0f)
        {
        }

        public Damage(Damage copy)
            : this(copy.value)
        {
        }

        public static Damage operator *(Damage lhs, float rhs)
        {
            return new Damage(lhs.value * rhs);
        }
    }
}

