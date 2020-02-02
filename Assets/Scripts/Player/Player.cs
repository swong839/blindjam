using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour
{
    //private static GameObject playerInstance;

    [SerializeField]
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    [SerializeField]
    private int roomNumber;
    public int RoomNumber
    {
        get { return roomNumber; }
        set { roomNumber = value; }
    }

    private int prevRoom;
    public int PrevRoom
    {
        get { return prevRoom; }
        set { prevRoom = value; }
    }


    [SerializeField]
    private int floor = 0;
    public int Floor
    {
        get { return floor; }
        set { floor = value; }
    }
    
    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //    floor += 1;
    //    transform.position = new Vector2(0.4f, -0.4f);
    //    if (playerInstance == null)
    //    {
    //        playerInstance = gameObject;
    //    }
    //    else
    //    {
    //        GameObject.Destroy(gameObject);
    //    }
    //}
    
}
