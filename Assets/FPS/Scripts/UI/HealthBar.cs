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
        [SerializeField] Image      m_back;
        [SerializeField] Mask       m_mask;
        [SerializeField] Image      m_fill;
        [Space]
        [SerializeField] Gradient   m_backColor;
        [SerializeField] Gradient   m_fillColor;
        [Range(0f, 1f)]
        [SerializeField] float      m_fillAmount = 0.5f;

        /* Properties
        * * * * * * * * * * * * * * * */
        public float fillAmount
        {
            get { return m_fillAmount; }
            set { m_fillAmount = value; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if(m_back && m_mask && m_fill)
            {
                m_mask.showMaskGraphic = false;
                m_back.color = m_backColor.Evaluate(m_fillAmount);
                m_fill.type = Image.Type.Filled;
                m_fill.color = m_fillColor.Evaluate(m_fillAmount);
                m_fill.fillAmount = m_fillAmount;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */

    }

}
