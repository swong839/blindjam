using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Room
{
    [SerializeField]
    private int roomNum;
    public int RoomNum
    {
        get { return roomNum; }
        set { roomNum = value; }
    }

    [SerializeField]
    private bool isEntrance = false;
    public bool IsEntrance
    {
        get { return isEntrance; }
        set { isEntrance = value; }
    }

    [SerializeField]
    private bool isExit = false;
    public bool IsExit
    {
        get { return isExit; }
        set { isExit = value; }
    }

    [SerializeField]
    private int roomSize;
    public int RoomSize
    {
        get { return roomSize; }
        set { roomSize = value; }
    }

    [SerializeField]
    private List<int> doorPositions;
    public List<int> DoorPositions
    {
        get { return doorPositions; }
        set { doorPositions = value; }
    }

    [SerializeField]
    private int numEnemies;
    public int NumEnemies
    {
        get { return numEnemies; }
        set { numEnemies = value; }
    }

    [SerializeField]
    private List<EnemyController> enemies = new List<EnemyController>();
    public List<EnemyController> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    [SerializeField]
    private int numOrbs;
    public int NumOrbs
    {
        get { return numOrbs; }
        set { numOrbs = value; }
    }

    [SerializeField]
    private List<Orb> orbs = new List<Orb>();
    public List<Orb> Orbs
    {
        get { return orbs; }
        set { orbs = value; }
    }
    
    [SerializeField]
    private Dictionary<string, Room> neighbors = new Dictionary<string, Room>();
    public Dictionary<string, Room> Neighbors
    {
        get { return neighbors; }
        set { neighbors = value; }
    }

    [SerializeField]
    private Dictionary<string, Tile> doors = new Dictionary<string, Tile>();
    public Dictionary<string, Tile> Doors
    {
        get { return doors; }
        set { doors = value; }
    }

    [SerializeField]
    private Dictionary<string, List<Tile>> walls = new Dictionary<string, List<Tile>>();
    public Dictionary<string, List<Tile>> Walls
    {
        get { return walls; }
        set { walls = value; }
    }

    [SerializeField]
    private List<List<GameObject>> floorTiles;
    public List<List<GameObject>> FloorTiles
    {
        get { return floorTiles; }
        set { floorTiles = value; }
    }
}
