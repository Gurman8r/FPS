using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [ExecuteInEditMode]
    public class UnitVision : UnitBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] Transform  m_head;
        [SerializeField] float      m_maxRange = 50f;
        [SerializeField] LayerMask  m_layerMask;

        [Header("Runtime")]
        [SerializeField] Vector3    m_lookingAt;

        private RaycastHit m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Transform head
        {
            get { return m_head; }
        }

        public Vector3 origin
        {
            get { return head.position; }
        }

        public Vector3 direction
        {
            get { return head.forward; }
        }

        public float maxRange
        {
            get { return m_maxRange; }
        }

        public Vector3 lookingAt
        {
            get { return m_lookingAt; }
            private set { m_lookingAt = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if(Physics.Raycast(new Ray(origin, direction), out m_hit, maxRange, m_layerMask))
            {
                lookingAt = m_hit.point;
            }
            else
            {
                lookingAt = origin + (direction * maxRange);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(origin, lookingAt);
            Gizmos.DrawWireSphere(lookingAt, 0.1f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(origin + direction, 0.05f);
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
