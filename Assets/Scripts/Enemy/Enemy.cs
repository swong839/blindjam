using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemy
{
    [SerializeField]
    private Vector2 position;
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    [SerializeField]
    private int health;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    [SerializeField]
    private Element type;
    public Element Type
    {
        get { return type; }
        set { type = value; }
    }

    [SerializeField]
    private int roomNumber;
    public int RoomNumber
    {
        get { return roomNumber; }
        set { roomNumber = value; }
    }

    [SerializeField]
    private int damage;
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

}
