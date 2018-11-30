using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class MeleeWeapon : WeaponBase
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Melee Settings")]
        [SerializeField] MeleeObject m_meleeObject;

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(string axis)
        {
            switch (shotMode)
            {
            case Mode.SingleShot:
            {
                if (Input.GetButtonDown(axis))
                {
                    Shoot();
                }
            }
            break;
            case Mode.Continuous:
            {
                if (Input.GetButtonDown(axis)) { }
                if (Input.GetButton(axis)) { }
                if (Input.GetButtonUp(axis)) { }
            }
            break;
            }
        }

        public override void UpdateSecondary(string axis)
        {
            // Block
        }

        protected override IEnumerator ShootCoroutine()
        {
            combatData.owner = owner;
            combatData.layerMask = layerMask;
            combatData.position = shotPos.position;
            combatData.direction = transform.forward;
            combatData.speed = 1f;
            combatData.lifeSpan = 0f;

            m_meleeObject.data = combatData;

            animator.SetTrigger("MeleeStart");

            yield return new WaitForSeconds(shotDelay);
        }

        private void OnAnimationEvent(string value)
        {
            switch(value)
            {
            case "MeleeStart": { m_meleeObject.SetActive(true); } break;
            case "MeleeStop": { m_meleeObject.SetActive(false); } break;
            }
        }
    }

}
