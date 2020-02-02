using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(OrbController))]
[RequireComponent(typeof(Collider2D))]
public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int maxOrbs = 5;
    private List<string> elementNames = new List<string>();
    private Dictionary<string, int> elementConversion = new Dictionary<string, int>();

    private Player player;
    private OrbController oc;

    [SerializeField]
    private Dictionary<string, int> orbInventory = new Dictionary<string, int>();
    private enum orbIndex
    {
        ICE,
        FIRE,
        LIGHTNING
    };

    private PlayerUI pUI;
    private PlayerAnimation pAnim;
    private OrbUse oAud;
    private OrbPickUp opAud;

    private void Awake()
    {
        elementNames.Add("Ice");
        elementNames.Add("Fire");
        elementNames.Add("Lightning");

        elementConversion.Add("Ice", 0);
        elementConversion.Add("Fire", 1);
        elementConversion.Add("Lightning", 2);

        for (int i = 0; i < 3; i++)
        {
            Orb orb = new Orb();
            Element elem = new Element();
            elem.Name = elementNames[i];
            orb.Type = elem;
            orbInventory.Add(elementNames[i], 0);
        }

        oc = GetComponent<OrbController>();
        player = GetComponent<Player>();

        pUI = GameObject.FindGameObjectWithTag("uiManager").GetComponent<PlayerUI>();
        pAnim = GetComponent<PlayerAnimation>();
        oAud = GameObject.FindGameObjectWithTag("OrbUse").GetComponent<OrbUse>();
        opAud = GameObject.FindGameObjectWithTag("OrbPickUp").GetComponent<OrbPickUp>();
    }

    #region Update Functions
    private void Update()
    {
        CheckOrbInputs();
    }
    #endregion

    #region Use Functions
    private void CheckOrbInputs()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            UseOrb(0);
        } else if (Input.GetKeyDown(KeyCode.K))
        {
            UseOrb(1);
        } else if (Input.GetKeyDown(KeyCode.L))
        {
            UseOrb(2);
        }
    }

    public void UseOrb(int orbNum)
    {
        string orb = elementNames[orbNum];
        if (orbInventory[orb] > 0)
        {

            oc.UseOrb(player.RoomNumber, orbNum);
            orbInventory[orb] -= 1;
            pUI.UpdateInventoryUI();
            pAnim.ChangeAnimation("use_orb");
            oAud.PlayCue(orbNum);
            //if (orbInventory[orb] == 0)
            //{
            //    oc.DestroyOrb();
            //}
        }
    }
    #endregion

    #region Counting Functions
    private void DecrementOrb(string orb)
    {
        orbInventory[orb] -= 1;
        if (orbInventory[orb] == 0)
        {
            //play noise for large break
        } else if (orbInventory[orb] == 1)
        {
            //play warning noise
        }

        //update ui
    }

    private void IncrementOrb(string orb)
    {
        if (orbInventory[orb] < maxOrbs)
        {
            orbInventory[orb] += 1;
            //Play sound to collect orb

            //Destroy orb object

            //update ui
        }

    }

    public int OrbCount(int i)
    {
        switch (i)
        {
            case 0:
                return orbInventory["Ice"];
            case 1:
                return orbInventory["Fire"];
            case 2:
                return orbInventory["Lightning"];
        }
        return 0;
    }
    #endregion

    #region Collect Functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("orb"))
        {
            OrbCollider orb = collision.gameObject.GetComponent<OrbCollider>();
            Orb orbType = orb.m_Orb;
            if (orbInventory[orbType.Type.Name] < maxOrbs)
            {
                opAud.PlayCue(elementConversion[orbType.Type.Name]);
                IncrementOrb(orbType.Type.Name);
                orb.DestroyObject();
                pUI.UpdateInventoryUI();
            }
        }
    }
    #endregion  

}
