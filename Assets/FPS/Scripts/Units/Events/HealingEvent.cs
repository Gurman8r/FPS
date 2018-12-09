using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class HealingEvent : InteractionEvent
    {
        public Healing healing;

        public HealingEvent(Unit source, Unit target, Healing healing)
            : base(source, target)
        {
            this.healing = healing;
        }
    }

}
