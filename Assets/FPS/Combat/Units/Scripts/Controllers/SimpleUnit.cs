using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class SimpleUnit : BaseUnitController
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] Canvas     m_canvas;
        [SerializeField] HealthBar  m_health;

        /* Properties
        * * * * * * * * * * * * * * * */

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if (Application.isPlaying && m_health)
            {
                m_health.imageAlpha = 0f;
            }
        }
        protected override void Update()
        {
            base.Update();

            if(Application.isPlaying && m_health)
            {
                m_health.fillAmount = unit.health.fillAmount;

                if (inCombat)
                {
                    m_health.imageAlpha = 1f;
                }
                else if (m_health.imageAlpha > 0f)
                {
                    m_health.imageAlpha -= Time.deltaTime;
                }

                FP_Camera cam;
                if(cam = FP_Camera.main)
                {
                    if(m_canvas)
                    {
                        m_canvas.worldCamera = cam.camera;
                        m_canvas.transform.LookAt(cam.transform);
                    }
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
