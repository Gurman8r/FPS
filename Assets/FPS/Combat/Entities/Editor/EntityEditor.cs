using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(BaseEntity), true)]
    [CanEditMultipleObjects]
    public class EntityEditor : Editor
    {
        new BaseEntity target
        {
            get { return base.target as BaseEntity; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
