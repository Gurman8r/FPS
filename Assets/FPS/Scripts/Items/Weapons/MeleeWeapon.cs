using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class MeleeWeapon : WeaponBase
    {
        public enum ActionType
        {
            Idle = 0, StartAttack = 1, EndAttack = 2,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Melee Settings")]
        [SerializeField] MeleeObject    m_meleeObject;

        [Header("Melee Runtime")]
        [SerializeField] ActionType     m_state;
        [SerializeField] string         m_triggerName;
        [SerializeField] bool           m_canDamage;


        /* Properties
        * * * * * * * * * * * * * * * */
        public MeleeObject meleeObject
        {
            get { return m_meleeObject; }
            set { m_meleeObject = value; }
        }

        public ActionType state
        {
            get { return m_state; }
            private set { m_state = value; }
        }

        public string triggerName
        {
            get { return m_triggerName; }
            private set { m_triggerName = value; }
        }

        public bool canDamage
        {
            get { return m_canDamage; }
            private set { m_canDamage = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (Application.isPlaying)
            {
                if(canShoot) { /*Update Inspector*/ }

                if (m_meleeObject)
                {
                    data.owner = owner;
                    data.pos = firePos.position;
                    data.dir = transform.forward;
                    meleeObject.data = data;
                    meleeObject.SetActive(m_canDamage);
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(InputState input)
        {
            if (input.press)
            {
                triggerName = "MeleePrimary";
                Shoot();
            }
        }

        public override void UpdateSecondary(InputState input)
        {
            if (input.press)
            {
                triggerName = "MeleeSecondary";
                Shoot();
            }
        }

        protected override IEnumerator ShootCoroutine()
        {
            animator.SetTrigger(m_triggerName);

            if (audio.clip) audio.Play();

            yield return null;
        }

        private void SetState(ActionType value)
        {
            switch (state = value)
            {
            case ActionType.Idle:
            {
                fireTimer = fireDelay;
            }
            break;
            case ActionType.StartAttack:
            {
                canDamage = true;
            }
            break;
            case ActionType.EndAttack:
            {
                triggerName = "";
                canDamage = false;
            }
            break;
            }

        }
    }

}
