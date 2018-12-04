using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class BulletObject : CombatObject
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Rigidbody       m_rigidbody;
        private SphereCollider  m_collider;

        [Header("Bullet Settings")]
        [Range(0.0001f, 1f)]
        [SerializeField] float  m_minMass = 0.0001f;
        [Range(0.0001f, 1f)]
        [SerializeField] float  m_maxMass = 1f;
        [SerializeField] bool   m_randomMass;
        [SerializeField] float  m_stopRadius = 0.5f;
        [Space]
        [SerializeField] bool   m_killOnHit;
        [SerializeField] bool   m_stopOnHit;
        [SerializeField] bool   m_solidOnHit;
        [SerializeField] bool   m_respawnOnHit;
        [SerializeField] bool   m_invertGravity;
        [Space]
        [SerializeField] UnityEvent m_onSpawn;
        [SerializeField] UnityEvent m_onKill;
        [SerializeField] UnityEvent m_onStop;

        [Header("Bullet Runtime")]
        [SerializeField] float  m_curMass = 0.5f;

        private Ray         m_ray = new Ray();
        private RaycastHit  m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
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

        public new SphereCollider collider
        {
            get
            {
                if(!m_collider)
                {
                    m_collider = GetComponent<SphereCollider>();
                }
                return m_collider;
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

            if(!m_randomMass)
            {
                rigidbody.mass = (m_curMass = m_minMass);
            }
        }

        private void FixedUpdate()
        {
            if (Application.isPlaying)
            {
                if(active)
                {
                    Collider[] hits = Physics.OverlapSphere(rigidbody.position, m_stopRadius, data.layerMask);

                    foreach (Collider c in hits)
                    {
                        Unit u;
                        if (CheckHitUnit(c, out u))
                        {
                            if (AddHit(u))
                            {
                                OnHitUnit(u);
                            }
                        }

                        if (m_killOnHit && !m_stopOnHit)
                        {
                            Kill();
                        }
                        else
                        {
                            Stop();
                        }
                    }
                }
                else
                {
                    rigidbody.useGravity = !m_invertGravity;

                    if (m_invertGravity)
                    {
                        rigidbody.velocity = -(Physics.gravity * rigidbody.mass);
                    }
                }

            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void Spawn()
        {
            base.Spawn();

            rigidbody.position = data.pos;

            rigidbody.velocity = data.dir * data.speed;

            rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;

            rigidbody.mass = (m_randomMass)
                ? (m_curMass = Random.Range(m_minMass, m_maxMass))
                : (m_curMass = m_minMass);

            collider.isTrigger = true;

            transform.LookAt(rigidbody.position + rigidbody.velocity);

            m_onSpawn.Invoke();
        }

        public override void Kill()
        {
            m_onKill.Invoke();

            base.Kill();
        }

        private void Stop()
        {
            if(active)
            {
                active = false;
                m_onStop.Invoke();
                collider.isTrigger = !m_solidOnHit;
                rigidbody.velocity = Vector3.zero;
                rigidbody.interpolation = RigidbodyInterpolation.None;
                if (m_respawnOnHit) { timer = 0f; }
            }
        }
    }
}
