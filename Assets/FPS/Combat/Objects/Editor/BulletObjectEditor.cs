using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BulletObject), true)]
    [CanEditMultipleObjects]
    public class BulletObjectEditor : CombatObjectEditor
    {
        new BulletObject target
        {
            get { return base.target as BulletObject; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            {

            }
            serializedObject.ApplyModifiedProperties();
        }
    }

}
