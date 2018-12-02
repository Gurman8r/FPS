using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FPS
{
    [CustomEditor(typeof(Room))]
    [CanEditMultipleObjects]
    public class RoomEditor : Editor
    {
        new Room target
        {
            get { return base.target as Room; }
        }

        /*  Functions
        * * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                if (GUILayout.Button("Generate"))
                {
                    target.Generate();
                }
                if (GUILayout.Button("Clear"))
                {
                    target.Clear();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}
