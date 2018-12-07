using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

// Editor for FPS.UnitTriggers
// Based on UnityEditor.EventSystems.EventTriggerEditor
// https://github.com/tenpn/unity3d-ui/blob/master/UnityEditor.UI/EventSystem/EventTriggerEditor.cs

namespace FPS
{
    [CustomEditor(typeof(UnitTriggers), true)]
    [CanEditMultipleObjects]
    public class UnitTriggersEditor : Editor
    {
        new UnitTriggers target
        {
            get { return base.target as UnitTriggers; }
        }


        /* Variables
        * * * * * * * * * * * * * * * */
        private SerializedProperty  m_ListProperty;
        private GUIContent          m_IconToolbarMinus;
        private GUIContent          m_EventIDName;
        private GUIContent[]        m_EventTypes;
        private GUIContent          m_AddButonContent;

        /* Functions
        * * * * * * * * * * * * * * * */
        protected virtual void OnEnable()
        {
            m_ListProperty = serializedObject.FindProperty("delegates");
            m_AddButonContent = new GUIContent("Add New Event Type");
            m_EventIDName = new GUIContent("");
            // Have to create a copy since otherwise the tooltip will be overwritten.
            m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_IconToolbarMinus.tooltip = "Remove all events in this list.";

            string[] eventNames = Enum.GetNames(typeof(EventType));
            m_EventTypes = new GUIContent[eventNames.Length];
            for (int i = 0; i < eventNames.Length; ++i)
            {
                m_EventTypes[i] = new GUIContent(eventNames[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            int toBeRemovedEntry = -1;

            EditorGUILayout.Space();

            Vector2 removeButtonSize = GUIStyle.none.CalcSize(m_IconToolbarMinus);

            for (int i = 0; i < m_ListProperty.arraySize; ++i)
            {
                SerializedProperty delegateProperty = m_ListProperty.GetArrayElementAtIndex(i);
                SerializedProperty eventProperty = delegateProperty.FindPropertyRelative("eventID");
                SerializedProperty callbacksProperty = delegateProperty.FindPropertyRelative("callback");
                m_EventIDName.text = eventProperty.enumDisplayNames[eventProperty.enumValueIndex];

                EditorGUILayout.PropertyField(callbacksProperty, m_EventIDName);
                Rect callbackRect = GUILayoutUtility.GetLastRect();

                Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, m_IconToolbarMinus, GUIStyle.none))
                {
                    toBeRemovedEntry = i;
                }

                EditorGUILayout.Space();
            }

            if (toBeRemovedEntry > -1)
            {
                RemoveEntry(toBeRemovedEntry);
            }

            Rect btPosition = GUILayoutUtility.GetRect(m_AddButonContent, GUI.skin.button);
            const float addButonWidth = 200f;
            btPosition.x = btPosition.x + (btPosition.width - addButonWidth) / 2;
            btPosition.width = addButonWidth;
            if (GUI.Button(btPosition, m_AddButonContent))
            {
                ShowAddTriggermenu();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            m_ListProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }

        void ShowAddTriggermenu()
        {
            // Now create the menu, add items and show it
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < m_EventTypes.Length; ++i)
            {
                bool active = true;

                // Check if we already have a Entry for the current eventType, if so, disable it
                for (int p = 0; p < m_ListProperty.arraySize; ++p)
                {
                    SerializedProperty delegateEntry = m_ListProperty.GetArrayElementAtIndex(p);
                    SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
                    if (eventProperty.enumValueIndex == i)
                    {
                        active = false;
                    }
                }
                if (active)
                    menu.AddItem(m_EventTypes[i], false, OnAddNewSelected, i);
                else
                    menu.AddDisabledItem(m_EventTypes[i]);
            }
            menu.ShowAsContext();
            Event.current.Use();
        }

        private void OnAddNewSelected(object index)
        {
            int selected = (int)index;

            m_ListProperty.arraySize += 1;
            SerializedProperty delegateEntry = m_ListProperty.GetArrayElementAtIndex(m_ListProperty.arraySize - 1);
            SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
            eventProperty.enumValueIndex = selected;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
