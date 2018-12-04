using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class WeaponBase : Item
    {
        public enum FireMode
        {
            SingleShot = 0,
            Continuous = 1,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Weapon Settings")]
        [SerializeField] Transform  m_firePos;
        [SerializeField] FireMode   m_fireMode      = FireMode.SingleShot;
        [SerializeField] float      m_fireDelay     = 1f;
        [SerializeField] float      m_minRange      = 0f;
        [SerializeField] float      m_maxRange      = 100;
        [SerializeField] ObjectData m_objectData;

        [Header("Weapon Runtime")]
        [SerializeField] Vector3    m_lookingAt;
        [SerializeField] bool       m_canShoot;
        [SerializeField] float      m_shotTimer;

        /* Properties
        * * * * * * * * * * * * * * * */
        public Transform firePos
        {
            get { return m_firePos; }
            set { m_firePos = value; }
        }

        public FireMode fireMode
        {
            get { return m_fireMode; }
            set { m_fireMode = value; }
        }

        public float fireDelay
        {
            get { return m_fireDelay; }
            set { m_fireDelay = value; }
        }

        public float minRange
        {
            get { return m_minRange; }
            set { m_minRange = value; }
        }

        public float maxRange
        {
            get { return m_maxRange; }
            set { m_maxRange = value; }
        }


        public ObjectData data
        {
            get { return m_objectData; }
            set { m_objectData = value; }
        }

        public Vector3 lookingAt
        {
            get { return m_lookingAt; }
            protected set { m_lookingAt = value; }
        }

        public bool canShoot
        {
            get { return (m_canShoot = (shotTimer >= fireDelay)); }
        }

        public float shotTimer
        {
            get { return m_shotTimer; }
            protected set { m_shotTimer = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            if(Application.isPlaying)
            {
            }
        }

        protected override void Update()
        {
            if (Application.isPlaying)
            {
                if(owner)
                {
                    lookingAt = owner.vision.lookingAt;

                    if (minRange > 0f)
                    {
                        float distA = Vector3.Distance(
                            firePos.position,
                            owner.vision.lookingAt);

                        float distB = Vector3.Distance(
                            firePos.position,
                            firePos.position + (transform.forward * minRange));

                        if (distA < distB)
                        {
                            lookingAt = firePos.position + (transform.forward * minRange);
                        }
                    }

                    firePos.LookAt(lookingAt);
                }
                else
                {
                    lookingAt = firePos.position + (transform.forward * maxRange);
                }
            }
        }

        new protected virtual void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if(firePos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(firePos.position, 0.1f);

                if(m_minRange > 0f)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawRay(firePos.position, transform.forward * minRange);
                    Gizmos.DrawWireSphere(firePos.position + (transform.forward * minRange), 0.05f);

                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(firePos.position, firePos.forward * minRange);
                    Gizmos.DrawWireSphere(firePos.position + (firePos.forward * minRange), 0.05f);
                }

                if (owner)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(firePos.position, lookingAt);
                    Gizmos.DrawWireSphere(lookingAt, 0.1f);
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public abstract override void UpdatePrimary(bool press, bool hold, bool release);

        public abstract override void UpdateSecondary(bool press, bool hold, bool release);


        protected virtual void Shoot()
        {
            if(canShoot)
            {
                StartCoroutine(ShootCoroutine());

                StartCooldown();
            }
        }

        protected abstract IEnumerator ShootCoroutine();


        public void StartCooldown()
        {
            shotTimer = 0f;
        }

        public void ResetCooldown()
        {
            shotTimer = fireDelay;
        }

        protected void UpdateCooldown()
        {
            if (!canShoot)
            {
                shotTimer += Time.deltaTime;
            }
        }
    }

}
