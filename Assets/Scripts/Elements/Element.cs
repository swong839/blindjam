using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Element
{
    [SerializeField]
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

}
