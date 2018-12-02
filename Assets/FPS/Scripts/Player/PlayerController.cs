using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(PlayerAudio))]
    [ExecuteInEditMode]
    public class PlayerController : UnitController
        , IDamageSource
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] PlayerAudio        m_audio;
        [SerializeField] PlayerCamera       m_camera;
        [SerializeField] PlayerHUD          m_hud;
        [SerializeField] PlayerInventory    m_inventory;
        [SerializeField] float              m_interactRange = 2.5f;
        [SerializeField] string             m_startingItem = "";
        [SerializeField] float              m_inventoryTime = 2.5f;

        [Header("Runtime")]
        [SerializeField] Vector2    m_moveInput;
        [SerializeField] bool       m_sprintInput;
        [SerializeField] Vector2    m_lookInput;
        [SerializeField] float      m_scrollInput;
        [SerializeField] int        m_selectInput = 0;

        private RaycastHit m_hit;


        /* Properties
        * * * * * * * * * * * * * * * */
        public new PlayerAudio audio
        {
            get { return m_audio; }
        }

        public new PlayerCamera camera
        {
            get { return m_camera; }
        }

        public PlayerHUD hud
        {
            get { return m_hud; }
        }
        
        public PlayerInventory inventory
        {
            get { return m_inventory; }
        }

        
        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            if(Application.isPlaying)
            {
                agent.enabled = false;

                if (!camera)
                {
                    Debug.LogError("Script Not Found: PlayerCamera", gameObject);
                }

                if(!hud)
                {
                    Debug.LogError("Script Not Found: PlayerHUD", gameObject);
                }

                if(!inventory)
                {
                    Debug.LogError("Script Not Found: PlayerInventory", gameObject);
                }
                else if(ItemDatabase.instance)
                {
                    Item item;
                    if(ItemDatabase.instance.GetPrefab(m_startingItem, out item))
                    {
                        inventory.Equip(inventory.primary, Instantiate(item));
                    }
                }
            }
        }

        private void Update()
        {
            if(Application.isPlaying)
            {
                // Scroll Input
                m_scrollInput = Input.GetAxis("Mouse ScrollWheel");

                // Cursor Lock
                if (Input.GetButtonDown("Cancel"))
                {
                    camera.cursorLock = !camera.cursorLock;
                }

                // Move
                m_moveInput = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));

                // Sprint
                m_sprintInput = Input.GetButton("Sprint");

                motor.Move(m_moveInput, moveSpeed * (m_sprintInput ? 2f : 1f));

                // Aim
                m_lookInput = new Vector2(
                    Input.GetAxis("Mouse X"),
                    Input.GetAxis("Mouse Y"));
                camera.SetLookDelta(m_lookInput);

                // Jump
                if (Input.GetButtonDown("Jump"))
                {
                    motor.Jump(jumpHeight);
                }


                // Interaction
                if (Physics.Raycast(camera.ray, out m_hit, m_interactRange))
                {
                    if (m_hit.transform.tag == Item.Tag)
                    {
                        Item item;
                        if ((item = m_hit.transform.GetComponent<Item>()) && !item.owner)
                        {
                            hud.ShowReticle(false);
                            hud.SetInfoText(string.Format("(E) Take {0}",
                                item.info.name));

                            if (Input.GetButtonDown("Interact"))
                            {
                                if (inventory.primary.item && inventory.Store(item))
                                {
                                    hud.inventory.ShowForSeconds(m_inventoryTime);

                                    hud.textFeed.Print("+" + item.info.name);
                                }
                                else if (inventory.Equip(inventory.primary, item))
                                {
                                    hud.inventory.ShowForSeconds(m_inventoryTime);

                                    hud.textFeed.Print("+" + item.info.name);
                                }
                            }
                        }
                    }
                    else if (m_hit.transform.tag == Wall.DoorTag)
                    {
                        Wall wall;
                        if ((wall = m_hit.transform.parent.GetComponent<Wall>()))
                        {
                            if (wall.isDoor && !wall.isOpen)
                            {
                                hud.ShowReticle(false);
                                hud.SetInfoText(string.Format("(E) Open"));

                                if (Input.GetButtonDown("Interact"))
                                {
                                    wall.Open();
                                }
                            }
                        }
                    }
                }
                else
                {
                    hud.ShowReticle(true);
                    hud.SetInfoText("");
                }

                // Item Use
                UpdateHand(inventory.primary);

                // Set Reticle Position
                hud.SetReticlePos(hud.center);

                // Set Health Bar Value
                hud.SetHealth(unit.health.fillAmount);


                for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
                {
                    if (Input.GetKeyDown(key))
                    {
                        m_selectInput = (key == KeyCode.Alpha0)
                            ? (inventory.capacity - 1) 
                            : (inventory.capacity - Mathf.Abs((int)key - (int)KeyCode.Alpha9) - 2);

                        if (!inventory.Equip(inventory.primary, m_selectInput))
                        {
                            inventory.Store(inventory.primary);
                        }
                        hud.inventory.ShowForSeconds(m_inventoryTime);

                        break;
                    }
                }

                if(m_scrollInput != 0f)
                {
                    if (m_scrollInput < 0f)
                    {
                        if (m_selectInput < inventory.capacity - 1)
                            m_selectInput++;
                        else
                            m_selectInput = 0;
                    }
                    else if (m_scrollInput > 0f)
                    {
                        if (m_selectInput > 0)
                            m_selectInput--;
                        else
                            m_selectInput = inventory.capacity - 1;
                    }

                    if (!inventory.Equip(inventory.primary, m_selectInput))
                    {
                        inventory.Store(inventory.primary);
                    }
                    hud.inventory.ShowForSeconds(m_inventoryTime);
                }

                hud.inventory.RefreshItems(inventory);
                hud.inventory.index = m_selectInput;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(camera.ray.origin, camera.ray.direction * m_interactRange);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        private void UpdateHand(PlayerInventory.Hand hand)
        {
            Item item;
            if(item = hand.item)
            {
                item.transform.localPosition = item.holdPos;

                item.UpdatePrimary("Mouse Left");

                item.UpdateSecondary("Mouse Right");

                if (Input.GetButtonDown("Drop"))
                {
                    if(inventory.Drop(hand))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);

                        hud.textFeed.Print("-" + item.info.name);
                    }
                }

                if(Input.GetButtonDown("Store"))
                {
                    if(inventory.Store(hand))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);
                    }
                }
            }
            else if (Input.GetButtonDown("Store"))
            {
                if (inventory.Equip(inventory.primary, m_selectInput))
                {
                    hud.inventory.ShowForSeconds(m_inventoryTime);
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
