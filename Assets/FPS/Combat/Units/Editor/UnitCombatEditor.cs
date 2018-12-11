using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(UnitCombat))]
    [CanEditMultipleObjects]
    public class UnitCombatEditor : Editor
    {
        new UnitCombat target
        {
            get { return base.target as UnitCombat; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
