using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    [CustomEditor(typeof(UnitManager))]
    public class UnitManagerEditor : Editor
    {
        new UnitManager target
        {
            get { return base.target as UnitManager; }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
