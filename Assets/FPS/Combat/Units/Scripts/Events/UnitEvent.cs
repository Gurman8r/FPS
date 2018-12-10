using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class UnitEvent
    {
        /* Variables
	    * * * * * * * * * * * * * * * */
        private readonly Unit m_source;


        /* Properties
	    * * * * * * * * * * * * * * * */
        public Unit source
        {
            get { return m_source; }
        }

        public bool used { get; private set; }


        /* Constructors
	    * * * * * * * * * * * * * * * */
        public UnitEvent(Unit source)
        {
            m_source = source;
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
