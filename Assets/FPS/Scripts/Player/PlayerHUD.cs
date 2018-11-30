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

        [SerializeField] Reticle        m_reticle;
        [SerializeField] Text           m_info;
        [SerializeField] HealthBar      m_healthBar;
        [SerializeField] Hitmarker      m_hitmarker;
        [SerializeField] InventoryUI    m_inventory;
        [SerializeField] TextFeed       m_textFeed;


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

        public Vector2 size
        {
            get { return new Vector2(Screen.width, Screen.height); }
        }

        public Vector2 center
        {
            get { return size / 2f; }
        }

        public InventoryUI inventory
        {
            get { return m_inventory; }
        }

        public TextFeed textFeed
        {
            get { return m_textFeed; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                SetInfoText("");
                ShowReticle(true);
                ShowHitmaker(false);
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetHealth(float value)
        {
            if(m_healthBar)
            {
                m_healthBar.fillAmount = Mathf.Clamp(value, 0f, 1f);
            }
        }

        public void SetInfoText(string value)
        {
            m_info.text = value;
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

        public void ShowHitmaker(bool value = true)
        {
            if(m_hitmarker)
            {
                if(value)
                {
                    m_hitmarker.Show();
                }
                else
                {
                    m_hitmarker.Hide();
                }
            }
        }

        public void ShowReticle(bool value)
        {
            m_reticle.image.enabled = value;
        }
    }
}
