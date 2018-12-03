using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BeamWeapon : WeaponBase
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Beam Weapon Settings")]
        [SerializeField] BeamObject m_beamPrefab;
        [SerializeField] float      m_beamWidth    = 0.1f;
        [SerializeField] float      m_beamPenetration = 0f;


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if (Application.isPlaying)
            {
                shotTimer = shotDelay;
            }
        }

        protected override void Update()
        {
            base.Update();
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(bool press, bool hold, bool release)
        {   
            switch (shotMode)
            {
            case Mode.SingleShot:
            {
                if(press) { Shoot(); }
            }
            break;
            case Mode.Continuous:
            {
                if(hold) { Shoot(); }
            }
            break;
            }
        }

        public override void UpdateSecondary(bool press, bool hold, bool release)
        {
            animator.SetBool("AimDownSights", hold);
        }


        protected override IEnumerator ShootCoroutine()
        {
            BeamObject obj;
            if(obj = SpawnLaser(m_beamPrefab))
            {
                obj.Spawn();

                if (audio.clip) audio.Play();

                animator.SetTrigger("Recoil");
            }

            yield return new WaitForSeconds(shotDelay);
        }
        
        private BeamObject SpawnLaser(BeamObject prefab)
        {
            BeamObject obj;
            if (prefab && (obj = Instantiate(m_beamPrefab)))
            {
                obj.gameObject.SetActive(true);
                obj.transform.SetParent(null, true);

                combatData.owner = owner;
                combatData.layerMask = layerMask;
                combatData.position = shotPos.position;
                combatData.direction = shotPos.forward;

                obj.data = combatData;
                obj.sourcePos = shotPos.position;
                obj.targetPos = lookingAt + (shotPos.forward * m_beamPenetration);
                obj.width = m_beamWidth;

                return obj;
            }
            return null;
        }
    }

}
