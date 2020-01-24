using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private int roomNumber;
    public int RoomNumber
    {
        get { return roomNumber; }
        set { roomNumber = value; }
    }

    [SerializeField]
    private bool isWall;
    public bool IsWall
    {
        get { return isWall; }
        set { isWall = value; }
    }

    [SerializeField]
    private bool isStairs;
    public bool IsStairs
    {
        get { return isStairs; }
        set { isStairs = value; }
    }

    [SerializeField]
    private bool isFloor;
    public bool IsFloor
    {
        get { return isFloor; }
        set { isFloor = value; }
    }


}
