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
        [SerializeField] HealthBar  m_fill;
        [SerializeField] Hitmarker  m_hitmarker;
        [SerializeField] float      m_speed = 10f;

        [Header("Runtime")]
        [SerializeField] Vector2    m_originalSize;
        [SerializeField] Vector2    m_targetSize;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Vector2 originalSize
        {
            get { return m_originalSize; }
            private set { m_originalSize = value; }
        }

        public Vector2 targetSize
        {
            get { return m_targetSize; }
            set { m_targetSize = value; }
        }

        public Vector2 sizeDelta
        {
            get { return m_fill.rectTransform.sizeDelta; }
            private set { m_fill.rectTransform.sizeDelta = value; }
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
            if(Application.isPlaying)
            {
                if(m_speed > 0f)
                {
                    sizeDelta = Vector2.Lerp(
                        sizeDelta, 
                        targetSize, 
                        Time.deltaTime * m_speed);
                }
                else
                {
                    sizeDelta = targetSize;
                }
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
