using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(UnitInventory))]
    [RequireComponent(typeof(UnitMetrics))]
    [RequireComponent(typeof(UnitMotor))]
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
        private UnitInventory   m_inventory;
        private UnitMetrics     m_metrics;
        private UnitMotor       m_motor;
        private UnitTriggers    m_triggers;
        private UnitVision      m_vision;

        [SerializeField] Health m_health;
        [SerializeField] Status m_status;


        /* Properties
        * * * * * * * * * * * * * * * */
        public UnitInventory inventory
        {
            get
            {
                if (!m_inventory)
                {
                    m_inventory = GetComponent<UnitInventory>();
                }
                return m_inventory;
            }
        }

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

        public UnitMotor motor
        {
            get
            {
                if (!m_motor)
                {
                    m_motor = GetComponent<UnitMotor>();
                }
                return m_motor;
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
            private set { m_health = value; }
        }

        public Status status
        {
            get { return m_status; }
            private set { m_status = value; }
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

        public void Spawn(GameObject gameObject)
        {
            if(gameObject)
                Instantiate(gameObject, null, true).transform.position = transform.position;
        }

        public void SpawnCombatObject(CombatObject value)
        {
            CombatObject obj;
            if (value && (obj = Instantiate(value, null)))
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.data.owner = this;
                obj.Spawn();
            }
        }


        /* Interfaces
        * * * * * * * * * * * * * * * */
        public void OnSpawn(UnitEvent ev)
        {
            health.SetDead(false);
            health.Set(health.maximum);
            ev.unitSystem.Register(this);
        }

        public void OnDeath(UnitEvent ev)
        {
            if(!health.dead)
            {
                health.SetDead(true);
                ev.unitSystem.Unregister(this);
            }
        }

        public void OnDoDamage(UnitEvent ev)
        {
        }

        public void OnDoHealing(UnitEvent ev)
        {
        }

        public void OnReceiveDamage(UnitEvent ev)
        {
            health.Modify(-ev.data.damage.amount);
            
            if(!health.dead && health.CheckDead())
            {
                triggers.Broadcast(EventType.OnDeath, ev);
            }
        }

        public void OnReceiveHealing(UnitEvent ev)
        {
            health.Modify(ev.data.healing.amount);
        }
    }

}
