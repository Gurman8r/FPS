using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ML
{
    [RequireComponent(typeof(Canvas))]
    [ExecuteInEditMode]
    public sealed class PlayerHUD : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Canvas m_canvas;

        [SerializeField] Reticle    m_reticle;
        [SerializeField] Text       m_info;
        [SerializeField] HealthBar  m_healthBar;
        [Space]
        [SerializeField] Hitmarker  m_hitmarkerPrefab;


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


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                ShowReticle(true);
                SetInfoText("");
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void ShowReticle(bool value)
        {
            m_reticle.image.enabled = value;
        }

        public void SetReticlePos(Vector2 value, float speed = 0f)
        {
            if(speed <= 0f)
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

        public void SetInfoText(string value)
        {
            m_info.text = value;
        }

        public Hitmarker CreateHitmarker(Vector2 position, float lifeSpan)
        {
            Hitmarker hit;
            if((hit = Instantiate(m_hitmarkerPrefab)))
            {
                hit.gameObject.SetActive(true);
                hit.transform.SetParent(transform);
                hit.lifeSpan = 0.5f;
                hit.Spawn();
            }
            return null;
        }

        public void SetHealth(float value)
        {
            if(m_healthBar)
            {
                m_healthBar.fillAmount = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }
}
