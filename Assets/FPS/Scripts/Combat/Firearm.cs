using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public class Firearm : Item
    {
        public enum Mode
        {
            SemiAuto = 0,
            FullAuto = 1,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Firearm Settings")]
        [SerializeField] LayerMask  m_groundLayer;
        [SerializeField] float      m_maxRange      = 100;
        [SerializeField] Transform  m_shotPos;              // Bullet spawn location
        [SerializeField] Mode       m_shotMode;             // How is input handled?
        [SerializeField] float      m_shotDelay     = 1f;   // Delay between shots
        [Space]
        [SerializeField] Bullet     m_bulletPrefab;         // Prefab to spawn
        [SerializeField] int        m_bulletCount   = 1;    // Bullets per shot
        [SerializeField] float      m_bulletDelay   = 0.1f; // Delay between bullets
        [SerializeField] float      m_bulletSpread  = 0.1f; // Bullet Spread/Bloom
        [SerializeField] float      m_bulletSpeed   = 10;   // Speed of bullet
        [SerializeField] float      m_bulletLifespan= 1;    // Bullet lifespan

        [Header("Runtime")]
        [SerializeField] BulletData m_bulletData;
        [SerializeField] Vector3    m_lookPos;
        [SerializeField] bool       m_canShoot;
        [SerializeField] float      m_shotTimer;

        /* Properties
        * * * * * * * * * * * * * * * */
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
        }

        public bool canShoot
        {
            get { return (m_canShoot = (m_shotTimer >= m_shotDelay)); }
        }

        public float shotTimer
        {
            get { return m_shotTimer; }
            private set { m_shotTimer = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if(Application.isPlaying)
            {
                m_shotTimer = m_shotDelay;
            }
        }

        protected override void Update()
        {
            base.Update();

            if(Application.isPlaying)
            {
                if (!canShoot)
                {
                    m_shotTimer += Time.deltaTime;
                }

                RaycastHit hit;
                Ray ray = new Ray(shotPos.position, transform.forward);
                if (Physics.Raycast(ray, out hit, m_maxRange, m_groundLayer))
                {
                    m_lookPos = hit.point;
                }
                else
                {
                    m_lookPos = shotPos.position + (transform.forward * m_maxRange);
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(shotPos.position, 0.1f);

            if(owner)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(shotPos.position, lookPos);
                Gizmos.DrawWireSphere(lookPos, 0.1f);
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(string axis)
        {
            bool button = false;

            switch (m_shotMode)
            {
            case Mode.SemiAuto:
                button = Input.GetButtonDown(axis);
                break;
            case Mode.FullAuto:
                button = Input.GetButton(axis);
                break;
            }

            if(button)
            {
                Shoot();
            }
        }

        public override void UpdateSecondary(string axis)
        {

        }


        protected virtual void Shoot()
        {
            if(canShoot)
            {
                StartCoroutine(ShootCoroutine());

                shotTimer = 0f;
            }
        }

        protected virtual IEnumerator ShootCoroutine()
        {
            for (int i = 0, imax = m_bulletCount; i < imax; i++)
            {
                Bullet b;
                if (b = SpawnBullet(m_bulletPrefab))
                {
                    b.Spawn();

                    if ((m_bulletDelay > 0f) && (i < (imax - 1)))
                    {
                        yield return new WaitForSeconds(m_bulletDelay);
                    }
                }
            }

            yield return null;
        }


        private Bullet SpawnBullet(Bullet prefab)
        {
            if(prefab)
            {
                Bullet b = null;
                if (b = Instantiate(prefab))
                {
                    b.gameObject.SetActive(true);
                    b.transform.SetParent(null, false);
                    b.data = (m_bulletData = new BulletData
                    {
                        owner       = owner,
                        layerMask   = m_groundLayer,
                        position    = shotPos.position,
                        direction   = transform.forward + GetBulletSpread(),
                        speed       = m_bulletSpeed,
                        lifeSpan    = m_bulletLifespan,
                    });
                    return b;
                }
            }
            return null;
        }

        private Vector3 GetBulletSpread()
        {
            if (m_bulletSpread != 0f)
            {
                return new Vector3(
                    UnityEngine.Random.Range(-m_bulletSpread, m_bulletSpread),
                    UnityEngine.Random.Range(-m_bulletSpread, m_bulletSpread),
                    UnityEngine.Random.Range(-m_bulletSpread, m_bulletSpread));
            }
            return Vector3.zero;
        }
    }

}
