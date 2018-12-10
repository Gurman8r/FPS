using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(MeleeObject), true)]
    [CanEditMultipleObjects]
    public class MeleeObjectEditor : Editor
    {
        new MeleeObject target
        {
            get { return base.target as MeleeObject; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
