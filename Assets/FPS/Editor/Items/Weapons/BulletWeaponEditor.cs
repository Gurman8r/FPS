using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BulletWeapon))]
    [CanEditMultipleObjects]
    public class BulletWeaponEditor : Editor
    {
        new BulletWeapon target
        {
            get { return base.target as BulletWeapon; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
