using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class DeathEventData : UnitEventData
    {
        public DeathEventData(Unit source)
            : base(source)
        {
        }
    }

}
