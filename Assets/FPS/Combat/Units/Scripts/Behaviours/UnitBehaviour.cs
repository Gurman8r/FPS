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
        private Unit m_self;

        /* Properties
        * * * * * * * * * * * * * * * */
        public Unit self
        {
            get
            {
                if(!m_self)
                {
                    m_self = GetComponent<Unit>();
                }
                return m_self;
            }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Awake() { }

        protected virtual void OnEnable() { }
        
        protected virtual void OnDisable() { }

        protected virtual void Start() { }

        protected virtual void OnDestroy() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }
    }

}
