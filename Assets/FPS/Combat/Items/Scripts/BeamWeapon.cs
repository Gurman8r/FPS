using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BeamWeapon : RangedWeapon
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Beam Weapon Settings")]
        [SerializeField] BeamObject m_beamPrefab;
        [SerializeField] float      m_beamWidth     = 0.1f;
        [SerializeField] float      m_beamRange     = 100f;
        [Range(0f, 2f)]
        [SerializeField] float      m_spreadFactor  = 1f;


        /* Properties
        * * * * * * * * * * * * * * * */
        public BeamObject beamPrefab
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
        public override void HandleInputPrimary(ItemInput input)
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

        public override void HandleInputSecondary(ItemInput input)
        {
            if(allowAiming)
                animator.SetBool("AimDownSights", !onCooldown && input.hold);
        }


        protected override IEnumerator ShootCoroutine()
        {
            BeamObject obj;
            if(obj = SpawnLaser(beamPrefab))
            {
                obj.Spawn();

                if (audio.clip) audio.Play();

                animator.SetTrigger("Recoil");
            }

            yield return new WaitForSeconds(useDelay);
        }
        
        private BeamObject SpawnLaser(BeamObject prefab)
        {
            BeamObject obj;
            if (obj = owner.CreateAndSpawnObject(prefab) as BeamObject)
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
