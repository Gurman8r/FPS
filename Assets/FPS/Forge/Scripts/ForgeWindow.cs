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
            [SerializeField] bool m_active;
            [Space]
            [SerializeField] Rect m_bounds;

            /* Properties
            * * * * * * * * * * * * * * * */
            public static ForgeWindow instance
            {
                get; private set;
            }

            public bool active
            {
                get { return m_active; }
                set { m_active = value; }
            }

            /* Core
            * * * * * * * * * * * * * * * */
            private void OnEnable()
            {
                if(!instance || instance == this)
                {
                    instance = this;

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(gameObject);
                    }
                }
                else
                {
                    if(Application.isPlaying)
                    {
                        Destroy(gameObject);
                    }
                }
            }
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
