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
        [SerializeField] bool       m_enableLog;
        [SerializeField] bool       m_changed;
        [Space]
        [SerializeField] Slider     m_volume;
        [SerializeField] Toggle     m_fullScreen;
        [SerializeField] Dropdown   m_screenResolution;
        [SerializeField] Slider     m_lookSensitivityX;
        [SerializeField] Slider     m_lookSensitivityY;
        [SerializeField] Button     m_saveButton;

        /* Properties
        * * * * * * * * * * * * * * * */
        public bool changed
        {
            get { return m_changed; }
            private set { m_changed = value; }
        }

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

            LoadAll();

            // Volume
            m_volume.value = AudioListener.volume;
            m_volume.onValueChanged.RemoveAllListeners();
            m_volume.onValueChanged.AddListener((float value) =>
            {
                AudioListener.volume = m_volume.value;
                changed = true;
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
                changed = true;
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
                    text = str
                });
            }
            m_screenResolution.captionText.text = Screen.currentResolution.ToString();
            m_screenResolution.onValueChanged.RemoveAllListeners();
            m_screenResolution.onValueChanged.AddListener((int value) =>
            {
                Resolution r = Screen.resolutions[m_screenResolution.value];
                Screen.SetResolution(
                    r.width, 
                    r.height, 
                    Screen.fullScreen);
                changed = true;
            });

            // Look Sensitivity Y
            m_lookSensitivityX.value = gs.lookSensitivityX;
            m_lookSensitivityX.onValueChanged.RemoveAllListeners();
            m_lookSensitivityX.onValueChanged.AddListener((float value) =>
            {
                gs.lookSensitivityX = m_lookSensitivityX.value;
                changed = true;
            });

            // Look Sensitivity Y
            m_lookSensitivityY.value = gs.lookSensitivityY;
            m_lookSensitivityY.onValueChanged.RemoveAllListeners();
            m_lookSensitivityY.onValueChanged.AddListener((float value) =>
            {
                gs.lookSensitivityY = m_lookSensitivityY.value;
                changed = true;
            });
        }

        private void Update()
        {
            if(m_saveButton)
            {
                m_saveButton.interactable = changed;
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void LoadAll()
        {
            GameSettings gs;
            if (gs = GameSettings.instance)
            {
                changed = false;
                AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
                gs.lookSensitivityX = PlayerPrefs.GetFloat("LookSensitivityX", 0.1f);
                gs.lookSensitivityY = PlayerPrefs.GetFloat("LookSensitivityY", 0.1f);

                if (m_enableLog) Debug.Log("Loaded Settings");
            }
            else
            {
                Debug.LogError("GameSettings instance not found");
            }
        }

        public void SaveAll()
        {
            GameSettings gs;
            if(gs = GameSettings.instance)
            {
                changed = false;
                PlayerPrefs.SetFloat("Volume", AudioListener.volume);
                PlayerPrefs.SetFloat("LookSensitivityX", gs.lookSensitivityX);
                PlayerPrefs.SetFloat("LookSensitivityY", gs.lookSensitivityY);
                PlayerPrefs.Save();

                if (m_enableLog) Debug.Log("Saved Settings");
            }
            else
            {
                Debug.LogError("GameSettings instance not found");
            }

        }
    }

}
