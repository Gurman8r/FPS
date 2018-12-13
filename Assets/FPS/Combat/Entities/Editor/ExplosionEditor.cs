using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Explosion), true)]
    [CanEditMultipleObjects]
    public class ExplosionEditor : BaseEntityEditor
    {
        new Explosion target
        {
            get { return base.target as Explosion; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
