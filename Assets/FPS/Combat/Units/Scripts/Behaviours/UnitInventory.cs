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
        [SerializeField] Transform  m_itemRoot;
        [SerializeField] int        m_capacity = 10;

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


        public bool Drop(CastingSource source)
        {
            Item item;
            if((item = source.item) && item.SetOwner(null))
            {
                if (BagContains(item)) items.Remove(item);

                source.item = null;
                item.OnDrop(null);
                transform.position = self.vision.origin + self.vision.direction;
                return true;
            }
            return false;
        }

        public bool Equip(CastingSource source, Item item)
        {
            if (HasRoom() && item && item.SetOwner(self))
            {
                if (CanAdd(item)) AddToBag(item);
                source.item = item;
                item.OnEquip(source.transform);
                return true;
            }
            return false;
        }

        public bool Equip(CastingSource source)
        {
            return Equip(source, source.item);
        }

        public bool Equip(CastingSource source, int index)
        {
            Item item;
            if(GetFromBag(index, out item))
            {
                if(source.item && (source.item != item))
                {
                    Store(source);
                }
                else if(source.item == item)
                {
                    return true;
                }
                return Equip(source, item);
            }
            return false;
        }
        
        public bool Store(CastingSource source)
        {
            Item item;
            if((item = source.item))
            {
                if(Store(item))
                {
                    source.item = null;
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
                item.OnStore(itemRoot);
                return true;
            }
            return false;
        }
    }
}
