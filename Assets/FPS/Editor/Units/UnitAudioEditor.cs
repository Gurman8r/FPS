using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(UnitAudio))]
    [CanEditMultipleObjects]
    public class UnitAudioEditor : Editor
    {
        new UnitAudio target
        {
            get { return base.target as UnitAudio; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Audio Source", target.audio, typeof(AudioSource), false);
            GUI.enabled = true;

            base.OnInspectorGUI();
        }
    }
}
