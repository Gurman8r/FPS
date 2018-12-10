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
                        owner.inventory.Drop(owner.inventory.hand);
                        Destroy(gameObject, m_destroyDelay);
                    }
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void HandleInputPrimary(InputState input)
        {
            switch(useMode)
            {
            case UseMode.Single:
            {
                if(input.press && !owner.health.isFull)
                {
                    DoHealing();
                    ConsumeResource();
                }
            }
            break;
            case UseMode.Continuous:
            {
                if (input.hold && !owner.health.isFull)
                {
                    DoHealing();
                    ConsumeResource();
                }
            }
            break;
            }
        }

        public override void HandleInputSecondary(InputState input)
        {
        }

        private void DoHealing()
        {
            owner.triggers.OnDoHealing(new HealingEvent(owner, owner, m_healing));
        }
    }

}
