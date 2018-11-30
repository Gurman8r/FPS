using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public class UnitEventData
    {
        /* Variables
	    * * * * * * * * * * * * * * * */
        private UnitSystem  m_unitSystem;
        private bool        m_used;


        /* Properties
	    * * * * * * * * * * * * * * * */
        public UnitSystem unitSystem
        {
            get { return m_unitSystem; }
        }

        public bool used
        {
            get { return m_used; }
        }

        public Damage damage { get; set; }


        /* Constructors
	    * * * * * * * * * * * * * * * */
        public UnitEventData(UnitSystem eventSystem)
        {
            m_unitSystem = eventSystem;
        }


        /* Functions
	    * * * * * * * * * * * * * * * */
        public void Reset()
        {
            m_used = false;
        }

        public void Use()
        {
            m_used = true;
        }
    }

}
