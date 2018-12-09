using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class DamageEvent : InteractionEvent
    {
        public Damage damage;

        public DamageEvent(Unit source, Unit target, Damage damage)
            : base(source, target)
        {
            this.damage = damage;
        }
    }

}
