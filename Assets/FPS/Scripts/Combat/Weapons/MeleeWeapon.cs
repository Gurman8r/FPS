using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class MeleeWeapon : WeaponBase
    {
        public enum ActionType
        {
            Idle = 0,
            StartAttack = 1, EndAttack = 2,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Melee Weapon Settings")]

        [Header("Melee Weapon Runtime")]
        [SerializeField] MeleeObject    m_meleeObject;
        [SerializeField] ActionType     m_state;
        [SerializeField] string         m_triggerName;
        [SerializeField] bool           m_canDamage;


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            ResetCooldown();
        }

        protected override void Update()
        {
            if (Application.isPlaying)
            {
                if(canShoot) { /*Update Inspector*/ }

                if (m_meleeObject)
                {
                    combatData.owner = owner;
                    combatData.layerMask = layerMask;
                    combatData.position = shotPos.position;
                    combatData.direction = transform.forward;
                    m_meleeObject.data = combatData;
                    m_meleeObject.SetActive(m_canDamage);
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(string axis)
        {
            if (Input.GetButtonDown(axis))
            {
                m_triggerName = "MeleePrimary";
                Shoot();
            }
        }

        public override void UpdateSecondary(string axis)
        {
            if (Input.GetButtonDown(axis))
            {
                m_triggerName = "MeleeSecondary";
                Shoot();
            }
        }

        protected override void Shoot()
        {
            if (canShoot)
            {
                StartCoroutine(ShootCoroutine());

                StartCooldown();
            }
        }

        protected override IEnumerator ShootCoroutine()
        {
            animator.SetTrigger(m_triggerName);
            if (audio.clip)
                audio.Play();
            yield return null;
        }

        private void SetState(ActionType value)
        {
            switch (m_state = value)
            {
            case ActionType.Idle:
            {
                ResetCooldown(); //canShoot = true
            }
            break;
            case ActionType.StartAttack:
            {
                m_canDamage = true;
            }
            break;
            case ActionType.EndAttack:
            {
                m_triggerName = "";
                m_canDamage = false;
            }
            break;
            }

        }
    }

}
