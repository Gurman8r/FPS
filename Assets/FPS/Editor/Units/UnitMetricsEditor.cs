using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(UnitMetrics))]
    [CanEditMultipleObjects]
    public class UnitMetricsEditor : Editor
    {
        new UnitMetrics target
        {
            get { return base.target as UnitMetrics; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.enabled = false;
            {
                EditorGUILayout.FloatField("Damage Dealt",      target.damageDealt);
                EditorGUILayout.FloatField("Damage Received",   target.damageRecieved);
                EditorGUILayout.FloatField("Healing Done",      target.healingDone);
                EditorGUILayout.FloatField("Healing Recieved",  target.healingRecieved);
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

        }
    }

}
