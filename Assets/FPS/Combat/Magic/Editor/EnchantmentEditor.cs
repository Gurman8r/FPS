using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Enchantment))]
    public class EnchantmentEditor : Editor
    {
        new Enchantment target
        {
            get { return base.target as Enchantment; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        private SerializedProperty m_list;
        private GUIContent m_addContent;
        private GUIContent m_removeButton;
        private AnimBool m_showEntries;


        /* Functions
        * * * * * * * * * * * * * * * */
        protected virtual void OnEnable()
        {
            m_list = serializedObject.FindProperty("m_effects");
            m_addContent = new GUIContent("Add New Effect");
            m_removeButton = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_removeButton.tooltip = "Remove this effect from the list.";
            m_showEntries = new AnimBool(true);
            m_showEntries.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", target, typeof(Enchantment), false);
            GUI.enabled = true;

            EditorGUILayout.HelpBox(
                "An Enchantment is a collection of one or more effects. " +
                "An Enchantment can be added to a weapon or piece of armor.",
                MessageType.Info);

            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_UID"));
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_castingType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_deliveryType"));
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    m_showEntries.target = GUILayout.Toggle(m_showEntries.target, ("Effects" + (m_showEntries.target ? "" : "...")), "Foldout");
                    if (EditorGUILayout.BeginFadeGroup(m_showEntries.faded))
                    {
                        DisplayListEntries();
                    }
                    EditorGUILayout.EndFadeGroup();
                }
                EditorGUILayout.EndVertical();

                Rect addRect = GUILayoutUtility.GetRect(m_addContent, GUI.skin.button);
                const float addWidth = 200f;
                addRect.x = addRect.x + (addRect.width - addWidth) / 2;
                addRect.width = addWidth;
                if (GUI.Button(addRect, m_addContent))
                {
                    AddNewEffect();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayListEntries()
        {
            int toRemove = -1;

            for (int i = 0; i < m_list.arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    SerializedProperty elem = m_list.GetArrayElementAtIndex(i);

                    if (GUILayout.Button(m_removeButton, GUIStyle.none, GUILayout.MaxWidth(16)) &&
                        (!elem.objectReferenceValue ||
                        EditorUtility.DisplayDialog(
                            "Remove Effect",
                            "Are you sure you want to remove " + elem.objectReferenceValue + "?",
                            "Yes", "No")))
                    {
                        toRemove = i;
                    }

                    EditorGUILayout.PropertyField(elem, new GUIContent(""));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (toRemove > -1)
            {
                m_list.DeleteArrayElementAtIndex(toRemove);
            }
        }

        private void AddNewEffect()
        {
            m_list.arraySize += 1;
            SerializedProperty item = m_list.GetArrayElementAtIndex(m_list.arraySize - 1);
            item.objectReferenceValue = null;
            serializedObject.ApplyModifiedProperties();
        }
    }

}
