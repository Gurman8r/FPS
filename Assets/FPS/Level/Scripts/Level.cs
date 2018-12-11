using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    [RequireComponent(typeof(NavMeshSurface))]
    [ExecuteInEditMode]
    public class Level : MonoBehaviour
    {
        public enum Mode : int
        {
            Linear = 0,
            Sprawling = 1,
        }

        public delegate void OnGenerateClbk(Room startRoom);


        /*  DS
        * * * * * * * * * * * * * * * * */
        [Serializable]
        public class Element
        {
            public string name = "";
            public Room prefab = null;
            public int min = 0;
        }

        [Serializable]
        public struct Link
        {
            public Room roomA;
            public Room roomB;
        }


        /*  Variables
        * * * * * * * * * * * * * * * * */
        private NavMeshSurface m_navMeshSurface;

        [Header("Settings")]
        [SerializeField] bool m_generateOnStart = true;
        [SerializeField] Transform m_rootTransform;
        [Space]
        [SerializeField] Mode m_generationMode = Mode.Sprawling;
        [SerializeField] Room m_firstRoom;
        [SerializeField] Room m_lastRoom;
        [SerializeField] int m_roomCount = 1;
        [SerializeField] Element[] m_roomPrefabs;

        [Header("Runtime")]
        [SerializeField] Vector3 m_centerPosition;
        [SerializeField] List<Room> m_activeRooms;
        [SerializeField] List<Vector3> m_roomPositions;
        [SerializeField] List<Room> m_allRooms;
        [SerializeField] List<Room> m_priorityRooms;
        [SerializeField] List<Link> m_roomLinks;

        /*  Properties
        * * * * * * * * * * * * * * * * */
        public NavMeshSurface navMeshSurface
        {
            get
            {
                if (!m_navMeshSurface)
                {
                    m_navMeshSurface = GetComponent<NavMeshSurface>();
                }
                return m_navMeshSurface;
            }
        }

        public int roomCount
        {
            get { return m_roomCount; }
        }

        public Vector3 centerPos
        {
            get { return m_centerPosition; }
            private set { m_centerPosition = value; }
        }

        public List<Vector3> roomPositions
        {
            get { return m_roomPositions; }
            private set { m_roomPositions = value; }
        }

        public List<Room> allRooms
        {
            get { return m_allRooms; }
            private set { m_allRooms = value; }
        }

        public List<Room> priorityRooms
        {
            get { return m_priorityRooms; }
            private set { m_priorityRooms = value; }
        }

        public List<Link> roomLinks
        {
            get { return m_roomLinks; }
            private set { m_roomLinks = value; }
        }

        public List<Room> activeRooms
        {
            get { return m_activeRooms; }
            private set { m_activeRooms = value; }
        }


        /*  Core
        * * * * * * * * * * * * * * * * */
        private void Start()
        {
            if (Application.isPlaying)
            {
                if (m_generateOnStart)
                {
                    Build();
                }
            }
        }


        /*  Functions
        * * * * * * * * * * * * * * * * */
        public void Build()
        {
            Clear();

            Reload();

            Room origin = null;

            if (m_firstRoom)
            {
                origin = CreateBranch(null, m_firstRoom);
                origin.name = "[First] " + origin.name;
                RegisterRoom(origin);
            }

            GenerateFromList(origin, allRooms, roomCount);

            GenerateAllFromList(origin, priorityRooms);

            if (m_lastRoom)
            {
                origin = GetFarthestRoom(GetFirstRoom());

                if (!origin || !origin.HasFreeWall())
                {
                    origin = GetRandomFreeRoom();
                }

                Room end;
                do
                {
                    end = CreateBranch(origin, m_lastRoom);
                }
                while (!end);

                end.name = "[Last] " + end.name;

                RegisterRoom(end);
            }

            UpdatePositions();

            navMeshSurface.BuildNavMesh();
        }

        public void Clear()
        {
            foreach (Room room in activeRooms)
            {
                if (room)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(room.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(room.gameObject);
                    }
                }
            }

            for (int i = m_rootTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = m_rootTransform.GetChild(i);

                if (child && child.GetComponent<Room>())
                {
                    if (Application.isPlaying)
                    {
                        Destroy(child.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }

            roomPositions = new List<Vector3>();
            roomLinks = new List<Link>();

            activeRooms.Clear();
            UpdatePositions();

            // Clear NavMeshSurface
            navMeshSurface.RemoveData();
            navMeshSurface.navMeshData = null;
        }

        public void Reload()
        {
            allRooms = new List<Room>();
            priorityRooms = new List<Room>();

            foreach (Element elem in m_roomPrefabs)
            {
                if (elem == null || !elem.prefab)
                    continue;

                if (elem.name == "" && elem.prefab)
                {
                    elem.name = elem.prefab.name;
                }

                if (elem.min == 0)
                {
                    allRooms.Add(elem.prefab);
                }
                else
                {
                    for (int i = 0; i < elem.min; i++)
                    {
                        priorityRooms.Add(elem.prefab);
                    }
                }
            }
        }


        private void GenerateFromList(Room origin, List<Room> list, int count)
        {
            if (list == null || list.Count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                Room prefab = ChooseRoomFromList(list);
                Room room = CreateBranch(origin, prefab);

                if (room)
                {
                    RegisterRoom(room);
                }
                else
                {
                    i--;
                }

                origin = GetOriginRoom();
            }
        }

        private void GenerateAllFromList(Room origin, List<Room> list)
        {
            if (list == null || list.Count == 0)
                return;

            List<Room> copy = list.ToList();

            while (copy.Count > 0)
            {
                Room prefab = ChooseRoomFromList(copy);
                Room room = CreateBranch(origin, prefab);

                if (room)
                {
                    RegisterRoom(room);
                    copy.Remove(prefab);
                }

                origin = GetOriginRoom();
            }
        }

        private void UpdatePositions()
        {
            centerPos = m_rootTransform.position;

            if (roomPositions.Count > 0)
            {
                roomPositions.ForEach(v => centerPos += v);

                centerPos /= roomPositions.Count;
            }
        }


        private Room CreateBranch(Room origin, Room prefab)
        {
            // No prefab, something went wrong
            if (!prefab)
            {
                Debug.LogError("No room prefab specified");
                return null;
            }

            // No origin, create starting room
            if (!origin)
            {
                return CreateRoom(prefab);
            }

            // Check if origin has free walls
            if (!origin.HasFreeWall())
            {
                Debug.LogError("Origin has no free walls");
                return null;
            }

            // Get random wall from origin
            Wall firstWall = origin.GetRandomWall();
            if (!firstWall || firstWall.targetWall)
            {
                Debug.LogError("First wall already has a connection");
                return null;
            }

            // Get spawn position
            Vector3 position = GetRoomSpawnPosition(origin, firstWall);
            if (!IsPositionFree(position))
            {
                //Debug.Log("Position is not free");
                return null;
            }

            // Create room prefab
            Room room = CreateRoom(prefab);

            // Get wall to connect to first
            Wall secondWall = room.GetOppositeWall(firstWall.siblingIndex);
            if (secondWall.targetWall)
            {
                Debug.LogError("Second wall already has a connection");
                Destroy(room.gameObject);
                return null;
            }

            // Make connection between walls
            MakeConnection(firstWall, secondWall);

            // Set room position
            room.transform.position = position;

            return room;
        }

        private Room CreateRoom(Room prefab)
        {
            if (!prefab)
                return null;

            Room room = Instantiate(prefab, m_rootTransform);

            room.gameObject.SetActive(true);

            room.gameObject.name = prefab.gameObject.name;

            return room;
        }

        private Room ChooseRoomFromList(List<Room> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        private Room GetOriginRoom()
        {
            switch (m_generationMode)
            {
            case Mode.Linear:
            return GetLastRoom();

            case Mode.Sprawling:
            default:
            return GetRandomFreeRoom();
            }
        }

        private Room GetRoom(int index)
        {
            return activeRooms[index];
        }

        private Room GetRandomRoom()
        {
            return GetRoom(UnityEngine.Random.Range(0, activeRooms.Count));
        }

        private Room GetRandomFreeRoom()
        {
            Room room;
            do
            {
                room = GetRandomRoom();
            }
            while (!room || !room.HasFreeWall());

            return room;
        }

        private Room GetNearestRoom(Room origin)
        {
            Room found = null;
            float near = float.MaxValue;

            foreach (Room room in activeRooms)
            {
                if (room == origin)
                    continue;

                float dist = Vector3.Distance(
                    origin.transform.position,
                    room.transform.position);

                if (dist < near)
                {
                    found = room;
                    near = dist;
                }
            }

            return found;
        }

        private Room GetFarthestRoom(Room origin)
        {
            Room found = null;
            float far = float.MinValue;

            foreach (Room room in activeRooms)
            {
                if (room == origin)
                    continue;

                float dist = Vector3.Distance(
                    origin.transform.position,
                    room.transform.position);

                if (dist > far)
                {
                    found = room;
                    far = dist;
                }
            }

            return found;
        }

        private Room GetFirstRoom()
        {
            if (activeRooms == null || activeRooms.Count == 0)
            {
                return null;
            }

            return activeRooms.First();
        }

        private Room GetLastRoom()
        {
            if (activeRooms == null || activeRooms.Count == 0)
            {
                return null;
            }

            return activeRooms.Last();
        }


        private void RegisterRoom(Room room)
        {
            roomPositions.Add(room.transform.position);
            activeRooms.Add(room);
        }

        private Vector3 GetRoomSpawnPosition(Room origin, Wall wall)
        {
            float spawnOffset = origin.roomSize + origin.wallAspect.z;

            Vector3 dir = (wall.transform.position - origin.transform.position).normalized;

            Vector3 pos = (wall.transform.position + (dir * spawnOffset));

            pos.y = 0f;

            return pos;
        }

        private bool IsPositionFree(Vector3 position)
        {
            if (roomPositions.Contains(position))
            {
                return false;
            }
            if (Physics.CheckSphere(position, 1.0f))
            {
                return false;
            }
            return true;
        }

        private void MakeConnection(Wall a, Wall b)
        {
            if (!a || !b)
            {
                Debug.LogError("Failed to create connection (one or more walls are null)");
                return;
            }

            if (!a.room || !b.room)
            {
                Debug.LogError("Failed to create connection (one or more rooms are null)");
                return;
            }

            a.targetWall = b;
            b.targetWall = a;

            a.room.linkMap[a] = b;
            b.room.linkMap[b] = a;

            a.room.roomList.Add(b.room);
            b.room.roomList.Add(a.room);

            roomLinks.Add(new Link
            {
                roomA = a.room,
                roomB = b.room
            });
        }

    }
}
