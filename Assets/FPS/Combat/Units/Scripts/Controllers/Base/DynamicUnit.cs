using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class DynamicUnit : BaseUnitController
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        private NavMeshAgent m_agent;

        /* Properties
        * * * * * * * * * * * * * * * */
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

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }


        /* Functions
        * * * * * * * * * * * * * * * */
    }
}
