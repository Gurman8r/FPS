using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FPS
{
    public class SettingsMenu : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] bool   m_enableLog;
        [SerializeField] bool   m_changed;
        [Space]
        [SerializeField] Slider     m_volume;
        [SerializeField] Toggle     m_fullScreen;
        [SerializeField] Toggle     m_fxaa;
        [SerializeField] Toggle     m_motionBlur;
        [SerializeField] Dropdown   m_screenResolution;
        [SerializeField] Dropdown   m_qualityLevel;
        [SerializeField] Slider     m_lookSensitivityX;
        [SerializeField] Slider     m_lookSensitivityY;
        [SerializeField] Button     m_saveButton;
        [SerializeField] Button     m_defaultButton;


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
            if (Application.isPlaying)
            {
                m_saveButton.onClick.RemoveAllListeners();
                m_saveButton.onClick.AddListener(() =>
                {
                    SaveSettings();
                });

                m_defaultButton.onClick.RemoveAllListeners();
                m_defaultButton.onClick.AddListener(() =>
                {
                    PlayerPrefs.DeleteAll();
                    LoadSettings();
                    changed = true;
                });

                LoadSettings();
            }
        }

        private void Update()
        {
            m_saveButton.interactable = changed;
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("Volume", m_volume.value);
            PlayerPrefs.SetInt("Fullscreen", m_fullScreen.isOn ? 1 : 0);
            PlayerPrefs.SetInt("EnableFXAA", m_fxaa.isOn ? 1 : 0);
            PlayerPrefs.SetInt("ScreenResolution", m_screenResolution.value);
            PlayerPrefs.SetInt("QualityLevel", m_qualityLevel.value);
            PlayerPrefs.SetFloat("LookSensitivityX", m_lookSensitivityX.value);
            PlayerPrefs.SetFloat("LookSensitivityY", m_lookSensitivityY.value);
            PlayerPrefs.Save();

            if (m_enableLog) { Debug.Log("Saved Settings"); }
            changed = false;
        }

        public void LoadSettings()
        {
            // Volume
            SetupSlider(m_volume, "Volume", 0.5f, (Slider s) =>
            {
                AudioListener.volume = s.value;
            });

            // Fullscreen
            SetupToggle(m_fullScreen, "Fullscreen", true, (Toggle t) =>
            {
                Screen.SetResolution(Screen.width, Screen.height, t.isOn);
            });

            // FXAA
            SetupToggle(m_fxaa, "EnableFXAA", true, (Toggle t) =>
            {
                PlayerCamera.main.fxaa.enabled = t.isOn;
            });

            // Motion Blur
            SetupToggle(m_motionBlur, "EnableMotionBlur", true, (Toggle t) =>
            {
                PlayerCamera.main.postProcessing.profile.motionBlur.enabled = t.isOn;
            });

            // Screen Resolution
            SetupDropdown(m_screenResolution, "ScreenResolution", Screen.resolutions.Length - 1, Screen.resolutions, (Dropdown d) =>
            {
                Resolution r = Screen.resolutions[d.value];
                Screen.SetResolution(r.width, r.height, Screen.fullScreen);
            });

            // Quality Level
            SetupDropdown(m_qualityLevel, "QualityLevel", QualitySettings.names.Length - 1, QualitySettings.names, (Dropdown d) =>
            {
                QualitySettings.SetQualityLevel(d.value);
            });

            // Look Sensitivity X
            SetupSlider(m_lookSensitivityX, "LookSensitivityX", 0.1f, (Slider slider) =>
            {
                PlayerCamera.main.lookSensitivity = new Vector2(slider.value, PlayerCamera.main.lookSensitivity.y);
            });

            // Look Sensitivity Y
            SetupSlider(m_lookSensitivityY, "LookSensitivityY", 0.1f, (Slider slider) =>
            {
                PlayerCamera.main.lookSensitivity = new Vector2(PlayerCamera.main.lookSensitivity.x, slider.value);
            });
            
            if (m_enableLog) { Debug.Log("Loaded Settings"); }
            changed = false;
        }

        

        private void SetupSlider(Slider slider, string key, float dv, UnityAction<Slider> call)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener((float t) => 
            {
                call(slider);
                changed = true;
            });
            slider.value = PlayerPrefs.GetFloat(key, dv);
        }

        private void SetupToggle(Toggle toggle, string key, bool dv, UnityAction<Toggle> call)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((bool t) =>
            {
                call(toggle);
                changed = true;
            });
            toggle.isOn = PlayerPrefs.GetInt(key, (dv ? 1 : 0)) == 1;
        }

        private void SetupDropdown<T>(Dropdown dropdown, string key, int dv, T[] options, UnityAction<Dropdown> call)
        {
            dropdown.options.Clear();
            for (int i = 0; i < options.Length; i++)
            {
                dropdown.options.Add(new Dropdown.OptionData
                {
                    text = options[i].ToString()
                });
            }
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener((int value) =>
            {
                call(dropdown);
                changed = true;
            });
            dropdown.value = PlayerPrefs.GetInt(key, dv);
        }
    }

}
