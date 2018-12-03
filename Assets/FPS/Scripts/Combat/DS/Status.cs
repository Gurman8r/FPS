using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class Status
    {
        public enum Effect
        {
            None = 0,
        }

        [SerializeField] Effect m_effect = Effect.None;
        [SerializeField] bool   m_canMove = true;
        [SerializeField] bool   m_canJump = true;

        public Effect effect
        {
            get { return m_effect; }
            private set { m_effect = value; }
        }

        public bool canMove
        {
            get { return m_canMove; }
            private set { m_canMove = value; }
        }

        public bool canJump
        {
            get { return m_canJump; }
            private set { m_canJump = value; }
        }
    }

}
