using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public class PlayerInventory : MonoBehaviour
    {
        [Serializable]
        public class Hand
        {
            public Transform    transform;
            public Item         item;
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] Hand       m_hand;
        [SerializeField] List<Item> m_items;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Hand hand
        {
            get { return m_hand; }
        }

        public List<Item> items
        {
            get
            {
                if(m_items == null)
                {
                    m_items = new List<Item>();
                }
                return m_items;
            }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
        }

        private void Update()
        {
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void Add(Item item)
        {
            m_items.Add(item);
        }

        public bool RemoveAt(int index)
        {
            if(index >= 0 && index < items.Count)
            {
                items.RemoveAt(index);
                return true;
            }
            return false;
        }


        public bool Drop(Hand hand)
        {
            Item item;
            if((item = hand.item) && item.SetOwner(null))
            {
                hand.item = null;
                item.transform.SetParent(null, true);
                item.EnablePhysics(true);
                return true;
            }
            return false;
        }

        public bool Equip(Item item, Hand hand, Unit owner)
        {
            if (item && item.SetOwner(owner))
            {
                hand.item = item;
                item.EnablePhysics(false);
                item.transform.SetParent(hand.transform);
                item.transform.localRotation = Quaternion.identity;
                item.transform.localPosition = Vector3.zero;
                return true;
            }
            return false;
        }

        public bool Stash(Hand hand)
        {
            return false;
        }
    }
}
