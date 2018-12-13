using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class ActorData
    {
        public bool isEssential;
        public bool isProtected;
        public bool canRespawn;
        public bool isUnique;
        public bool isSummonable;
        public bool isGhost;
        public bool isInvulnerable;
    }
}
