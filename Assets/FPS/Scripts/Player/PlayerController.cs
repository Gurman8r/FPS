using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(UnitAudio))]
    [ExecuteInEditMode]
    public class PlayerController : UnitController
        , IDamageSource
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Player Settings")]
        [SerializeField] int            m_playerIndex = 0;
        [SerializeField] PlayerCamera   m_camera;
        [SerializeField] PlayerHUD      m_hud;
        [SerializeField] float          m_inventoryTime = 2.5f;

        [Header("Player Runtime")]
        [SerializeField] bool m_hasKeyboard;
        [SerializeField] bool m_hasGamepad;

        private Rewired.Player  m_input;
        private RaycastHit      m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new PlayerCamera camera
        {
            get { return m_camera; }
        }

        public PlayerHUD hud
        {
            get { return m_hud; }
        }

        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if (Application.isPlaying)
            {
                m_input = Rewired.ReInput.players.GetPlayer(m_playerIndex);

                if (!camera)
                {
                    Debug.LogError("Script Not Found: PlayerCamera", gameObject);
                }

                if (!hud)
                {
                    Debug.LogError("Script Not Found: PlayerHUD", gameObject);
                }
            }
        }

        protected override void Update()
        {
            if (Application.isPlaying)
            {
                // Detect input source
                DetectController();

                // Cursor Lock
                if (m_input.GetButtonDown("Cancel"))
                {
                    camera.cursorLock = !camera.cursorLock;
                }

                // Move
                moveInput = new Vector2(
                    m_input.GetAxis("Move Horizontal"),
                    m_input.GetAxis("Move Vertical"));

                // Sprint
                sprintInput = m_input.GetButton("Sprint");

                // Jump
                jumpInput = m_input.GetButtonDown("Jump");

                // Aim
                lookInput = new Vector2(
                    m_input.GetAxis("Look Horizontal"),
                    m_input.GetAxis("Look Vertical"));
                camera.SetLookDelta(lookInput);

                UpdateInteraction();

                UpdateHand(unit.inventory.primary);

                // Set Reticle Position
                hud.SetReticlePos(hud.center);

                // Set Health Bar Value
                hud.SetHealth(unit.health.fillAmount);

                // Select Item (0-9)
                for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
                {
                    if (Input.GetKeyDown(key))
                    {
                        selectInput = (key == KeyCode.Alpha0)
                            ? (9)
                            : (8 - Mathf.Abs((int)key - (int)KeyCode.Alpha9));

                        if (!unit.inventory.Equip(unit.inventory.primary, selectInput))
                        {
                            unit.inventory.Store(unit.inventory.primary);
                        }

                        hud.inventory.ShowForSeconds(m_inventoryTime);

                        break;
                    }
                }

                // Scroll m_input
                if ((scrollInput = m_input.GetAxis("Select Scroll")) != 0f)
                {
                    if (scrollInput < 0f)
                    {
                        selectInput = (selectInput < unit.inventory.count - 1)
                            ? (selectInput + 1)
                            : (0);
                    }
                    else if (scrollInput > 0f)
                    {
                        selectInput = (selectInput > 0)
                            ? (selectInput - 1)
                            : (unit.inventory.count - 1);
                    }

                    if (!unit.inventory.Equip(unit.inventory.primary, selectInput))
                    {
                        unit.inventory.Store(unit.inventory.primary);
                    }

                    hud.inventory.ShowForSeconds(m_inventoryTime);
                }

                hud.inventory.RefreshItems(unit.inventory);
                hud.inventory.index = selectInput;
            }

            base.Update();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(camera.ray.origin, camera.ray.direction * interactRange);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        private void DetectController()
        {
            Rewired.Controller controller;
            if ((controller = m_input.controllers.GetLastActiveController()) != null)
            {
                switch (controller.type)
                {
                case Rewired.ControllerType.Keyboard:
                case Rewired.ControllerType.Mouse:
                m_hasKeyboard = true;
                m_hasGamepad = false;
                break;

                case Rewired.ControllerType.Joystick:
                m_hasKeyboard = false;
                m_hasGamepad = true;
                break;

                default:
                m_hasKeyboard = false;
                m_hasGamepad = false;
                break;
                }
            }
            else
            {
                m_hasKeyboard = false;
                m_hasGamepad = false;
            }
        }

        private void UpdateInteraction()
        {
            if (Physics.Raycast(camera.ray, out m_hit, interactRange))
            {
                switch (m_hit.transform.tag)
                {
                case Item.Tag:
                {
                    Item item;
                    if ((item = m_hit.transform.GetComponent<Item>()) && !item.owner)
                    {
                        //hud.ShowReticle(false);
                        hud.SetInfoText(string.Format("(E) Take {0}",
                            item.info.name));

                        if (m_input.GetButtonDown("Interact"))
                        {
                            if (unit.inventory.primary.item && unit.inventory.Store(item))
                            {
                                hud.inventory.ShowForSeconds(m_inventoryTime);
                                hud.textFeed.Print("+" + item.info.name);
                            }
                            else if (unit.inventory.Equip(unit.inventory.primary, item))
                            {
                                hud.inventory.ShowForSeconds(m_inventoryTime);
                                hud.textFeed.Print("+" + item.info.name);
                            }
                        }
                    }
                }
                break;
                case Wall.DoorTag:
                {
                    Wall wall;
                    if ((wall = m_hit.transform.parent.GetComponent<Wall>()))
                    {
                        if (wall.isDoor && !wall.isOpen)
                        {
                            //hud.ShowReticle(false);
                            hud.SetInfoText(string.Format("(E) Open"));

                            if (m_input.GetButtonDown("Interact"))
                            {
                                wall.Open();
                            }
                        }
                    }
                }
                break;
                default:
                {
                    //hud.ShowReticle(true);
                    hud.SetInfoText("");
                }
                break;
                }
            }
            else
            {
                //hud.ShowReticle(true);
                hud.SetInfoText("");
            }
        }

        private void UpdateHand(UnitInventory.Hand hand)
        {
            Item item;
            if (item = hand.item)
            {
                WeaponBase w;
                if ((w = item as WeaponBase))
                {
                    hud.SetReticle(w.shotDelta);
                }
                else
                {
                    hud.SetReticle(0f);
                }

                item.transform.localPosition = item.holdPos.localPosition;

                item.UpdatePrimary(
                    m_input.GetButtonDown("Use Item Primary"),
                    m_input.GetButton("Use Item Primary"),
                    m_input.GetButtonUp("Use Item Primary"));

                item.UpdateSecondary(
                    m_input.GetButtonDown("Use Item Secondary"),
                    m_input.GetButton("Use Item Secondary"),
                    m_input.GetButtonUp("Use Item Secondary"));

                if (m_input.GetButtonDown("Drop"))
                {
                    if (unit.inventory.Drop(hand))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);
                        hud.textFeed.Print("-" + item.info.name);

                        if(unit.inventory.Equip(hand, selectInput))
                        {
                            return;
                        }
                    }
                }

                if (m_input.GetButtonDown("Store"))
                {
                    if (unit.inventory.Store(hand))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);
                    }
                }
            }
            else
            {
                hud.SetReticle(0f);

                if (m_input.GetButtonDown("Store"))
                {
                    if (unit.inventory.Equip(unit.inventory.primary, selectInput))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);
                    }
                }
            }
        }


        /* Interfaces
        * * * * * * * * * * * * * * * */
        public void OnDoDamage(UnitEvent unitEvent)
        {
            hud.ShowHitmaker();
        }
    }
}

