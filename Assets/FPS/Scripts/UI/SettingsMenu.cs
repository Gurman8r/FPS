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
        [SerializeField] Slider     m_volume;
        [SerializeField] Toggle     m_fullScreen;
        [SerializeField] Dropdown   m_screenResolution;
        [SerializeField] Slider     m_lookSensitivityX;
        [SerializeField] Slider     m_lookSensitivityY;

        /* Properties
        * * * * * * * * * * * * * * * */

        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(!Application.isPlaying)
            {
                return;
            }

            GameSettings gs;
            if (!(gs = GameSettings.instance))
            {
                Debug.LogError("GameSettings instance not found");
                return;
            }

            // Volume
            m_volume.value = AudioListener.volume;
            m_volume.onValueChanged.RemoveAllListeners();
            m_volume.onValueChanged.AddListener((float value) =>
            {
                AudioListener.volume = m_volume.value;
            });

            // Fullscreen
            m_fullScreen.isOn = Screen.fullScreen;
            m_fullScreen.onValueChanged.RemoveAllListeners();
            m_fullScreen.onValueChanged.AddListener((bool value) =>
            {
                Screen.SetResolution(
                    Screen.currentResolution.width, 
                    Screen.currentResolution.height, 
                    m_fullScreen.isOn);
            });

            // Screen Resolution
            m_screenResolution.options.Clear();
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                string str = Screen.resolutions[i].ToString();

                if (str == Screen.currentResolution.ToString())
                {
                    m_screenResolution.value = i;
                }

                m_screenResolution.options.Add(new Dropdown.OptionData
                {
                    text = str.Substring(0, str.IndexOf('@'))
                });
            }
            //m_screenResolution.captionText.text = string.Format("{0} x {1}", Screen.width, Screen.height);
            m_screenResolution.onValueChanged.RemoveAllListeners();
            m_screenResolution.onValueChanged.AddListener((int value) =>
            {
                Resolution r = Screen.resolutions[m_screenResolution.value];
                Screen.SetResolution(
                    r.width, 
                    r.height, 
                    Screen.fullScreen);
            });

            // Look Sensitivity Y
            m_lookSensitivityX.value = gs.lookSensitivityX;
            m_lookSensitivityX.onValueChanged.RemoveAllListeners();
            m_lookSensitivityX.onValueChanged.AddListener((float value) =>
            {
                gs.lookSensitivityX = m_lookSensitivityX.value;
            });

            // Look Sensitivity Y
            m_lookSensitivityY.value = gs.lookSensitivityY;
            m_lookSensitivityY.onValueChanged.RemoveAllListeners();
            m_lookSensitivityY.onValueChanged.AddListener((float value) =>
            {
                gs.lookSensitivityY = m_lookSensitivityY.value;
            });
        }
    }

}
