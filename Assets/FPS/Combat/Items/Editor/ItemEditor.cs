using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Item), true)]
    [CanEditMultipleObjects]
    public class ItemEditor : Editor
    {
        new Item target
        {
            get { return base.target as Item; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Animator", target.animator, typeof(Animator), false);
            EditorGUILayout.ObjectField("Audio", target.audio, typeof(AudioSource), false);
            EditorGUILayout.ObjectField("Collider", target.collider, typeof(Collider), false);
            EditorGUILayout.ObjectField("Rigidbody", target.rigidbody, typeof(Rigidbody), false);
            GUI.enabled = true;

            base.OnInspectorGUI();
        }
    }

}
