using System;

namespace FPS
{
    [Serializable]
    public abstract class BaseEventData
    {
        /* Variables
	    * * * * * * * * * * * * * * * */
        private bool m_used;

        /* Properties
	    * * * * * * * * * * * * * * * */
        public bool used
        {
            get { return m_used; }
        }

        /* Functions
	    * * * * * * * * * * * * * * * */
        public void Reset()
        {
            m_used = false;
        }

        public void Use()
        {
            m_used = true;
        }

        /* Operators
	    * * * * * * * * * * * * * * * */
        public static implicit operator bool(BaseEventData value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }

}
