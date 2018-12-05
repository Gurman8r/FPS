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
            public Transform    transform;
            public Item         item;

            public static implicit operator bool(Hand value)
            {
                return !object.ReferenceEquals(value, null);
            }
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] Hand       m_primary;
        [SerializeField] int        m_capacity = 10;
        [SerializeField] List<Item> m_bagList;
        [SerializeField] Transform  m_bagTransform;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Hand primary
        {
            get { return m_primary; }
        }

        public int capacity
        {
            get { return m_capacity; }
        }

        public List<Item> bagList
        {
            get { return m_bagList; }
        }

        public Transform bagTransform
        {
            get { return m_bagTransform; }
        }

        public int count
        {
            get { return m_bagList.Count; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {

            }
        }

        private void Update()
        {
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void AddToBag(Item item)
        {
            if (CanAdd(item))
            {
                bagList.Add(item);
            }
        }

        public bool BagContains(Item item)
        {
            return item && bagList.Contains(item);
        }

        public bool CanAdd(Item item)
        {
            return !BagContains(item);
        }

        public Item GetFromBag(int index)
        {
            if (index >= 0 && index < bagList.Count)
            {
                return bagList[index];
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
            return (bagList.Count < m_capacity);
        }



        public bool Drop(Hand hand)
        {
            Item item;
            if((item = hand.item) && item.SetOwner(null))
            {
                if (BagContains(item)) bagList.Remove(item);

                hand.item = null;
                item.OnDrop();
                item.Reparent(null, true);
                item.EnablePhysics(true);
                item.EnableAnimator(false);
                return true;
            }
            return false;
        }


        public bool Equip(Hand hand, Item item)
        {
            if (HasRoom() && item && item.SetOwner(unit))
            {
                if (CanAdd(item)) AddToBag(item);

                hand.item = item;
                item.Reparent(hand.transform, false);
                item.EnablePhysics(false);
                item.EnableAnimator(true);
                item.EnableGameObject(true);
                item.OnEquip();
                return true;
            }
            return false;
        }

        public bool Equip(Hand hand, int index)
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
        

        public bool Store(Hand hand)
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

                item.OnStore();
                item.Reparent(bagTransform, false);
                item.EnablePhysics(false);
                item.EnableAnimator(false);
                item.EnableGameObject(false);
                return true;
            }
            return false;
        }
    }
}
