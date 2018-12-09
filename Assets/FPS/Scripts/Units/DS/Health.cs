using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class Health
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] float  m_current;
        [SerializeField] float  m_minimim;
        [SerializeField] float  m_maximum;
        [SerializeField] bool   m_isDead;

        /* Properties
        * * * * * * * * * * * * * * * */
        public float current
        {
            get { return m_current; }
            private set { m_current = value; }
        }

        public float minimum
        {
            get { return m_minimim; }
            private set { m_minimim = value; }
        }

        public float maximum
        {
            get { return m_maximum; }
            private set { m_maximum = value; }
        }

        public float fillAmount
        {
            get
            {
                return
                    (maximum > 0f)
                        ? (current <= maximum)
                            ? (current / maximum)
                            : 1f
                        : 0f;
            }
        }

        public bool isDead
        {
            get { return m_isDead; }
        }

        public bool isFull
        {
            get { return current >= maximum; }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void Set(float value)
        {
            current = Mathf.Clamp(value, minimum, maximum);
        }

        public void Modify(float value)
        {
            Set(current + value);
        }

        public bool CheckDead()
        {
            return (current <= 0f);
        }

        public void SetDead(bool value)
        {
            m_isDead = value;
        }

        public void Modify(Healing healing)
        {
            Modify(healing.value);
        }

        public void Modify(Damage damage)
        {
            Modify(-damage.value);
        }
    }

}
