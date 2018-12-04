using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class GameSettings : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */

        /* Properties
        * * * * * * * * * * * * * * * */
        public static GameSettings instance
        {
            get; private set;
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Awake()
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

        private void Update()
        {

        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
