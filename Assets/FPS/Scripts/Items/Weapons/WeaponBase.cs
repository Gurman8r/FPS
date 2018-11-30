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
        [SerializeField] Vector3    m_lookPos;
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

        public Vector3 lookPos
        {
            get { return m_lookPos; }
            protected set { m_lookPos = value; }
        }

        public bool canShoot
        {
            get { return (m_canShoot = (m_shotTimer >= m_shotDelay)); }
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

                RaycastHit hit;
                Ray ray = new Ray(shotPos.position, transform.forward);
                if (Physics.Raycast(ray, out hit, maxRange, layerMask))
                {
                    lookPos = hit.point;
                }
                else
                {
                    lookPos = shotPos.position + (transform.forward * maxRange);
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public abstract override void UpdatePrimary(string axis);

        public abstract override void UpdateSecondary(string axis);


        protected virtual void Shoot()
        {
            if(canShoot)
            {
                StartCoroutine(ShootCoroutine());

                shotTimer = 0f;
            }
        }

        protected abstract IEnumerator ShootCoroutine();
    }

}
