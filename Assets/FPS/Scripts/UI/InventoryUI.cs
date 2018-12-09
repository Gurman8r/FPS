using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class InventoryUI : BaseUI
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] Transform  m_content;
        [SerializeField] ItemSlot   m_slotPrefab;
        [SerializeField] int        m_slotCount = 10;
        [SerializeField] Image      m_selector;
        [SerializeField] float      m_fadeSpeed = 1f;
        [SerializeField] float      m_showTime = 2f;

        [Header("Runtime")]
        [SerializeField] ItemSlot[] m_slots = null;
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
            if(Application.isPlaying)
            {
                if(m_content)
                {
                    for (int i = (m_content.childCount - 1); i >= 0; i--)
                    {
                        Destroy(m_content.GetChild(i));
                    }

                    if (m_slotPrefab && (m_slotCount > 0))
                    {
                        m_slots = new ItemSlot[m_slotCount];

                        for (int i = 0, imax = m_slotCount; i < imax; i++)
                        {
                            m_slots[i] = Instantiate(m_slotPrefab, m_content);
                        }
                    }
                }

                m_graphics = GetComponentsInChildren<Graphic>();

                imageAlpha = 1f;
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

                if (m_selector && (index >= 0))
                {
                    ItemSlot slot;
                    if (slot = m_slots[index])
                    {
                        m_selector.transform.position = m_slots[index].rectTransform.position;
                    }
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void Refresh(UnitInventory inventory)
        {
            index = (inventory.count > 0)
                ? inventory.index
                : -1;

            for (int i = 0, imax = m_slots.Length; i < imax; i++)
            {
                BaseItem item;
                if(inventory.GetFromBag(i, out item))
                {
                    m_slots[i].gameObject.SetActive(true);
                    m_slots[i].Refresh(item);
                }
                else if(m_slots[i].gameObject.activeInHierarchy)
                {
                    m_slots[i].Refresh(null);
                    m_slots[i].gameObject.SetActive(false);
                }
            }
        }

        public void Show()
        {
            StartCoroutine(ShowForSecondsCoroutine(m_showTime));
        }

        private IEnumerator ShowForSecondsCoroutine(float value)
        {
            inUse = true;
            yield return new WaitForSeconds(value);
            inUse = false;
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
