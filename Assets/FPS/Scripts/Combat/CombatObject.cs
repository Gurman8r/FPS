using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [ExecuteInEditMode]
    public abstract class CombatObject : MonoBehaviour
    {
        public static readonly string Tag = "Combat";

        /* Variables
        * * * * * * * * * * * * * * * */
        private Collider    m_collider;
        private Rigidbody   m_rigidbody;

        [Header("Object Settings")]
        [SerializeField] LayerMask  m_solidLayer;
        [SerializeField] LayerMask  m_unitLayer;
        [Space]
        [SerializeField] Damage     m_damage;
        [SerializeField] Motion     m_motion;
        [SerializeField] float      m_lifeSpan;
        [SerializeField] bool       m_canDamageSelf;
        [Space]
        [SerializeField] UnityEvent m_onSpawn;
        [SerializeField] UnityEvent m_onKill;
        [SerializeField] UnityEvent m_onStop;

        [Header("Object Runtime")]
        [SerializeField] Unit       m_owner;
        [SerializeField] bool       m_active;
        [SerializeField] float      m_timer;
        [SerializeField] List<Unit> m_hits;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new Collider collider
        {
            get
            {
                if(!m_collider)
                {
                    m_collider = GetComponent<Collider>();
                }
                return m_collider;
            }
        }

        public new Rigidbody rigidbody
        {
            get
            {
                if(!m_rigidbody)
                {
                    m_rigidbody = GetComponent<Rigidbody>();
                }
                return m_rigidbody;
            }
        }
        
        public LayerMask solidLayer
        {
            get { return m_solidLayer; }
        }

        public LayerMask unitLayer
        {
            get { return m_unitLayer; }
        }

        public float lifeSpan
        {
            get { return m_lifeSpan; }
            protected set { m_lifeSpan = value; }
        }

        public Damage damage
        {
            get { return m_damage; }
            set { m_damage = value; }
        }

        public Motion motion
        {
            get { return m_motion; }
            set { m_motion = value; }
        }

        public UnityEvent onSpawn
        {
            get { return m_onSpawn; }
        }

        public UnityEvent onKill
        {
            get { return m_onKill; }
        }

        public UnityEvent onStop
        {
            get { return m_onStop; }
        }


        public Unit owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public bool active
        {
            get { return m_active; }
            set { m_active = value; }
        }

        public float timer
        {
            get { return m_timer; }
            protected set { m_timer = value; }
        }

        public List<Unit> hits
        {
            get { return m_hits; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Update()
        {
            if(Application.isPlaying)
            {
                if (lifeSpan > 0f)
                {
                    if (timer >= lifeSpan)
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

        protected virtual void OnTriggerEnter(Collider c)
        {
            if (active)
            {
                Unit u;
                if (IsValidHit(c, out u))
                {
                    if (RegisterHit(u))
                    {
                        OnHit(u);
                    }
                }
            }
        }

        protected virtual void OnTriggerStay(Collider c)
        {
            OnTriggerEnter(c);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        protected bool IsValidHit(Collider hit, out Unit other)
        {
            if (hit && (hit.gameObject.tag == Unit.Tag))
            {
                if ((other = hit.gameObject.GetComponent<Unit>()))
                {
                    return (m_canDamageSelf || (other != owner));
                }
            }
            other = null;
            return false;
        }

        protected void ClearHits()
        {
            hits.Clear();
        }

        protected Unit RegisterHit(Unit other)
        {
            if (other && !hits.Contains(other))
            {
                hits.Add(other);
                return other;
            }
            return null;
        }

        protected virtual void OnHit(Unit other)
        {
            if (other)
            {
                other.triggers.OnReceiveDamage(new DamageEvent(owner, other, m_damage));
            }
        }


        public virtual void Kill()
        {
            if (Application.isPlaying)
            {
                onKill.Invoke();
                Destroy(gameObject);
            }
        }

        public virtual void Spawn()
        {
            active = true;
            timer = 0f;
            gameObject.SetActive(true);
            onSpawn.Invoke();
        }


        public void SpawnObject(CombatObject prefab)
        {
            CombatObject obj;
            if (owner && (obj = owner.CreateAndSpawnObject(prefab)))
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
            }
        }
    }

}
