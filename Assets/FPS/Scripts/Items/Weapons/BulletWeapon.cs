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


        /* Properties
        * * * * * * * * * * * * * * * */
        public BulletObject bulletPrefab
        {
            get { return m_bulletPrefab; }
            set { m_bulletPrefab = value; }
        }

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
        public override void UpdatePrimary(InputState input)
        {
            bool button = false;

            switch (fireMode)
            {
            case FireMode.SingleShot:
                button = input.press;
                break;
            case FireMode.Continuous:
                button = input.hold;
                break;
            }

            if(button)
            {
                Shoot();
            }
        }

        public override void UpdateSecondary(InputState input)
        {
            // ADS
            animator.SetBool("AimDownSights", !isReloading && input.hold);
        }

        protected override IEnumerator ShootCoroutine()
        {
            for (int i = 0, imax = bulletCount; i < imax; i++)
            {
                BulletObject obj;
                if (obj = SpawnBullet(bulletPrefab))
                {
                    obj.Spawn();

                    obj.rigidbody.velocity += GetBulletSpread();

                    if (audio.clip) audio.Play();
                    
                    animator.SetTrigger("Recoil");

                    if (bulletDelay > 0f) 
                    {
                        if((i < (imax - 1)))
                        {
                            yield return new WaitForSeconds(bulletDelay);
                        }
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

                data.owner  = owner;
                data.pos    = firePos.position;
                data.dir    = firePos.forward;
                data.dest   = lookingAt;

                obj.data = data;
                return obj;
            }
            return null;
        }

        private Vector3 GetBulletSpread()
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
