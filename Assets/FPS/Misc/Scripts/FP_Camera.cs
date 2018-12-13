using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [DisallowMultipleComponent]
    public sealed class FP_Camera : CameraController
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        
        /* Properties
        * * * * * * * * * * * * * * * */
        public static FP_Camera main
        {
            get; private set;
        }

        /* Core
        * * * * * * * * * * * * * * * */
        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (!main)
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
            if (Application.isPlaying)
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
            camera.transform.localPosition = localPosition;

            camera.fieldOfView = (currentFieldOfView = (fieldOfView / zoomLevel));

            if (Application.isPlaying)
            {
                Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;

                Cursor.visible = !cursorLock;

                ApplyRotation(cursorLock ? (lookDelta * lookSpeed) : Vector2.zero);
            }
        }
    }
}
