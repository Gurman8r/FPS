using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(ExplosionEntity), true)]
    [CanEditMultipleObjects]
    public class ExplosionObjectEditor : EntityEditor
    {
        new ExplosionEntity target
        {
            get { return base.target as ExplosionEntity; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
