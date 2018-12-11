using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomPropertyDrawer(typeof(Health))]
    public class HealthDrawer : PropertyDrawer
    {
        /* Variables
        * * * * * * * * * * * * * * * */

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                SerializedProperty current = property.FindPropertyRelative("m_current");
                SerializedProperty minimum = property.FindPropertyRelative("m_minimum");
                SerializedProperty maximum = property.FindPropertyRelative("m_maximum");

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
                    EditorGUI.ProgressBar(
                        EditorGUILayout.GetControlRect(),
                        (current.floatValue / maximum.floatValue),
                        string.Format("({0}/{1})", current.floatValue, maximum.floatValue));

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Min/Max", GUILayout.MaxWidth(64));

                            float min = EditorGUILayout.FloatField(minimum.floatValue);
                            minimum.floatValue = min >= 0f ? min : 0f;

                            float max = EditorGUILayout.FloatField(maximum.floatValue);
                            maximum.floatValue = max > min ? max : min;
                        }
                        EditorGUILayout.EndHorizontal();

                        current.floatValue = EditorGUILayout.Slider(
                            new GUIContent("Current"),
                            current.floatValue,
                            minimum.floatValue,
                            maximum.floatValue);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndProperty();
        }

        private void HealthEditor(Health value)
        {
            
        }
    }

}
