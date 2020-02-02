using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    private MapManager MapManager;
    public MapManager m_mm
    {
        get { return MapManager; }
        set { MapManager = value; }
    }

    [SerializeField]
    private List<Vector2> occupiedSpaces = new List<Vector2>();
    public List<Vector2> OccupiedSpaces
    {
        get { return occupiedSpaces; }
        set { occupiedSpaces = value; }
    }

    private Vector2 playerNextPos;
    public Vector2 PlayerNextPos
    {
        get { return playerNextPos; }
        set { playerNextPos = value; }
    }



    #region Initialization
    private void Awake()
    {
        m_mm = GetComponent<MapManager>();

    }
    #endregion

    #region Enemy Movement
    public void EnemyMove()
    {
        //Debug.Log(m_mm.Enemies.Count);
        foreach (List<EnemyController> enemyList in m_mm.Enemies)
        {
            //Debug.Log(enemyList.Count);
            foreach (EnemyController enemy in enemyList)
            {
                enemy.Move();
            }
        }
    }
    #endregion
}
