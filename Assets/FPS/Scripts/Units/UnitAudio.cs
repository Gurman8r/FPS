using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(AudioSource))]
    public class UnitAudio : UnitBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private AudioSource m_audioSource;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new AudioSource audio
        {
            get
            {
                if(!m_audioSource)
                {
                    m_audioSource = GetComponent<AudioSource>();
                }
                return m_audioSource;
            }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }
}
