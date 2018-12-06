using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public class BulletObject : CombatObject
    {

        /* Variables
        * * * * * * * * * * * * * * * */
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
        [SerializeField] Collider[] m_hitColliders;
        [SerializeField] float      m_checkSphereRadius     = 0.5f;
        [SerializeField] float      m_checkRaycastRadius    = 0.5f;
        [SerializeField] bool       m_checkUpdate           = true;
        [SerializeField] bool       m_checkFixedUpdate      = true;
        [SerializeField] bool       m_checkOnTriggerEnter   = true;

        [Header("Flags")]
        [SerializeField] bool       m_enableLog;
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
                    if(m_checkUpdate)
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
                    if(m_checkFixedUpdate)
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

        private void OnTriggerEnter(Collider other)
        {
            if(active)
            {
                if(m_checkOnTriggerEnter)
                {
                    Unit u;
                    if ((u = CheckUnitCollision(other)))
                    {
                        Debug.Log("Trigger Check");
                        OnHitUnit(u);
                        OnHitAny(other);
                        return;
                    }
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void Spawn()
        {
            base.Spawn();

            transform.position = data.pos;

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

            // First Check
            if (Physics.Raycast(m_ray, out m_hit, m_checkRaycastRadius, data.unitLayer))
            {
                Unit u;
                if ((u = CheckUnitCollision(m_hit.collider)))
                {
                    if (m_enableLog) Debug.Log("First Check");
                    rigidbody.position = m_hit.point + (m_hit.normal * m_normalScale);
                    OnHitUnit(u);
                    OnHitAny(m_hit.collider);
                    return;
                }
            }

            // Second Check
            if (Physics.Raycast(m_ray, out m_hit, m_checkRaycastRadius, data.solidLayer))
            {
                if (m_enableLog) Debug.Log("Second Check");
                rigidbody.position = m_hit.point + (m_hit.normal * m_normalScale);
                OnHitAny(m_hit.collider);
                return;
            }

            // Third Check
            m_hitColliders = Physics.OverlapSphere(m_ray.origin, m_checkSphereRadius, data.unitLayer);
            foreach (Collider c in m_hitColliders)
            {
                Unit u;
                if ((u = CheckUnitCollision(c)))
                {
                    if (m_enableLog) Debug.Log("Third Check");
                    OnHitUnit(u);
                    OnHitAny(c);
                    return;
                }
            }

            // Fourth Check
            if (Physics.CheckSphere(m_ray.origin, m_checkSphereRadius, data.solidLayer))
            {
                if(m_enableLog) Debug.Log("Fourth Check");
                OnHitAny(null);
                return;
            }
        }

        private Unit CheckUnitCollision(Collider c)
        {
            Unit u;
            if (CheckHitUnit(m_hit.collider, out u))
            {
                if (AddHit(u))
                {
                    return u;
                }
            }
            return null;
        }

        private void OnHitAny(Collider c)
        {
            if (m_destroyOnHit && !m_piercing)
            {
                Kill();
            }
            else if (active)
            {
                active = false;
                collider.isTrigger = !m_solid;
                rigidbody.velocity = Vector3.zero;
                rigidbody.interpolation = RigidbodyInterpolation.None;
                maxLifespan = m_persistance;
                timer = 0f;
                m_onStop.Invoke();
            }
        }
    }
}
