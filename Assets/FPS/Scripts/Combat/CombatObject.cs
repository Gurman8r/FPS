using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [ExecuteInEditMode]
    public abstract class CombatObject : MonoBehaviour
    {
        public static readonly string Tag = "Combat";

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Object Settings")]
        [SerializeField] bool       m_active;
        [SerializeField] ObjectData m_data;

        [Header("Object Runtime")]
        [SerializeField] float      m_timer;
        [SerializeField] float      m_maxTimer;
        [SerializeField] List<Unit> m_hitList;


        /* Properties
        * * * * * * * * * * * * * * * */
        public bool active
        {
            get { return m_active; }
            set { m_active = value; }
        }

        public ObjectData data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public float timer
        {
            get { return m_timer; }
            protected set { m_timer = value; }
        }

        public float maxLifespan
        {
            get { return m_maxTimer; }
            protected set { m_maxTimer = value; }
        }

        public List<Unit> hitList
        {
            get { return m_hitList; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Start()
        {
            if(Application.isPlaying)
            {
                if (gameObject.tag != Tag) gameObject.tag = Tag;
            }
        }

        protected virtual void Update()
        {
            if(Application.isPlaying)
            {
                if (maxLifespan > 0f)
                {
                    if (timer >= maxLifespan)
                    {
                        Kill();

                        return;
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        protected bool AddHit(Unit other)
        {
            if (other && !hitList.Contains(other))
            {
                hitList.Add(other);
                return true;
            }
            return false;
        }

        protected bool CheckHitUnit(Collider hit, out Unit other)
        {
            if (hit && (hit.gameObject.tag == Unit.Tag))
            {
                if ((other = hit.gameObject.GetComponent<Unit>()))
                {
                    return (other != data.owner);
                }
            }
            other = null;
            return false;
        }

        protected virtual void OnHitUnit(Unit other)
        {
            if(other)
            {
                other.triggers.Broadcast(EventType.OnRecieveDamage, new UnitEvent
                {
                    combat = data
                });
            }

            if (data.owner)
            {
                data.owner.triggers.Broadcast(EventType.OnDoDamage, new UnitEvent
                {
                    combat = data
                });
            }
        }


        public virtual void Kill()
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public virtual void Spawn()
        {
            active = true;
            timer = 0f;
            maxLifespan = data.lifeSpan;
            gameObject.SetActive(true);
        }


        public void SpawnObject(CombatObject value)
        {
            CombatObject obj;
            if(value && (obj = Instantiate(value, null)))
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.data.owner = data.owner;
                obj.Spawn();
            }
        }
    }

}
