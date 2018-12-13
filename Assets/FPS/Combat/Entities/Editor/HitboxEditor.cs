using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(Hitbox), true)]
    [CanEditMultipleObjects]
    public class HitboxEditor : Editor
    {
        new Hitbox target
        {
            get { return base.target as Hitbox; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
