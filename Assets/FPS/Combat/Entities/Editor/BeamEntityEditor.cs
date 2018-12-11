using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BeamEntity), true)]
    [CanEditMultipleObjects]
    public class BeamObjectEditor : EntityEditor
    {
        new BeamEntity target
        {
            get { return base.target as BeamEntity; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
