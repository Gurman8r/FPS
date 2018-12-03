using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(MeleeWeapon))]
    [CanEditMultipleObjects]
    public class MeleeWeaponEditor : WeaponBaseEditor
    {
        new MeleeWeapon target
        {
            get { return base.target as MeleeWeapon; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
