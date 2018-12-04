using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Item))]
    [CanEditMultipleObjects]
    public abstract class ItemEditor : Editor
    {
        protected static bool useDefaultEditor = true;

        new Item target
        {
            get { return base.target as Item; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            useDefaultEditor = !GUILayout.Toggle(
                (!useDefaultEditor),
                (useDefaultEditor ? "Item Inspector (Beta)" : "Default Inspector"), 
                ("Button"));
            if (useDefaultEditor) { base.OnInspectorGUI(); return; }
            else EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Item Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                target.model = EditorGUILayout.ObjectField(new GUIContent("Model"), target.model, typeof(Transform), true) as Transform;
                target.holdPos = EditorGUILayout.ObjectField(new GUIContent("Hold Pos"), target.holdPos, typeof(Transform), true) as Transform;
                target.info.name = EditorGUILayout.TextField(new GUIContent("Name"), target.info.name);
                target.info.desc = EditorGUILayout.TextField(new GUIContent("Description"), target.info.desc);
                target.info.color = EditorGUILayout.ColorField(new GUIContent("Color"), target.info.color);
                target.info.sprite = EditorGUILayout.ObjectField(new GUIContent("Sprite"), target.info.sprite, typeof(Sprite), false) as Sprite;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

}
