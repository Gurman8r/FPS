using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Item))]
    public class ItemBehaviour : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Item m_item;

        /* Properties
        * * * * * * * * * * * * * * * */
        public Item item
        {
            get
            {
                if(!m_item)
                {
                    m_item = GetComponent<Item>();
                }
                return m_item;
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
    }

}
