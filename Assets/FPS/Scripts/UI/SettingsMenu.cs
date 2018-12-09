using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class SettingsMenu : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] bool   m_changed;

        [Header("Audio")]
        [SerializeField] Slider     m_volume;

        [Header("Gameplay")]
        [SerializeField] Slider     m_lookSensitivityX;
        [SerializeField] Slider     m_lookSensitivityY;
        [SerializeField] Slider     m_fieldOfView;

        [Header("Video")]
        [SerializeField] Dropdown   m_resolution;
        [SerializeField] Dropdown   m_displayMode;
        [SerializeField] Dropdown   m_qualityLevel;

        [Header("Post Processing")]
        [SerializeField] Toggle     m_antialiasing;
        [SerializeField] Toggle     m_ambientOcclusion;
        [SerializeField] Toggle     m_bloom;
        [SerializeField] Toggle     m_motionBlur;
        [SerializeField] Toggle     m_vignette;

        [Header("UI Reference")]
        [SerializeField] Button     m_saveButton;
        [SerializeField] Button     m_defaultButton;
        [SerializeField] Scrollbar  m_scrollBar;
        [Range(0f, 1f)]
        [SerializeField] float      m_scrollValue;

        private List<Vector2Int> m_modes;


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
                LoadSettings();
            }
        }

        private void Update()
        {
            if(Application.isPlaying)
            {
                if (m_saveButton)
                {
                    m_saveButton.interactable = changed;
                }
            }
            else
            {
                if(m_scrollBar)
                {
                    m_scrollBar.value = m_scrollValue;
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SaveSettings()
        {
            // Audio
            PlayerPrefs.SetFloat("Volume", m_volume.value);

            // Gameplay
            PlayerPrefs.SetFloat("LookSensitivityX", m_lookSensitivityX.value);
            PlayerPrefs.SetFloat("LookSensitivityY", m_lookSensitivityY.value);
            PlayerPrefs.SetFloat("FieldOfView", m_fieldOfView.value);

            // Video
            PlayerPrefs.SetInt("ScreenResolution", m_resolution.value);
            PlayerPrefs.SetInt("QualityLevel", m_qualityLevel.value);
            PlayerPrefs.SetInt("DisplayMode", m_displayMode.value);

            // Post Processing
            PlayerPrefs.SetInt("EnableAntialiasing", m_antialiasing.isOn ? 1 : 0);
            PlayerPrefs.SetInt("EnableAmbientOcclusion", m_ambientOcclusion.isOn ? 1 : 0);
            PlayerPrefs.SetInt("EnableBloom", m_bloom.isOn ? 1 : 0);
            PlayerPrefs.SetInt("EnableMotionBlur", m_motionBlur.isOn ? 1 : 0);
            PlayerPrefs.SetInt("EnableVignette", m_vignette.isOn ? 1 : 0);

            PlayerPrefs.Save();
            changed = false;
        }

        public void LoadSettings()
        {
            // Volume
            SetupSlider(m_volume, "Volume", 0.5f, (Slider s) =>
            {
                AudioListener.volume = s.value;
            });

            // Look Sensitivity X
            SetupSlider(m_lookSensitivityX, "LookSensitivityX", 0.1f, (Slider s) =>
            {
                FirstPersonCamera.main.lookSensitivity = new Vector2(s.value, FirstPersonCamera.main.lookSensitivity.y);
            });

            // Look Sensitivity Y
            SetupSlider(m_lookSensitivityY, "LookSensitivityY", 0.1f, (Slider s) =>
            {
                FirstPersonCamera.main.lookSensitivity = new Vector2(FirstPersonCamera.main.lookSensitivity.x, s.value);
            });

            // Field of View
            SetupSlider(m_fieldOfView, "FieldOfView", 60f, (Slider s) => 
            {
                FirstPersonCamera.main.fieldOfView = s.value;
            });

            // Screen Resolution
            m_modes = new List<Vector2Int>();
            foreach(Resolution r in Screen.resolutions)
            {
                Vector2Int s = new Vector2Int(r.width, r.height);
                if(!m_modes.Contains(s))
                {
                    m_modes.Add(s);
                }
            }
            SetupDropdown(m_resolution, "ScreenResolution", m_modes.Count - 1, m_modes.ToArray(), (Dropdown d) =>
            {
                Screen.SetResolution(m_modes[d.value].x, m_modes[d.value].y, Screen.fullScreen);
            });

            // Display Mode
            SetupDropdown(m_displayMode, "DisplayMode", 0, Enum.GetNames(typeof(FullScreenMode)), (Dropdown d) => 
            {
                Screen.SetResolution(
                    m_modes[m_resolution.value].x,
                    m_modes[m_resolution.value].y,
                    (FullScreenMode)d.value);
            }, true);

            // Quality Level
            SetupDropdown(m_qualityLevel, "QualityLevel", QualitySettings.names.Length - 1, QualitySettings.names, (Dropdown d) =>
            {
                QualitySettings.SetQualityLevel(d.value);
            });

            // Antialiasing
            SetupToggle(m_antialiasing, "EnableAntialiasing", true, (Toggle t) =>
            {
                FirstPersonCamera.main.postProcessing.profile.antialiasing.enabled = t.isOn;
            });

            // Ambient Occlusion
            SetupToggle(m_ambientOcclusion, "EnableAmbientOcclusion", true, (Toggle t) =>
            {
                FirstPersonCamera.main.postProcessing.profile.ambientOcclusion.enabled = t.isOn;
            });

            // Bloom
            SetupToggle(m_bloom, "EnableBloom", true, (Toggle t) =>
            {
                FirstPersonCamera.main.postProcessing.profile.bloom.enabled = t.isOn;
            });

            // Motion Blur
            SetupToggle(m_motionBlur, "EnableMotionBlur", true, (Toggle t) =>
            {
                FirstPersonCamera.main.postProcessing.profile.motionBlur.enabled = t.isOn;
            });

            // Vignette
            SetupToggle(m_vignette, "EnableVignette", true, (Toggle t) =>
            {
                FirstPersonCamera.main.postProcessing.profile.vignette.enabled = t.isOn;
            });
            
            
            changed = false;
        }

        public void LoadDefaults()
        {
            PlayerPrefs.DeleteAll();
            LoadSettings();
            changed = true;
        }

        

        private void SetupSlider(Slider value, string key, float dv, UnityAction<Slider> call)
        {
            if (!value)
                return;

            value.onValueChanged.RemoveAllListeners();
            value.onValueChanged.AddListener((float t) => 
            {
                call(value);
                changed = true;
            });
            value.value = PlayerPrefs.GetFloat(key, dv);
        }

        private void SetupToggle(Toggle value, string key, bool dv, UnityAction<Toggle> call)
        {
            if (!value)
                return;

            value.onValueChanged.RemoveAllListeners();
            value.onValueChanged.AddListener((bool t) =>
            {
                call(value);
                changed = true;
            });
            value.isOn = PlayerPrefs.GetInt(key, (dv ? 1 : 0)) == 1;
        }

        private void SetupDropdown<T>(Dropdown value, string key, int dv, T[] options, UnityAction<Dropdown> call, bool fmt = false)
        {
            if (!value)
                return;

            value.options.Clear();
            for (int i = 0; i < options.Length; i++)
            {
                value.options.Add(new Dropdown.OptionData
                {
                    text = fmt 
                        ? Regex.Replace(Regex.Replace(options[i].ToString(), @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2")
                        : options[i].ToString()
                });
            }
            value.onValueChanged.RemoveAllListeners();
            value.onValueChanged.AddListener((int t) =>
            {
                call(value);
                changed = true;
            });
            value.value = PlayerPrefs.GetInt(key, dv);
        }
    }

}
