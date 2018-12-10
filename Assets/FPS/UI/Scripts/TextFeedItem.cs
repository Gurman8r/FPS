using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class TextFeedItem : BaseUI
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] Text m_text;

        /* Properties
        * * * * * * * * * * * * * * * */
        public Text text
        {
            get { return m_text; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Update()
        {
            Vector2Int size = new Vector2Int(
                (int)text.rectTransform.sizeDelta.x,
                (int)text.rectTransform.sizeDelta.y);

            if (text.rectTransform.sizeDelta != size)
            {
                text.rectTransform.sizeDelta = size;
            }

            if(rectTransform.sizeDelta != size)
            {
                rectTransform.sizeDelta = size;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetText(string value)
        {
            if(text)
            {
                text.text = value;
            }
        }
    }

}
