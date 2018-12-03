using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class WeaponBase : Item
    {
        public enum Mode
        {
            SingleShot = 0,
            Continuous = 1,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Weapon Settings")]
        [SerializeField] LayerMask  m_groundLayer;
        [SerializeField] float      m_maxRange      = 100;
        [SerializeField] Transform  m_shotPos;              // Bullet spawn location
        [SerializeField] Mode       m_shotMode;             // How is input handled?
        [SerializeField] float      m_shotDelay     = 1f;   // Delay between shots

        [Header("Weapon Runtime")]
        [SerializeField] CombatData m_combatData;
        [SerializeField] Vector3    m_lookingAt;
        [SerializeField] bool       m_canShoot;
        [SerializeField] float      m_shotTimer;

        /* Properties
        * * * * * * * * * * * * * * * */
        public CombatData combatData
        {
            get { return m_combatData; }
            set { m_combatData = value; }
        }

        public LayerMask layerMask
        {
            get { return m_groundLayer; }
        }

        public float maxRange
        {
            get { return m_maxRange; }
        }

        public Transform shotPos
        {
            get { return m_shotPos; }
        }

        public Mode shotMode
        {
            get { return m_shotMode; }
        }

        public float shotDelay
        {
            get { return m_shotDelay; }
        }

        public Vector3 lookingAt
        {
            get { return m_lookingAt; }
            protected set { m_lookingAt = value; }
        }

        public bool canShoot
        {
            get { return (m_canShoot = (shotTimer >= shotDelay)); }
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

        }

        protected override void Update()
        {
            if (Application.isPlaying)
            {
                if (!canShoot)
                {
                    shotTimer += Time.deltaTime;
                }

                if(owner)
                {
                    lookingAt = owner.vision.lookingAt;
                }
                else
                {
                    lookingAt = shotPos.position + (transform.forward * maxRange);
                }

                shotPos.LookAt(lookingAt);
            }
        }

        new protected virtual void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if(shotPos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(shotPos.position, 0.1f);

                if (owner)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(shotPos.position, lookingAt);
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
            shotTimer = shotDelay;
        }
    }

}
