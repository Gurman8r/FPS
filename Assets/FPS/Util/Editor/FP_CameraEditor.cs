using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(FP_Camera))]
    public class FP_CameraEditor : Editor
    {
        new FP_Camera target
        {
            get { return base.target as FP_Camera; }
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
