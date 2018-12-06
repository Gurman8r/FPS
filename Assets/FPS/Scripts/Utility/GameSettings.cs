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
        [SerializeField] float m_masterVolume = 0.5f;
        [Range(0f, 1f)]
        [SerializeField] float m_soundVolume = 1.0f;
        [Range(0f, 1f)]
        [SerializeField] float m_musicVolume = 1.0f;

        /* Properties
        * * * * * * * * * * * * * * * */
        public static GameSettings instance
        {
            get; private set;
        }

        public float masterVolume
        {
            get { return m_masterVolume; }
        }

        public float soundVolume
        {
            get { return m_soundVolume; }
        }

        public float musicVolume
        {
            get { return m_musicVolume; }
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
        public void SetMasterVolume(float value)
        {
            m_masterVolume = Mathf.Clamp(value, 0f, 1f);
        }

        public void SetSoundVolume(float value)
        {
            m_soundVolume = Mathf.Clamp(value, 0f, 1f);
        }

        public void SetMusicVolume(float value)
        {
            m_musicVolume = Mathf.Clamp(value, 0f, 1f);
        }
    }

}
