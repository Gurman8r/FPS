using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public sealed class Hitmarker : BaseElement
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] float m_lifeSpan;
        [SerializeField] float m_timer;

        /* Properties
        * * * * * * * * * * * * * * * */
        public float lifeSpan
        {
            get { return m_lifeSpan; }
            set { m_lifeSpan = value; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if(Application.isPlaying && m_timer <= 0f)
            {
                Destroy(gameObject);
            }
            else
            {
                m_timer -= Time.deltaTime;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void Spawn()
        {
            m_timer = m_lifeSpan;
        }
    }

}
