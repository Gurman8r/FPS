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
                    if (!hasResource && m_removeOnEmpty)
                    {
                        interactable = false;
                        owner.inventory.Drop(hand);
                        Destroy(gameObject, m_destroyDelay);
                    }
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(InputState input)
        {
            switch(useMode)
            {
            case UseMode.Single:
            {
                if(input.press && !owner.health.full)
                {
                    DoHealing();
                    ConsumeResource();
                }
            }
            break;
            case UseMode.Continuous:
            {
                if (input.hold && !owner.health.full)
                {
                    DoHealing();
                    ConsumeResource();
                }
            }
            break;
            }
        }

        public override void UpdateSecondary(InputState input)
        {
        }

        private void DoHealing()
        {            
            UnitEvent ev = new UnitEvent
            {
                data = new ObjectData
                {
                    owner = owner,
                    target = owner,
                    healing = m_healing
                }
            };
            owner.triggers.Broadcast(EventType.OnDoHealing, ev);
            owner.triggers.Broadcast(EventType.OnReceiveHealing, ev);
        }
    }

}
