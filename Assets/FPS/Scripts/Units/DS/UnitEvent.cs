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
        private UnitSystem m_unitSystem;
        private bool       m_used;

        [SerializeField] ObjectData m_data;

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

        public ObjectData data
        {
            get { return m_data; }
            set { m_data = value; }
        }


        /* Constructors
	    * * * * * * * * * * * * * * * */
        public UnitEvent()
            : this(UnitSystem.instance)
        {
        }

        public UnitEvent(UnitSystem unitSystem)
        {
            this.unitSystem = unitSystem;
        }

        public UnitEvent(UnitEvent copy)
            : this(UnitSystem.instance)
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
