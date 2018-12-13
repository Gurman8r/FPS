using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Beam), true)]
    [CanEditMultipleObjects]
    public class BeamEditor : BaseEntityEditor
    {
        new Beam target
        {
            get { return base.target as Beam; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
