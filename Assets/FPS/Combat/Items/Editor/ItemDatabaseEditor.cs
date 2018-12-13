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
        private AnimBool m_showList;

        /* Functions
        * * * * * * * * * * * * * * * */
        protected virtual void OnEnable()
        {
            m_list = serializedObject.FindProperty("m_items");
            m_addContent = new GUIContent("Add New Item");
            m_removeButton = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_removeButton.tooltip = "Remove this item from the list.";
            m_showList = new AnimBool(true);
            m_showList.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                ShowDatabase();

                if (GUILayout.Button("Validate"))
                {
                    target.ReloadPrefabs();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowDatabase()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                m_showList.target = GUILayout.Toggle(m_showList.target, ("Items" + (m_showList.target ? "" : "...")), "Foldout");
                if (EditorGUILayout.BeginFadeGroup(m_showList.faded))
                {
                    ShowListEntries();

                    if (ShowAddButton())
                    {
                        AddNewItem();
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndVertical();
        }

        private void ShowListEntries()
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


                    Item obj;
                    if (obj = (elemValue.objectReferenceValue as Item))
                    {
                        EditorGUILayout.LabelField((elemName.stringValue = obj.UID.ID));
                    }
                    else
                    {
                        EditorGUILayout.LabelField("EMPTY", EditorStyles.boldLabel, GUILayout.MaxWidth(96));
                    }
                    EditorGUILayout.PropertyField(elemValue, new GUIContent(""));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (toRemove > -1)
            {
                m_list.DeleteArrayElementAtIndex(toRemove);
            }
        }

        private bool ShowAddButton()
        {
            Rect addRect = GUILayoutUtility.GetRect(m_addContent, GUI.skin.button);
            const float addWidth = 200f;
            addRect.x = addRect.x + (addRect.width - addWidth) / 2;
            addRect.width = addWidth;
            return GUI.Button(addRect, m_addContent);
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
