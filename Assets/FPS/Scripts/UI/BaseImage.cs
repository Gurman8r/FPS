using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [RequireComponent(typeof(Image))]
    public abstract class BaseImage : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Image m_image;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new RectTransform transform
        {
            get { return base.transform as RectTransform; }
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
            set
            {
                if(color != value)
                {
                    SetColor(value);
                }
            }
        }

        public virtual float imageAlpha
        {
            get { return color.a; }
            set
            {
                if(color.a != value)
                {
                    SetAlpha(value);
                }
            }
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
