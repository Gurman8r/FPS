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
                GameSettings gs;
                if (gs = GameSettings.instance)
                {
                    audio.volume = gs.masterVolume * gs.soundVolume;
                }
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                GameSettings gs;
                if (gs = GameSettings.instance)
                {
                    audio.volume = gs.masterVolume * gs.soundVolume;
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }
}
