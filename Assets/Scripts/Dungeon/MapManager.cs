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

    [SerializeField]
    private List<List<EnemyController>> enemies = new List<List<EnemyController>>();
    public List<List<EnemyController>> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    private List<EnemyController> enemiesToRemove = new List<EnemyController>();
    public List<EnemyController> EnemiesToRemove
    {
        get { return enemiesToRemove; }
        set { enemiesToRemove = value; }
    }


    #region Enemy Functions
    public void removeEnemies(int roomNum)
    {
        foreach(EnemyController ec in enemiesToRemove)
        {
            enemies[roomNum].Remove(ec);
        }
        enemiesToRemove.Clear();
    }
    #endregion
}
