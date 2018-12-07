using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] RectTransform  m_menu;
        [SerializeField] Image          m_damage;
        [SerializeField] Text           m_controls;
        [SerializeField] TextAsset      m_controlsFile;
        [SerializeField] float          m_damageAlpha = 0.25f;
        [SerializeField] float          m_damageFade = 1f;
        [Space]
        [SerializeField] UnityEvent     m_onShowMenu;
        [SerializeField] UnityEvent     m_onHideMenu;

        [Header("Runtime")]
        [SerializeField] bool           m_showMenu;
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

        public RectTransform menu
        {
            get { return m_menu; }
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

            if (m_controls && m_controlsFile)
            {
                m_controls.text = m_controlsFile.text;
            }

            if (!Application.isPlaying)
            {
                ShowMenu(m_showMenu);
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

        public void ShowMenu(bool value)
        {
            m_showMenu = (Application.isPlaying ? value : m_showMenu);
            if (menu)
            {
                if(m_showMenu)
                {
                    menu.gameObject.SetActive(true);
                    m_onShowMenu.Invoke();
                }
                else if(menu.gameObject.activeInHierarchy)
                {
                    m_onHideMenu.Invoke();
                    menu.gameObject.SetActive(false);
                }
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
