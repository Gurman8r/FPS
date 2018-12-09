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
        [SerializeField] Transform  m_eye;
        [SerializeField] float      m_maxRange = 50f;
        [SerializeField] LayerMask  m_blockingLayers;

        [Header("Runtime")]
        [SerializeField] Vector3    m_lookingAt;

        private RaycastHit m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Transform eye
        {
            get { return m_eye; }
        }

        public float maxRange
        {
            get { return m_maxRange; }
        }

        public LayerMask blockingLayers
        {
            get { return m_blockingLayers; }
        }

        public Vector3 lookingAt
        {
            get { return m_lookingAt; }
            private set { m_lookingAt = value; }
        }

        public Vector3 origin
        {
            get { return eye.position; }
        }

        public Vector3 direction
        {
            get { return eye.forward; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Update()
        {
            if(Application.isPlaying)
            {
                if (Physics.Raycast(origin, direction, out m_hit, maxRange, blockingLayers))
                {
                    lookingAt = m_hit.point;
                }
                else
                {
                    lookingAt = origin + (direction * maxRange);
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
