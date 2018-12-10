using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(UnitInventory))]
    [CanEditMultipleObjects]
    public class UnitInventoryEditor : Editor
    {
        new UnitInventory target
        {
            get { return base.target as UnitInventory; }
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
