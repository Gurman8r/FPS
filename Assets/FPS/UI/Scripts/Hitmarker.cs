using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public sealed class Hitmarker : BaseUI
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] float      m_fadeSpeed = 1f;
        [SerializeField] UnityEvent m_onShow;

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

            m_onShow.Invoke();
        }

        public void Hide()
        {
            imageAlpha = 0f;
        }
    }

}
