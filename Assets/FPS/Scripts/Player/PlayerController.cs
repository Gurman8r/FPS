using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS
{
    [ExecuteInEditMode]
    public class PlayerController : UnitController
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Player Settings")]
        [SerializeField] PlayerCamera   m_camera;
        [SerializeField] PlayerHUD      m_hud;
        [Space]
        [SerializeField] int            m_playerIndex = 0;
        [SerializeField] float          m_inventoryTime = 2.5f;
        [SerializeField] float          m_reticleSpeed = 10f;
        [SerializeField] float          m_fadeSpeed = 10f;
        [SerializeField] float          m_zoomSpeed = 5f;
        [Range(0.00001f, 1f)]
        [SerializeField] float          m_aimSpeed = 0.5f;

        [Header("Player Runtime")]
        [SerializeField] bool       m_isPaused;
        [SerializeField] bool       m_isAiming;
        [SerializeField] float      m_zoomLevel;
        [SerializeField] bool       m_hasKeyboard;
        [SerializeField] bool       m_hasGamepad;
        [SerializeField] bool       m_canInteract;

        private Rewired.Player  m_input;
        private RaycastHit      m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new PlayerCamera camera
        {
            get
            {
                if(!m_camera)
                {
                    m_camera = GetComponentInChildren<PlayerCamera>();
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
                    if (m_input.GetButtonDown("Restart"))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }

                    // Move
                    moveInput = new Vector2(
                        m_input.GetAxis("Move Horizontal"),
                        m_input.GetAxis("Move Vertical"));

                    // Sprint
                    sprintInput = m_input.GetButton("Sprint") && !m_isAiming;

                    // Jump
                    jumpInput = m_input.GetButtonDown("Jump");

                    // Aim
                    lookInput = new Vector2(
                        m_input.GetAxis("Look Horizontal"),
                        m_input.GetAxis("Look Vertical"));

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
                                hud.inventory.ShowForSeconds(m_inventoryTime);
                                break;
                            }
                        }

                        // Scroll Input
                        if ((scrollInput = m_input.GetAxis("Select Scroll")) != 0f)
                        {
                            hud.inventory.ShowForSeconds(m_inventoryTime);
                        }
                    }

                    // Check Interaction
                    if (m_canInteract = Physics.Raycast(camera.ray, out m_hit, interactRange))
                    {
                        if(!CheckInteraction(m_hit))
                        {
                            hud.reticle.SetText("");
                        }
                    }
                    else
                    {
                        hud.reticle.SetText("");
                    }

                    UpdateItem(unit.inventory.primary);
                }
                else
                {
                    moveInput = Vector2.zero;
                    sprintInput = false;
                    jumpInput = false;
                    lookInput = Vector2.zero;
                }

                // Update Camera
                camera.cursorLock = !isPaused;
                camera.lookSpeed = m_isAiming ? m_aimSpeed : 1f;
                camera.SetLookDelta(lookInput);
                camera.zoomLevel = Mathf.Lerp(
                    camera.zoomLevel,
                    m_zoomLevel,
                    Time.deltaTime * m_zoomSpeed);

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

        private bool CheckInteraction(RaycastHit value)
        {
            switch (value.transform.tag)
            {
            case Item.Tag:
            {
                Item item;
                if ((item = value.transform.GetComponent<Item>()) && item.interactable && !item.owner)
                {
                    if (unit.inventory.HasRoom())
                    {
                        hud.reticle.SetText("(E) Take " + item.info.name);

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
                    else
                    {
                        hud.reticle.SetText("Inventory Full");
                    }
                    return true;
                }
            }
            break;
            case Wall.DoorTag:
            {
                Wall wall;
                if ((wall = value.transform.parent.GetComponent<Wall>()) && wall.isDoor)
                {
                    hud.reticle.SetText(!wall.isOpen ? "(E) Open" : "Opening...");

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

        private void UpdateItem(UnitInventory.Hand hand)
        {
            Item item;
            if (item = hand.item)
            {
                GunBase gun;
                m_zoomLevel = (gun = item as GunBase) ? gun.zoomLevel : PlayerCamera.MinZoom;
                m_isAiming = (m_zoomLevel != PlayerCamera.MinZoom);

                //item.transform.localPosition = item.holdPos.localPosition;

                fire0.press = m_input.GetButtonDown("Fire0");
                fire0.hold = m_input.GetButton("Fire0");
                fire0.release = m_input.GetButtonUp("Fire0");
                item.UpdatePrimary(fire0);

                fire1.press = m_input.GetButtonDown("Fire1");
                fire1.hold = m_input.GetButton("Fire1");
                fire1.release = m_input.GetButtonUp("Fire1");
                item.UpdateSecondary(fire1);

                if (m_input.GetButtonDown("Drop"))
                {
                    if (unit.inventory.Drop(hand))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);
                        hud.textFeed.Print("-" + item.info.name);

                        if (unit.inventory.index >= unit.inventory.count)
                        {
                            unit.inventory.index = unit.inventory.count - 1;
                        }

                        if (unit.inventory.Equip(hand, unit.inventory.index))
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
                m_isAiming = false;
                m_zoomLevel = 1f;

                if (m_input.GetButtonDown("Store"))
                {
                    if (unit.inventory.Equip(unit.inventory.primary, unit.inventory.index))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);
                    }
                }
            }
        }

        private void UpdateHUD()
        {
            Item item;
            if (item = unit.inventory.primary.item)
            {
                hud.reticle.SetFill(item.onCooldown
                        ? item.reloadDelta
                        : item.useDelta);

                hud.SetAmmo(item.resourceDelta);

                if (m_input.GetButtonDown("Reload"))
                {
                    item.Reload();
                }

                if (item.onCooldown)
                {
                    hud.reticle.SetText("Reloading");
                }

                GunBase gun;
                if (gun = item as GunBase)
                {
                    hud.reticle.sizeDelta = Vector2.Lerp(
                        hud.reticle.sizeDelta,
                        hud.reticle.originalSize + (hud.reticle.originalSize * gun.bulletSpread / 2f),
                        Time.deltaTime * m_reticleSpeed);
                }
                else
                {
                    hud.reticle.sizeDelta = Vector2.Lerp(
                        hud.reticle.sizeDelta,
                        hud.reticle.originalSize,
                        Time.deltaTime * m_reticleSpeed);
                }
            }
            else
            {
                hud.reticle.sizeDelta = Vector2.Lerp(
                    hud.reticle.sizeDelta,
                    hud.reticle.originalSize,
                    Time.deltaTime * m_reticleSpeed);

                hud.reticle.SetFill(1f);
                hud.SetAmmo(-1f);
            }

            hud.ShowMenu(isPaused);
            hud.SetHealth(unit.health.fillAmount);
            hud.inventory.Refresh(unit.inventory);

            if(inCombat && hud.healthBar.imageAlpha < 1f)
            {
                hud.healthBar.imageAlpha += Time.deltaTime * m_fadeSpeed;
            }
            else if(hud.healthBar.imageAlpha > 0f)
            {
                hud.healthBar.imageAlpha -= Time.deltaTime;
            }
        }
    }
}

