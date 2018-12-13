using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Hitbox : BaseEntity
    {
        /* Core
        * * * * * * * * * * * * * * * */
        protected override void OnTriggerEnter(Collider c)
        {
            base.OnTriggerEnter(c);
        }

        protected override void OnTriggerStay(Collider c)
        {
            base.OnTriggerStay(c);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetActive(bool value)
        {
            if(active && !value)
            {
                ClearHits();
            }
            active = value;
        }
    }

}
