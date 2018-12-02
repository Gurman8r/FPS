using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class InventoryUI : BaseImage
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] Transform  m_content;
        [SerializeField] Image      m_selector;
        [SerializeField] float      m_fadeSpeed = 1f;
        [SerializeField] int        m_slotCount;

        [Header("Runtime")]
        [SerializeField] ItemSlot[] m_slots;
        [SerializeField] bool       m_inUse;
        [SerializeField] Graphic[]  m_graphics;
        [SerializeField] int        m_index;

        /* Properties
        * * * * * * * * * * * * * * * */
        public bool inUse
        {
            get { return m_inUse; }
            set { m_inUse = value; }
        }
        
        public int index
        {
            get { return m_index; }
            set { m_index = value; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            imageAlpha = 1f;

            if(Application.isPlaying)
            {
                RefreshSlots();
                RefreshGraphics();
            }
        }

        private void Update()
        {
            if(Application.isPlaying)
            {
                if(!inUse)
                {
                    if(imageAlpha > 0f)
                    {
                        imageAlpha -= m_fadeSpeed * Time.deltaTime;
                    }
                    else
                    {
                        imageAlpha = 0f;
                    }
                }
                else
                {
                    imageAlpha = 1f;
                }

                if (m_selector)
                {
                    m_selector.transform.position = m_slots[m_index].transform.position;
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void RefreshItems(PlayerInventory playerInventory)
        {
            for(int i = 0, imax = m_slotCount; i < imax; i++)
            {
                Item item;
                if(playerInventory.GetFromBag(i, out item))
                {
                    m_slots[i].Refresh(item);
                }
                else
                {
                    m_slots[i].Refresh(null);
                }
            }
        }

        public void ShowForSeconds(float value)
        {
            StartCoroutine(ShowForSecondsCoroutine(value));
        }

        private IEnumerator ShowForSecondsCoroutine(float value)
        {
            inUse = true;
            yield return new WaitForSeconds(value);
            inUse = false;
        }

        private void RefreshSlots()
        {
            m_slots = m_content.GetComponentsInChildren<ItemSlot>();

            m_slotCount = m_slots.Length;
        }

        private void RefreshGraphics()
        {
            m_graphics = GetComponentsInChildren<Graphic>();
        }

        protected override void SetAlpha(float value)
        {
            foreach (Graphic g in m_graphics)
            {
                Color color = g.color;
                if(color.a != value)
                {
                    color.a = value;
                    g.color = color;
                }
            }
        }
    }

}
