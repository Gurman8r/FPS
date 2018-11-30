using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public class Laser : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] LayerMask  m_layerMask;
        [SerializeField] float      m_width = 0.1f;
        [SerializeField] float      m_speed = 0f;

        [Header("Runtime")]
        [SerializeField] Vector3    m_posA;
        [SerializeField] Vector3    m_posB;
        [SerializeField] Vector3    m_temp;
        [SerializeField] Ray        m_ray;
        [SerializeField] RaycastHit m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Vector3 posA
        {
            get { return m_posA; }
            set { m_posA = value; }
        }

        public Vector3 posB
        {
            get { return m_posB; }
            set { m_posB = value; }
        }

        public float width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        public float speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if(Application.isPlaying)
            {
                transform.position = (posA + posB) / 2f;

                transform.localScale = new Vector3(
                    width,
                    width,
                    Vector3.Distance(posA, posB));

                transform.LookAt(posB);

                if (speed > 0f)
                {

                }

                m_ray = new Ray(posA, posB);

                if (Physics.Raycast(m_ray, out m_hit, transform.localScale.z, m_layerMask))
                {

                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
