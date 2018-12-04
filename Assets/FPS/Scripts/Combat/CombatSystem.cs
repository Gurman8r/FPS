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
        public static CombatSystem instance
        {
            get;
            private set;
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                if (!instance)
                {
                    DontDestroyOnLoad(gameObject);
                    instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnDisable()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
