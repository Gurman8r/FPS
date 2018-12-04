using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public abstract class WeaponBase : Item
    {
        public const float FireDelayThreshold = 0.09f;

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
        [SerializeField] int        m_maxAmmo       = 0;
        [SerializeField] float      m_reloadDelay   = 2.5f;
        [SerializeField] bool       m_autoReload    = true;
        [SerializeField] bool       m_reticleOn     = false;
        [Space]
        [SerializeField] ObjectData m_objectData;

        [Header("Weapon Runtime")]
        [SerializeField] Vector3    m_lookingAt;
        [SerializeField] bool       m_canShoot;
        [SerializeField] float      m_fireTimer;
        [SerializeField] int        m_curAmmo;
        [SerializeField] bool       m_isReloading;
        [SerializeField] float      m_reloadTimer;

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

        public bool alwaysOn
        {
            get { return m_reticleOn; }
        }

        public int maxAmmo
        {
            get { return m_maxAmmo; }
        }

        public float reloadDelay
        {
            get { return m_reloadDelay; }
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
            get
            {
                return m_canShoot =
                    (!isReloading) && 
                    (fireTimer >= fireDelay) &&
                    (hasAmmo);
            }
        }

        public float fireTimer
        {
            get { return m_fireTimer; }
            protected set { m_fireTimer = value; }
        }

        public int curAmmo
        {
            get { return m_curAmmo; }
            private set { m_maxAmmo = value; }
        }

        public bool isReloading
        {
            get { return m_isReloading; }
            protected set { m_isReloading = value; }
        }

        public float reloadTimer
        {
            get { return m_reloadTimer; }
            protected set { m_reloadTimer = value; }
        }
        

        public float shotDelta
        {
            get
            {
                return (isReloading)
                    ? 0f
                    : (alwaysOn)
                        ? (1f)
                        : (fireDelay > 0f)
                            ? (!canShoot)
                                ? (fireTimer / fireDelay)
                                : (1f)
                            : (1f);
            }
        }
        
        public bool hasAmmo
        {
            get
            {
                return (maxAmmo > 0)
                    ? (curAmmo > 0)
                    : (true);
            }
        }

        public float ammoDelta
        {
            get
            {
                return (maxAmmo > 0)
                    ? ((float)(curAmmo) / (float)(maxAmmo))
                    : (1f);
            }
        }

        public float reloadDelta
        {
            get
            {
                return (reloadDelay > 0f)
                    ? (reloadTimer / reloadDelay)
                    : (1f);
            }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            if(Application.isPlaying)
            {
                fireTimer = fireDelay;

                SetAmmo(maxAmmo);

                onDrop.AddListener(()   => { CancelReload(); });
                onEquip.AddListener(()  => { CancelReload(); });
                onStore.AddListener(()  => { CancelReload(); });
            }
        }

        protected override void Update()
        {
            if (Application.isPlaying)
            {
                if (owner)
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

                    if (m_autoReload && !hasAmmo)
                    {
                        Reload();
                    }
                }

                animator.SetBool("Reloading", isReloading);
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if(firePos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(firePos.position, 0.1f);

                if(minRange > 0f)
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
        public abstract override void UpdatePrimary(InputState input);

        public abstract override void UpdateSecondary(InputState input);


        protected void Shoot()
        {
            if(canShoot)
            {
                fireTimer = 0f;

                StartCoroutine(ShootCoroutine());

                ConsumeAmmo();
            }
            else if(!hasAmmo)
            {
                Reload();
            }
        }

        protected abstract IEnumerator ShootCoroutine();
        

        protected virtual void UpdateCooldown()
        {
            if (!canShoot && (fireDelay > 0f))
            {
                fireTimer += Time.deltaTime;
            }
        }


        public void Reload()
        {
            if(!isReloading && (curAmmo != maxAmmo))
            {
                StartCoroutine(ReloadCoroutine());
            }
        }

        public void CancelReload()
        {
            StopAllCoroutines();
            isReloading = false;
        }

        private IEnumerator ReloadCoroutine()
        {
            isReloading = true;

            for(reloadTimer = (reloadDelay * ((float)curAmmo / (float)maxAmmo)); 
                reloadTimer < reloadDelay; 
                reloadTimer += Time.deltaTime)
            {
                SetAmmo((int)(maxAmmo * reloadDelta));

                yield return null;
            }

            SetAmmo(maxAmmo);

            isReloading = false;
        }


        protected void SetAmmo(int value)
        {
            m_curAmmo = Mathf.Clamp(value, 0, m_maxAmmo);
        }

        protected void ConsumeAmmo()
        {
            SetAmmo(curAmmo - 1);
        }
    }

}
