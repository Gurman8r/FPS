using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ML
{
    [RequireComponent(typeof(UnitMotor))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class UnitController : UnitBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private UnitMotor m_motor;
        private NavMeshAgent m_agent;

        [SerializeField] float m_jumpHeight = 1;
        [SerializeField] float m_moveSpeed = 1;

        /* Properties
        * * * * * * * * * * * * * * * */
        public UnitMotor motor
        {
            get
            {
                if(!m_motor)
                {
                    m_motor = GetComponent<UnitMotor>();
                }
                return m_motor;
            }
        }

        public NavMeshAgent agent
        {
            get
            {
                if (!m_agent)
                {
                    m_agent = GetComponent<NavMeshAgent>();
                }
                return m_agent;
            }
        }

        public float jumpHeight
        {
            get { return m_jumpHeight; }
            protected set { m_jumpHeight = value; }
        }

        public float moveSpeed
        {
            get { return m_moveSpeed; }
            protected set { m_moveSpeed = value; }
        }
    }
}
