using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL
{
    [ExecuteInEditMode]
    public class Room : MonoBehaviour
    {
        public const int MaxWalls = 6;

        [Serializable]
        public struct Link
        {
            public Wall first;
            public Wall second;
        }

        /*  Variables
        * * * * * * * * * * * * * * * * */
        [Header("Settings")]
        [SerializeField] float      m_roomSize      = 30f;
        [SerializeField] GameObject m_ground;
        [SerializeField] float      m_groundScale   = 1.125f;
        [SerializeField] Transform  m_wallsParent;
        [SerializeField] Wall       m_wallPrefab;
        [SerializeField] Vector3    m_wallAspect    = new Vector3(36f, 5f, 2f);
        [SerializeField] float      m_wallScale     = 100f;
        [SerializeField] float      m_wallAngle     = -60f;
        [SerializeField] float      m_wallOffset    = -90f;
        [SerializeField] float      m_wallY         = 3.5f;
        [SerializeField] float      m_wallHeight    = 5f;

        [Header("Runtime")]
        [SerializeField] Wall[]     m_wallArray;
        [SerializeField] List<Link> m_linkList;
        [SerializeField] List<Room> m_roomList;
        [SerializeField] Material[] m_materials;
        Dictionary<Wall, Wall>      m_linkMap = new Dictionary<Wall, Wall>();


        /*  Properties
        * * * * * * * * * * * * * * * * */
        public float roomSize
        {
            get { return m_roomSize; }
        }

        public GameObject ground
        {
            get { return m_ground; }
        }

        public float groundScale
        {
            get { return m_groundScale; }
        }

        public Transform wallsParent
        {
            get { return m_wallsParent; }
        }

        public Wall wallPrefab
        {
            get { return m_wallPrefab; }
        }

        public Vector3 wallAspect
        {
            get { return m_wallAspect; }
        }

        public float wallScale
        {
            get { return m_wallScale; }
        }

        public float wallAngle
        {
            get { return m_wallAngle; }
        }

        public float wallOffset
        {
            get { return m_wallOffset; }
        }

        public float wallY
        {
            get { return m_wallY; }
        }


        public Vector3 wallSize
        {
            get { return wallAspect * wallScale; }
        }

        public int childCount
        {
            get { return wallsParent ? wallsParent.childCount : 0; }
        }

        public Wall[] wallArray
        {
            get { return m_wallArray; }
            private set { m_wallArray = value; }
        }

        public Dictionary<Wall, Wall> linkMap
        {
            get { return m_linkMap; }
        }

        public List<Link> linkList
        {
            get { return m_linkList; }
        }

        public List<Room> roomList
        {
            get { return m_roomList; }
        }

        public Vector3[] wallDirections
        {
            get
            {
                Vector3[] dir = new Vector3[MaxWalls];

                for (int i = 0; i < MaxWalls; i++)
                {
                    float radian = m_wallOffset + (m_wallAngle * i) * Mathf.Deg2Rad;

                    dir[i] = new Vector3(Mathf.Cos(radian), 0f, Mathf.Sin(-radian));
                }

                return dir;
            }
        }

        public Vector3[] wallPositions
        {
            get
            {
                Vector3[] pos = wallDirections.ToArray();

                for (int i = 0; i < pos.Length; i++)
                {
                    pos[i] = transform.localPosition + pos[i] * roomSize + (Vector3.up * m_wallY);
                }

                return pos;
            }
        }

        


        /*  Core
        * * * * * * * * * * * * * * * * */
        private void Start()
        {
            RefreshWalls();

            RefreshGround();
        }

        private void Update()
        {
            if (wallArray == null || wallArray.Length != childCount)
            {
                RefreshWalls();
            }

            RefreshGround();

            for(int i = 0, imax = wallArray.Length; i < imax; i++)
            {
                UpdateWall(wallArray[i], i);
            }
        }

        private void LateUpdate()
        {
            if (Application.isPlaying)
            {
                UpdateConnections();
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 origin = transform.position + Vector3.up;

            Gizmos.color = Color.cyan * 0.75f;

            Gizmos.DrawWireSphere(origin, roomSize * m_groundScale);

            foreach (Vector3 dir in wallDirections)
            {
                Gizmos.DrawRay(origin, dir * roomSize * m_groundScale);
            }

            foreach (Vector3 pos in wallPositions)
            {
                Gizmos.DrawWireSphere(pos, 0.1f);
                Gizmos.DrawRay(pos, Vector3.down * wallY);
            }

            foreach (Wall wall in wallArray)
            {
                if (wall && wall.targetWall)
                {
                    Gizmos.color = wall.isOpen ? Color.green : Color.red;

                    Gizmos.DrawLine(
                        origin + (Vector3.up * wallY),
                        wall.transform.position);
                }
            }
        }


        /*  Functions
        * * * * * * * * * * * * * * * * */
        public void Clear()
        {
            if (wallArray == null)
                return;

            foreach (Wall wall in wallArray)
            {
                if (wall)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(wall.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(wall.gameObject);
                    }
                }
            }

            wallArray = null;
        }

        public void Generate()
        {
            if (!wallPrefab)
            {
                Debug.LogError("No wall prefab specified");
                return;
            }

            if (wallArray != null)
            {
                Clear();
            }

            wallArray = new Wall[MaxWalls];

            for (int i = 0; i < MaxWalls; i++)
            {
                wallArray[i] = CreateWall(i);
            }

            RefreshWalls();
        }

        public void OpenDoors(bool value)
        {
            //Debug.Log((value ? "Opening " : " Closing ") + " Doors");

            foreach (Wall w in wallArray)
            {
                if (w.targetWall)
                {
                    w.isOpen = value;
                    w.targetWall.isOpen = value;
                }
            }
        }


        private Wall CreateWall(int index)
        {
            if (!wallPrefab)
                return null;

            Wall wall = Instantiate(wallPrefab, wallsParent);

            wall.gameObject.name = wallPrefab.gameObject.name;

            UpdateWall(wall, index);

            return wall;
        }

        private void UpdateWall(Wall wall, int index)
        {
            Vector3 dir = wallDirections[index];
            Vector3 pos = wallPositions[index];
            wall.transform.position = pos;
            wall.transform.rotation = Quaternion.LookRotation(dir);
            wall.localScale = wallSize;
        }

        private void UpdateConnections()
        {
            if (linkList.Count != linkMap.Count)
            {
                linkList.Clear();
                roomList.Clear();

                foreach (var pair in linkMap)
                {
                    Link c = new Link
                    {
                        first = pair.Key,
                        second = pair.Value
                    };

                    linkList.Add(c);

                    Room a = c.first.room;
                    Room b = c.second.room;

                    if (a != this && !roomList.Contains(a))
                    {
                        roomList.Add(a);
                    }

                    if (b != this && !roomList.Contains(b))
                    {
                        roomList.Add(b);
                    }
                }
            }
        }


        public void RefreshWalls()
        {
            wallArray = wallsParent.GetComponentsInChildren<Wall>();

            for (int i = 0; i < wallArray.Length; i++)
            {
                wallArray[i].transform.SetSiblingIndex(i);
            }
        }

        public void RefreshGround()
        {
            if (ground)
            {
                ground.transform.localScale = new Vector3(
                    roomSize * 2f * groundScale,
                    ground.transform.localScale.y,
                    roomSize * 2f * groundScale);
            }
        }

        public bool IndexInRange(int index)
        {
            return index >= 0 && index < childCount;
        }

        public Wall GetWall(int index)
        {
            if (IndexInRange(index))
            {
                return wallArray[index];
            }

            return null;
        }

        public Wall GetRandomWall()
        {
            List<Wall> list = GetFreeWalls();

            if (list.Count == 0)
            {
                return null;
            }

            int index = UnityEngine.Random.Range(0, list.Count);

            return list[index];
        }

        public bool HasFreeWall()
        {
            return GetFreeWalls().Count > 0;
        }

        public List<Wall> GetFreeWalls()
        {
            List<Wall> list = new List<Wall>();

            foreach (Wall w in wallArray)
            {
                if (!w.targetWall)
                {
                    list.Add(w);
                }
            }

            return list;
        }

        public int GetOppositeIndex(int index)
        {
            int count = MaxWalls / 2;

            if (index < count)
            {
                return (index + count);
            }
            else
            {
                return (index - count);
            }
        }

        public Wall GetOppositeWall(int index)
        {
            int inverse = GetOppositeIndex(index);

            return GetWall(inverse);
        }
        
    }

}
