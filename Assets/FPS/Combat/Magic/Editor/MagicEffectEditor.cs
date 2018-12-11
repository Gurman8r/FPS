using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(MagicEffect), true)]
    public class MagicEffectEditor : Editor
    {
        new MagicEffect target
        {
            get { return base.target as MagicEffect; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        private AnimBool m_showBase;
        private AnimBool m_showFlags;
        private AnimBool m_showEvents;


        /* Functions
        * * * * * * * * * * * * * * * */
        private void OnEnable()
        {
            m_showBase = new AnimBool(true);
            m_showBase.valueChanged.AddListener(Repaint);

            m_showFlags = new AnimBool(true);
            m_showFlags.valueChanged.AddListener(Repaint);

            m_showEvents = new AnimBool(true);
            m_showEvents.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", target, typeof(MagicEffect), false);
            GUI.enabled = true;

            EditorGUILayout.HelpBox(
               "Magic Effects represent the primary visual and functional effects of a Spell.",
               MessageType.Info);

            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_UID"));

                EditorGUILayout.BeginVertical(GUI.skin.box);
                m_showBase.target = GUILayout.Toggle(m_showBase.target, "General", "Foldout");
                if (EditorGUILayout.BeginFadeGroup(m_showBase.faded))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_effectType"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_castingType"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_deliveryType"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_taperCurve"));
                    GUI.enabled = !target.noMagnitude;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_magnitude"));
                    GUI.enabled = !target.noArea;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_area"));
                    GUI.enabled = !target.noDuration;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_duration"));
                    GUI.enabled = true;
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                m_showFlags.target = GUILayout.Toggle(m_showFlags.target, "Flags", "Foldout");
                if (EditorGUILayout.BeginFadeGroup(m_showFlags.faded))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_hostile"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_detrimental"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_noRecast"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_noMagnitude"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_noArea"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_noDuration"));
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                m_showEvents.target = GUILayout.Toggle(m_showEvents.target, "Events", "Foldout");
                if (EditorGUILayout.BeginFadeGroup(m_showEvents.faded))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_onCharge"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_onReady"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_onRelease"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_onCastLoop"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_onHit"));
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
