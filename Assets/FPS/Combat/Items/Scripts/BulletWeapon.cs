using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BulletWeapon : RangedWeapon
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Bullet Weapon Settings")]
        [SerializeField] BulletObject m_bulletPrefab;


        /* Properties
        * * * * * * * * * * * * * * * */
        public BulletObject bulletPrefab
        {
            get { return m_bulletPrefab; }
            set { m_bulletPrefab = value; }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void HandleInputPrimary(ItemInput input)
        {
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

        public override void HandleInputSecondary(ItemInput input)
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
            if(obj = owner.CreateAndSpawnObject(prefab) as BulletObject)
            {
                obj.owner  = owner;
                obj.damage = damage;
                obj.motion.origin = fireRoot.position;
                obj.motion.forward = fireRoot.forward;
                obj.motion.target = owner.vision.lookingAt;

                return obj;
            }
            return null;
        }
    }

}
