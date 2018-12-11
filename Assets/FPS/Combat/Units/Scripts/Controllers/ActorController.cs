using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class ActorController : UnitController
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        private NavMeshAgent m_agent;

        [Header("Actor Settings")]
        [SerializeField] string     m_startingItem = "";
        [SerializeField] float      m_jumpHeight = 1f;
        [SerializeField] float      m_moveSpeed = 1f;
        [SerializeField] float      m_sprintSpeed = 2f;
        [SerializeField] float      m_interactRange = 2.5f;

        [Header("Actor Runtime")]
        [SerializeField] ButtonState  m_fire0;
        [SerializeField] ButtonState  m_fire1;
        [SerializeField] Vector2    m_moveInput;
        [SerializeField] bool       m_sprintInput;
        [SerializeField] Vector2    m_lookInput;
        [SerializeField] int        m_selectInput = -1;
        [SerializeField] float      m_scrollInput;
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

        
        
        public ButtonState fire0
        {
            get { return m_fire0; }
            protected set { m_fire0 = value; }
        }

        public ButtonState fire1
        {
            get { return m_fire1; }
            protected set { m_fire1 = value; }
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

        public int selectInput
        {
            get { return m_selectInput; }
            protected set { m_selectInput = value; }
        }

        public float scrollInput
        {
            get { return m_scrollInput; }
            protected set { m_scrollInput = value; }
        }

        public bool jumpInput
        {
            get { return m_jumpInput; }
            protected set { m_jumpInput = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Update();

            if (Application.isPlaying)
            {
                Item item;
                if (ItemDatabase.instance.GetPrefab(startingItem, out item))
                {
                    self.inventory.Equip(self.combat.right, Instantiate(item));
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if(Application.isPlaying)
            {
                self.motor.Move(moveInput, (sprintInput ? sprintSpeed : moveSpeed));

                if(jumpInput)
                {
                    jumpInput = false;

                    self.motor.Jump(jumpHeight);
                }

                if(!self.inventory.empty)
                {
                    // Select Input
                    if ((selectInput >= 0) && (selectInput < self.inventory.items.Count))
                    {
                        self.inventory.index = selectInput;

                        if (!self.inventory.Equip(self.combat.right, self.inventory.index))
                        {
                            self.inventory.Store(self.combat.right);

                            selectInput = -1;
                        }
                    }

                    // Scroll Input
                    if (scrollInput != 0f)
                    {
                        if (scrollInput < 0f)
                        {
                            self.inventory.index = (self.inventory.index < self.inventory.count - 1)
                                ? (self.inventory.index + 1)
                                : (0);
                        }
                        else if (scrollInput > 0f)
                        {
                            self.inventory.index = (self.inventory.index > 0)
                                ? (self.inventory.index - 1)
                                : (self.inventory.count - 1);
                        }

                        if (!self.inventory.Equip(self.combat.right, self.inventory.index))
                        {
                            self.inventory.Store(self.combat.right);
                        }
                    }
                }
            }
        }
    }
}
