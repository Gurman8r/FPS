using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    // Any event with more than just one unit. 
    // Target and source can be the same.
    [Serializable]
    public class InteractionEvent : UnitEvent
    {
        public Unit     target;     // Which unit is being targeted
        public Vector3  position;   // Where did the interaction occur

        public InteractionEvent(Unit source, Unit target)
            : this(source, target, Vector3.zero)
        {
        }

        public InteractionEvent(Unit source, Unit target, Vector3 position)
            : base(source)
        {
            this.target = target;
            this.position = position;
        }
    }

}
