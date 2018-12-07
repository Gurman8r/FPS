using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public abstract class WeaponBase : Item
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Weapon Settings")]
        [SerializeField] Transform  m_firePos;
        [SerializeField] ObjectData m_objectData;

        [Header("Weapon Runtime")]
        [SerializeField] Vector3    m_lookingAt;
        
        

        /* Properties
        * * * * * * * * * * * * * * * */
        public Transform firePos
        {
            get { return m_firePos; }
            set { m_firePos = value; }
        }

        public ObjectData data
        {
            get { return m_objectData; }
            set { m_objectData = value; }
        }
        
        public Vector3 lookingAt
        {
            get { return m_lookingAt; }
            protected set { m_lookingAt = value; }
        }

        
        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (Application.isPlaying)
            {
                if (owner)
                {
                    lookingAt = owner.vision.lookingAt;

                    if(onCooldown)
                    {
                        lookingAt = firePos.position + transform.forward;
                    }

                    firePos.LookAt(lookingAt);

                    if (autoReload && !hasResource)
                    {
                        Reload();
                    }
                }

                animator.SetBool("Reloading", onCooldown);
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if(firePos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(firePos.position, 0.1f);

                if (owner)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(firePos.position, lookingAt);
                    Gizmos.DrawWireSphere(lookingAt, 0.1f);
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public abstract override void UpdatePrimary(InputState input);

        public abstract override void UpdateSecondary(InputState input);


        protected void Shoot()
        {
            if(canUse)
            {
                useTimer = 0f;

                StartCoroutine(ShootCoroutine());

                ConsumeResource();
            }
            else if(!hasResource)
            {
                Reload();
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
