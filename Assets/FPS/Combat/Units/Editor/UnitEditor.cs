using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Unit))]
    [CanEditMultipleObjects]
    public class UnitEditor : Editor
    {
        new Unit target
        {
            get { return base.target as Unit; }
        }

        /* Variables
        * * * * * * * * * * * * * * * */

        /* Functions
        * * * * * * * * * * * * * * * */
        private void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Info
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField(
                    new GUIContent(
                        string.Format("ID: {0}", (target.id)),
                        "The ID of this Unit"),
                    EditorStyles.boldLabel);
            }
            EditorGUILayout.EndVertical();

            // Behaviours
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Behaviours", EditorStyles.boldLabel);
                BehaviourToggle("Triggers", target.triggers);
                BehaviourToggle("Motor", target.motor);
                BehaviourToggle("Inventory", target.inventory);
                BehaviourToggle("Vision", target.vision);
            }
            EditorGUILayout.EndVertical();

            // Health
            HealthEditor(target.health);
            
            serializedObject.ApplyModifiedProperties();

            //base.OnInspectorGUI();
        }

        private void BehaviourToggle<T>(string label, T scr, float maxWidth = 64f) where T : MonoBehaviour
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.MaxWidth(maxWidth));

                GUI.enabled = false;
                EditorGUILayout.ObjectField(scr, typeof(T), false);
                GUI.enabled = true;

                scr.enabled = GUILayout.Toggle(
                    scr.enabled,
                    scr.enabled ? "Enabled " : "Disabled", 
                    "Button");
            }
            EditorGUILayout.EndHorizontal();
        }

        private void HealthEditor(Health value)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
                EditorGUI.ProgressBar(
                    EditorGUILayout.GetControlRect(),
                    value.fillAmount,
                    string.Format("({0}/{1})",
                    value.current,
                    value.maximum));

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Min/Max", GUILayout.MaxWidth(64));

                        EditorGUI.BeginChangeCheck();
                        float min = EditorGUILayout.FloatField(value.minimum);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(base.target, "Changed Target Minimum Health ");
                            value.SetMinMax(min, value.maximum);
                        }

                        EditorGUI.BeginChangeCheck();
                        float max = EditorGUILayout.FloatField(value.maximum);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(base.target, "Changed Target Maximum Health ");
                            value.SetMinMax(value.minimum, max);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginChangeCheck();
                    float cur = EditorGUILayout.Slider(
                        new GUIContent("Current"),
                        value.current,
                        value.minimum,
                        value.maximum);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(base.target, "Changed Target Current Health ");
                        value.SetCurrent(cur);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }

}
