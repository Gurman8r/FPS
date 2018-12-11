using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    // An Enchantment is a collection of one or more effects
    // An Enchantment can be added to a weapon or piece of armor.
    [ExecuteInEditMode]
    public class Enchantment : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] UniqueID           m_UID;
        [SerializeField] CastingType        m_castingType;
        [SerializeField] DeliveryType       m_deliveryType;
        [SerializeField] List<MagicEffect>  m_effects;

        /* Properties
        * * * * * * * * * * * * * * * */
        public UniqueID UID
        {
            get { return m_UID; }
        }

        public CastingType castingType
        {
            get { return m_castingType; }
        }

        public DeliveryType deliveryType
        {
            get { return m_deliveryType; }
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
