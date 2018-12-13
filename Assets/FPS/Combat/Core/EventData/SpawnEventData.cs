using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class SpawnEvent : UnitEventData
    {
        public SpawnEvent(Unit source)
            : base(source)
        {
        }
    }

}
