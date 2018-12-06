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
            if (useDefaultEditor) { return; }
            else { EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Beam Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                target.beamPrefab = EditorGUILayout.ObjectField("Beam Prefab", target.beamPrefab, typeof(BeamObject), true) as BeamObject;
                target.beamWidth = EditorGUILayout.FloatField("Beam Width", target.beamWidth);
                //target.beamPen = EditorGUILayout.FloatField("Beam Pen", target.beamPen);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

}
