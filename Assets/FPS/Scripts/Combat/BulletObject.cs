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
        private Rigidbody           m_rigidbody;
        private SphereCollider      m_collider;

        [Header("Physics")]
        [Range(0.0001f, 1f)]
        [SerializeField] float      m_minMass       = 0.0001f;
        [Range(0.0001f, 1f)]
        [SerializeField] float      m_maxMass       = 1f;
        [SerializeField] float      m_curMass       = 0.5f;
        [SerializeField] bool       m_randomMass    = false;

        [Header("Transform")]
        [SerializeField] Vector3    m_curScale      = Vector3.one * 1.0f;
        [SerializeField] Vector3    m_minScale      = Vector3.one * 0.1f;
        [SerializeField] Vector3    m_maxScale      = Vector3.one * 2.0f;
        [SerializeField] bool       m_randomScale   = false;
        [SerializeField] bool       m_uniformScale  = true;

        [Header("Collisions")]
        [SerializeField] Collider[] m_check;
        [SerializeField] float      m_firstCheckRadius  = 0.1f;
        [SerializeField] float      m_doubleCheckRadius = 0.5f;
        [SerializeField] bool       m_checkInUpdate     = true;
        [SerializeField] bool       m_checkInFixedUpdate= true;
        [SerializeField] bool       m_checkInTrigger    = true;

        [Header("Flags")]
        [SerializeField] bool       m_useGravity = true;
        [SerializeField] bool       m_invertGravity;
        [SerializeField] bool       m_destroyOnHit;
        [SerializeField] bool       m_piercing;
        [SerializeField] bool       m_solid;
        [SerializeField] float      m_persistance;
        [SerializeField] float      m_normalScale;

        [Header("Events")]
        [SerializeField] UnityEvent m_onSpawn;
        [SerializeField] UnityEvent m_onKill;
        [SerializeField] UnityEvent m_onStop;

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

            if(!Application.isPlaying)
            {
                if (active)
                {
                    if(m_checkInUpdate)
                    {
                        CheckForHits();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (Application.isPlaying)
            {
                if (active)
                {
                    if(m_checkInFixedUpdate)
                    {
                        CheckForHits();
                    }
                }
                else
                {
                    if (m_useGravity)
                    {
                        rigidbody.useGravity = !m_invertGravity;

                        if (m_invertGravity)
                        {
                            rigidbody.velocity = -(Physics.gravity * rigidbody.mass);
                        }
                    }
                    else
                    {
                        rigidbody.useGravity = false;
                    }
                }

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * m_firstCheckRadius));

            Gizmos.DrawWireSphere(transform.position, m_doubleCheckRadius);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(active)
            {
                if(m_checkInTrigger)
                {
                    Unit u;
                    if (CheckHitUnit(other, out u))
                    {
                        if (AddHit(u))
                        {
                            OnHitUnit(u);
                            OnHitAny();
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

            rigidbody.position = data.pos;

            rigidbody.velocity = data.dir * data.speed;

            rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;

            rigidbody.mass = (m_randomMass)
                ? (m_curMass = Random.Range(m_minMass, m_maxMass))
                : (m_curMass = m_minMass);

            transform.localScale = 
                (m_randomScale)
                    ? (m_uniformScale)
                        ? (Vector3.one * Random.Range(-m_minScale.x, m_maxScale.x))
                        : new Vector3(
                            Random.Range(-m_minScale.x, m_maxScale.x),
                            Random.Range(-m_minScale.y, m_maxScale.y),
                            Random.Range(-m_minScale.z, m_maxScale.z))
                    : (m_curScale);

            collider.isTrigger = true;

            transform.LookAt(rigidbody.position + rigidbody.velocity);

            m_onSpawn.Invoke();
        }

        public override void Kill()
        {
            m_onKill.Invoke();

            base.Kill();
        }

        private void CheckForHits()
        {
            m_ray.origin = rigidbody.position;
            m_ray.direction = rigidbody.velocity;

            if (Physics.Raycast(m_ray, out m_hit, m_firstCheckRadius, data.layerMask))
            {
                rigidbody.position = m_hit.point + (m_hit.normal * m_normalScale);

                Unit u;
                if (CheckHitUnit(m_hit.collider, out u))
                {
                    if (AddHit(u))
                    {
                        OnHitUnit(u);
                        OnHitAny();
                        Debug.Log("First Hit Unit");
                    }
                }
                else
                {
                    OnHitAny();
                    Debug.Log("First Hit Any");
                }
            }
            else
            {
                m_check = Physics.OverlapSphere(m_ray.origin, m_doubleCheckRadius, data.layerMask);
                foreach (Collider c in m_check)
                {
                    Unit u;
                    if (CheckHitUnit(c, out u))
                    {
                        if (AddHit(u))
                        {
                            OnHitUnit(u);
                            OnHitAny();
                            Debug.Log("Second Hit Unit");
                        }
                    }
                    else
                    {
                        OnHitAny();
                        Debug.Log("Second Hit Any");
                    }
                }
            }
        }

        private void OnHitAny()
        {
            if (m_destroyOnHit && !m_piercing)
            {
                Kill();
            }
            else
            {
                Stop();
            }
        }
        
        private void Stop()
        {
            if(active)
            {
                active = false;
                m_onStop.Invoke();
                collider.isTrigger = !m_solid;
                rigidbody.velocity = Vector3.zero;
                rigidbody.interpolation = RigidbodyInterpolation.None;
                data.lifeSpan = m_persistance;
                timer = 0f;
            }
        }
    }
}
