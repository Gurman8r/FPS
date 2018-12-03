using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    [RequireComponent(typeof(NavMeshAgent))]
    [DisallowMultipleComponent]
    public abstract class UnitController : UnitBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private NavMeshAgent    m_agent;

        [Header("Controller Settings")]
        [SerializeField] float      m_jumpHeight = 1f;
        [SerializeField] float      m_moveSpeed = 1f;
        [SerializeField] float      m_sprintSpeed = 2f;
        [SerializeField] float      m_interactRange = 2.5f;
        [SerializeField] string     m_startingItem = "";

        [Header("Controller Runtime")]
        [SerializeField] Vector2    m_moveInput;
        [SerializeField] bool       m_sprintInput;
        [SerializeField] Vector2    m_lookInput;
        [SerializeField] float      m_scrollInput;
        [SerializeField] int        m_selectInput = 0;
        [SerializeField] bool       m_jumpInput;


        /* Properties
        * * * * * * * * * * * * * * * */
        public NavMeshAgent agent
        {
            get
            {
                if (!m_agent)
                {
                    m_agent = GetComponent<NavMeshAgent>();
                }
                return m_agent;
            }
        }

        public float jumpHeight
        {
            get { return m_jumpHeight; }
        }

        public float moveSpeed
        {
            get { return m_moveSpeed; }
        }

        public float sprintSpeed
        {
            get { return m_sprintSpeed; }
        }

        public float interactRange
        {
            get { return m_interactRange; }
        }

        public string startingItem
        {
            get { return m_startingItem; }
        }


        public Vector2 moveInput
        {
            get { return m_moveInput; }
            protected set { m_moveInput = value; }
        }

        public bool sprintInput
        {
            get { return m_sprintInput; }
            protected set { m_sprintInput = value; }
        }
        public Vector2 lookInput
        {
            get { return m_lookInput; }
            protected set { m_lookInput = value; }
        }

        public float scrollInput
        {
            get { return m_scrollInput; }
            protected set { m_scrollInput = value; }
        }

        public int selectInput
        {
            get { return m_selectInput; }
            protected set { m_selectInput = value; }
        }

        public bool jumpInput
        {
            get { return m_jumpInput; }
            protected set { m_jumpInput = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Start()
        {
            if(Application.isPlaying)
            {
                agent.enabled = false;

                if (ItemDatabase.instance)
                {
                    Item item;
                    if (ItemDatabase.instance.GetPrefab(startingItem, out item))
                    {
                        unit.inventory.Equip(unit.inventory.primary, Instantiate(item));
                    }
                }
            }
        }

        protected virtual void Update()
        {
            if(Application.isPlaying)
            {
                unit.motor.Move(moveInput, (sprintInput ? sprintSpeed : moveSpeed));

                if(jumpInput)
                {
                    jumpInput = false;
                    unit.motor.Jump(jumpHeight);
                }
            }
        }
    }
}
