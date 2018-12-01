using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(UnitMetrics))]
    [RequireComponent(typeof(UnitTriggers))]
    [RequireComponent(typeof(UnitVision))]
    [DisallowMultipleComponent]
    public sealed class Unit : MonoBehaviour
        , ISpawnHandler
        , IDamageSource
        , IHealingSource
        , IDamageTarget
        , IHealingTarget
    {
        public static readonly string Tag = "Unit";

        /* Variables
        * * * * * * * * * * * * * * * */
        private UnitMetrics     m_metrics;
        private UnitTriggers    m_triggers;
        private UnitVision      m_vision;

        [SerializeField] Health m_health;


        /* Properties
        * * * * * * * * * * * * * * * */
        public UnitMetrics metrics
        {
            get
            {
                if(!m_metrics)
                {
                    m_metrics = GetComponent<UnitMetrics>();
                }
                return m_metrics;
            }
        }

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

        public UnitVision vision
        {
            get
            {
                if(!m_vision)
                {
                    m_vision = GetComponent<UnitVision>();
                }
                return m_vision;
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
            if(Application.isPlaying)
            {
                if (gameObject.tag != Tag) gameObject.tag = Tag;

                triggers.Broadcast(EventType.OnSpawn, new UnitEvent
                {
                });
            }
        }

        private void Update()
        {
            if(Application.isPlaying)
            {
                if ((health.current <= 0f) && (!health.dead))
                {
                    
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void Destroy()
        {
            if(Application.isPlaying)
            {
                Destroy(gameObject);
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void OnDeath(UnitEvent unitEvent)
        {
            if(!health.dead)
            {
                health.SetDead(true);
            }
        }

        public void OnSpawn(UnitEvent unitEvent)
        {
            health.SetDead(false);
            health.Set(health.maximum);
        }

        public void OnDoDamage(UnitEvent unitEvent)
        {
            metrics.damageDealt += Mathf.Abs(unitEvent.combat.damage.amount);
        }

        public void OnDoHealing(UnitEvent unitEvent)
        {
            metrics.healingDone += Mathf.Abs(unitEvent.combat.healing.amount);
        }

        public void OnRecieveDamage(UnitEvent unitEvent)
        {
            float value = Mathf.Abs(unitEvent.combat.damage.amount);
            metrics.damageRecieved += value;
            health.Modify(-value);
            
            if(!health.dead && health.CheckDead())
            {
                triggers.Broadcast(EventType.OnDeath, new UnitEvent
                {
                });
            }
        }

        public void OnRecieveHealing(UnitEvent unitEvent)
        {
            float value = Mathf.Abs(unitEvent.combat.healing.amount);
            health.Modify(+value);
            metrics.healingRecieved += value;
        }
    }

}
