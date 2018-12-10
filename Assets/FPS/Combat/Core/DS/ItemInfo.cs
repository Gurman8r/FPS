using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public sealed class ItemInfo
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        public string   name    = "New Item";
        public string   desc    = "New Item Description";
        public Color    color   = Color.white;
        public Sprite   sprite  = null;

    }
}
