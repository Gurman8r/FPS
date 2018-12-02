using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(SphereCollider))]
    public class ExplosionObject : CombatObject
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        private SphereCollider m_collider;

        [Header("Settings")]
        [SerializeField] float m_minSize = 1f;
        [SerializeField] float m_maxSize = 10f;

        [Header("Runtime")]
        [SerializeField] float m_curSize;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new SphereCollider collider
        {
            get
            {
                if(!m_collider)
                {
                    m_collider = GetComponent<SphereCollider>();
                }
                return m_collider;
            }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            if(Application.isPlaying)
            {
                if (data.lifeSpan > 0f)
                {
                    base.Update();

                    m_curSize = Mathf.Lerp(m_curSize, m_maxSize, timer / data.lifeSpan);

                    SetLocalScale(m_curSize);
                }
                else if(data.speed > 0f)
                {
                    if(m_curSize < m_maxSize)
                    {
                        m_curSize = Mathf.Lerp(m_curSize, m_maxSize, data.speed * Time.deltaTime);

                        SetLocalScale(m_curSize);
                    }
                    else
                    {
                        Kill();
                    }
                }
                else
                {
                    Kill();
                }
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            Unit other;
            if(CheckHit(collider, out other))
            {
                if(AddHit(other))
                {
                    OnHit(other);
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void Spawn()
        {
            base.Spawn();

            m_curSize = m_minSize;

            SetLocalScale(m_curSize);
        }

        private void SetLocalScale(float value)
        {
            transform.localScale = new Vector3(value, value, value);
        }
    }

}
