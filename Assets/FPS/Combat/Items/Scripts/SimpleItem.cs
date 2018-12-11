using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public sealed class SimpleItem : Item
    {
        /* Variables
        * * * * * * * * * * * * * * * */

        /* Properties
        * * * * * * * * * * * * * * * */

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        public override void HandleInputPrimary(ButtonState input)
        {
        }

        public override void HandleInputSecondary(ButtonState input)
        {
        }
    }

}
