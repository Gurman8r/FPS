using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    [ExecuteInEditMode]
    public class PlayerController : UnitController
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] PlayerCamera       m_camera;
        [SerializeField] PlayerHUD          m_hud;
        [SerializeField] PlayerInventory    m_inventory;
        [SerializeField] float              m_pickupRange = 2.5f;
        [SerializeField] LayerMask          m_itemLayer;

        [Header("Runtime")]
        [SerializeField] Vector2    m_moveAxes;
        [SerializeField] Vector2    m_lookAxes;
        [SerializeField] Vector3    m_lookPos;


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
        
        public PlayerInventory inventory
        {
            get { return m_inventory; }
        }

        
        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            agent.enabled = false;

            if(Application.isPlaying && ItemDatabase.instance)
            {
                inventory.Equip(
                    Instantiate(ItemDatabase.instance.GetPrefab("Raygun")),
                    inventory.hand,
                    unit);
            }
        }

        private void Update()
        {
            if(Application.isPlaying)
            {
                // Cursor Lock
                if(Input.GetButtonDown("Cancel"))
                {
                    camera.cursorLock = !camera.cursorLock;
                }

                // Move
                m_moveAxes = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));
                motor.Move(m_moveAxes, moveSpeed);

                // Aim
                m_lookAxes = new Vector2(
                    Input.GetAxis("Mouse X"),
                    Input.GetAxis("Mouse Y"));
                camera.SetLookDelta(m_lookAxes);

                // Jump
                if (Input.GetButtonDown("Jump"))
                {
                    motor.Jump(jumpHeight);
                }


                // Item Pickup
                Ray ray = new Ray(camera.transform.position, camera.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, m_pickupRange, m_itemLayer))
                {
                    if (hit.transform.tag == Item.Tag)
                    {
                        Item item;
                        if ((item = hit.transform.GetComponent<Item>()) && !item.owner)
                        {
                            hud.ShowReticle(false);

                            hud.SetInfoText(string.Format("(E) {0} {1}", 
                                (inventory.hand.item ? "Swap" : "Pickup"),
                                item.info.name));

                            if (Input.GetButtonDown("Interact"))
                            {
                                if (inventory.hand.item)
                                {
                                    inventory.Drop(inventory.hand);
                                }

                                if (inventory.Equip(item, inventory.hand, unit))
                                {
                                    Debug.Log("Equip Item: " + item, item);
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
                UpdateHand(inventory.hand);

                Firearm fa;
                if (inventory.hand.item && (fa = inventory.hand.item as Firearm))
                {
                    hud.SetReticlePos(camera.camera.WorldToScreenPoint(fa.lookPos));
                }
                else
                {
                    hud.SetReticlePos(hud.center);
                }

                hud.SetHealth(unit.health.current / unit.health.maximum);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(
                camera.transform.position, 
                camera.transform.forward * m_pickupRange);
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

                if (Input.GetButtonDown("Drop") && inventory.Drop(hand))
                {
                    Debug.Log("Drop Item: " + item.info.name, item.gameObject);
                }

                if(Input.GetButtonDown("Stash") && inventory.Stash(hand))
                {
                    Debug.Log("Stash Item: " + item.info.name, item.gameObject);
                }
            }
        }

    }
}
