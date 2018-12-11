using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : Editor
    {
        new ItemDatabase target
        {
            get { return base.target as ItemDatabase; }
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
            m_list = serializedObject.FindProperty("m_items");
            m_addContent = new GUIContent("Add New Item");
            m_removeButton = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_removeButton.tooltip = "Remove this item from the list.";
            m_showElements = new AnimBool(true);
            m_showElements.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                m_showElements.target = GUILayout.Toggle(m_showElements.target, ("Items" + (m_showElements.target ? "" : "...")), "Foldout");
                if (EditorGUILayout.BeginFadeGroup(m_showElements.faded))
                {
                    DisplayListEntries();

                    Rect addRect = GUILayoutUtility.GetRect(m_addContent, GUI.skin.button);
                    const float addWidth = 200f;
                    addRect.x = addRect.x + (addRect.width - addWidth) / 2;
                    addRect.width = addWidth;
                    if (GUI.Button(addRect, m_addContent))
                    {
                        AddNewItem();
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndVertical();

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
                    SerializedProperty elemName = elem.FindPropertyRelative("name");
                    SerializedProperty elemValue = elem.FindPropertyRelative("value");

                    if (GUILayout.Button(m_removeButton, GUIStyle.none, GUILayout.MaxWidth(16)) &&
                        (!elemValue.objectReferenceValue ||
                        EditorUtility.DisplayDialog(
                            "Remove Item",
                            "Are you sure you want to remove " + elemName.stringValue + "?",
                            "Yes", "No")))
                    {
                        toRemove = i;
                    }


                    Item item;
                    if(item = (elemValue.objectReferenceValue as Item))
                    {
                        GUI.enabled = false;
                        if (item.UID.ID == "") elemValue
                                .FindPropertyRelative("m_UID")
                                .FindPropertyRelative("m_ID")
                                .stringValue = item.name;
                        EditorGUILayout.LabelField(new GUIContent(item.UID.name, item.UID.ID), GUILayout.MaxWidth(96));
                    }
                    else
                    {
                        EditorGUILayout.LabelField("EMPTY", EditorStyles.boldLabel, GUILayout.MaxWidth(96));
                    }
                    GUI.enabled = true;
                    EditorGUILayout.PropertyField(elemValue, new GUIContent(""));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (toRemove > -1)
            {
                m_list.DeleteArrayElementAtIndex(toRemove);
            }
        }

        private void AddNewItem()
        {
            m_list.arraySize += 1;
            SerializedProperty item = m_list.GetArrayElementAtIndex(m_list.arraySize - 1);
            SerializedProperty itemName = item.FindPropertyRelative("name");
            SerializedProperty itemValue = item.FindPropertyRelative("value");
            itemName.stringValue = "New Item";
            itemValue.objectReferenceValue = null;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
