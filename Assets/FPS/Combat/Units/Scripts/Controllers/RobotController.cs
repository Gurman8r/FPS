using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class RobotController : ActorController
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Robot Settings")]
        [SerializeField] FriendlyFire   m_friendlyFire;
        [SerializeField] CombatStyle    m_combatStyle;

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
