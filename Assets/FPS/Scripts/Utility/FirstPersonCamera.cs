using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace FPS
{
    [RequireComponent(typeof(PostProcessingBehaviour))]
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public sealed class FirstPersonCamera : MonoBehaviour
    {
        public const float MinZoom  = 1f;
        public const float MaxZoom  = 10f;
        public const float MinFOV   = 0.00001f;
        public const float MaxFOV   = 179f;

        /* Variables
        * * * * * * * * * * * * * * * */
        private PostProcessingBehaviour m_postProcessing;
        private Camera  m_camera;
        private Ray     m_ray = new Ray();

        [Header("Settings")]
        [SerializeField] Transform  m_parent;
        [SerializeField] Vector3    m_offset            = Vector2.zero;
        [SerializeField] Vector2    m_lookDelta         = Vector2.zero;
        [SerializeField] Vector2    m_lookSensitivity   = Vector2.one;
        [SerializeField] float      m_lookSpeed         = 1f;
        [SerializeField] float      m_minHeight         = -80;
        [SerializeField] float      m_maxHeight         = 80;
        [SerializeField] bool       m_cursorLock        = false;
        [Range(MinFOV, MaxFOV)]
        [SerializeField] float      m_fieldOfView       = 60f;
        [Range(MinZoom, MaxZoom)]
        [SerializeField] float      m_zoomLevel         = 1f;

        [Header("Runtime")]
        [SerializeField] float m_currentFieldOfView;

        /* Properties
        * * * * * * * * * * * * * * * */
        public static FirstPersonCamera main
        {
            get; private set;
        }

        public PostProcessingBehaviour postProcessing
        {
            get
            {
                if(!m_postProcessing)
                {
                    m_postProcessing = GetComponent<PostProcessingBehaviour>();
                }
                return m_postProcessing;
            }
        }

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

        public Ray ray
        {
            get
            {
                m_ray.origin = camera.transform.position;
                m_ray.direction = camera.transform.forward;
                return m_ray;
            }
        }

        public Transform parent
        {
            get
            {
                if(!m_parent)
                {
                    m_parent = camera.transform.parent;
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

        public float lookSpeed
        {
            get { return m_lookSpeed; }
            set { m_lookSpeed = value; }
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

        public float fieldOfView
        {
            get { return m_fieldOfView; }
            set { m_fieldOfView = Mathf.Clamp(value, MinFOV, MaxFOV); }
        }

        public float zoomLevel
        {
            get { return m_zoomLevel; }
            set { m_zoomLevel = Mathf.Clamp(value, MinZoom, MaxZoom); }
        }

        


        /* Core
        * * * * * * * * * * * * * * * */
        private void Awake()
        {
            if(Application.isPlaying)
            {
                if(!main)
                {
                    main = this;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void Start()
        {
            if(Application.isPlaying)
            {
                if (!parent)
                {
                    Debug.LogError("PlayerCamera Parent not set");
                    return;
                }
            }
        }

        private void Update()
        {
            camera.transform.localPosition = offset;

            camera.fieldOfView = (m_currentFieldOfView = (fieldOfView / zoomLevel));

            if (Application.isPlaying)
            {
                Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;

                Cursor.visible = !cursorLock;

                ApplyRotation(cursorLock ? (lookDelta * lookSpeed) : Vector2.zero);
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

        private void ApplyRotation(Vector2 value)
        {
            camera.transform.localRotation = Quaternion.Euler(value.y, 0f, 0f);

            parent.Rotate(0f, value.x, 0f);
        }
    }

}
