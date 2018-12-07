using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class HealthBar : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        private Image m_maskImage;

        [SerializeField] Image      m_back;
        [SerializeField] Mask       m_mask;
        [SerializeField] Image      m_fill;
        [Space]
        [SerializeField] Gradient   m_backColor;
        [SerializeField] Gradient   m_fillColor;
        [Range(0f, 1f)]
        [SerializeField] float      m_fillAmount = 0.5f;
        [Range(0f, 1f)]
        [SerializeField] float      m_imageAlpha = 1f;

        /* Properties
        * * * * * * * * * * * * * * * */
        public RectTransform rectTransform
        {
            get { return transform as RectTransform; }
        }

        public float fillAmount
        {
            get { return m_fillAmount; }
            set { m_fillAmount = value; }
        }

        public float imageAlpha
        {
            get { return m_imageAlpha; }
            set { m_imageAlpha = Mathf.Clamp(value, 0f, 1f); }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if(m_back && m_mask && m_fill)
            {
                if (!m_maskImage)
                    m_maskImage = m_mask.GetComponent<Image>();

                m_mask.showMaskGraphic = false;

                m_back.color = m_backColor.Evaluate(m_fillAmount) * m_imageAlpha;

                m_fill.type = Image.Type.Filled;
                m_fill.color = m_fillColor.Evaluate(m_fillAmount) * m_imageAlpha;
                m_fill.fillAmount = m_fillAmount;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */

    }

}
