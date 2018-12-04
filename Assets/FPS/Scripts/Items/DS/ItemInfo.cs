using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class ItemInfo
    {
        /* Defaults
        * * * * * * * * * * * * * * * */
        public const string             DefaultName     = "New Item";
        public const string             DefaultDesc     = "Description";
        public static readonly Color    DefaultColor    = Color.white;

        /* Variables
        * * * * * * * * * * * * * * * */
        public string   name    = DefaultName;
        public string   desc    = DefaultDesc;
        public Color    color   = DefaultColor;
        public Sprite   sprite  = null;

    }
}
