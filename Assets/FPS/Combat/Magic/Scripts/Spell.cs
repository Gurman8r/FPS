using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    // Spells are the most common means of using or applying Magic Effects.
    // Spells include actual Spells, Powers, Perk effects, Diseases, and Poisons.
    [ExecuteInEditMode]
    public class Spell : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] UniqueID           m_UID;
        [SerializeField] SpellType          m_spellType;
        [SerializeField] CastingType        m_castingType;
        [SerializeField] DeliveryType       m_deliveryType;
        [SerializeField] EquipType          m_equipType;
        [SerializeField] float              m_range;
        [SerializeField] float              m_chargeTime;
        [SerializeField] List<MagicEffect>  m_effects;

        /* Properties
        * * * * * * * * * * * * * * * */
        public UniqueID UID
        {
            get { return m_UID; }
        }

        public SpellType spellType
        {
            get { return m_spellType; }
        }

        public CastingType castingType
        {
            get { return m_castingType; }
        }

        public DeliveryType deliveryType
        {
            get { return m_deliveryType; }
        }

        public float range
        {
            get { return m_range; }
        }

        public float chargeTime
        {
            get { return m_chargeTime; }
        }

        public List<MagicEffect> effects
        {
            get { return m_effects; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if (Application.isPlaying)
            {

            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
