using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class MeleeObject : CombatObject
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] bool m_active;


        /* Properties
        * * * * * * * * * * * * * * * */
        public bool active
        {
            get { return m_active; }
            set { m_active = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(active)
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
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetActive(bool value)
        {
            if(active && !value)
            {
                hitList.Clear();
            }
            active = value;
        }
    }

}
