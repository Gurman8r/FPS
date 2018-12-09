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
            EditorGUILayout.Space();

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Triggers", target.triggers, typeof(UnitTriggers), false);
            EditorGUILayout.ObjectField("Motor", target.motor, typeof(UnitMotor), false);
            EditorGUILayout.ObjectField("Inventory", target.inventory, typeof(UnitInventory), false);
            EditorGUILayout.ObjectField("Vision", target.vision, typeof(UnitVision), false);
            GUI.enabled = true;

            base.OnInspectorGUI();

            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), target.health.fillAmount, 
                string.Format("Health ({0}/{1})", 
                    target.health.current, 
                    target.health.maximum));
        }
    }

}
