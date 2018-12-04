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

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Melee Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                target.meleeObject = EditorGUILayout.ObjectField("Melee Object", target.meleeObject, typeof(MeleeObject), true) as MeleeObject;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

}
