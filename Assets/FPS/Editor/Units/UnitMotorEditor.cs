using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(UnitMotor))]
    [CanEditMultipleObjects]
    public class UnitMotorEditor : Editor
    {
        new UnitMotor target
        {
            get { return base.target as UnitMotor; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Collider", target.collider, typeof(Collider), false);
            EditorGUILayout.ObjectField("Rigidbody", target.rigidbody, typeof(Rigidbody), false);
            GUI.enabled = true;

            base.OnInspectorGUI();
        }
    }

}
