using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BeamWeapon : WeaponBase
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Beam Settings")]
        [SerializeField] BeamObject m_beamPrefab;
        [SerializeField] float      m_beamWidth    = 0.1f;
        [SerializeField] float      m_beamLifespan  = 0.5f;
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

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(shotPos.position, 0.1f);

            if (owner)
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
            switch (shotMode)
            {
            case Mode.SingleShot:
            {
                if(Input.GetButtonDown(axis))
                {
                    Shoot();
                }
            }
            break;
            case Mode.Continuous:
            {
                if(Input.GetButtonDown(axis)) { }
                if(Input.GetButton(axis)) { }
                if(Input.GetButtonUp(axis)) { }
            }
            break;
            }
        }

        public override void UpdateSecondary(string axis)
        {
            // ADS
        }

        protected override IEnumerator ShootCoroutine()
        {
            BeamObject obj;
            if(obj = SpawnLaser(m_beamPrefab))
            {
                obj.Spawn();
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
                combatData.direction = transform.forward;
                combatData.speed = 0f;
                combatData.lifeSpan = m_beamLifespan;

                obj.data = combatData;
                obj.posA = shotPos.position;
                obj.posB = lookPos + (shotPos.forward * m_beamPenetration);
                obj.width = m_beamWidth;

                return obj;
            }
            return null;
        }
    }

}