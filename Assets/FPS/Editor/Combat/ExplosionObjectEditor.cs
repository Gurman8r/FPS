using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(ExplosionObject))]
    [CanEditMultipleObjects]
    public class ExplosionObjectEditor : CombatObjectEditor
    {
        new ExplosionObject target
        {
            get { return base.target as ExplosionObject; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
