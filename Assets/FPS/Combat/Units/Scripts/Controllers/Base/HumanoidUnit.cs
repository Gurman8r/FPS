using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class HumanoidUnit : DynamicUnit
    {        
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Humanoid Settings")]
        [SerializeField] string     m_startingItem = "";
        [SerializeField] float      m_jumpHeight = 1f;
        [SerializeField] float      m_moveSpeed = 1f;
        [SerializeField] float      m_sprintSpeed = 2f;
        [SerializeField] float      m_interactRange = 2.5f;

        [Header("Humanoid Runtime")]
        [SerializeField] ItemInput  m_fire0;
        [SerializeField] ItemInput  m_fire1;
        [SerializeField] Vector2    m_moveInput;
        [SerializeField] bool       m_sprintInput;
        [SerializeField] Vector2    m_lookInput;
        [SerializeField] int        m_selectInput = -1;
        [SerializeField] float      m_scrollInput;
        [SerializeField] bool       m_jumpInput;


        /* Properties
        * * * * * * * * * * * * * * * */
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

        
        
        public ItemInput fire0
        {
            get { return m_fire0; }
            protected set { m_fire0 = value; }
        }

        public ItemInput fire1
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
                if (ItemManager.instance)
                {
                    Item item;
                    if (ItemManager.instance.GetPrefab(startingItem, out item))
                    {
                        unit.inventory.Equip(unit.inventory.hand, Instantiate(item));
                    }
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if(Application.isPlaying)
            {
                unit.motor.Move(moveInput, (sprintInput ? sprintSpeed : moveSpeed));

                if(jumpInput)
                {
                    jumpInput = false;

                    unit.motor.Jump(jumpHeight);
                }

                if(!unit.inventory.empty)
                {
                    // Select Input
                    if ((selectInput >= 0) && (selectInput < unit.inventory.items.Count))
                    {
                        unit.inventory.index = selectInput;

                        if (!unit.inventory.Equip(unit.inventory.hand, unit.inventory.index))
                        {
                            unit.inventory.Store(unit.inventory.hand);

                            selectInput = -1;
                        }
                    }

                    // Scroll Input
                    if (scrollInput != 0f)
                    {
                        if (scrollInput < 0f)
                        {
                            unit.inventory.index = (unit.inventory.index < unit.inventory.count - 1)
                                ? (unit.inventory.index + 1)
                                : (0);
                        }
                        else if (scrollInput > 0f)
                        {
                            unit.inventory.index = (unit.inventory.index > 0)
                                ? (unit.inventory.index - 1)
                                : (unit.inventory.count - 1);
                        }

                        if (!unit.inventory.Equip(unit.inventory.hand, unit.inventory.index))
                        {
                            unit.inventory.Store(unit.inventory.hand);
                        }
                    }
                }
            }
        }
    }
}
