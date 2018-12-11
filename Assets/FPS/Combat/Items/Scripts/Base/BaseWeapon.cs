using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public abstract class BaseWeapon : Item
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Base Weapon Settings")]
        [SerializeField] Transform  m_fireRoot;
        [SerializeField] Damage     m_damage;


        /* Properties
        * * * * * * * * * * * * * * * */
        public Transform fireRoot
        {
            get { return m_fireRoot; }
            set { m_fireRoot = value; }
        }

        public Damage damage
        {
            get { return m_damage; }
        }

        
        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Update()
        {
            base.Update();

            if (Application.isPlaying)
            {
                if (owner && fireRoot)
                {
                    fireRoot.LookAt(owner.vision.lookingAt);
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if(fireRoot)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(fireRoot.position, 0.1f);

                if (owner)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(fireRoot.position, owner.vision.lookingAt);
                    Gizmos.DrawWireSphere(owner.vision.lookingAt, 0.1f);
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public abstract override void HandleInputPrimary(ButtonState input);

        public abstract override void HandleInputSecondary(ButtonState input);


        protected void Shoot()
        {
            if(canUse)
            {
                useTimer = 0f;

                StartCoroutine(ShootCoroutine());

                ConsumeResource();
            }
            else if(!resourceAvailable)
            {
                StartCooldown();
            }
        }

        protected abstract IEnumerator ShootCoroutine();
        
        protected virtual void UpdateCooldown()
        {
            if (!canUse && (useDelay > 0f))
            {
                useTimer += Time.deltaTime;
            }
        }
    }

}
