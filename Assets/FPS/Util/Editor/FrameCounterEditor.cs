using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace FPS
{
    [CustomEditor(typeof(FrameCounter))]
    public class FrameCounterEditor : Editor
    {
        new FrameCounter target
        {
            get { return base.target as FrameCounter; }
        }

        /*  Variables
        * * * * * * * * * * * * * * * * */

        /*  Core
        * * * * * * * * * * * * * * * * */
        private void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            target.text = (Text)EditorGUILayout.ObjectField("Text", target.text, typeof(Text), true);
            target.format = EditorGUILayout.TextField("Format", target.format);

            if (!target.text)
            {
                target.offset = EditorGUILayout.Vector2Field("Offset", target.offset);
                target.anchor = (TextAnchor)EditorGUILayout.EnumPopup("Anchor", target.anchor);
                target.color = EditorGUILayout.ColorField("Color", target.color);
            }
            else
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField("Some properties being determined by Text.", EditorStyles.boldLabel);
                EditorGUILayout.EndVertical();
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change FPS Display Value");
            }
        }

        /*  Functions
        * * * * * * * * * * * * * * * * */

    }
}
