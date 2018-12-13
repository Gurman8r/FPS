using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class CombatStyle
    {
        [Header("General")]
        [Tooltip("Chance to attack")]
        [Range(0f, 1f)] public float offensiveMult      = 0.5f;
        [Tooltip("Chance to defend")]
        [Range(0f, 1f)] public float defensiveMult      = 0.5f;
        [Tooltip("This will over-ride the offensive mult above. The higher the mult, the more offensive they'll be in groups.")]
        [Range(0f, 1f)] public float groupOffensiveMult = 0.5f;
        [Tooltip("Chance to run away")]
        [Range(0f, 1f)] public float avoidThreatChance  = 0.5f;
    }
}
