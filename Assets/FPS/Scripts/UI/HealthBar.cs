using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class HealthBar : MonoBehaviour
    {
        private static readonly Gradient DefaultBackGradient = new Gradient()
        {
            alphaKeys = new GradientAlphaKey[2]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            },
            colorKeys = new GradientColorKey[2]
            {
                new GradientColorKey(Color.gray, 0f),
                new GradientColorKey(Color.gray, 1f)
            }
        };
        private static readonly Gradient DefaultFillGradient = new Gradient()
        {
            alphaKeys = new GradientAlphaKey[2]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            },
            colorKeys = new GradientColorKey[2]
            {
                new GradientColorKey(Color.red, 0f),
                new GradientColorKey(Color.green, 1f)
            }
        };

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] Image      m_back;
        [SerializeField] Mask       m_mask;
        [SerializeField] Image      m_fill;
        [Space]
        [SerializeField] Gradient   m_backColor = DefaultBackGradient;
        [SerializeField] Gradient   m_fillColor = DefaultFillGradient;
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
        private void Start()
        {
        }

        private void Update()
        {
            if(m_back && m_mask && m_fill)
            {
                m_mask.showMaskGraphic = false;

                m_back.color = m_backColor.Evaluate(m_fillAmount);

                m_fill.color = m_fillColor.Evaluate(m_fillAmount);

                m_fill.fillAmount = m_fillAmount;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
