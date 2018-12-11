using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class MotionData
    {
        [SerializeField] float      m_speed;
        [SerializeField] Vector3    m_origin;
        [SerializeField] Vector3    m_target;
        [SerializeField] Vector3    m_forward;

        public float speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        public Vector3 origin
        {
            get { return m_origin; }
            set { m_origin = value; }
        }

        public Vector3 forward
        {
            get { return m_forward; }
            set { m_forward = value; }
        }

        public Vector3 target
        {
            get { return m_target; }
            set { m_target = value; }
        }
    }
}
