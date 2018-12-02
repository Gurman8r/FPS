using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BeamObject : CombatObject
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Laser Settings")]
        [SerializeField] float      m_width = 0.1f;
        [SerializeField] float      m_sourceSpeed = 0.5f;
        [SerializeField] float      m_targetSpeed = 1f;

        [Header("Laser Runtime")]
        [SerializeField] Vector3    m_sourcePos;
        [SerializeField] Vector3    m_targetPos;
        [SerializeField] Vector3    m_realTarget;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Vector3 sourcePos
        {
            get { return m_sourcePos; }
            set { m_sourcePos = value; }
        }

        public Vector3 targetPos
        {
            get { return m_targetPos; }
            set { m_targetPos = value; }
        }

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
                    sourcePos = Vector3.Lerp(
                        sourcePos,
                        targetPos,
                        (timer / data.lifeSpan) * m_sourceSpeed);

                    targetPos = Vector3.Lerp(
                        targetPos,
                        m_realTarget,
                        (timer / data.lifeSpan) * m_targetSpeed);
                }
                else 
                {
                    targetPos = m_realTarget;
                }

                transform.position = (sourcePos + targetPos) / 2f;

                transform.localScale = new Vector3(
                    width, width, Vector3.Distance(sourcePos, targetPos));

                transform.LookAt(targetPos);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            Unit other;
            if (CheckHit(collider, out other))
            {
                if (AddHit(other))
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

            m_realTarget = targetPos;

            targetPos = sourcePos;
        }
    }

}
