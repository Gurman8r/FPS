using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(CameraController), true)]
    public class CameraControllerEditor : Editor
    {
        new CameraController target
        {
            get { return base.target as CameraController; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Camera", target.camera, typeof(Camera), false);
            GUI.enabled = true;

            base.OnInspectorGUI();
        }
    }
}
