using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class UnitInventory : UnitBehaviour
    {
        [Serializable]
        public class Hand
        {
            public Transform transform;
            public BaseItem item;

            public static implicit operator bool(Hand value)
            {
                return !object.ReferenceEquals(value, null);
            }
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] Transform  m_bagRoot;
        [SerializeField] Hand       m_hand;
        [SerializeField] int        m_capacity = 10;
        [SerializeField] int        m_index = 0;
        [Space]
        [SerializeField] List<BaseItem> m_items;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Transform bagRoot
        {
            get { return m_bagRoot; }
        }

        public Hand hand
        {
            get { return m_hand; }
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

        public List<BaseItem> items
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
        public void AddToBag(BaseItem item)
        {
            if (CanAdd(item))
            {
                items.Add(item);
            }
        }

        public bool BagContains(BaseItem item)
        {
            return item && items.Contains(item);
        }

        public bool CanAdd(BaseItem item)
        {
            return !BagContains(item);
        }

        public BaseItem GetFromBag(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return null;
        }

        public bool GetFromBag(int index, out BaseItem item)
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


        public bool Drop(Hand hand)
        {
            BaseItem item;
            if((item = hand.item) && item.SetOwner(null))
            {
                if (BagContains(item)) items.Remove(item);

                hand.item = null;
                item.onDrop.Invoke();
                item.SetRoot(null, true);
                item.EnablePhysics(true);
                item.EnableAnimator(false);
                item.transform.position = unit.vision.origin + unit.vision.direction;
                return true;
            }
            return false;
        }

        public void Drop(BaseItem item) { }

        public bool Equip(Hand hand, BaseItem item)
        {
            if (HasRoom() && item && item.SetOwner(unit))
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

        public bool Equip(Hand hand)
        {
            return Equip(hand, hand.item);
        }

        public bool Equip(Hand hand, int index)
        {
            BaseItem item;
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
        
        public bool Store(Hand hand)
        {
            BaseItem item;
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

        public bool Store(BaseItem item)
        {
            if (HasRoom() && item)
            {
                if (CanAdd(item)) AddToBag(item);

                item.onStore.Invoke();
                item.SetRoot(bagRoot, false);
                item.EnablePhysics(false);
                item.EnableAnimator(false);
                item.EnableGameObject(false);
                return true;
            }
            return false;
        }
    }
}