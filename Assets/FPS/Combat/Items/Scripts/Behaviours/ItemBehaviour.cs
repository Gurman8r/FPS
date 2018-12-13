using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Item))]
    public abstract class ItemBehaviour : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        private Item m_self;

        /* Properties
        * * * * * * * * * * * * * * * */
        public Item self
        {
            get
            {
                if (!m_self)
                {
                    m_self = GetComponent<Item>();
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
