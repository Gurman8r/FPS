using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class ItemDatabase : MonoBehaviour
    {
        [Serializable]
        public struct Element
        {
            public string   name;
            public Item     value;
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
                    ReloadPrefabs();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void ReloadPrefabs()
        {
            m_database = new Dictionary<string, Item>();

            for(int i = 0, imax = m_items.Count; i < imax; i++)
            {
                Element e = m_items[i];
                if(e.value)
                {
                    if (!RegisterPrefab(e.name, e.value))
                    {
                        Debug.LogError("Duplicate Item Found: \'" + e.name + "\'");
                    }
                }
                else
                {
                    Debug.LogError("Item Prefab Not Found: " + e.name);
                }
            }
        }

        public bool RegisterPrefab(string name, Item value)
        {
            if(!m_database.ContainsKey(name))
            {
                m_database[name] = value;
                return true;
            }
            return false;
        }

        public bool RemovePrefab(string name)
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
