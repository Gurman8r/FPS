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

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Functions
        * * * * * * * * * * * * * * * */
        private void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Behaviour", EditorStyles.boldLabel);
                BehaviourToggle("Combat", target.inventory);
                BehaviourToggle("Motor", target.motor);
                BehaviourToggle("Inventory", target.inventory);
                BehaviourToggle("Triggers", target.triggers);
                BehaviourToggle("Vision", target.vision);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_health"));
            
            serializedObject.ApplyModifiedProperties();
        }

        private void BehaviourToggle<T>(string label, T scr, float maxWidth = 64f) where T : MonoBehaviour
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.MaxWidth(maxWidth));

                GUI.enabled = false;
                EditorGUILayout.ObjectField(scr, typeof(T), false);
                GUI.enabled = true;

                scr.enabled = GUILayout.Toggle(
                    scr.enabled,
                    scr.enabled ? "Enabled " : "Disabled", 
                    "Button");
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
