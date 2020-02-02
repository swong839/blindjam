using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    [SerializeField]
    private MapManager mm;
    public MapManager m_MapManager
    {
        get { return mm; }
        set { mm = value; }
    }

    [SerializeField]
    private Orb[] orb;
    public Orb[] m_Orb
    {
        get { return orb; }
        set { orb = value; }
    }

    private OrbCollider orbCollider;
    public OrbCollider m_OrbCollider
    {
        get { return orbCollider; }
        set { orbCollider = value; }
    }


    private void Awake()
    {
        mm = GameObject.FindGameObjectWithTag("gameManager").GetComponent<MapManager>();
        m_OrbCollider = GetComponent<OrbCollider>();
    }

    public void UseOrb(int roomNumber, int type)
    {
        Debug.Log("used orb " + type.ToString());
        RaycastHit2D[] rays = Physics2D.CircleCastAll(transform.position, 1f, Vector2.zero);

        foreach (RaycastHit2D ray in rays)
        {
            Debug.Log(ray.collider.tag);
            if (ray.transform.CompareTag("enemy"))
            {
                EnemyController enemy = ray.transform.gameObject.GetComponent<EnemyController>();
                enemy.DecreaseHealth(orb[type].Damage, type.ToString());
            }
        }
        //List<EnemyController> enemies = mm.Rooms[roomNumber].Enemies;
        //foreach (EnemyController enemy in enemies)
        //{
        //    enemy.DecreaseHealth(orb[type].Damage, type.ToString());
        //}
        //mm.removeEnemies(roomNumber);
    }

    public void DestroyOrb()
    {
        //play noise for large break
    }

}
