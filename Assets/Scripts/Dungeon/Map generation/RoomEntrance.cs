using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour
{
    [SerializeField]
    private int roomNumber;
    public int RoomNumber
    {
        get { return roomNumber; }
        set { roomNumber = value; }
    }

}
