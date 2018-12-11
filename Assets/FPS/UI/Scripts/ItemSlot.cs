using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class ItemSlot : BaseUI
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] Image  m_icon;

        [Header("Runtime")]
        [SerializeField] Sprite m_sprite;
        [SerializeField] Item   m_item;

        /* Core
        * * * * * * * * * * * * * * * */
        public Image icon
        {
            get { return m_icon; }
        }

        public Sprite sprite
        {
            get { return m_sprite; }
            set { m_sprite = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
        }

        private void Update()
        {
            if(m_icon)
            {
                m_icon.enabled = m_sprite;
                m_icon.sprite = m_sprite;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void Refresh(Item item)
        {
            if(item)
            {
                m_item = item;
                m_sprite = m_item.UID.sprite;
            }
            else
            {
                m_item = null;
                m_sprite = null;
            }
        }
    }

}
