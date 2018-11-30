using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public sealed class Hitmarker : BaseImage
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] float m_fadeSpeed = 1f;

        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            if(Application.isPlaying)
            {
                if(imageAlpha > 0f)
                {
                    imageAlpha -= m_fadeSpeed * Time.deltaTime;
                }
                else
                {
                    Hide();
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void Show()
        {
            imageAlpha = 1f;
        }

        public void Hide()
        {
            imageAlpha = 0f;
        }
    }

}
