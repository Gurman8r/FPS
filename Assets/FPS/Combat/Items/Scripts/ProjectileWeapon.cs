using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class ProjectileWeapon : BaseRangedWeapon
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Projectile Weapon Settings")]
        [SerializeField] Projectile m_bulletPrefab;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Projectile bulletPrefab
        {
            get { return m_bulletPrefab; }
            set { m_bulletPrefab = value; }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void HandleInput(ButtonState lhs, ButtonState rhs)
        {
            switch (useMode)
            {
            case UseMode.Single:
            {
                if (lhs.press) { Shoot(); }
            }
            break;
            case UseMode.Continuous:
            {
                if (lhs.hold) { Shoot(); }
            }
            break;
            }

            if (allowAiming)
            {
                animator.SetBool("AimDownSights", !onCooldown && rhs.hold);
            }
        }

        protected override IEnumerator ShootCoroutine()
        {
            for (int i = 0, imax = bulletCount; i < imax; i++)
            {
                Projectile obj;
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
        
        private Projectile SpawnBullet(Projectile prefab)
        {
            Projectile obj;
            if(obj = owner.CreateAndSpawnObject(prefab) as Projectile)
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
