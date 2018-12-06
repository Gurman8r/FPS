using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Unit))]
    [CanEditMultipleObjects]
    public class UnitEditor : Editor
    {
        new Unit target
        {
            get { return base.target as Unit; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Triggers", target.triggers, typeof(UnitTriggers), false);
            EditorGUILayout.ObjectField("Metrics", target.metrics, typeof(UnitMetrics), false);
            EditorGUILayout.ObjectField("Motor", target.motor, typeof(UnitMotor), false);
            EditorGUILayout.ObjectField("Audio", target.audio, typeof(UnitAudio), false);
            EditorGUILayout.ObjectField("Vision", target.vision, typeof(UnitVision), false);
            EditorGUILayout.ObjectField("Inventory", target.inventory, typeof(UnitInventory), false);
            GUI.enabled = true;

            base.OnInspectorGUI();
        }
    }

}
