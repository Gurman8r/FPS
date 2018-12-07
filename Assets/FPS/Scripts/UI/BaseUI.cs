using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [RequireComponent(typeof(Image))]
    public abstract class BaseUI : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Image m_image;


        /* Properties
        * * * * * * * * * * * * * * * */
        public RectTransform rectTransform
        {
            get { return transform as RectTransform; }
        }

        public Image image
        {
            get
            {
                if (!m_image)
                {
                    m_image = GetComponent<Image>();
                }
                return m_image;
            }
        }

        public Color color
        {
            get { return image.color; }
            set { SetColor(value); }
        }

        public virtual float imageAlpha
        {
            get { return color.a; }
            set { SetAlpha(value); }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        protected virtual void SetAlpha(float value)
        {
            Color c = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                value);
            SetColor(c);
        }

        protected virtual void SetColor(Color color)
        {
            image.color = color;
        }
    }

}
