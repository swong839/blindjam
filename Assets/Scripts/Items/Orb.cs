using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb
{
    [SerializeField]
    private Vector2 position;
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    [SerializeField]
    private Element type;

    public Element Type
    {
        get { return type; }
        set { type = value; }
    }
}
