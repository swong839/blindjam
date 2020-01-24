using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{

    [Header("Room prefabs")]
    [SerializeField]
    private GameObject roomParent;
    [SerializeField]
    private GameObject stairs;

    [Header("Walls")]
    [SerializeField]
    private GameObject northWall;

    [SerializeField]
    private GameObject eastWall;

    [SerializeField]
    private GameObject southWall;

    [SerializeField]
    private GameObject westWall;

    [Header("Tunnels")]
    [SerializeField]
    private GameObject northTunnel;

    [SerializeField]
    private GameObject eastTunnel;

    [SerializeField]
    private GameObject southTunnel;

    [SerializeField]
    private GameObject westTunnel;

    [Header("Corners")]
    [SerializeField]
    private GameObject neCorner;

    [SerializeField]
    private GameObject seCorner;

    [SerializeField]
    private GameObject swCorner;

    [SerializeField]
    private GameObject nwCorner;

    [Header("Floor")]
    [SerializeField]
    private GameObject floor;

    [Header("Enemies")]
    [SerializeField]
    [Tooltip("Enemy prefabs")]
    private List<GameObject> enemies;

    [Header("Items")]
    [SerializeField]
    [Tooltip("Item prefabs")]
    private List<GameObject> items;

    private int roomNumber;
    private float roomWidth = 0.4f;

    //For items and enemies
    private List<List<GameObject>> roomPieces = new List<List<GameObject>>();
    private Vector2 entrancePos;
    private Vector2 exitPos;
    private Dictionary<int, string> directionIndex = new Dictionary<int, string>();

    private enum typeList
    {
        ICE,
        FIRE,
        LIGHTNING
    }

    private void Awake()
    {
        directionIndex.Add(0, "north");
        directionIndex.Add(1, "east");
        directionIndex.Add(2, "south");
        directionIndex.Add(3, "west");

        roomWidth = GameObject.FindGameObjectWithTag("mapGenerator").GetComponent<DungeonGeneration>().TileWidth;
    }

    public GameObject createRoom(Room room, int dungeonSize, int roomNum)
    {
        roomNumber = roomNum;
        Vector2 coord = new Vector2((room.RoomNum % dungeonSize) * 4.8f, ((int)(room.RoomNum / dungeonSize)) * -4.8f);
        GameObject rm = Instantiate(roomParent);
        rm.transform.position = coord;

        // Check and set exit positions
        exitPos = checkExit(room);

        // TOP LAYER
        createTopLayer(room, rm);

        // MIDDLE LAYERS
        for (int i = 1; i < room.RoomSize + 1; i++)
        {
            createRow(room, rm, i);
        }

        // LAST LAYER
        createLastLayer(room, rm);

        // Place items

        // Place enemies
        
        // Place entrance (and entrance location)

        return rm;
    }

    private Vector2 checkExit(Room room)
    {
        if (room.IsExit)
        {
            return new Vector2((int)Random.Range(1, room.RoomSize - 1), (int)Random.Range(1, room.RoomSize - 1));
        }
        else
        {
            return new Vector2(0, 0);
        }
    }

    private void createTopLayer(Room room, GameObject rm)
    {

        //LAYER 1
        // Create left corner piece
        GameObject corner = Instantiate(nwCorner);
        corner.transform.parent = rm.transform;
        corner.transform.localPosition = new Vector2(0, 0);
        corner.GetComponent<Tile>().RoomNumber = roomNumber;

        // Create wall pieces
        int doorPos = room.DoorPositions[0];
        for (int i = 0; i < room.RoomSize; i++)
        {
            // If this is top most room, do not create a tunnel
            if (room.Neighbors[directionIndex[0]] == null)
            {
                GameObject wall = Instantiate(northWall);
                wall.transform.parent = rm.transform;
                wall.transform.localPosition = new Vector2((i + 1) * roomWidth, 0);
                wall.GetComponent<Tile>().RoomNumber = roomNumber;
            }
            else
            {
                // Place door or wall based on predetermined variable (DungeonGeneration.Awake) 
                if (i == doorPos)
                {
                    GameObject door = Instantiate(northTunnel);
                    door.transform.parent = rm.transform;
                    door.transform.localPosition = new Vector2((i + 1) * roomWidth, 0);
                    door.GetComponent<Tile>().RoomNumber = roomNumber;
                    room.Doors["north"] = door.GetComponent<Tile>();
                }
                else
                {
                    GameObject wall = Instantiate(northWall);
                    wall.transform.parent = rm.transform;
                    wall.transform.localPosition = new Vector2((i + 1) * roomWidth, 0);
                    wall.GetComponent<Tile>().RoomNumber = roomNumber;
                }
            }
            
        }

        // Create right corner piece
        corner = Instantiate(neCorner);
        corner.transform.parent = rm.transform;
        corner.transform.localPosition = new Vector2((room.RoomSize + 1) * roomWidth, 0);
        corner.GetComponent<Tile>().RoomNumber = roomNumber;

    }

    private void createRow(Room room, GameObject rm, int row)
    {
        roomPieces.Add(new List<GameObject>());

        // Create left wall piece
        if (row == room.DoorPositions[3] && room.Neighbors[directionIndex[3]] != null)
        {
            GameObject door = Instantiate(westTunnel);
            door.transform.parent = rm.transform;
            door.transform.localPosition = new Vector2(0, -1f * row * roomWidth);
            door.GetComponent<Tile>().RoomNumber = roomNumber;
            room.Doors["west"] = door.GetComponent<Tile>();
        }
        else
        {
            GameObject wall = Instantiate(westWall);
            wall.transform.parent = rm.transform;
            wall.transform.localPosition = new Vector2(0, -1f * row * roomWidth);
            wall.GetComponent<Tile>().RoomNumber = roomNumber;
        }

        // Create floor pieces
        for (int i = 0; i < room.RoomSize; i++)
        {
            if (exitPos.x == row && exitPos.y == i)
            {
                GameObject tile = Instantiate(stairs);
                tile.transform.parent = rm.transform;
                tile.transform.localPosition = new Vector2((i + 1) * roomWidth, -1f * row * roomWidth);
                tile.GetComponent<Tile>().RoomNumber = roomNumber;
                roomPieces[row - 1].Add(tile);
            }
            else {
                GameObject tile = Instantiate(floor);
                tile.transform.parent = rm.transform;
                tile.transform.localPosition = new Vector2((i + 1) * roomWidth, -1f * row * roomWidth);
                tile.GetComponent<Tile>().RoomNumber = roomNumber;
                roomPieces[row - 1].Add(tile);
            }

        }

        // Create right wall piece
        if (row == room.DoorPositions[1] && room.Neighbors[directionIndex[1]] != null)
        {
            GameObject door = Instantiate(eastTunnel);
            door.transform.parent = rm.transform;
            door.transform.localPosition = new Vector2((room.RoomSize + 1) * roomWidth, -1f * row * roomWidth);
            door.GetComponent<Tile>().RoomNumber = roomNumber;
            room.Doors["east"] = door.GetComponent<Tile>();
        }
        else
        {
            GameObject wall = Instantiate(eastWall);
            wall.transform.parent = rm.transform;
            wall.transform.localPosition = new Vector2((room.RoomSize + 1) * roomWidth, -1f * row * roomWidth);
            wall.GetComponent<Tile>().RoomNumber = roomNumber;
        }

        room.FloorTiles = roomPieces;
    }

    private void createLastLayer(Room room, GameObject rm)
    {
        int row = room.RoomSize + 1;
        
        // Create left corner piece
        GameObject corner = Instantiate(swCorner);
        corner.transform.parent = rm.transform;
        corner.transform.localPosition = new Vector2(0, -1f * row * roomWidth);
        corner.GetComponent<Tile>().RoomNumber = roomNumber;

        // Create wall pieces
        int doorPos = room.DoorPositions[2];
        for (int i = 0; i < room.RoomSize; i++)
        {
            // If this is top most room, do not create a tunnel
            if (room.Neighbors[directionIndex[2]] == null)
            {
                GameObject wall = Instantiate(southWall);
                wall.transform.parent = rm.transform;
                wall.transform.localPosition = new Vector2((i + 1) * roomWidth, -1f * row * roomWidth);
                wall.GetComponent<Tile>().RoomNumber = roomNumber;
            }
            else
            {
                // Place door or wall based on predetermined variable (DungeonGeneration.Awake) 
                if (i == doorPos)
                {
                    GameObject door = Instantiate(southTunnel);
                    door.transform.parent = rm.transform;
                    door.transform.localPosition = new Vector2((i + 1) * roomWidth, -1f * row * roomWidth);
                    door.GetComponent<Tile>().RoomNumber = roomNumber;
                    room.Doors["south"] = door.GetComponent<Tile>();
                }
                else
                {
                    GameObject wall = Instantiate(southWall);
                    wall.transform.parent = rm.transform;
                    wall.transform.localPosition = new Vector2((i + 1) * roomWidth, -1f * row * roomWidth);
                    wall.GetComponent<Tile>().RoomNumber = roomNumber;
                }
            }
        }

        // Create right corner piece
        corner = Instantiate(seCorner);
        corner.transform.parent = rm.transform;
        corner.transform.localPosition = new Vector2((room.RoomSize + 1) * roomWidth, -1f * row * roomWidth);
        corner.GetComponent<Tile>().RoomNumber = roomNumber;

    }
}
