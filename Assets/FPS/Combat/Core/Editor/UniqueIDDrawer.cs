using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomPropertyDrawer(typeof(UniqueID))]
    public class UniqueIDDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, label);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(property.FindPropertyRelative("m_ID"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("m_name"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("m_sprite"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("m_description"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUI.EndProperty();
        }
    }
}
