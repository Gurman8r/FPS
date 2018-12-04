using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(WeaponBase))]
    [CanEditMultipleObjects]
    public abstract class WeaponBaseEditor : ItemEditor
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
            if (useDefaultEditor) { return; }
            else { EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Weapon Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                target.firePos = EditorGUILayout.ObjectField("Fire Position", target.firePos, typeof(Transform), true) as Transform;
                target.fireMode = (WeaponBase.FireMode)EditorGUILayout.EnumPopup("Fire Mode", target.fireMode);
                target.fireDelay = EditorGUILayout.FloatField("Fire Delay", target.fireDelay);
                target.minRange = EditorGUILayout.FloatField("Min Range", target.minRange);
                target.maxRange = EditorGUILayout.FloatField("Max Range", target.maxRange);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Object Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                target.data.speed = EditorGUILayout.FloatField("Speed", target.data.speed);
                target.data.lifeSpan = EditorGUILayout.FloatField("Lifespan", target.data.lifeSpan);
                target.data.solidLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(
                    EditorGUILayout.MaskField(
                        "Layer Mask",
                        InternalEditorUtility.LayerMaskToConcatenatedLayersMask(target.data.solidLayer),
                        InternalEditorUtility.layers));
                target.data.damage.amount = EditorGUILayout.FloatField("Damage", target.data.damage.amount);
                target.data.healing.amount = EditorGUILayout.FloatField("Healing", target.data.healing.amount);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

}
