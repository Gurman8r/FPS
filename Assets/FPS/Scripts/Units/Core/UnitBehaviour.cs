using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Unit))]
    public abstract class UnitBehaviour : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Unit m_unit;

        /* Properties
        * * * * * * * * * * * * * * * */
        public Unit unit
        {
            get
            {
                if(!m_unit)
                {
                    m_unit = GetComponent<Unit>();
                }
                return m_unit;
            }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }
    }

}
