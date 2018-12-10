using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class TextFeed : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] TextFeedItem   m_itemPrefab;
        [SerializeField] Transform      m_content;
        [SerializeField] float          m_itemLifespan = 4f;

        /* Functions
        * * * * * * * * * * * * * * * */
        public bool Print(string text)
        {
            if(m_itemPrefab && m_content)
            {
                TextFeedItem item;
                if ((item = Instantiate(m_itemPrefab, m_content)))
                {
                    item.SetText(text);

                    item.rectTransform.SetAsLastSibling();

                    item.gameObject.SetActive(true);

                    Destroy(item.gameObject, m_itemLifespan);

                    return true;
                }
            }
            return false;
        }
    }

}
