using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BeamObject : CombatObject
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


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Update()
        {
            base.Update();

            if(Application.isPlaying)
            {
                if(data.speed > 0f && data.lifeSpan > 0f)
                {
                    data.pos = Vector3.Lerp(
                        data.pos,
                        data.dest,
                        (timer / data.lifeSpan) * m_sourceSpeed);

                    data.dest = Vector3.Lerp(
                        data.dest,
                        m_target,
                        (timer / data.lifeSpan) * m_targetSpeed);
                }
                else 
                {
                    data.dest = m_target;
                }

                transform.position = (data.pos + data.dest) / 2f;

                transform.localScale = new Vector3(
                    width, width, Vector3.Distance(data.pos, data.dest));

                transform.LookAt(data.dest);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            Unit other;
            if (CheckHitUnit(collider, out other))
            {
                if (AddHit(other))
                {
                    OnHitUnit(other);
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */

        public override void Spawn()
        {
            base.Spawn();

            m_target = data.dest;

            data.dest = data.pos;
        }
    }

}
