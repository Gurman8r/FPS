using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class UnitCombat : UnitBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] CastingSource  m_right;
        [SerializeField] CastingSource  m_left;
        [SerializeField] CastingSource  m_instant;
        [SerializeField] float  m_outOfCombatDelay = 5f;

        [Header("Runtime")]
        [SerializeField] bool   m_inCombat;
        [SerializeField] float  m_outOfCombatTimer;

        /* Properties
        * * * * * * * * * * * * * * * */
        public CastingSource right
        {
            get { return m_right; }
        }

        public CastingSource left
        {
            get { return m_left; }
        }

        public CastingSource instant
        {
            get { return m_instant; }
        }

        public float outOfCombatDelay
        {
            get { return m_outOfCombatDelay; }
        }

        public float outOfCombatTimer
        {
            get { return m_outOfCombatTimer; }
            private set { m_outOfCombatTimer = value; }
        }

        public bool inCombat
        {
            get { return m_inCombat = (outOfCombatTimer > 0f); }
            set { outOfCombatTimer = (m_inCombat = value) ? outOfCombatDelay : 0f; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Update()
        {
            base.Update();

            if (Application.isPlaying)
            {
                if (inCombat)
                {
                    outOfCombatTimer -= Time.deltaTime;
                }
            }
        }
    }

}
