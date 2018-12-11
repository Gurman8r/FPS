using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BallisticEntity), true)]
    [CanEditMultipleObjects]
    public class BulletObjectEditor : EntityEditor
    {
        new BallisticEntity target
        {
            get { return base.target as BallisticEntity; }
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
