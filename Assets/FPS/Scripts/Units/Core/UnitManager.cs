using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [DisallowMultipleComponent]
    public class UnitManager : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [SerializeField] List<Unit> m_units;


        /* Properties
        * * * * * * * * * * * * * * * */
        public static UnitManager instance
        {
            get; set;
        }

        public List<Unit> units
        {
            get { return m_units; }
            private set { m_units = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                if (!instance)
                {
                    DontDestroyOnLoad(gameObject);
                    instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnDisable()
        {
            if(instance == this)
            {
                instance = null;
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public bool Register(Unit value)
        {
            if(value && !units.Contains(value))
            {
                value.id = units.Count;
                units.Add(value);
                return true;
            }
            return false;
        }

        public bool Unregister(Unit value)
        {
            if(value && units.Contains(value))
            {
                units.RemoveAt(value.id);
                return true;
            }
            return false;
        }

    }

}