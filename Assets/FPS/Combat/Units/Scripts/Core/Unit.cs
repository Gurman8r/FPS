using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(UnitTriggers))]
    [RequireComponent(typeof(UnitMotor))]
    [RequireComponent(typeof(UnitInventory))]
    [RequireComponent(typeof(UnitVision))]
    [DisallowMultipleComponent]
    public sealed class Unit : MonoBehaviour
    {
        public const string Tag = "Unit";

        /* Variables
        * * * * * * * * * * * * * * * */
        private UnitTriggers    m_triggers;
        private UnitMotor       m_motor;
        private UnitInventory   m_inventory;
        private UnitVision      m_vision;

        [SerializeField] int    m_id;
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

        public int id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public Health health
        {
            get { return m_health; }
            private set { m_health = value; }
        }

        
        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                triggers.OnSpawn(new UnitEvent(this){});
            }
        }
        

        /* Functions
        * * * * * * * * * * * * * * * */
        public void DestroySelf()
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
        }

        public CombatObject CreateObject(CombatObject prefab)
        {
            CombatObject obj = null;
            if (prefab && (obj = Instantiate(prefab, null, true)))
            {
                obj.owner = this;
            }
            return obj;
        }

        public CombatObject CreateAndSpawnObject(CombatObject prefab)
        {
            CombatObject obj = null;
            if (prefab && (obj = Instantiate(prefab, null, true)))
            {
                obj.gameObject.SetActive(true);
                obj.owner = this;
                obj.Spawn();
            }
            return obj;
        }

        public void SpawnAtSelf(CombatObject prefab)
        {
            CombatObject obj;
            if (prefab && (obj = Instantiate(prefab, null, true)))
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.gameObject.SetActive(true);
                obj.owner = this;
                obj.Spawn();
            }
        }
    }

}
