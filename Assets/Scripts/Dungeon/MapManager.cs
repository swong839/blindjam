using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    private List<Room> rooms;
    public List<Room> Rooms
    {
        get { return rooms; }
        set { rooms = value; }
    }

}
