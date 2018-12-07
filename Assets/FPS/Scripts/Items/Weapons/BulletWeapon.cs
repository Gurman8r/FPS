using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BulletWeapon : GunBase
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Bullet Settings")]
        [SerializeField] BulletObject   m_bulletPrefab;


        /* Properties
        * * * * * * * * * * * * * * * */
        public BulletObject bulletPrefab
        {
            get { return m_bulletPrefab; }
            set { m_bulletPrefab = value; }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(InputState input)
        {
            //bulletSpread = m_hipSpread;

            switch (useMode)
            {
            case UseMode.Single:
            if(input.press)
                Shoot();
            break;
            case UseMode.Continuous:
            if(input.hold)
                Shoot();
            break;
            }
        }

        public override void UpdateSecondary(InputState input)
        {
            if(allowAiming)
                animator.SetBool("AimDownSights", !onCooldown && input.hold);
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

        

        
    }

}
