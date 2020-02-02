using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGeneration : MonoBehaviour
{

    #region Editor Variables
    [Header("Room properties")]
    [SerializeField]
    private int minNumRooms = 3;
    [SerializeField]
    private int maxNumRooms = 4;

    [SerializeField]
    private int minRoomSize = 4;
    [SerializeField]
    private int maxRoomSize = 5;

    [SerializeField]
    private int minEnemyNum = 0;
    [SerializeField]
    private int maxEnemyNum = 2;

    [SerializeField]
    private int minOrbNum = 1;
    [SerializeField]
    private int maxOrbNum = 3;

    [SerializeField]
    private float tileWidth = 0.4f;
    public float TileWidth
    {
        get { return tileWidth; }
        set { tileWidth = value; }
    }


    #endregion

    #region Private Variables

    private RoomGeneration rg;
    private TunnelGeneration tg;
    [SerializeField]
    private List<Room> rooms = new List<Room>();
    private List<GameObject> tunnels = new List<GameObject>();
    private List<GameObject> roomGO = new List<GameObject>();
    private int entrance;
    private int exit;
    private int totalSize;
    private int totalNumRooms;

    #endregion

    #region Delegates
    public delegate float GetTileWidth();
    public static event GetTileWidth ReturnTileWidth;

    #endregion

    /// <summary>
    /// The dungeon is formatted by 9 rooms where 1 is the entrance and 1 is the exit. 
    /// The rooms are numbered in this format:
    /// 0   1   2
    /// 3   4   5  
    /// 6   7   8
    /// They are connected via tunnels that connect them
    /// </summary>
    /// 
    #region Initialization

    private void Awake()
    {
        roomSetUp();
    }

    #endregion

    #region Updates

    private void Update()
    {

    }

    #endregion

    #region Room Generation

    public void roomSetUp()
    {
        rg = GetComponent<RoomGeneration>();
        totalSize = UnityEngine.Random.Range(minNumRooms, maxNumRooms + 1);
        totalNumRooms = totalSize * totalSize;

        // Select a random room for entrance generation
        List<int> availableRooms = Enumerable.Range(0, totalNumRooms).ToList();
        //entrance = availableRooms[UnityEngine.Random.Range(0, availableRooms.Count)];
        entrance = 0;

        availableRooms.Remove(entrance);
        exit = availableRooms[UnityEngine.Random.Range(0, availableRooms.Count)];

        for (int i = 0; i < totalNumRooms; i++)
        {
            Room room = new Room();
            room.RoomNum = i;
            room.RoomSize = UnityEngine.Random.Range(minRoomSize, maxRoomSize + 1);
            room.NumEnemies = UnityEngine.Random.Range(minEnemyNum, maxEnemyNum + 1);
            room.NumOrbs = UnityEngine.Random.Range(minOrbNum, maxOrbNum + 1);

            List<int> doorPos = new List<int>();
            for (int j = 0; j < 4; j++)
            {
                doorPos.Add(UnityEngine.Random.Range(1, room.RoomSize));
            }
            room.DoorPositions = doorPos;

            if (i == entrance)
            {
                room.IsEntrance = true;
            }

            if (i == exit)
            {
                room.IsExit = true;
            }

            rooms.Add(room);
        }

        for (int i = 0; i < totalNumRooms; i++)
        {
            assignNeighbors(rooms[i]);
        }

        for (int i = 0; i < totalNumRooms; i++)
        {
            roomGO.Add(rg.createRoom(rooms[i], totalSize, i));
        }

        tg = gameObject.GetComponent<TunnelGeneration>();

        for (int i = 0; i < totalNumRooms; i++)
        {
            if (rooms[i].Neighbors["east"] != null)
            {
                GameObject t = tg.createHorizontalTunnel(rooms[i], rooms[i + 1]);
                tunnels.Add(t);
            }

            if (rooms[i].Neighbors["south"] != null)
            {
                GameObject t = tg.createVerticalTunnel(rooms[i], rooms[i + totalSize]);
                tunnels.Add(t);
            }
        }

        //for (int i = 0; i < totalNumRooms; i++)
        //{
        //    rg.createEnemies(rooms[i], roomGO[i]);
        //    rg.createItems(rooms[i], roomGO[i]);
        //}
    }


    private void assignNeighbors(Room rm)
    {
        int roomNum = rm.RoomNum;
        int roomSize = totalSize;
        int roomMod = (int)Math.Floor((double)(roomNum / roomSize));
        int roomModPlus = (int)Math.Floor((double)((roomNum + 1) / roomSize));
        int roomModMinus = (int)Math.Floor((double)((roomNum - 1) / roomSize));
        if (roomNum == 0)
        {
            roomModMinus = 10;
        }
        rm.Neighbors = new Dictionary<string, Room>();
        rm.Neighbors.Add("north", roomNum - roomSize < 0 ? null : rooms[roomNum - roomSize]);
        rm.Neighbors.Add("east", roomMod != roomModPlus ? null : rooms[roomNum + 1]);
        rm.Neighbors.Add("south", roomNum + roomSize >= roomSize * roomSize ? null : rooms[roomNum + roomSize]);
        rm.Neighbors.Add("west", roomMod != roomModMinus ? null : rooms[roomNum - 1]);
    }

    private void resetRooms()
    {
        //for (int i = 0; i < roomGO.Count; i++)
        //{
        //    Destroy(roomGO[i]);
        //}


        //roomGO.Clear();
        //rooms.Clear();
        //roomSetUp();

        SceneManager.LoadScene("MapGeneration");
    }

    #endregion

    #region Tunnel Generation

    #endregion

    #region Reset Dungeon
    public void resetLists()
    {
        rooms.Clear();
        foreach(GameObject tunnel in tunnels)
        {
            Destroy(tunnel);
        }

        foreach(GameObject room in roomGO)
        {
            Destroy(room);
        }

        roomGO.Clear();
        tunnels.Clear();
    }
    #endregion
}
