using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class UnitInventory : UnitBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] Transform      m_itemRoot;
        [SerializeField] int            m_capacity = 10;

        [Header("Runtime")]
        [SerializeField] int        m_index = 0;
        [SerializeField] List<Item> m_items;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Transform itemRoot
        {
            get { return m_itemRoot; }
        }

        public int capacity
        {
            get { return m_capacity; }
        }

        public int index
        {
            get { return m_index; }
            set { m_index = Mathf.Clamp(value, 0, items.Count - 1); }
        }

        public List<Item> items
        {
            get { return m_items; }
        }
        
        public int count
        {
            get { return m_items.Count; }
        }

        public bool empty
        {
            get { return count == 0; }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void AddToBag(Item item)
        {
            if (CanAdd(item))
            {
                items.Add(item);
            }
        }

        public bool BagContains(Item item)
        {
            return item && items.Contains(item);
        }

        public bool CanAdd(Item item)
        {
            return !BagContains(item);
        }

        public Item GetFromBag(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return null;
        }

        public bool GetFromBag(int index, out Item item)
        {
            item = null;
            if ((item = GetFromBag(index)))
            {
                return true;
            }
            return false;
        }

        public bool HasRoom()
        {
            return (items.Count < m_capacity);
        }


        public bool Drop(CastingSource hand)
        {
            Item item;
            if((item = hand.item) && item.SetOwner(null))
            {
                if (BagContains(item)) items.Remove(item);

                hand.item = null;
                item.onDrop.Invoke();
                item.SetRoot(null, true);
                item.EnablePhysics(true);
                item.EnableAnimator(false);
                item.transform.position = self.vision.origin + self.vision.direction;
                return true;
            }
            return false;
        }

        public void Drop(Item item) { }

        public bool Equip(CastingSource hand, Item item)
        {
            if (HasRoom() && item && item.SetOwner(self))
            {
                if (CanAdd(item)) AddToBag(item);

                hand.item = item;
                item.SetRoot(hand.transform, false);
                item.EnablePhysics(false);
                item.EnableAnimator(true);
                item.EnableGameObject(true);
                item.onEquip.Invoke();
                return true;
            }
            return false;
        }

        public bool Equip(CastingSource hand)
        {
            return Equip(hand, hand.item);
        }

        public bool Equip(CastingSource hand, int index)
        {
            Item item;
            if(GetFromBag(index, out item))
            {
                if(hand.item && (hand.item != item))
                {
                    Store(hand);
                }
                else if(hand.item == item)
                {
                    return true;
                }

                return Equip(hand, item);
            }
            return false;
        }
        
        public bool Store(CastingSource hand)
        {
            Item item;
            if((item = hand.item))
            {
                if(Store(item))
                {
                    hand.item = null;
                    return true;
                }
            }
            return false;
        }

        public bool Store(Item item)
        {
            if (HasRoom() && item)
            {
                if (CanAdd(item)) AddToBag(item);

                item.onStore.Invoke();
                item.SetRoot(itemRoot, false);
                item.EnablePhysics(false);
                item.EnableAnimator(false);
                item.EnableGameObject(false);
                return true;
            }
            return false;
        }
    }
}
