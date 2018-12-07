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
        [SerializeField] Text           m_text;
        [SerializeField] float          m_textScalar = 2f;

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

        private void Update()
        {
            if(m_fill && m_text)
            {
                m_text.rectTransform.position =
                     m_fill.rectTransform.position -
                     new Vector3(0f,
                         originalSize.y +
                         (m_fill.rectTransform.sizeDelta.y / m_textScalar));
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

        public void SetText(string value)
        {
            if(m_text)
            {
                m_text.text = value;
            }
        }
    }
}
