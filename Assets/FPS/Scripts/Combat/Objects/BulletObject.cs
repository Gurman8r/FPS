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

        [Header("Settings")]
        [SerializeField] bool   m_invertGravity;
        [SerializeField] bool   m_randomMass;
        [Range(0.0001f, 1f)]
        [SerializeField] float  m_minMass = 0.0001f;
        [Range(0.0001f, 1f)]
        [SerializeField] float  m_maxMass = 1f;
        [SerializeField] float  m_curMass = 0.5f;
        [Space]
        [SerializeField] float  m_stopRadius = 0.5f;
        [SerializeField] bool   m_destroyOnHit;
        [SerializeField] bool   m_makeSafeOnHit;
        [SerializeField] bool   m_makeSolid;
        [SerializeField] bool   m_resetTimer;
        [Space]
        [SerializeField] UnityEvent m_onSpawn;
        [SerializeField] UnityEvent m_onKill;
        [SerializeField] UnityEvent m_onHitUnit;
        [SerializeField] UnityEvent m_onHitAny;

        [Header("Runtime")]
        [SerializeField] bool m_stopped;


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

        public bool stopped
        {
            get { return m_stopped; }
            private set { m_stopped = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if(Application.isPlaying)
            {
                if (m_randomMass)
                {
                    m_curMass = UnityEngine.Random.Range(m_minMass, m_maxMass);
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if(!m_randomMass)
            {
                m_curMass = m_minMass;
            }

            rigidbody.mass = m_curMass;
        }

        private void FixedUpdate()
        {
            if (Application.isPlaying)
            {
                if (!stopped)
                {
                    if (Physics.CheckSphere(rigidbody.position, m_stopRadius, data.layerMask))
                    {
                        if (m_destroyOnHit && !m_makeSafeOnHit)
                        {
                            Kill();
                        }
                        else
                        {
                            MakeSafe();
                        }

                        m_onHitAny.Invoke();
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

        private void OnTriggerEnter(Collider collider)
        {
            if(!stopped)
            {
                Unit other;
                if (CheckHit(collider, out other))
                {
                    if (AddHit(other))
                    {
                        OnHit(other);

                        m_onHitAny.Invoke();

                        if(m_destroyOnHit && !m_makeSafeOnHit)
                        {
                            Kill();
                        }
                        else
                        {
                            MakeSafe();
                        }
                    }
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void Spawn()
        {
            base.Spawn();

            rigidbody.position = data.position;
            rigidbody.velocity = data.direction * data.speed;
            rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
            collider.isTrigger = true;
            transform.LookAt(rigidbody.position + rigidbody.velocity);
            stopped = false;

            m_onSpawn.Invoke();
        }

        public override void Kill()
        {
            m_onKill.Invoke();

            base.Kill();
        }

        protected override void OnHit(Unit other)
        {
            base.OnHit(other);
        }

        public void MakeSafe()
        {
            rigidbody.velocity = Vector3.zero;
            collider.isTrigger = !m_makeSolid;
            stopped = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;

            if (m_resetTimer)
                timer = 0f;
        }
    }
}
