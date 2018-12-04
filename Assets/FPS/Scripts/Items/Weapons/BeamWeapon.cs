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
        [SerializeField] float      m_beamPen = 0f;


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

        public float beamPen
        {
            get { return m_beamPen; }
            set { m_beamPen = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if (Application.isPlaying)
            {
                shotTimer = fireDelay;
            }
        }

        protected override void Update()
        {
            base.Update();

            UpdateCooldown();
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(bool press, bool hold, bool release)
        {   
            switch (fireMode)
            {
            case FireMode.SingleShot:
            {
                if(press) { Shoot(); }
            }
            break;
            case FireMode.Continuous:
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
            if(obj = SpawnLaser(beamPrefab))
            {
                obj.Spawn();

                if (audio.clip) audio.Play();

                animator.SetTrigger("Recoil");
            }

            yield return new WaitForSeconds(fireDelay);
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
                data.dir = firePos.forward;
                data.dest = lookingAt + (firePos.forward * beamPen);

                if (beamWidth > 0f) { obj.width = beamWidth; }

                obj.data = data;
                return obj;
            }
            return null;
        }
    }

}
