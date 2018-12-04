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

            if(Application.isPlaying)
            {
                m_input = Rewired.ReInput.players.GetPlayer(m_playerIndex);

                if (!camera)
                {
                    Debug.LogError("Script Not Found: PlayerCamera", gameObject);
                }

                if(!hud)
                {
                    Debug.LogError("Script Not Found: PlayerHUD", gameObject);
                }
            }
        }

        protected override void Update()
        {
            if(Application.isPlaying)
            {
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


                // Interaction
                if (Physics.Raycast(camera.ray, out m_hit, interactRange))
                {
                    if (m_hit.transform.tag == Item.Tag)
                    {
                        Item item;
                        if ((item = m_hit.transform.GetComponent<Item>()) && !item.owner)
                        {
                            hud.ShowReticle(false);
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
                    else if (m_hit.transform.tag == Wall.DoorTag)
                    {
                        Wall wall;
                        if ((wall = m_hit.transform.parent.GetComponent<Wall>()))
                        {
                            if (wall.isDoor && !wall.isOpen)
                            {
                                hud.ShowReticle(false);
                                hud.SetInfoText(string.Format("(E) Open"));

                                if (m_input.GetButtonDown("Interact"))
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
                            ? (unit.inventory.capacity - 1) 
                            : (unit.inventory.capacity - Mathf.Abs((int)key - (int)KeyCode.Alpha9) - 2);

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
                        selectInput = (selectInput < unit.inventory.capacity - 1) 
                            ? (selectInput + 1) 
                            : (0);
                    }
                    else if (scrollInput > 0f)
                    {
                        selectInput = (selectInput > 0)
                            ? (selectInput - 1)
                            : (unit.inventory.capacity - 1);
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
        private void UpdateHand(UnitInventory.Hand hand)
        {
            Item item;
            if(item = hand.item)
            {
                item.transform.localPosition = item.holdPos.localPosition;

                item.UpdatePrimary(
                    m_input.GetButtonDown("Use Item Primary"),
                    m_input.GetButton(    "Use Item Primary"),
                    m_input.GetButtonUp(  "Use Item Primary"));

                item.UpdateSecondary(
                    m_input.GetButtonDown("Use Item Secondary"),
                    m_input.GetButton(    "Use Item Secondary"),
                    m_input.GetButtonUp(  "Use Item Secondary"));

                if (m_input.GetButtonDown("Drop"))
                {
                    if(unit.inventory.Drop(hand))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);

                        hud.textFeed.Print("-" + item.info.name);
                    }
                }

                if(m_input.GetButtonDown("Store"))
                {
                    if(unit.inventory.Store(hand))
                    {
                        hud.inventory.ShowForSeconds(m_inventoryTime);
                    }
                }
            }
            else if (m_input.GetButtonDown("Store"))
            {
                if (unit.inventory.Equip(unit.inventory.primary, selectInput))
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

