using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [ExecuteInEditMode]
    public class Wall : MonoBehaviour
    {
        public const string DoorTag = "Door";

        /*  Variables
        * * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] GameObject m_wall;
        [SerializeField] GameObject m_door;
        [SerializeField] GameObject m_frame;

        [SerializeField] bool m_isDoor;
        [SerializeField] bool m_isOpen;
        [SerializeField] float m_doorSpeed = 1f;
        [SerializeField] Vector3 m_openPosition = Vector3.down;
        [SerializeField] Vector3 m_closedPosition = Vector3.zero;

        [Header("Runtime")]
        [SerializeField] Room m_room;
        [SerializeField] int m_siblingIndex;
        [SerializeField] Wall m_destWall;
        [SerializeField] Vector3 m_localScale;


        /*  Properties
        * * * * * * * * * * * * * * * * */
        public GameObject wall
        {
            get { return m_wall; }
            private set { m_wall = value; }
        }

        public GameObject door
        {
            get { return m_door; }
            private set { m_door = value; }
        }

        public GameObject frame
        {
            get { return m_frame; }
            private set { m_frame = value; }
        }


        public bool isDoor
        {
            get { return m_isDoor; }
            set { m_isDoor = value; }
        }

        public bool isOpen
        {
            get { return m_isOpen; }
            set { m_isOpen = value; }
        }

        public float doorSpeed
        {
            get { return m_doorSpeed; }
            set { m_doorSpeed = value; }
        }

        public Vector3 openPosition
        {
            get { return m_openPosition; }
            set { m_openPosition = value; }
        }

        public Vector3 closedPosition
        {
            get { return m_closedPosition; }
            set { m_closedPosition = value; }
        }

        public Vector3 doorPosition
        {
            get { return isOpen ? openPosition : closedPosition; }
        }

        public Wall targetWall
        {
            get { return m_destWall; }
            set { m_destWall = value; }
        }

        public Room room
        {
            get
            {
                if (!m_room)
                {
                    m_room = GetComponentInParent<Room>();
                }
                return m_room;
            }
        }

        public int siblingIndex
        {
            get { return (m_siblingIndex = transform.GetSiblingIndex()); }
        }

        public Vector3 localScale
        {
            get { return m_localScale; }
            set { SetLocalScale(value); }
        }


        /*  Core
        * * * * * * * * * * * * * * * * */
        private void Update()
        {
            if (!wall || !door || !frame) // hand holding
            {
                Transform temp;

                if (!wall && (temp = transform.Find("Wall")))
                {
                    wall = temp.gameObject;
                }
                else
                {
                    Debug.LogError("Wall Object Not Found", this);
                    return;
                }

                if (!door && (temp = transform.Find("Door")))
                {
                    door = temp.gameObject;
                }
                else
                {
                    Debug.LogError("Door Object Not Found", this);
                    return;
                }

                if (!frame && (temp = transform.Find("Frame")))
                {
                    frame = temp.gameObject;
                }
                else
                {
                    Debug.LogError("Frame Object Not Found", this);
                    return;
                }
            }

            if (Application.isPlaying)
            {
                isDoor = (targetWall != null);

                transform.name = "Wall (" + siblingIndex + ")";

                if (targetWall)
                {
                    transform.name += " >> [" + targetWall.siblingIndex + "]";
                }

                door.transform.localPosition = Vector3.MoveTowards(
                    door.transform.localPosition,
                    doorPosition,
                    Time.deltaTime * doorSpeed);
            }
            else
            {
                door.transform.localPosition = doorPosition;
            }

            SetActive(wall, !isDoor);
            SetActive(door, isDoor);
            SetActive(frame, isDoor);

            if (room && (localScale != room.wallSize))
            {
                localScale = room.wallSize;
            }
        }


        /*  Functions
        * * * * * * * * * * * * * * * * */
        private void SetActive(GameObject target, bool value)
        {
            target.GetComponent<MeshRenderer>().enabled = value;
            target.GetComponent<Collider>().enabled = value;
        }

        private void SetLocalScale(Vector3 value)
        {
            m_localScale = value;
            wall.transform.localScale = value;
            frame.transform.localScale = value;
        }


        public void Open()
        {
            if (!isOpen)
            {
                isOpen = true;

                if (targetWall)
                {
                    targetWall.isOpen = true;
                }
            }
        }
    }
}
