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
        public static UnitSystem instance
        {
            get; set;
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
            if(instance == this)
            {
                instance = null;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
