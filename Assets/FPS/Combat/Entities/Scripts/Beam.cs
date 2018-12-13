using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Beam : BaseEntity
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Beam Settings")]
        [SerializeField] float      m_width = 0.1f;
        [SerializeField] float      m_sourceSpeed = 0.5f;
        [SerializeField] float      m_targetSpeed = 1f;

        [Header("Beam Runtime")]
        [SerializeField] Vector3    m_target;


        /* Properties
        * * * * * * * * * * * * * * * */
        public float width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        public Vector2 target
        {
            get { return m_target; }
            set { m_target = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Update()
        {
            base.Update();

            if(Application.isPlaying)
            {
                transform.position = (motion.origin + motion.target) / 2f;

                transform.localScale = new Vector3(
                    width, 
                    width, 
                    Vector3.Distance(motion.origin, motion.target));

                transform.LookAt(motion.target);
            }
        }

        protected override void OnTriggerEnter(Collider c)
        {
            base.OnTriggerEnter(c);
        }

        protected override void OnTriggerStay(Collider c)
        {
            base.OnTriggerStay(c);
        }

    }

}
