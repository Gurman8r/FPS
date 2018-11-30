using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ML
{
    [RequireComponent(typeof(Image))]
    public abstract class BaseElement : MonoBehaviour
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
    }

}
