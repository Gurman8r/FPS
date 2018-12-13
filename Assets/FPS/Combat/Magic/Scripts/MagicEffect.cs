using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public enum SpellType
    {
        Normal = 0,  // A standard spell
        Ability,    // An always-on, constant-effect spell
    }

    public enum EffectType
    {
        None = 0,
        ValueModifier,
        DualValueModifier,
    }

    public enum CastingType
    {
        ConstantEffect = 0,
        FireAndForget,
        Concentration,
    }

    public enum DeliveryType
    {
        Self = 0,       // Effect is applied to the caster.
        Contact,        // Effect is applied to the target by contact (a hit event). This only works for Weapons.
        Aimed,          // Effect is attached to a Projectile which is then fired at the crosshairs. If it makes contact with a valid target, the effect is applied.
        TargetUnit,     // Effect is immediately applied to an Actor in the crosshairs. No projectile is fired.
        TargetLocation, // Effects is applied to the object/landscape under the crosshairs. No projectile is fired.
    }

    public enum EquipType
    {
        EitherHand = 0, // One Handed
        BothHands,      // Two Handed
        HandsFree,      // Always active
    }

    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class MagicEffect : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] UniqueID       m_UID;
        [SerializeField] EffectType     m_effectType;
        [SerializeField] CastingType    m_castingType;
        [SerializeField] DeliveryType   m_deliveryType;
        [SerializeField] AnimationCurve m_taperCurve = AnimationCurve.Constant(0f, 1f, 1f);
        [SerializeField] float          m_magnitude;
        [SerializeField] float          m_area;
        [SerializeField] float          m_duration;
        
        [Tooltip("This Effect is treated as an attack.")]
        [SerializeField] bool m_hostile;
        [Tooltip("This Effect is applied as a negative value (damage)")]
        [SerializeField] bool m_detrimental;
        [Tooltip("Once the magic effect is applied to a target, it cannot be cast again on the same target until it has worn off or been dispelled.")]
        [SerializeField] bool m_noRecast;
        [Tooltip("The effect does not use the Magnitude field.")]
        [SerializeField] bool m_noMagnitude;
        [Tooltip("The effect does not use the Area field.")]
        [SerializeField] bool m_noArea;
        [Tooltip("The effect is instantaneous. The Duration field is not available for objects with this effect.")]
        [SerializeField] bool m_noDuration;

        [SerializeField] UnityEvent m_onCharge;
        [SerializeField] UnityEvent m_onReady;
        [SerializeField] UnityEvent m_onRelease;
        [SerializeField] UnityEvent m_onCastLoop;
        [SerializeField] UnityEvent m_onHit;

        /* Properties
        * * * * * * * * * * * * * * * */
        public UniqueID UID
        {
            get { return m_UID; }
        }

        public EffectType effectType
        {
            get { return m_effectType; }
        }

        public CastingType castingType
        {
            get { return m_castingType; }
        }

        public DeliveryType deliveryType
        {
            get { return m_deliveryType; }
        }

        public AnimationCurve taperCurve
        {
            get { return m_taperCurve; }
        }

        public float magnitude
        {
            get { return m_magnitude; }
        }

        public float area
        {
            get { return m_area; }
        }

        public float duration
        {
            get { return m_duration; }
        }


        // Flags
        public bool hostile
        {
            get { return m_hostile; }
        }

        public bool detrimental
        {
            get { return m_detrimental; }
        }

        public bool noRecast
        {
            get { return m_noRecast; }
        }

        public bool noDuration
        {
            get { return m_noDuration; }
        }

        public bool noMagnitude
        {
            get { return m_noMagnitude; }
        }

        public bool noArea
        {
            get { return m_noArea; }
        }


        // Events
        public UnityEvent onCharge
        {
            get { return m_onCharge; }
        }

        public UnityEvent onReady
        {
            get { return m_onReady; }
        }

        public UnityEvent onRelease
        {
            get { return m_onRelease; }
        }

        public UnityEvent onCastLoop
        {
            get { return m_onCastLoop; }
        }

        public UnityEvent onHit
        {
            get { return m_onHit; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if(Application.isPlaying)
            {

            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
