using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(UnitTriggers))]
    [RequireComponent(typeof(UnitMotor))]
    [RequireComponent(typeof(UnitCombat))]
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
        private UnitCombat      m_combat;
        private UnitInventory   m_inventory;
        private UnitVision      m_vision;

        [SerializeField] Health m_health;


        /* Properties
        * * * * * * * * * * * * * * * */
        public UnitCombat combat
        {
            get
            {
                if(!m_combat)
                {
                    m_combat = GetComponent<UnitCombat>();
                }
                return m_combat;
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

        
        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                triggers.OnSpawn(new SpawnEvent(this) { });
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

        public BaseEntity CreateObject(BaseEntity prefab)
        {
            BaseEntity obj = null;
            if (prefab && (obj = Instantiate(prefab, null, true)))
            {
                obj.owner = this;
            }
            return obj;
        }

        public BaseEntity CreateAndSpawnObject(BaseEntity prefab)
        {
            BaseEntity obj = null;
            if (prefab && (obj = Instantiate(prefab, null, true)))
            {
                obj.gameObject.SetActive(true);
                obj.owner = this;
                obj.Spawn();
            }
            return obj;
        }

        public void SpawnAtSelf(BaseEntity prefab)
        {
            BaseEntity obj;
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
