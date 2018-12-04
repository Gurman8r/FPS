using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BulletWeapon))]
    [CanEditMultipleObjects]
    public class BulletWeaponEditor : WeaponBaseEditor
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
            if (useDefaultEditor) { return; }
            else { EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Bullet Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                target.bulletPrefab = EditorGUILayout.ObjectField("Bullet Prefab", target.bulletPrefab, typeof(BulletObject), true) as BulletObject;
                target.bulletCount = EditorGUILayout.IntField("Bullet Count", target.bulletCount);
                target.bulletDelay = EditorGUILayout.FloatField("Bullet Delay", target.bulletDelay);
                target.bulletSpread = EditorGUILayout.FloatField("Bullet Spread", target.bulletSpread);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

}
