using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Spell), true)]
    public class SpellEditor : Editor
    {
        new Spell target
        {
            get { return base.target as Spell; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        private SerializedProperty m_list;
        private GUIContent m_addContent;
        private GUIContent m_removeButton;
        private AnimBool m_showElements;

        /* Functions
        * * * * * * * * * * * * * * * */
        protected virtual void OnEnable()
        {
            m_list = serializedObject.FindProperty("m_effects");
            m_addContent = new GUIContent("Add New Effect");
            m_removeButton = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_removeButton.tooltip = "Remove this effect from the list.";
            m_showElements = new AnimBool(true);
            m_showElements.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", target, typeof(Spell), false);
            GUI.enabled = true;
            
            EditorGUILayout.HelpBox(
                "Spells are the most common means of using or applying Magic Effects. " +
                "Spells include actual Spells, Powers, Perk effects, Diseases, and Poisons.",
                MessageType.Info);

            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_UID"));
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_spellType"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_castingType"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_deliveryType"));
                    GUI.enabled = target.deliveryType == DeliveryType.Aimed;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_range"));
                    GUI.enabled = target.castingType == CastingType.FireAndForget;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_chargeTime"));
                    GUI.enabled = true;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    m_showElements.target = GUILayout.Toggle(m_showElements.target, ("Effects" + (m_showElements.target ? "" : "...")), "Foldout");
                    if (EditorGUILayout.BeginFadeGroup(m_showElements.faded))
                    {
                        DisplayListEntries();

                        Rect addRect = GUILayoutUtility.GetRect(m_addContent, GUI.skin.button);
                        const float addWidth = 200f;
                        addRect.x = addRect.x + (addRect.width - addWidth) / 2;
                        addRect.width = addWidth;
                        if (GUI.Button(addRect, m_addContent))
                        {
                            AddNewEffect();
                        }
                    }
                    EditorGUILayout.EndFadeGroup();
                }
                EditorGUILayout.EndVertical();
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
