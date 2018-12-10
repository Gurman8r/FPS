using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(UnitVision))]
    [CanEditMultipleObjects]
    public class UnitVisionEditor : Editor
    {
        new UnitVision target
        {
            get { return base.target as UnitVision; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
