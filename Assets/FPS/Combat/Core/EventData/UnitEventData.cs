using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class UnitEventData : BaseEventData
    {
        public Unit source { get; set; }

        public Unit target { get; set; }


        public UnitEventData(Unit source)
            : this(source, source)
        {
        }

        public UnitEventData(Unit source, Unit target)
        {
            this.source = source;
            this.target = target;
        }
    }
}
