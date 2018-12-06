using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS
{
    [RequireComponent(typeof(UnitAudio))]
    [ExecuteInEditMode]
    public class PlayerController : UnitController
        , IDamageSource
        , IDamageTarget
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Player Settings")]
        [SerializeField] int            m_playerIndex = 0;
        [SerializeField] PlayerCamera   m_camera;
        [SerializeField] PlayerHUD      m_hud;
        [SerializeField] float          m_inventoryTime = 2.5f;

        [Header("Player Runtime")]
        [SerializeField] InputState m_fire0;
        [SerializeField] InputState m_fire1;
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

                //hud.reticle.ResetSize();
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

                if (camera.cursorLock)
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
                    sprintInput = m_input.GetButton("Sprint");

                    // Jump
                    jumpInput = m_input.GetButtonDown("Jump");

                    // Aim
                    lookInput = new Vector2(
                        m_input.GetAxis("Look Horizontal"),
                        m_input.GetAxis("Look Vertical"));
                    camera.SetLookDelta(lookInput);

                    UpdateInteraction();
                    UpdateItemSelection();
                    UpdateItemUsage(unit.inventory.primary);
                }
                else
                {
                    moveInput = Vector2.zero;
                    sprintInput = false;
                    jumpInput = false;
                    lookInput = Vector2.zero;
                }

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

        private void UpdateInteraction()
        {
            if (m_canInteract = Physics.Raycast(camera.ray, out m_hit, interactRange))
            {
                switch (m_hit.transform.tag)
                {
                case Item.Tag:
                {
                    Item item;
                    if ((item = m_hit.transform.GetComponent<Item>()) && !item.owner)
                    {
                        if(unit.inventory.HasRoom())
                        {
                            hud.reticle.SetText("(E) Take " + item.info.name);
                            if (m_input.GetButtonDown("Interact"))
                            {
                                if(unit.inventory.HasRoom())
                                {
                                    if (unit.inventory.primary.item && 
                                        unit.inventory.Store(item))
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
                            hud.reticle.SetText("(E) Open");

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
                    hud.reticle.SetText("");
                }
                break;
                }
            }
            else
            {
                hud.reticle.SetText("");
            }
        }

        private void UpdateItemSelection()
        {
            // Select Item (0-9)
            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    int index = (key == KeyCode.Alpha0)
                        ? (9)
                        : (8 - Mathf.Abs((int)key - (int)KeyCode.Alpha9));

                    if (index >= 0 && index < unit.inventory.list.Count)
                    {
                        unit.inventory.index = index;

                        if (!unit.inventory.Equip(unit.inventory.primary, unit.inventory.index))
                        {
                            unit.inventory.Store(unit.inventory.primary);
                        }
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

                if (!unit.inventory.Equip(unit.inventory.primary, unit.inventory.index))
                {
                    unit.inventory.Store(unit.inventory.primary);
                }

                hud.inventory.ShowForSeconds(m_inventoryTime);
            }
        }

        private void UpdateItemUsage(UnitInventory.Hand hand)
        {
            Item item;
            if (item = hand.item)
            {
                item.transform.localPosition = item.holdPos.localPosition;

                m_fire0.press = m_input.GetButtonDown("Fire0");
                m_fire0.hold = m_input.GetButton("Fire0");
                m_fire0.release = m_input.GetButtonUp("Fire0");
                item.UpdatePrimary(m_fire0);

                m_fire1.press = m_input.GetButtonDown("Fire1");
                m_fire1.hold = m_input.GetButton("Fire1");
                m_fire1.release = m_input.GetButtonUp("Fire1");
                item.UpdateSecondary(m_fire1);

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
                WeaponBase weapon;
                if ((weapon = item as WeaponBase))
                {
                    hud.reticle.SetFill(weapon.isReloading
                        ? weapon.reloadDelta
                        : weapon.shotDelta);

                    hud.SetAmmo(weapon.ammoDelta);

                    if (m_input.GetButtonDown("Reload"))
                    {
                        weapon.Reload();
                    }

                    if (weapon.isReloading)
                    {
                        hud.reticle.SetText("Reloading");
                    }

                    GunBase gun;
                    if (gun = weapon as GunBase)
                    {
                        hud.reticle.sizeDelta = Vector2.Lerp(
                            hud.reticle.sizeDelta,
                            hud.reticle.originalSize + (hud.reticle.originalSize * gun.bulletSpread / 2f),
                            Time.deltaTime * 10f);
                    }
                    else
                    {
                        hud.reticle.sizeDelta = Vector2.Lerp(
                            hud.reticle.sizeDelta,
                            hud.reticle.originalSize,
                            Time.deltaTime * 10f);
                    }
                }
                else
                {
                    hud.reticle.SetFill(0f);
                    hud.SetAmmo(0f);
                }
            }
            else
            {
                hud.reticle.sizeDelta = Vector2.Lerp(
                    hud.reticle.sizeDelta,
                    hud.reticle.originalSize,
                    Time.deltaTime * 10f);
                hud.reticle.SetFill(1f);
                hud.SetAmmo(-1f);
            }

            hud.SetPaused(!camera.cursorLock);
            hud.SetHealth(unit.health.fillAmount);
            hud.inventory.Refresh(unit.inventory);
        }


        /* Interfaces
        * * * * * * * * * * * * * * * */
        public void OnDoDamage(UnitEvent unitEvent)
        {
            hud.reticle.ShowHitmaker();
        }

        public void OnRecieveDamage(UnitEvent unitEvent)
        {
            hud.ShowTakeDamage();
        }
    }
}

