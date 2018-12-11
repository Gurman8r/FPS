using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public abstract class UnitController : UnitBehaviour
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

    }
}
