using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [ExecuteInEditMode]
    public class ItemManager : MonoBehaviour
    {
        [Serializable]
        public struct Element
        {
            public string   name;   // key
            public BaseItem     value;  // prefab
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] List<Element> m_items;

        private Dictionary<string, BaseItem> m_database;
        

        /* Properties
        * * * * * * * * * * * * * * * */
        public static ItemManager instance
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
            m_database = new Dictionary<string, BaseItem>();

            for(int i = 0, imax = m_items.Count; i < imax; i++)
            {
                Element e = m_items[i];
                if(e.value)
                {
                    e.name = e.value.info.name;
                    if (!Register(e.name, e.value))
                    {
                        Debug.LogError("Item \'" + e.name + "\' already exists: " + e.value.name);
                    }
                }
                else
                {
                    Debug.LogWarning("Item Prefab Not Found: " + e.name);
                }
            }
        }

        public bool Register(string name, BaseItem value)
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

        public BaseItem GetPrefab(string name)
        {
            if(m_database.ContainsKey(name))
            {
                return m_database[name];
            }
            return null;
        }

        public bool GetPrefab(string name, out BaseItem item)
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
