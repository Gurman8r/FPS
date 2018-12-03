using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(WeaponBase))]
    [CanEditMultipleObjects]
    public class WeaponBaseEditor : Editor
    {
        new WeaponBase target
        {
            get { return base.target as WeaponBase; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
