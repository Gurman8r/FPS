using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BulletObject))]
    [CanEditMultipleObjects]
    public class BulletObjectEditor : CombatObjectEditor
    {
        new BulletObject target
        {
            get { return base.target as BulletObject; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
