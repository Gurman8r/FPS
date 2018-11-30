using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ML
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [ExecuteInEditMode]
    public class UnitMotor : UnitBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Collider            m_collider;
        private Rigidbody           m_rigidbody;

        [Header("Settings")]
        [SerializeField] LayerMask  m_groundLayer;
        [SerializeField] bool       m_enableGravity;

        [Header("Runtime")]
        [SerializeField] Vector3    m_velocity;
        [SerializeField] bool       m_isMoving;
        [SerializeField] bool       m_isGrounded;
        [SerializeField] bool       m_isJumping;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new Collider collider
        {
            get
            {
                if (!m_collider)
                {
                    m_collider = GetComponent<Collider>();
                }
                return m_collider;
            }
        }

        public new Rigidbody rigidbody
        {
            get
            {
                if (!m_rigidbody)
                {
                    m_rigidbody = GetComponent<Rigidbody>();
                }
                return m_rigidbody;
            }
        }

        public Vector3 velocity
        {
            get { return m_velocity; }
            private set { m_velocity = value; }
        }

        public bool enableGravity
        {
            get { return m_enableGravity; }
        }

        public bool isMoving
        {
            get { return m_isMoving; }
            private set { m_isMoving = value; }
        }

        public bool isGrounded
        {
            get { return m_isGrounded; }
            private set { m_isGrounded = value; }
        }

        public bool isJumping
        {
            get { return m_isJumping; }
            private set { m_isJumping = value; }
        }



        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if (Application.isPlaying)
            {
                rigidbody.useGravity = false;
            }
        }

        private void Update()
        {
            if(Application.isPlaying)
            {
            }
        }

        private void FixedUpdate()
        {
            if(Application.isPlaying)
            {
                isGrounded = Physics.Raycast(
                    transform.position, 
                    -Vector3.up, 
                    collider.bounds.extents.y);

                if(isJumping && isGrounded)
                {
                    isJumping = false;
                }

                if (enableGravity && !isGrounded)
                {
                    velocity += Physics.gravity * Time.deltaTime;
                }

                if(isGrounded && !isJumping && velocity.y < 0f)
                {
                    velocity = new Vector3(velocity.x, 0f, velocity.z);
                }

                isMoving = velocity.sqrMagnitude > 0f;

                rigidbody.velocity = velocity;
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void Jump(float height)
        {
            if(!isJumping && isGrounded)
            {
                velocity = new Vector3(
                    velocity.x,
                    height,
                    velocity.z);
                isJumping = true;
            }
        }

        public void Move(Vector2 axes, float speed)
        {
            Vector3 forward = transform.forward * axes.y;

            Vector3 strafe = transform.right * axes.x;

            Vector3 direction = (forward + strafe) * speed;

            velocity = new Vector3(
                direction.x, 
                velocity.y,
                direction.z);
        }

        public void Stop()
        {
            velocity = Vector3.zero;
        }

    }

}
