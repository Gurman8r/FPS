using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    [RequireComponent(typeof(UnitTriggers))]
    public sealed class Unit : MonoBehaviour
    {
        public static readonly string Tag = "Unit";

        /* Variables
        * * * * * * * * * * * * * * * */
        private UnitTriggers m_triggers;

        [SerializeField] Health m_health;


        /* Properties
        * * * * * * * * * * * * * * * */
        public UnitTriggers triggers
        {
            get
            {
                if(!m_triggers)
                {
                    m_triggers = GetComponent<UnitTriggers>();
                }
                return m_triggers;
            }
        }

        public Health health
        {
            get { return m_health; }
            set { m_health = value; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if (gameObject.tag != Tag)
                gameObject.tag = Tag;

            triggers.OnSpawn(new UnitEventData(UnitSystem.current)
            {
            });
        }

        private void Update()
        {
            if(health.current <= 0f)
            {
                triggers.OnDeath(new UnitEventData(UnitSystem.current)
                {
                });
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
