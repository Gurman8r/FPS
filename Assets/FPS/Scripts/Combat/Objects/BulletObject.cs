using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletObject : CombatObject
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Rigidbody m_rigidbody;


        /* Properties
        * * * * * * * * * * * * * * * */
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


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Update()
        {
            base.Update();
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

            rigidbody.position = data.position;

            rigidbody.velocity = data.direction * data.speed;
        }
    }
}
