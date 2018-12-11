using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BallisticEntity : BaseEntity
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
        [SerializeField] float      m_checkSphereRadius     = 0.2f;
        [SerializeField] float      m_checkRaycastRadius    = 2.0f;

        [Header("Flags")]
        [SerializeField] bool       m_enableLog;
        [SerializeField] bool       m_useGravity = true;
        [SerializeField] bool       m_invertGravity;
        [SerializeField] bool       m_destroyOnHit;
        [SerializeField] bool       m_piercing;
        [SerializeField] bool       m_solid;
        [SerializeField] float      m_persistance;
        [SerializeField] float      m_normalScale;

        private Ray         m_ray = new Ray();
        private RaycastHit  m_hit;


        /* Core
        * * * * * * * * * * * * * * * */
        private void FixedUpdate()
        {
            if (Application.isPlaying)
            {
                if (active)
                {
                    CheckForHits();
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

        protected override void OnTriggerEnter(Collider c)
        {
            base.OnTriggerEnter(c);
        }

        protected override void OnTriggerStay(Collider c)
        {
            base.OnTriggerStay(c);
        }



        /* Functions
        * * * * * * * * * * * * * * * */
        public override void Spawn()
        {
            base.Spawn();

            transform.position = motion.origin;

            rigidbody.velocity = motion.forward * motion.speed;

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
        }

        private bool CheckForHits()
        {
            m_ray.origin = rigidbody.position;
            m_ray.direction = rigidbody.velocity;

            // First Check
            if (Physics.Raycast(m_ray, out m_hit, m_checkRaycastRadius, unitLayer))
            {
                Unit u;
                if (IsValidHit(m_hit.collider, out u) && RegisterHit(u))
                {
                    if (m_enableLog) Debug.Log("First Check");
                    rigidbody.position = m_hit.point + (m_hit.normal * m_normalScale);
                    OnHit(u);
                    OnHitAny(m_hit.collider);
                    return true;
                }
            }

            // Second Check
            if (Physics.Raycast(m_ray, out m_hit, m_checkRaycastRadius, solidLayer))
            {
                if (m_enableLog) Debug.Log("Second Check");
                rigidbody.position = m_hit.point + (m_hit.normal * m_normalScale);
                OnHitAny(m_hit.collider);
                return true;
            }

            // Third Check
            m_hitColliders = Physics.OverlapSphere(m_ray.origin, m_checkSphereRadius, unitLayer);
            foreach (Collider c in m_hitColliders)
            {
                Unit u;
                if (IsValidHit(m_hit.collider, out u) && RegisterHit(u))
                {
                    if (m_enableLog) Debug.Log("Third Check");
                    OnHit(u);
                    OnHitAny(c);
                    return true;
                }
            }

            // Fourth Check
            if (Physics.CheckSphere(m_ray.origin, m_checkSphereRadius, solidLayer))
            {
                if(m_enableLog) Debug.Log("Fourth Check");
                OnHitAny(null);
                return true;
            }

            return false;
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
                lifeSpan = m_persistance;
                timer = 0f;
                onStop.Invoke();
            }
        }
    }
}
