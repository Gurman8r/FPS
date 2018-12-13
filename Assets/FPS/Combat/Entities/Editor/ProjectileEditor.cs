using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Projectile), true)]
    [CanEditMultipleObjects]
    public class ProjectileEditor : BaseEntityEditor
    {
        new Projectile target
        {
            get { return base.target as Projectile; }
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
