using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class Reticle : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] HealthBar  m_fill;
        [SerializeField] Hitmarker  m_hitmarker;
        [SerializeField] Text       m_text;

        [Header("Runtime")]
        [SerializeField] Vector2    m_originalSize;


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            originalSize = m_fill.rectTransform.sizeDelta;
        }


        /* Properties
        * * * * * * * * * * * * * * * */
        public Vector2 originalSize
        {
            get { return m_originalSize; }
            private set { m_originalSize = value; }
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

        public void SetText(string value)
        {
            if(m_text)
            {
                m_text.text = value;
            }
        }

        public void ResetSize()
        {
            SetSize(originalSize);
        }

        public void SetSize(Vector2 value)
        {
            m_fill.rectTransform.sizeDelta = value;
        }
    }
}
