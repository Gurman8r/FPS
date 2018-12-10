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
