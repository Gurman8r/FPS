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
        new Item target
        {
            get { return base.target as Item; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Item Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                target.model = EditorGUILayout.ObjectField("Model", target.model, typeof(Transform), true) as Transform;
                target.holdPos = EditorGUILayout.ObjectField("Hold Pos", target.holdPos, typeof(Transform), true) as Transform;
                target.info.name = EditorGUILayout.TextField("Name", target.info.name);
                target.info.desc = EditorGUILayout.TextField("Description", target.info.desc);
                target.info.color = EditorGUILayout.ColorField("Color", target.info.color);
                target.info.sprite = EditorGUILayout.ObjectField("Sprite", target.info.sprite, typeof(Sprite), false) as Sprite;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

}
