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
        [Range(0f, 1f)]
        [SerializeField] float m_lookSensitivityX = 1.0f;
        [Range(0f, 1f)]
        [SerializeField] float m_lookSensitivityY = 1.0f;

        /* Properties
        * * * * * * * * * * * * * * * */
        public static GameSettings instance
        {
            get; private set;
        }

        public float lookSensitivityX
        {
            get { return m_lookSensitivityX; }
            set { m_lookSensitivityX = Mathf.Clamp(value, 0f, 1f); }
        }

        public float lookSensitivityY
        {
            get { return m_lookSensitivityY; }
            set { m_lookSensitivityY = Mathf.Clamp(value, 0f, 1f); }
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


        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
