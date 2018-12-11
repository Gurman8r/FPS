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
        [SerializeField] BeamEntity m_beamPrefab;
        [SerializeField] float      m_beamWidth     = 0.1f;
        [SerializeField] float      m_beamRange     = 100f;
        [Range(0f, 2f)]
        [SerializeField] float      m_spreadFactor  = 1f;


        /* Properties
        * * * * * * * * * * * * * * * */
        public BeamEntity beamPrefab
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
        public override void HandleInputPrimary(ButtonState input)
        {   
            switch (useMode)
            {
            case UseMode.Single:
            {
                if(input.press) { Shoot(); }
            }
            break;
            case UseMode.Continuous:
            {
                if(input.hold) { Shoot(); }
            }
            break;
            }
        }

        public override void HandleInputSecondary(ButtonState input)
        {
            if(allowAiming)
                animator.SetBool("AimDownSights", !onCooldown && input.hold);
        }


        protected override IEnumerator ShootCoroutine()
        {
            BeamEntity obj;
            if(obj = SpawnLaser(beamPrefab))
            {
                obj.Spawn();

                if (audio.clip) audio.Play();

                animator.SetTrigger("Recoil");
            }

            yield return new WaitForSeconds(useDelay);
        }
        
        private BeamEntity SpawnLaser(BeamEntity prefab)
        {
            BeamEntity obj;
            if (obj = owner.CreateAndSpawnObject(prefab) as BeamEntity)
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
