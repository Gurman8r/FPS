using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    namespace Forge
    {
        [ExecuteInEditMode]
        public class ForgeWindow : MonoBehaviour
        {
            /* Variables
            * * * * * * * * * * * * * * * */
            [SerializeField] Rect m_bounds;

            /* Properties
            * * * * * * * * * * * * * * * */

            /* Core
            * * * * * * * * * * * * * * * */
            private void Start()
            {

            }

            private void Update()
            {

            }

            private void OnGUI()
            {
                GUILayout.BeginArea(m_bounds, GUI.skin.box);
                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Here")) { Debug.Log("Here!"); }
                    if (GUILayout.Button("Here")) { Debug.Log("Here!"); }
                    if (GUILayout.Button("Here")) { Debug.Log("Here!"); }
                    if (GUILayout.Button("Here")) { Debug.Log("Here!"); }
                }
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }

            /* Functions
            * * * * * * * * * * * * * * * */
        }

    }
}
