using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Consumable : Item
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Consumable Settings")]
        [SerializeField] Healing    m_healing;
        [SerializeField] bool       m_removeOnEmpty = true;
        [SerializeField] float      m_destroyDelay   = 1f;


        /* Properties
        * * * * * * * * * * * * * * * */

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if(Application.isPlaying)
            {
                if(owner)
                {
                    if (!resourceAvailable && m_removeOnEmpty)
                    {
                        interactable = false;
                        owner.inventory.Drop(owner.combat.right);
                        Destroy(gameObject, m_destroyDelay);
                    }
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void HandleInput(ButtonState lhs, ButtonState rhs)
        {
            switch(useMode)
            {
            case UseMode.Single:
            {
                if(lhs.press && !owner.health.isFull)
                {
                    DoHealing();
                    ConsumeResource();
                }
            }
            break;
            case UseMode.Continuous:
            {
                if (lhs.hold && !owner.health.isFull)
                {
                    DoHealing();
                    ConsumeResource();
                }
            }
            break;
            }
        }

        private void DoHealing()
        {
            owner.triggers.OnDoHealing(new HealingEventData(owner, owner, m_healing));
        }
    }

}
