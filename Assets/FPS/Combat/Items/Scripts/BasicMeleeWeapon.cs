using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BasicMeleeWeapon : BaseMeleeWeapon
    {
        public enum ActionType
        {
            Idle = 0, StartAttack = 1, EndAttack = 2,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Melee Weapon Settings")]
        [SerializeField] MeleeEntity    m_meleeObject;

        private ActionType     m_state;
        private string         m_triggerName;
        private bool           m_canDamage;


        /* Properties
        * * * * * * * * * * * * * * * */
        public MeleeEntity meleeObject
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
                if(canUse) { /*Update Inspector*/ }

                if (meleeObject)
                {
                    meleeObject.owner = owner;
                    meleeObject.damage = damage;
                    meleeObject.SetActive(m_canDamage);
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void HandleInputPrimary(ButtonState input)
        {
            if (input.press)
            {
                triggerName = "MeleePrimary";
                Shoot();
            }
        }

        public override void HandleInputSecondary(ButtonState input)
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
                useTimer = useDelay;
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
