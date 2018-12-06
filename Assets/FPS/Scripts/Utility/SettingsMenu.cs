using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class SettingsMenu : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] Slider m_masterVolumeSlider;
        [SerializeField] Slider m_soundVolumeSlider;
        [SerializeField] Slider m_musicVolumeSlider;

        /* Properties
        * * * * * * * * * * * * * * * */

        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                m_masterVolumeSlider.onValueChanged.RemoveAllListeners();
                m_soundVolumeSlider.onValueChanged.RemoveAllListeners();
                m_musicVolumeSlider.onValueChanged.RemoveAllListeners();

                GameSettings gs;
                if (!(gs = GameSettings.instance))
                {
                    Debug.LogError("GameSettings instance not found");
                    return;
                }

                m_masterVolumeSlider.value = gs.masterVolume;
                m_soundVolumeSlider.value = gs.soundVolume;
                m_musicVolumeSlider.value = gs.musicVolume;

                m_masterVolumeSlider.onValueChanged.AddListener((float value) =>
                {
                    gs.SetMasterVolume(m_masterVolumeSlider.value);
                });

                m_soundVolumeSlider.onValueChanged.AddListener((float value) =>
                {
                    gs.SetSoundVolume(m_soundVolumeSlider.value);
                });

                m_musicVolumeSlider.onValueChanged.AddListener((float value) =>
                {
                    gs.SetMusicVolume(m_musicVolumeSlider.value);
                });
            }
        }

        private void Update()
        {

        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
