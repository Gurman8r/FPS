using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [RequireComponent(typeof(Canvas))]
    [ExecuteInEditMode]
    public sealed class PlayerHUD : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Canvas m_canvas;

        [Header("Settings")]
        [SerializeField] TextFeed       m_textFeed;
        [SerializeField] Reticle        m_reticle;
        [SerializeField] InventoryUI    m_inventory;
        [SerializeField] HealthBar      m_healthBar;
        [SerializeField] HealthBar      m_ammoBar;
        [SerializeField] RectTransform  m_pauseMenu;
        [Space]
        [SerializeField] Image          m_damage;
        [SerializeField] float          m_damageAlpha = 0.25f;
        [SerializeField] float          m_damageFade = 1f;

        [Header("Runtime")]
        [SerializeField] bool           m_showPause;
        [SerializeField] float          m_damageTimer = 0f;



        /* Properties
        * * * * * * * * * * * * * * * */
        public Canvas canvas
        {
            get
            {
                if(!m_canvas)
                {
                    m_canvas = GetComponent<Canvas>();
                }
                return m_canvas;
            }
        }

        public Reticle reticle
        {
            get { return m_reticle; }
        }

        public InventoryUI inventory
        {
            get { return m_inventory; }
        }

        public TextFeed textFeed
        {
            get { return m_textFeed; }
        }

        public HealthBar healthBar
        {
            get { return m_healthBar; }
        }

        public HealthBar ammoBar
        {
            get { return m_ammoBar; }
        }

        public RectTransform pauseMenu
        {
            get { return m_pauseMenu; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                reticle.SetText("");
                reticle.SetFill(0f);
                reticle.ShowHitmaker(false);
            }
        }

        private void Update()
        {
            if (m_damage)
            {
                if(Application.isPlaying)
                {
                    if (m_damageTimer > 0f)
                    {
                        m_damageTimer -= Time.deltaTime * m_damageFade;
                    }
                }

                Color c = m_damage.color;
                c.a = m_damageTimer;
                m_damage.color = c;
            }

            if (!Application.isPlaying)
            {
                SetPaused(m_showPause);
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetAmmo(float value)
        {
            if (ammoBar)
            {
                if (value >= 0f)
                {
                    ammoBar.gameObject.SetActive(true);
                    ammoBar.fillAmount = value;
                }
                else
                {
                    ammoBar.fillAmount = 0f;
                    ammoBar.gameObject.SetActive(false);
                }
            }
        }

        public void SetHealth(float value)
        {
            if(m_healthBar)
            {
                m_healthBar.fillAmount = Mathf.Clamp(value, 0f, 1f);
            }
        }

        public void SetPaused(bool value)
        {
            m_showPause = (Application.isPlaying ? value : m_showPause);

            if (pauseMenu)
            {
                if(m_showPause)
                {
                    pauseMenu.gameObject.SetActive(true);
                }
                else if(pauseMenu.gameObject.activeInHierarchy)
                {
                    pauseMenu.gameObject.SetActive(false);
                }
            }
        }

        public void SetReticlePos(Vector2 value, float speed = 0f)
        {
            if (speed <= 0f)
            {
                m_reticle.transform.position = value;
            }
            else
            {
                m_reticle.transform.position = Vector3.Lerp(
                    m_reticle.transform.position,
                    value,
                    speed);
            }
        }

        public void ShowTakeDamage()
        {
            if(m_damage)
            {
                m_damageTimer = m_damageAlpha;
            }
        }


        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
