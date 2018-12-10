using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public abstract class BaseUnitController : UnitBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Base Settings")]
        [SerializeField] bool   m_inCombat;
        [SerializeField] float  m_combatDelay = 5f;
        [SerializeField] float  m_combatTimer;

        /* Properties
        * * * * * * * * * * * * * * * */
        public float combatDelay
        {
            get { return m_combatDelay; }
        }

        public float combatTimer
        {
            get { return m_combatTimer; }
            private set { m_combatTimer = value; }
        }

        public bool inCombat
        {
            get { return m_inCombat = (combatTimer > 0f); }
            set { combatTimer = (m_inCombat = value) ? combatDelay : 0f; }
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

            if(Application.isPlaying)
            {
                if (inCombat)
                {
                    combatTimer -= Time.deltaTime;
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */

    }
}
