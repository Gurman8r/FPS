using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class GunBase : WeaponBase
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Gun Settings")]
        [SerializeField] int    m_bulletCount   = 1;
        [SerializeField] float  m_bulletDelay   = 0.1f;
        [Space]
        [SerializeField] bool   m_allowAds      = true;
        [SerializeField] float  m_hipSpread     = 0.5f;
        [SerializeField] float  m_adsSpread     = 0.1f;

        [Header("Gun Runtime")]
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

        public bool allowAds
        {
            get { return m_allowAds; }
        }

        public float bulletSpread
        {
            get { return m_bulletSpread; }
            set { m_bulletSpread = value; }
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
            if(allowAds)
            {
                bulletSpread = (value != 0) ? m_adsSpread : m_hipSpread;
            }
            else
            {
                bulletSpread = m_hipSpread;
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
