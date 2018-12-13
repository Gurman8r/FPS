using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class FriendlyFire
    {
        [Tooltip("A friendly hit expires after this amount of time")]
        public float friendHitTimer = 10f;
        [Tooltip("A friendly hit is added only when this amount of time has passed since the last friendly hit")]
        public float friendMinimumLastHitTime = 0.5f;
        [Tooltip("The number of hits that are allowed by a friend when they are in combat before they will attack you")]
        public int friendHitCombatAllowed = 3;
        [Tooltip("The number of hits that are allowed by a friend when they are not in combat before they will attack you")]
        public int friendHitNonCombatAllowed = 0;
        [Tooltip("The number of hits that are allowed by an ally when they are in combat before they will attack you")]
        public int allyHitCombatAllowed = 3;
        [Tooltip("The number of hits that are allowed by an ally when they are not in combat before they will attack you")]
        public int allyHitNonCombatAllowed = 0;
    }
}
