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
        [SerializeField] Healing m_healing;

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
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(InputState input)
        {
            if(input.press)
            {
                owner.triggers.Broadcast(EventType.OnDoHealing, new UnitEvent
                {
                    data = new ObjectData
                    {
                        owner   = owner,
                        target  = owner,
                        healing = m_healing
                    }
                });
            }
        }

        public override void UpdateSecondary(InputState input)
        {
        }
    }

}
