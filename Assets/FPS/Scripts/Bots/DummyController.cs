using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
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
        [SerializeField] bool       m_inCombat;
        [SerializeField] float      m_regenSpeed = 1f;
        [SerializeField] float      m_regenDelay =  5f;
        [SerializeField] float      m_regenTimer;
        [Space]
        [SerializeField] int        m_hitCount;
        [SerializeField] float      m_damageTotal;
        [SerializeField] float      m_damagePerSec;
        [SerializeField] float      m_damageDuration;
        [SerializeField] float      m_damageTime;
        [SerializeField] string     m_format;

        private Camera m_camera;

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
            m_camera = FindObjectOfType<Camera>();
            m_damageTotal = 0f;
            m_text.text = "";
        }
        protected override void Update()
        {
            base.Update();

            m_format =  "Total Damage:\t\t{0}\n" +
                        "DPS:\t\t\t\t\t{1}\n" +
                        "Elapsed:\t\t\t\t{2}s\n" +
                        "Hit Count:\t\t\t\t{3}\n" +
                        "In Combat:\t\t\t{4}s";

            if (Application.isPlaying)
            {
                if (m_inCombat = (m_regenTimer > 0f))
                {
                    m_regenTimer -= Time.deltaTime;
                    m_text.text = string.Format(m_format,
                        (int)m_damageTotal,
                        FormatFloat(m_damagePerSec),
                        FormatFloat(m_damageDuration),
                        m_hitCount,
                        FormatFloat(m_regenTimer));
                }
                else if (unit.health.fillAmount < 1f)
                {
                    unit.health.Modify(Time.deltaTime * m_regenSpeed);
                    m_damageTotal = 0f;
                    m_text.text = "";
                }

                if (m_health)
                {
                    m_health.fillAmount = unit.health.fillAmount;
                }

                if (m_canvas && m_camera)
                {
                    m_canvas.worldCamera = m_camera;
                    m_canvas.transform.LookAt(m_camera.transform);
                }
            }
            else
            {
                m_text.text = m_format;
            }
            
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void OnRecieveDamage(UnitEvent unitEvent)
        {
            if(m_damageTotal == 0f)
            {
                m_damageTotal = unitEvent.combat.damage.amount;
                m_damageTime = Time.time;
                m_damageDuration = 0f;
                m_damagePerSec = 0f;
                m_hitCount = 1;
            }
            else
            {
                m_damageTotal += unitEvent.combat.damage.amount;
                m_damageDuration = (Time.time - m_damageTime);
                m_damagePerSec = m_damageTotal / (m_damageDuration > 0f ? m_damageDuration : 1f);
                m_hitCount++;
            }

            m_regenTimer = m_regenDelay;
        }

        public static string FormatFloat(float value)
        {
            if(value > 0f)
            {
                if(value >= 1f)
                {
                    return value.ToString("##.#");
                }
                else
                {
                    return "0" + value.ToString("##.#");
                }
            }
            return "0";
        }
    }

}
