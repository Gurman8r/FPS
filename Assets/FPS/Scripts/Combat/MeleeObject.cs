using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class MeleeObject : CombatObject
    {

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Properties
        * * * * * * * * * * * * * * * */


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
                if (CheckHitUnit(collider, out other))
                {
                    if (AddHit(other))
                    {
                        OnHitUnit(other);
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnter(other);
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetActive(bool value)
        {
            if(active && !value)
            {
                hitUnits.Clear();
            }
            active = value;
        }
    }

}
