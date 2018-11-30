using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [ExecuteInEditMode]
    public abstract class Item : MonoBehaviour
    {
        public static readonly string Tag = "Item";

        /* Variables
        * * * * * * * * * * * * * * * */
        private Collider    m_collider;
        private Rigidbody   m_rigidbody;
        private Unit        m_owner;

        [Header("Item Settings")]
        [SerializeField] Transform  m_model;
        [SerializeField] Transform  m_holdPosition;
        [SerializeField] ItemInfo   m_info;

        /* Properties
        * * * * * * * * * * * * * * * */
        public new Collider collider
        {
            get
            {
                if(!m_collider)
                {
                    m_collider = GetComponent<Collider>();
                }
                return m_collider;
            }
        }

        public new Rigidbody rigidbody
        {
            get
            {
                if(!m_rigidbody)
                {
                    m_rigidbody = GetComponent<Rigidbody>();
                }
                return m_rigidbody;
            }
        }

        public Unit owner
        {
            get { return m_owner; }
            private set { m_owner = value; }
        }

        public Vector3 holdPos
        {
            get { return m_holdPosition ? m_holdPosition.localPosition : Vector3.zero; }
        }

        public ItemInfo info
        {
            get { return m_info; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Start()
        {
            if (gameObject.tag != Tag)
                gameObject.tag = Tag;
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + holdPos, 0.1f);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public bool SetOwner(Unit value)
        {
            if(owner != value)
            {
                owner = value;
                return true;
            }
            return false;
        }

        public void EnablePhysics(bool value)
        {
            rigidbody.isKinematic = !value;
            rigidbody.useGravity = value;
            collider.isTrigger = !value;
        }

        public abstract void UpdatePrimary(string axis);

        public abstract void UpdateSecondary(string axis);
    }
}
