using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [DisallowMultipleComponent]
    public class CombatSystem : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Properties
        * * * * * * * * * * * * * * * */
        public static CombatSystem current
        {
            get;
            private set;
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void OnEnable()
        {
            if (!current)
            {
                current = this;
            }
            else
            {
                Debug.LogWarning("Multiple CombatSystem in scene... this is not supported");
            }
        }

        private void OnDisable()
        {
            if (current == this)
            {
                current = null;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
