using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ML
{
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : Editor
    {
        new ItemDatabase target
        {
            get { return base.target as ItemDatabase; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
