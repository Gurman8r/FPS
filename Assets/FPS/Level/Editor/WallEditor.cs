using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Wall))]
    [CanEditMultipleObjects]
    public class WallEditor : Editor
    {
        new Wall target
        {
            get { return base.target as Wall; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI(); return;

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                {
                    EditorGUILayout.ObjectField(target.wall, typeof(GameObject), false);
                    EditorGUILayout.ObjectField(target.frame, typeof(GameObject), false);
                    EditorGUILayout.ObjectField(target.door, typeof(GameObject), false);
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox); // BEGIN HORIZONTAL

                EditorGUI.BeginChangeCheck();
                bool isDoor = GUILayout.Toggle(target.isDoor, "Door", "Button");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(base.target, "Change Target \'isDoor\'");
                    target.isDoor = isDoor;
                    EditorUtility.SetDirty(target);
                }

                GUI.enabled = target.isDoor;
                {
                    EditorGUI.BeginChangeCheck();
                    bool isOpen = GUILayout.Toggle(target.isOpen, (target.isOpen ? "Close" : "Open"), "Button");
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(base.target, "Change Target \'isOpen\'");
                        target.isOpen = isOpen;
                        EditorUtility.SetDirty(target);

                        if (target.targetWall)
                        {
                            Undo.RecordObject(base.target, "Change Target's Target \'isOpen\'");

                            target.targetWall.isOpen = target.isOpen;

                            EditorUtility.SetDirty(target.targetWall);
                        }
                    }

                    EditorGUILayout.EndHorizontal(); // END HORIZONTAL

                    EditorGUI.BeginChangeCheck();
                    float moveSpeed = EditorGUILayout.FloatField("Door Speed", target.doorSpeed);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(base.target, "Change Target \'moveSpeed\'");
                        target.doorSpeed = moveSpeed;
                        EditorUtility.SetDirty(target);
                    }

                    EditorGUI.BeginChangeCheck();
                    Vector3 openPosition = EditorGUILayout.Vector3Field("Open Position", target.openPosition);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(base.target, "Change Target \'openPosition\'");
                        target.openPosition = openPosition;
                        EditorUtility.SetDirty(target);
                    }

                    EditorGUI.BeginChangeCheck();
                    Vector3 closedPosition = EditorGUILayout.Vector3Field("Closed Position", target.closedPosition);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(base.target, "Change Target \'closedPosition\'");
                        target.closedPosition = closedPosition;
                        EditorUtility.SetDirty(target);
                    }
                }
                GUI.enabled = true;

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Runtime", EditorStyles.boldLabel);
                //GUI.enabled = false;
                {
                    EditorGUILayout.ObjectField("Room", target.room, typeof(Room), false);
                    EditorGUILayout.IntField("Sibling Index", target.siblingIndex);
                    EditorGUILayout.ObjectField("Target Wall", target.targetWall, typeof(Wall), false);
                    EditorGUILayout.Vector3Field("Scale", target.localScale);
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
