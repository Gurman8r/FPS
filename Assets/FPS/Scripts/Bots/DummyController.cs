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
        [SerializeField] float      m_regenSpeed = 20f;
        [SerializeField] float      m_fadeSpeed = 1f;
        [SerializeField] string     m_format;
        [SerializeField] int        m_hitCount;
        [SerializeField] float      m_damageTotal;
        [SerializeField] float      m_damagePerSec;
        [SerializeField] float      m_damageDuration;
        [SerializeField] float      m_damageTime;

        private Camera m_camera;

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
            
            if(Application.isPlaying)
            {
                m_camera = FindObjectOfType<Camera>();
                m_damageTotal = 0f;
                m_text.text = "";
                m_health.imageAlpha = 0f;
            }
        }

        protected override void Update()
        {
            base.Update();

            m_format =  "Damage:\t\t\t\t{0}\n" +
                        "Elapsed:\t\t\t\t{2}s\n" +
                        "DPS:\t\t\t\t\t{1}\n" +
                        "Hit Count:\t\t\t\t{3}\n" +
                        "In Combat:\t\t\t{4}s";

            if (Application.isPlaying)
            {
                Item item;
                if(item = unit.inventory.primary.item)
                {
                    item.UpdatePrimary(fire0);
                    item.UpdateSecondary(fire1);
                }

                if (inCombat)
                {
                    m_text.text = string.Format(m_format,
                        (int)m_damageTotal,
                        FormatFloat(m_damagePerSec),
                        FormatFloat(m_damageDuration),
                        m_hitCount,
                        FormatFloat(combatTimer));

                    m_health.imageAlpha = 1f;

                    Color c = m_text.color;
                    c.a = 1f;
                    m_text.color = c;
                }
                else
                {
                    Color c;
                    if((c = m_text.color).a > 0f)
                    {
                        c.a -= Time.deltaTime * m_fadeSpeed;
                        m_text.color = c;
                    }
                    else
                    {
                        m_text.text = "";
                    }

                    if (unit.health.fillAmount < 1f)
                    {
                        unit.triggers.Broadcast(EventType.OnDoHealing, new UnitEvent
                        {
                            data = new ObjectData
                            {
                                owner   = unit,
                                target  = unit,
                                healing = new Healing
                                {
                                    amount = m_regenSpeed * Time.deltaTime
                                }
                            }
                        });

                        m_damageTotal = 0f;
                    }
                    else if (m_health.imageAlpha > 0f)
                    {
                        m_health.imageAlpha -= Time.deltaTime * m_fadeSpeed;
                    }
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
                m_damageTotal = unitEvent.data.damage.amount;
                m_damageTime = Time.time;
                m_damageDuration = 0f;
                m_damagePerSec = m_damageTotal;
                m_hitCount = 1;
            }
            else
            {
                m_damageTotal += unitEvent.data.damage.amount;
                m_damageDuration = (Time.time - m_damageTime);
                m_damagePerSec = m_damageTotal / (m_damageDuration > 0f ? m_damageDuration : 1f);
                m_hitCount++;
            }
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
