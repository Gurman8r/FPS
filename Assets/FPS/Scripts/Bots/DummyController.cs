using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class DummyController : BotController
        , IDamageTarget
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Dummy Settings")]
        [SerializeField] Canvas     m_canvas;
        [SerializeField] Text       m_text;
        [SerializeField] HealthBar  m_health;
        [Space]
        [SerializeField] float      m_regen = 1f;
        [SerializeField] float      m_regenDelay =  5f;
        [SerializeField] bool       m_outOfCombat;
        [SerializeField] float      m_combatTimer;
        [SerializeField] float      m_damageTaken;
        [SerializeField] float      m_lastTime;
        [SerializeField] float      m_dps;

        private Camera m_camera;

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
            m_camera = FindObjectOfType<Camera>();
            m_damageTaken = 0f;
            m_text.text = "";
        }
        protected override void Update()
        {
            base.Update();

            if (m_health)
            {
                m_health.fillAmount = unit.health.fillAmount;
            }

            if (m_canvas)
            {
                m_canvas.worldCamera = m_camera;

                if (m_camera)
                {
                    m_canvas.transform.LookAt(m_camera.transform);
                }
            }

            if (Application.isPlaying)
            {
                if(!(m_outOfCombat = (m_combatTimer <= 0f)))
                {
                    m_combatTimer -= Time.deltaTime;
                }
                else if (unit.health.fillAmount < 1f)
                {
                    unit.health.Modify(Time.deltaTime * m_regen);
                    m_damageTaken = 0f;
                    m_text.text = "";
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void OnRecieveDamage(UnitEvent unitEvent)
        {
            if(m_damageTaken == 0f)
            {
                m_damageTaken = unitEvent.combat.damage.amount;
                m_lastTime = Time.time;
                m_dps = 0f;
            }
            else
            {
                m_damageTaken += unitEvent.combat.damage.amount;
                m_dps = m_damageTaken / (Time.time - m_lastTime);
            }

            m_text.text = string.Format(
                "Damage: {0}\n" +
                "Time: {1}s\n" +
                "DPS: {2}\n",
                (int)m_damageTaken,
                (Time.time - m_lastTime).ToString("##.##"),
                (int)m_dps);

            m_combatTimer = m_regenDelay;
        }
    }

}
