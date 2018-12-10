using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS
{
    public class PlayerUnit : HumanoidUnit
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Player Settings")]
        [SerializeField] PlayerHUD  m_hud;
        [SerializeField] FP_Camera  m_camera;
        [Space]
        [SerializeField] int        m_playerIndex = 0;
        [SerializeField] float      m_zoomSpeed = 5f;
        [Range(0.00001f, 1f)]
        [SerializeField] float      m_aimSpeed = 0.5f;
        [Space]
        [SerializeField] float      m_editHoldRange = 1f;
        [SerializeField] float      m_editRotSpeed = 2.5f;

        [Header("Player Runtime")]
        [SerializeField] Item   m_editing;
        [SerializeField] bool       m_isPaused;
        [SerializeField] bool       m_isAiming;
        [SerializeField] float      m_zoomLevel;
        [SerializeField] bool       m_hasKeyboard;
        [SerializeField] bool       m_hasGamepad;
        [SerializeField] bool       m_canInteract;
        [SerializeField] Rewired.ControllerType m_controllerType;

        private Rewired.Player  m_input;
        private RaycastHit      m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new FP_Camera camera
        {
            get
            {
                if(!m_camera)
                {
                    m_camera = GetComponentInChildren<FP_Camera>();
                }
                return m_camera;
            }
        }

        public PlayerHUD hud
        {
            get
            {
                if(!m_hud)
                {
                    m_hud = FindObjectOfType<PlayerHUD>();
                }
                return m_hud;
            }
        }

        public bool isPaused
        {
            get { return m_isPaused; }
            set { m_isPaused = value; }
        }

        public Vector3 holdPos
        {
            get { return unit.vision.origin + unit.vision.direction * m_editHoldRange; }
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
                    return;
                }

                if (!hud)
                {
                    Debug.LogError("Script Not Found: PlayerHUD", gameObject);
                    return;
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
                if (m_input.GetButtonDown("Pause"))
                {
                    isPaused = !isPaused;
                }

                if (!isPaused)
                {
                    // Restart
                    if (m_input.GetButtonDown("Restart"))
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    
                    // Move
                    moveInput = new Vector2(m_input.GetAxis("Move Horizontal"), m_input.GetAxis("Move Vertical"));
                    
                    // Sprint
                    sprintInput = m_input.GetButton("Sprint") && !m_isAiming;
                    
                    // Jump
                    jumpInput = m_input.GetButtonDown("Jump");
                    
                    // Aim
                    lookInput = new Vector2(m_input.GetAxis("Look Horizontal"), m_input.GetAxis("Look Vertical"));

                    // Not Holding
                    if((m_canInteract = !m_editing))
                    {
                        // Update Item
                        UpdateHand(unit.inventory.hand);

                        if (m_canInteract = Physics.Raycast(camera.ray, out m_hit, interactRange))
                        {
                            if (!CheckInteraction(m_hit))
                            {
                                hud.ShowActions("");
                            }
                        }
                        else
                        {
                            hud.ShowActions("");
                        }

                        if (!unit.inventory.empty)
                        {
                            // Select Input
                            selectInput = -1;
                            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
                            {
                                if (Input.GetKeyDown(key))
                                {
                                    selectInput = (key == KeyCode.Alpha0)
                                        ? (9)
                                        : (8 - Mathf.Abs((int)key - (int)KeyCode.Alpha9));
                                    hud.inventory.Show();
                                    break;
                                }
                            }

                            // Scroll Input
                            if ((scrollInput = m_input.GetAxis("Select Scroll")) != 0f)
                            {
                                hud.inventory.Show();
                            }
                        }
                    }
                    else // Holding
                    {
                        m_editing.rigidbody.position = holdPos;

                        if(m_input.GetButton("Edit"))
                        {
                            hud.ShowActions(string.Format(
                                "[{0}] {1}\n" +
                                "[{2}] {3}\n" +
                                "[{4}] {5}\n",
                                GetActionName("RotateX"), "RotateX",
                                GetActionName("RotateY"), "RotateY",
                                GetActionName("RotateZ"), "RotateZ"));

                            bool rx = m_input.GetButton("RotateX");
                            bool ry = m_input.GetButton("RotateY");
                            bool rz = m_input.GetButton("RotateZ");
                            if(rx || ry || rz)
                            {
                                float speed = m_editRotSpeed * Time.deltaTime;
                                if (rx)
                                {
                                    m_editing.transform.RotateAround(m_editing.transform.position, camera.transform.right, lookInput.y * speed);
                                }
                                if (ry)
                                {
                                    m_editing.transform.RotateAround(m_editing.transform.position, camera.transform.up, -lookInput.x * speed);
                                }
                                if (rz)
                                {
                                    m_editing.transform.RotateAround(m_editing.transform.position, camera.transform.forward, -lookInput.x * speed);
                                }
                            }
                            
                            lookInput = Vector2.zero;
                            moveInput = Vector2.zero;
                            jumpInput = false;
                            sprintInput = false;
                        }
                        else
                        {
                            hud.ShowActions(string.Format(
                                "[{0}] {1}\n" +
                                "[{2}] {3}\n" +
                                "[{4}] {5}\n",
                                GetActionName("Equip"), "Equip",
                                GetActionName("Place"), "Place",
                                GetActionName("Edit"), "Edit"));
                        }

                        if (unit.inventory.Store(unit.inventory.hand))
                        {
                            hud.inventory.Show();
                        }

                        if (m_input.GetButtonDown("Equip"))
                        {
                            if(unit.inventory.Equip(unit.inventory.hand, m_editing))
                            {
                                hud.inventory.Show();
                                m_editing = null;
                            }
                        }
                        else if (m_input.GetButtonDown("Place"))
                        {
                            SetEditItem(null);
                            if(unit.inventory.Equip(unit.inventory.hand, unit.inventory.index))
                            {
                                hud.inventory.Show();
                            }
                        }
                    }
                }
                else
                {
                    moveInput = Vector2.zero;
                    sprintInput = false;
                    jumpInput = false;
                    lookInput = Vector2.zero;
                }

                UpdateCamera();
                UpdateHUD();
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
                switch (m_controllerType = controller.type)
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

        private bool CheckInteraction(RaycastHit hit)
        {
            switch (hit.transform.tag)
            {
            case CombatConsts.ItemTag:
            {
                Item item;
                if ((item = hit.transform.GetComponent<Item>()) && item.interactable && !item.owner)
                {
                    hud.ShowActions(string.Format(
                        "[{0}] {1} {2}", 
                        GetActionName("Equip"), "Equip", item.info.name));

                    if (m_input.GetButtonLongPressDown("Equip") && !m_editing)
                    {
                        SetEditItem(item);
                    }
                    else if (m_input.GetButtonUp("Equip"))
                    {
                        if (unit.inventory.hand.item && unit.inventory.Store(item))
                        {
                            hud.inventory.Show();
                            hud.textFeed.Print("+" + item.info.name);
                        }
                        else if (unit.inventory.Equip(unit.inventory.hand, item))
                        {
                            hud.inventory.Show();
                            hud.textFeed.Print("+" + item.info.name);
                        }
                    }
                    return true;
                }
            }
            break;
            case Wall.DoorTag:
            {
                Wall wall;
                if ((wall = hit.transform.parent.GetComponent<Wall>()) && wall.isDoor)
                {
                    hud.ShowActions(string.Format(
                        "[{0}] {1}", 
                        GetActionName("Interact"), 
                        (!wall.isOpen ? "Open" : "Opening...")));

                    if (m_input.GetButtonDown("Interact"))
                    {
                        wall.Open();
                    }

                    return true;
                }
            }
            break;
            }
            return false;
        }

        private void UpdateHand(UnitInventory.Hand hand)
        {
            Item item;
            if (item = hand.item)
            {
                RangedWeapon gun;
                m_zoomLevel = (gun = item as RangedWeapon) ? gun.zoomLevel : FP_Camera.MinZoom;
                m_isAiming = (m_zoomLevel != FP_Camera.MinZoom);

                fire0.press = m_input.GetButtonDown("Fire0");
                fire0.hold = m_input.GetButton("Fire0");
                fire0.release = m_input.GetButtonUp("Fire0");
                item.HandleInputPrimary(fire0);

                fire1.press = m_input.GetButtonDown("Fire1");
                fire1.hold = m_input.GetButton("Fire1");
                fire1.release = m_input.GetButtonUp("Fire1");
                item.HandleInputSecondary(fire1);

                if (m_input.GetButtonDown("Reload"))
                {
                    item.StartCooldown();
                }

                if (m_input.GetButtonDoublePressDown("Drop"))
                {
                    if (unit.inventory.Drop(hand))
                    {
                        hud.inventory.Show();
                        hud.textFeed.Print("-" + item.info.name);
                        unit.inventory.index--;
                        SetEditItem(item);
                    }
                }
                else if(m_input.GetButtonLongPressDown("Drop"))
                {
                    if(unit.inventory.Drop(hand))
                    {
                        hud.textFeed.Print("-" + item.info.name);
                        unit.inventory.index--;
                    }
                }
            }
            else
            {
                m_isAiming = false;
                m_zoomLevel = 1f;
            }
        }

        private void UpdateCamera()
        {
            camera.cursorLock = !isPaused;
            camera.lookSpeed = m_isAiming ? m_aimSpeed : 1f;
            camera.SetLookDelta(lookInput);
            camera.zoomLevel = Mathf.Lerp(
                camera.zoomLevel,
                m_zoomLevel,
                Time.deltaTime * m_zoomSpeed);
        }

        private void UpdateHUD()
        {
            Item item;
            if (item = unit.inventory.hand.item)
            {
                hud.reticle.SetFill(item.onCooldown
                        ? item.reloadDelta
                        : item.useDelta);

                hud.SetAmmo(item.resourceDelta);

                RangedWeapon gun;
                if (gun = item as RangedWeapon)
                {
                    hud.reticle.targetSize =
                        hud.reticle.originalSize + 
                        (hud.reticle.originalSize * (gun.bulletSpread / 2f));
                }
                else
                {
                    hud.reticle.targetSize = hud.reticle.originalSize;
                }
            }
            else
            {
                hud.reticle.targetSize = hud.reticle.originalSize;
                hud.reticle.SetFill(1f);
                hud.SetAmmo(-1f);
            }

            hud.ShowMenu(isPaused);
            hud.SetHealth(unit.health.fillAmount);
            hud.inventory.Refresh(unit.inventory);

            if(inCombat && hud.healthBar.imageAlpha < 1f)
            {
                hud.healthBar.imageAlpha = 1f;
            }
            else if(hud.healthBar.imageAlpha > 0f)
            {
                hud.healthBar.imageAlpha -= Time.deltaTime;
            }
        }

        private void SetEditItem(Item value)
        {
            if(value)
            {
                m_editing = value;
                m_editing.collider.enabled = true;
                m_editing.rigidbody.isKinematic = false;
                m_editing.rigidbody.useGravity = false;
                m_editing.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else if(m_editing)
            {
                m_editing.EnablePhysics(true);
                m_editing.rigidbody.constraints = RigidbodyConstraints.None;
                m_editing = null;
            }
        }

        private string GetActionName(string value)
        {
            return m_input.controllers.maps.GetFirstButtonMapWithAction(value, true).elementIdentifierName;
        }
    }
}

