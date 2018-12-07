using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class Health
    {
        [SerializeField] float m_current = 100;
        [SerializeField] float m_minimim = 0f;
        [SerializeField] float m_maximum = 100;
        [SerializeField] bool m_dead = false;

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

        public bool dead
        {
            get { return m_dead; }
            private set { m_dead = value; }
        }

        public float fillAmount
        {
            get
            {
                if (maximum > 0f)
                {
                    if (current <= maximum)
                    {
                        return current / maximum;
                    }
                    return 1f;
                }
                return 0f;
            }
        }

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
            dead = value;
        }
    }

}
