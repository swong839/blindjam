using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollider : MonoBehaviour
{
    [SerializeField]
    private Orb orb;
    public Orb m_Orb
    {
        get { return orb; }
        set { orb = value; }
    }

    public void DestroyObject()
    {
        //Play audio for destroy item

        Destroy(gameObject);
    }
}
