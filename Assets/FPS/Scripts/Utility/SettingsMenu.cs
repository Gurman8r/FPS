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
        [SerializeField] Slider m_masterVolume;
        [SerializeField] Slider m_soundVolume;
        [SerializeField] Slider m_musicVolume;
        [SerializeField] Slider m_lookSensitivityX;
        [SerializeField] Slider m_lookSensitivityY;

        /* Properties
        * * * * * * * * * * * * * * * */

        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                GameSettings gs;
                if (!(gs = GameSettings.instance))
                {
                    Debug.LogError("GameSettings instance not found");
                    return;
                }

                m_masterVolume.value = gs.masterVolume;
                m_masterVolume.onValueChanged.RemoveAllListeners();
                m_masterVolume.onValueChanged.AddListener((float value) =>
                {
                    gs.masterVolume = m_masterVolume.value;
                });

                m_soundVolume.value = gs.soundVolume;
                m_soundVolume.onValueChanged.RemoveAllListeners();
                m_soundVolume.onValueChanged.AddListener((float value) =>
                {
                    gs.soundVolume = m_soundVolume.value;
                });

                m_musicVolume.value = gs.musicVolume;
                m_musicVolume.onValueChanged.RemoveAllListeners();
                m_musicVolume.onValueChanged.AddListener((float value) =>
                {
                    gs.musicVolume = m_musicVolume.value;
                });

                m_lookSensitivityX.value = gs.lookSensitivityX;
                m_lookSensitivityX.onValueChanged.RemoveAllListeners();
                m_lookSensitivityX.onValueChanged.AddListener((float value) =>
                {
                    gs.lookSensitivityX = m_lookSensitivityX.value;
                });

                m_lookSensitivityY.value = gs.lookSensitivityY;
                m_lookSensitivityY.onValueChanged.RemoveAllListeners();
                m_lookSensitivityY.onValueChanged.AddListener((float value) =>
                {
                    gs.lookSensitivityY = m_lookSensitivityY.value;
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
