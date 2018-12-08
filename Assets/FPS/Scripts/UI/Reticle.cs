using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class Reticle : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] HealthBar      m_fill;
        [SerializeField] Hitmarker      m_hitmarker;

        [Header("Runtime")]
        [SerializeField] Vector2    m_originalSize;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Vector2 originalSize
        {
            get { return m_originalSize; }
            private set { m_originalSize = value; }
        }

        public Vector2 sizeDelta
        {
            get { return m_fill.rectTransform.sizeDelta; }
            set { m_fill.rectTransform.sizeDelta = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                originalSize = m_fill.rectTransform.sizeDelta;
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetFill(float value)
        {
            if (m_fill)
            {
                if (value >= 0f)
                {
                    m_fill.gameObject.SetActive(true);
                    m_fill.fillAmount = value;
                }
                else
                {
                    m_fill.fillAmount = 0f;
                    m_fill.gameObject.SetActive(false);
                }
            }
        }

        public void ShowHitmaker(bool value = true)
        {
            if (m_hitmarker)
            {
                if (value)
                {
                    m_hitmarker.Show();
                }
                else
                {
                    m_hitmarker.Hide();
                }
            }
        }
    }
}
