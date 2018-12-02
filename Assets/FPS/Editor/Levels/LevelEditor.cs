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
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        new Level target
        {
            get { return base.target as Level; }
        }


        /*  Functions
        * * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            {
                string buildText = target.activeRooms.Count == 0 ? "Build" : "Rebuild";
                if (GUILayout.Button(new GUIContent(buildText, "Generate new level")))
                {
                    target.Build();
                }

                if (GUILayout.Button(new GUIContent("Clear", "Destroy current level")))
                {
                    target.Clear();
                }

                if (GUILayout.Button(new GUIContent("Reload", "Update level's data structures")))
                {
                    target.Reload();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}
