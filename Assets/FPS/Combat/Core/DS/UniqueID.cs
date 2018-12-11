using System;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public struct UniqueID
    {
        [SerializeField] string m_ID;
        [SerializeField] string m_name;
        [SerializeField] Sprite m_sprite;
        [TextArea]
        [SerializeField] string m_description;


        public string ID
        {
            get { return m_ID; }
        }

        public string name
        {
            get { return m_name; }
        }

        public string description
        {
            get { return m_description; }
        }

        public Sprite sprite
        {
            get { return m_sprite; }
        }
    }
}
