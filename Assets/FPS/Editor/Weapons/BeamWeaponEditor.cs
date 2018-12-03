using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BeamWeapon))]
    [CanEditMultipleObjects]
    public class BeamWeaponEditor : WeaponBaseEditor
    {
        new BeamWeapon target
        {
            get { return base.target as BeamWeapon; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
