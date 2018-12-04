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
        [SerializeField] Reticle        m_reticle;
        [SerializeField] HealthBar      m_healthBar;
        [SerializeField] InventoryUI    m_inventory;
        [SerializeField] TextFeed       m_textFeed;
        [SerializeField] HealthBar      m_ammoBar;
        [SerializeField] RectTransform  m_pauseMenu;

        [Header("Runtime")]
        [SerializeField] bool m_isPaused;


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
            if(!Application.isPlaying)
            {
                SetPaused(m_isPaused);
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
            m_isPaused = (Application.isPlaying ? value : m_isPaused);

            if (pauseMenu)
            {
                if(m_isPaused)
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
    }
}
