using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [DisallowMultipleComponent]
    public class UnitSystem : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Properties
        * * * * * * * * * * * * * * * */
        public static UnitSystem current
        {
            get; set;
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
                Debug.LogWarning("Multiple UnitEventSystems in scene... this is not supported");
            }
        }

        private void OnDisable()
        {
            if(current == this)
            {
                current = null;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
