using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class BaseRangedWeapon : BaseWeapon
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Base Ranged Weapon Settings")]
        [SerializeField] int    m_bulletCount   = 1;
        [SerializeField] float  m_bulletDelay   = 0.1f;
        [SerializeField] bool   m_allowAiming   = true;
        [SerializeField] float  m_spreadHip     = 0.5f;
        [SerializeField] float  m_spreadAim     = 0.1f;
        [Range(FirstPersonCamera.MinZoom, FirstPersonCamera.MaxZoom)]
        [SerializeField] float  m_zoomAiming    = 1f;

        [Header("Base Ranged Weapon Settings")]
        [SerializeField] bool   m_isAiming      = false;
        [SerializeField] float  m_bulletSpread  = 0.1f;


        /* Properties
        * * * * * * * * * * * * * * * */
        public int bulletCount
        {
            get { return m_bulletCount; }
            set { m_bulletCount = value; }
        }

        public float bulletDelay
        {
            get { return m_bulletDelay; }
            set { m_bulletDelay = value; }
        }

        public bool allowAiming
        {
            get { return m_allowAiming; }
        }

        public float bulletSpread
        {
            get { return m_bulletSpread; }
            private set { m_bulletSpread = value; }
        }

        public bool isAiming
        {
            get { return m_isAiming; }
            protected set { m_isAiming = value; }
        }

        public float zoomLevel
        {
            get
            {
                return 
                    isAiming
                        ? allowAiming
                            ? m_zoomAiming
                            : 1f
                        : 1f;
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

            if(Application.isPlaying)
            {
                UpdateCooldown();
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        protected void SetAimDownSights(int value)
        {
            if(isAiming = (allowAiming && (value == 1 ? true : false)))
            {
                bulletSpread = (value != 0) ? m_spreadAim : m_spreadHip;
            }
            else
            {
                bulletSpread = m_spreadHip;
            }
        }

        protected Vector3 GetBulletSpread()
        {
            if (bulletSpread != 0f)
            {
                return new Vector3(
                    UnityEngine.Random.Range(-bulletSpread, bulletSpread),
                    UnityEngine.Random.Range(-bulletSpread, bulletSpread),
                    UnityEngine.Random.Range(-bulletSpread, bulletSpread));
            }
            return Vector3.zero;
        }
    }

}
