using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [ExecuteInEditMode]
    public class ItemDatabase : MonoBehaviour
    {
        [Serializable]
        public struct Element
        {
            public string   name;   // key
            public Item     value;  // prefab
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] List<Element> m_items;

        private Dictionary<string, Item> m_database;
        

        /* Properties
        * * * * * * * * * * * * * * * */
        public static ItemDatabase instance
        {
            get; private set;
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Awake()
        {
            if(Application.isPlaying)
            {
                if (!instance)
                {
                    DontDestroyOnLoad(gameObject);
                    instance = this;
                    Reload();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void Update()
        {
            if(Application.isPlaying)
            {

            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void Reload()
        {
            m_database = new Dictionary<string, Item>();

            for(int i = 0, imax = m_items.Count; i < imax; i++)
            {
                Element e = m_items[i];
                e.name = e.value.info.name;
                if (!Register(e.name, e.value))
                {
                    Debug.LogError("Item \'" + e.name + "\' already exists", e.value);
                }
                else if(e.name == ItemInfo.DefaultName)
                {
                    Debug.LogWarning("Item Info Requires Update", e.value);
                }
            }
        }

        public bool Register(string name, Item value)
        {
            if(!m_database.ContainsKey(name))
            {
                m_database[name] = value;
                return true;
            }
            return false;
        }

        public bool Remove(string name)
        {
            if(m_database.ContainsKey(name))
            {
                m_database.Remove(name);
                return true;
            }
            return false;
        }

        public Item GetPrefab(string name)
        {
            if(m_database.ContainsKey(name))
            {
                return m_database[name];
            }
            return null;
        }

        public bool GetPrefab(string name, out Item item)
        {
            if((item = GetPrefab(name)))
            {
                return true;
            }
            item = null;
            return false;
        }
    }
}
