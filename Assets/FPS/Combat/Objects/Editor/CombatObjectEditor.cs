using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(CombatObject), true)]
    [CanEditMultipleObjects]
    public class CombatObjectEditor : Editor
    {
        new CombatObject target
        {
            get { return base.target as CombatObject; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
