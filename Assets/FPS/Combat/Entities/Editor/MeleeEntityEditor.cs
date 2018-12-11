using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(MeleeEntity), true)]
    [CanEditMultipleObjects]
    public class MeleeObjectEditor : Editor
    {
        new MeleeEntity target
        {
            get { return base.target as MeleeEntity; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
