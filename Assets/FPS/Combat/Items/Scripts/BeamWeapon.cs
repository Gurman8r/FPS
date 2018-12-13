using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BeamWeapon : BaseRangedWeapon
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Beam Weapon Settings")]
        [SerializeField] Beam m_beamPrefab;
        [SerializeField] float      m_beamWidth     = 0.1f;
        [SerializeField] float      m_beamRange     = 100f;
        [Range(0f, 2f)]
        [SerializeField] float      m_spreadFactor  = 1f;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Beam beamPrefab
        {
            get { return m_beamPrefab; }
            set { m_beamPrefab = value; }
        }

        public float beamWidth
        {
            get { return m_beamWidth; }
            set { m_beamWidth = value; }
        }
        

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void HandleInput(ButtonState lhs, ButtonState rhs)
        {   
            switch (useMode)
            {
            case UseMode.Single:
            {
                if(lhs.press) { Shoot(); }
            }
            break;
            case UseMode.Continuous:
            {
                if(lhs.hold) { Shoot(); }
            }
            break;
            }
        
            if(allowAiming)
            {
                animator.SetBool("AimDownSights", !onCooldown && rhs.hold);
            }
        }


        protected override IEnumerator ShootCoroutine()
        {
            Beam obj;
            if(obj = SpawnLaser(beamPrefab))
            {
                obj.Spawn();

                if (audio.clip) audio.Play();

                animator.SetTrigger("Recoil");
            }

            yield return new WaitForSeconds(useDelay);
        }
        
        private Beam SpawnLaser(Beam prefab)
        {
            Beam obj;
            if (obj = owner.CreateAndSpawnObject(prefab) as Beam)
            {
                obj.owner = owner;
                obj.damage = damage;
                obj.motion.origin = fireRoot.position;
                obj.motion.forward = (owner.vision.lookingAt - obj.motion.origin).normalized;
                obj.motion.target = 
                    (obj.motion.origin) + 
                    (obj.motion.forward * m_beamRange) + 
                    (GetBulletSpread() * m_spreadFactor);

                if (beamWidth > 0f) { obj.width = beamWidth; }

                return obj;
            }
            return null;
        }
    }

}
