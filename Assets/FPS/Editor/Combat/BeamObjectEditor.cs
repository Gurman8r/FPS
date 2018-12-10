using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BeamObject), true)]
    [CanEditMultipleObjects]
    public class BeamObjectEditor : CombatObjectEditor
    {
        new BeamObject target
        {
            get { return base.target as BeamObject; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
