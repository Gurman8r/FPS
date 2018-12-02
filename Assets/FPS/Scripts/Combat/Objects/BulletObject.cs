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
        [SerializeField] bool   m_destroyOnHit;
        [SerializeField] bool   m_penetration;
        [SerializeField] float  m_stopRadius = 0.5f;

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


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Update()
        {
            base.Update();
        }

        private void FixedUpdate()
        {
            if (Application.isPlaying)
            {
                if (m_stopped)
                    return;

                if (!m_penetration)
                {
                    if (Physics.CheckSphere(rigidbody.position, m_stopRadius, data.layerMask))
                    {
                        MakeSafe();
                    }
                }

            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(!m_stopped)
            {
                Unit other;
                if (CheckHit(collider, out other))
                {
                    if (AddHit(other))
                    {
                        OnHit(other);

                        if(m_destroyOnHit)
                        {
                            Destroy(gameObject);
                        }
                        else if (!m_penetration)
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

            m_stopped = false;

            collider.isTrigger = true;

            rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
        }

        public void MakeSafe()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.useGravity = true;
            collider.isTrigger = false;
            m_stopped = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
        }
    }
}
