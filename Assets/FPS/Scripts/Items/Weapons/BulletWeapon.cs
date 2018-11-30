using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BulletWeapon : WeaponBase
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Bullet Settings")]
        [SerializeField] BulletObject   m_bulletPrefab;         // Prefab to spawn
        [SerializeField] int            m_bulletCount   = 1;    // Bullets per shot
        [SerializeField] float          m_bulletDelay   = 0.1f; // Delay between bullets
        [SerializeField] float          m_bulletSpread  = 0.1f; // Bullet Spread/Bloom
        [SerializeField] float          m_bulletSpeed   = 10;   // Speed of bullet
        [SerializeField] float          m_bulletLifespan= 1;    // Bullet lifespan


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if(Application.isPlaying)
            {
                shotTimer = shotDelay;
            }
        }

        protected override void Update()
        {
            base.Update();
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

            switch (shotMode)
            {
            case Mode.SingleShot:
                button = Input.GetButtonDown(axis);
                break;
            case Mode.Continuous:
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
            // ADS
        }

        protected override IEnumerator ShootCoroutine()
        {
            for (int i = 0, imax = m_bulletCount; i < imax; i++)
            {
                BulletObject obj;
                if (obj = SpawnBullet(m_bulletPrefab))
                {
                    obj.Spawn();

                    if ((m_bulletDelay > 0f) && (i < (imax - 1)))
                    {
                        yield return new WaitForSeconds(m_bulletDelay);
                    }
                }
            }

            yield return null;
        }


        private BulletObject SpawnBullet(BulletObject prefab)
        {
            BulletObject obj;
            if (prefab && (obj = Instantiate(prefab)))
            {
                obj.gameObject.SetActive(true);
                obj.transform.SetParent(null, false);

                combatData.owner        = owner;
                combatData.layerMask    = layerMask;
                combatData.position     = shotPos.position;
                combatData.direction    = transform.forward + GetBulletSpread();
                combatData.speed        = m_bulletSpeed;
                combatData.lifeSpan     = m_bulletLifespan;

                obj.data = combatData;

                return obj;
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
