using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class HealingEventData : UnitEventData
    {
        public Healing healing { get; set; }

        public HealingEventData(Unit source, Unit target, Healing healing)
            : base(source, target)
        {
            this.healing = healing;
        }
    }

}
