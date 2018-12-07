using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BeamWeapon : GunBase
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Beam Settings")]
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
        public override void UpdatePrimary(InputState input)
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

        public override void UpdateSecondary(InputState input)
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
            if (prefab && (obj = Instantiate(prefab)))
            {
                obj.gameObject.SetActive(true);
                obj.transform.SetParent(null, true);

                data.owner = owner;
                data.pos = firePos.position;
                data.dir = (lookingAt - data.pos).normalized;
                data.dest = 
                    (data.pos) + 
                    (data.dir * m_beamRange) + 
                    (GetBulletSpread() * m_spreadFactor);

                if (beamWidth > 0f) { obj.width = beamWidth; }

                obj.data = data;
                return obj;
            }
            return null;
        }
    }

}
