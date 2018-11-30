using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public enum EventType
    {
        OnSpawn, OnDeath,
        OnRecieveDamage, OnRecieveHealing,
        OnDoDamage, OnDoHealing,
    }

    [Serializable]
    public class UnitEvent
    {
        /* Variables
	    * * * * * * * * * * * * * * * */
        [SerializeField] UnitSystem m_unitSystem;
        [SerializeField] bool       m_used;
        [Space]
        [SerializeField] CombatData m_combat;

        /* Properties
	    * * * * * * * * * * * * * * * */
        public UnitSystem unitSystem
        {
            get { return m_unitSystem; }
            private set { m_unitSystem = value; }
        }

        public bool used
        {
            get { return m_used; }
            private set { m_used = value; }
        }

        public CombatData combat
        {
            get { return m_combat; }
            set { m_combat = value; }
        }


        /* Constructors
	    * * * * * * * * * * * * * * * */
        public UnitEvent()
            : this(UnitSystem.current)
        {
        }

        public UnitEvent(UnitSystem unitSystem)
        {
            this.unitSystem = unitSystem;
        }

        public UnitEvent(UnitEvent copy)
            : this(UnitSystem.current)
        {

        }


        /* Functions
	    * * * * * * * * * * * * * * * */
        public void Reset()
        {
            used = false;
        }

        public void Use()
        {
            used = true;
        }


        /* Operators
	    * * * * * * * * * * * * * * * */
        public static implicit operator bool(UnitEvent value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }

}
