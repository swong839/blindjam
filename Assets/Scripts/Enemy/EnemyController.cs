using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Enemy enemy;
    public Enemy m_Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }

    private EnemyMovement em;
    private MapManager mm;

    private SpriteRenderer sr;
    private EnemyAnimation eAnim;
    private EnemyAudio eAud;
    #endregion

    #region Initialization
    private void Awake()
    {
        em = GetComponent<EnemyMovement>();
        sr = GetComponent<SpriteRenderer>();
        eAnim = GetComponent<EnemyAnimation>();
        mm = GameObject.FindGameObjectWithTag("gameManager").GetComponent<MapManager>();
        eAud = GameObject.FindGameObjectWithTag("EnemyAudio").GetComponent<EnemyAudio>();
    }
    #endregion

    #region Update Functions
    #endregion

    public void DecreaseHealth(int health, string type)
    {
        enemy.Health -= health;
        if (enemy.Health <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        mm.Enemies[enemy.RoomNumber].Remove(this);
        eAud.EnemyDeath();
        StartCoroutine(EnemyDieAnim());
    }

    IEnumerator EnemyDieAnim()
    {
        eAnim.ChangeAnimation("down_die");
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }

    public void Move()
    {
        em.Move();
    }
}
