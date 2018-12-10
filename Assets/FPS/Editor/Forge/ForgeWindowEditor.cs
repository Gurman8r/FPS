using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    namespace Forge
    {
        [CustomEditor(typeof(ForgeWindow))]
        public class ForgeWindowEditor : Editor
        {
            new ForgeWindow target
            {
                get { return base.target as ForgeWindow; }
            }

            /* Variables
            * * * * * * * * * * * * * * * */

            /* Functions
            * * * * * * * * * * * * * * * */
            public override void OnInspectorGUI()
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    base.OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
            }
        }

    }
}
