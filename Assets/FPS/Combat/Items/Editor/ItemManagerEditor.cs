using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(ItemManager))]
    public class ItemManagerEditor : Editor
    {
        new ItemManager target
        {
            get { return base.target as ItemManager; }
        }

        /* Variables
       * * * * * * * * * * * * * * * */
        private SerializedProperty m_listProperty;
        private GUIContent m_addContent;
        private GUIContent m_removeButton;
        private AnimBool m_showEntries;

        /* Functions
        * * * * * * * * * * * * * * * */
        protected virtual void OnEnable()
        {
            m_listProperty = serializedObject.FindProperty("m_items");

            m_addContent = new GUIContent("Add New Item");

            m_removeButton = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));

            m_removeButton.tooltip = "Remove this item from the list.";

            m_showEntries = new AnimBool(true);
            m_showEntries.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            m_showEntries.target = GUILayout.Toggle(m_showEntries.target, "Database", "Foldout");
            if(EditorGUILayout.BeginFadeGroup(m_showEntries.faded))
            {
                DisplayListEntries();
            }
            EditorGUILayout.EndFadeGroup();

            Rect addRect = GUILayoutUtility.GetRect(m_addContent, GUI.skin.button);
            const float addWidth = 200f;
            addRect.x = addRect.x + (addRect.width - addWidth) / 2;
            addRect.width = addWidth;
            if (GUI.Button(addRect, m_addContent))
            {
                AddNewItem();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            m_listProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }

        private void DisplayListEntries()
        {
            int toBeRemovedEntry = -1;

            for (int i = 0; i < m_listProperty.arraySize; ++i)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    SerializedProperty item = m_listProperty.GetArrayElementAtIndex(i);
                    SerializedProperty itemName = item.FindPropertyRelative("name");
                    SerializedProperty itemValue = item.FindPropertyRelative("value");

                    EditorGUILayout.PropertyField(itemName, new GUIContent(""));
                    EditorGUILayout.PropertyField(itemValue, new GUIContent(""));

                    if (GUILayout.Button(m_removeButton, GUIStyle.none) &&
                        EditorUtility.DisplayDialog(
                            "Remove Item",
                            "Are you sure you want to remove " + itemName.stringValue + "?",
                            "Yes", "No"))
                    {
                        toBeRemovedEntry = i;
                    }
                }
                EditorGUILayout.EndVertical();
            }

            if (toBeRemovedEntry > -1)
            {
                RemoveEntry(toBeRemovedEntry);
            }
        }

        private void AddNewItem()
        {
            m_listProperty.arraySize += 1;
            SerializedProperty item = m_listProperty.GetArrayElementAtIndex(m_listProperty.arraySize - 1);
            SerializedProperty itemName = item.FindPropertyRelative("name");
            SerializedProperty itemValue = item.FindPropertyRelative("value");
            itemName.stringValue = "New Item";
            itemValue.objectReferenceValue = null;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
