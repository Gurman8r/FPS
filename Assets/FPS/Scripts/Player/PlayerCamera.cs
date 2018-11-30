using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public sealed class PlayerCamera : MonoBehaviour
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        private Camera m_camera;        

        [Header("Settings")]
        [SerializeField] Transform  m_parent;
        [SerializeField] Vector3    m_offset            = Vector2.zero;
        [SerializeField] Vector2    m_lookDelta         = Vector2.zero;
        [SerializeField] Vector2    m_lookSensitivity   = Vector2.one;
        [SerializeField] float      m_minHeight         = -80;
        [SerializeField] float      m_maxHeight         = 80;
        [SerializeField] bool       m_cursorLock        = false;

        //[Header("Runtime")]


        /* Properties
        * * * * * * * * * * * * * * * */
        public new Camera camera
        {
            get
            {
                if(!m_camera)
                {
                    m_camera = GetComponent<Camera>();
                }
                return m_camera;
            }
        }

        public Transform parent
        {
            get
            {
                if(!m_parent)
                {
                    m_parent = transform.parent;
                }
                return m_parent;
            }
        }

        public Vector3 offset
        {
            get { return m_offset; }
            set { m_offset = value; }
        }

        public Vector2 lookDelta
        {
            get { return m_lookDelta; }
            private set { m_lookDelta = value; }
        }

        public Vector2 lookSensitivity
        {
            get { return m_lookSensitivity; }
            set { m_lookSensitivity = value; }
        }

        public float minHeight
        {
            get { return m_minHeight; }
        }

        public float maxHeight
        {
            get { return m_maxHeight; }
        }

        public bool cursorLock
        {
            get { return m_cursorLock; }
            set { m_cursorLock = value; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(!parent)
            {
                Debug.LogError("PlayerCamera needs parent transform", this);
            }
        }

        private void Update()
        {
            transform.localPosition = offset;

            if (Application.isPlaying)
            {
                Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;

                Cursor.visible = !cursorLock;

                lookDelta = cursorLock ? lookDelta : Vector2.zero;

                transform.localRotation = Quaternion.Euler(lookDelta.y, 0, 0f);

                parent.Rotate(0, lookDelta.x, 0);
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetLookDelta(Vector2 value)
        {
            lookDelta = new Vector2(
                (value.x * lookSensitivity.x),
                Mathf.Clamp(
                    lookDelta.y - (value.y * lookSensitivity.y), 
                    minHeight, 
                    maxHeight));
        }
    }

}
