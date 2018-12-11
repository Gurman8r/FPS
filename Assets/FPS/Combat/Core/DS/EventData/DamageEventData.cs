using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class DamageEventData : UnitEventData
    {
        public Damage damage { get; set; }

        public DamageEventData(Unit source, Unit target, Damage damage)
            : base(source, target)
        {
            this.damage = damage;
        }
    }

}
