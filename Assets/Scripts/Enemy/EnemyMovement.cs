using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float travelTime;

    private Enemy enemy;

    private Rigidbody2D rb;
    private Vector2 position;
    private float tileWidth;
    private SpriteRenderer sr;

    private gameManager gm;
    private bool moved = false;
    private List<Vector2> cardDirections = new List<Vector2>();
    private EnemyAnimation eAnim;

    private int stuck = 0;
    private string directionStr;
    #endregion

    #region Initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        enemy = GetComponent<EnemyController>().m_Enemy;
        position = rb.position;
        tileWidth = GameObject.FindGameObjectWithTag("mapGenerator").GetComponent<DungeonGeneration>().TileWidth;
        eAnim = GetComponent<EnemyAnimation>();

        cardDirections.Add(new Vector2(0, tileWidth));
        cardDirections.Add(new Vector2(tileWidth, 0));
        cardDirections.Add(new Vector2(0, -1 * tileWidth));
        cardDirections.Add(new Vector2(-1 * tileWidth, 0));
    }

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }
    #endregion

    #region Movement Functions
    public void Move()
    {
        moved = false;

        //Check if the player is next to the enemy
        foreach (Vector2 c in cardDirections)
        {
            Vector2 potentialPosition = new Vector2((float)Math.Round(c.x + transform.position.x, 1), (float)Math.Round(c.y + transform.position.y, 1));
            if (potentialPosition == gm.PlayerNextPos)
            {
                Debug.Log("attack player check");
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                AttackPlayer(player, c);
            }

            //RaycastHit2D[] rayCasts = Physics2D.CircleCastAll(transform.position, 0.1f, c, tileWidth);
            //foreach (RaycastHit2D ray in rayCasts)
            //{
            //    if (ray.collider.tag == "Player")
            //    {
            //        AttackPlayer(ray.collider.gameObject, c);
            //    }
            //}
        }

        if (!moved)
        {
            //Check if the player is nearby
            RaycastHit2D[] rays = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero);
            foreach (RaycastHit2D ray in rays)
            {
                if (ray.collider.tag == "Player")
                {
                    MoveTowardsPlayer(ray.collider.gameObject);
                }
            }
        }

        if (!moved)
        {
            MoveRandom();
        }
    }

    private void AttackPlayer(GameObject player, Vector2 c)
    {
        Debug.Log("attack player");
        moved = true;
        FacePlayer(c);
        //Attack animation based on direction
        AttackAnimation();
        PlayerHealth ph = player.GetComponent<PlayerHealth>();

        ph.decreaseHealth(enemy.Damage);
        Vector2 roundedPos = new Vector2((float)Math.Round(transform.position.x, 1), (float)Math.Round(transform.position.y, 1));
        gm.OccupiedSpaces.Add(roundedPos);
    }

    private void MoveTowardsPlayer(GameObject player)
    {
        position = rb.position;
        Player o_Player = player.GetComponent<Player>();
        if (o_Player.RoomNumber != -1)
        {
            Vector2 playerPos = player.transform.position;
            Vector2 targetPos = transform.position;

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                if (playerPos.x < transform.position.x)
                {
                    if (isTileEmpty(3))
                    {
                        targetPos.x -= tileWidth;
                        directionStr = "left";
                    }
                }
                else if (playerPos.x > transform.position.x)
                {
                    if (isTileEmpty(1))
                    {
                        targetPos.x += tileWidth;
                        directionStr = "right";
                    }
                }
                else if (playerPos.y < transform.position.y)
                {
                    if (isTileEmpty(2))
                    {
                        targetPos.y -= tileWidth;
                        directionStr = "down";
                    }
                }
                else if (playerPos.y > transform.position.y)
                {
                    if (isTileEmpty(0))
                    {
                        targetPos.y += tileWidth;
                        directionStr = "up";
                    }
                }
            }
            else
            {
                if (playerPos.y < transform.position.y)
                {
                    if (isTileEmpty(2))
                    {
                        targetPos.y -= tileWidth;
                        directionStr = "down";
                    }
                }
                else if (playerPos.y > transform.position.y)
                {
                    if (isTileEmpty(0))
                    {
                        targetPos.y += tileWidth;
                        directionStr = "left";
                    }
                } else if (playerPos.x < transform.position.x)
                {
                    if (isTileEmpty(3))
                    {
                        targetPos.x -= tileWidth;
                        directionStr = "up";
                    }
                }
                else if (playerPos.x > transform.position.x)
                {
                    if (isTileEmpty(1))
                    {
                        targetPos.x += tileWidth;
                        directionStr = "right";
                    }
                }
            }

            if (targetPos.x == transform.position.x && targetPos.y == transform.position.y)
            {
                return;
            }

            Vector2 roundedPos = new Vector2((float)Math.Round(targetPos.x, 1), (float)Math.Round(targetPos.y, 1));
            if (!gm.OccupiedSpaces.Contains(roundedPos))
            {
                PlayAnimation(directionStr);
                gm.OccupiedSpaces.Add(roundedPos);
                StartCoroutine(EnemySpriteMove(roundedPos, travelTime));
            }
        }
    }

    private void MoveRandom()
    {
        position = rb.position;
        int direction = UnityEngine.Random.Range(0, 4);
        int origin = direction % 4;

        Vector2 targetPosition = getTargetPosition(direction);
        Vector2 roundedPos = new Vector2((float)Math.Round(targetPosition.x, 1), (float)Math.Round(targetPosition.y, 1));
        while (!isTileEmpty(direction) || gm.OccupiedSpaces.Contains(roundedPos))
        {
            direction += 1;
            direction = direction % 4;
            targetPosition = getTargetPosition(direction);
            roundedPos = new Vector2((float)Math.Round(targetPosition.x, 1), (float)Math.Round(targetPosition.y, 1));
            if (direction == origin)
            {
                stuck += 1;
                if (stuck == 3)
                {
                    //Destroy(gameObject);
                }
                Debug.Log("Enemy cannot find position to move to");
                return;
            }
        }

        PlayAnimation(directionStr);
        roundedPos = new Vector2((float)Math.Round(targetPosition.x, 1), (float)Math.Round(targetPosition.y, 1));
        gm.OccupiedSpaces.Add(roundedPos);
        StartCoroutine(EnemySpriteMove(roundedPos, travelTime));
    }

    private bool isTileEmpty(int direction)
    {
        Vector2 directionVector = Vector2.zero;
        switch (direction)
        {
            case 0:
                directionVector.y += tileWidth;
                break;

            case 1:
                directionVector.x += tileWidth;
                break;

            case 2:
                directionVector.y -= tileWidth;
                break;

            case 3:
                directionVector.x -= tileWidth;
                break;
        }
        RaycastHit2D[] rays = Physics2D.RaycastAll(position, directionVector, tileWidth);

        foreach (RaycastHit2D ray in rays)
        {
            if (ray.collider.CompareTag("wall") || ray.collider.CompareTag("Player"))
            {
                return false;
            }
        }
        return true;
    }

    private Vector2 getTargetPosition(int direction)
    {
        Vector2 directionVector = position;
        switch (direction)
        {
            case 0:
                directionVector.y += tileWidth;
                directionStr = "up";
                break;

            case 1:
                directionVector.x += tileWidth;
                directionStr = "right";
                break;

            case 2:
                directionVector.y -= tileWidth;
                directionStr = "down";
                break;

            case 3:
                directionVector.x -= tileWidth;
                directionStr = "left";
                break;
        }
        return directionVector;
    }

    private void PlayAnimation(string direction)
    {
        if (direction == "right")
        {
            sr.flipX = true;
            eAnim.ChangeAnimation("left");
        }
        else
        {
            sr.flipX = false;
            eAnim.ChangeAnimation(direction);
        }
    }

    IEnumerator EnemySpriteMove(Vector2 targetPos, float travelTime)
    {
        Vector2 originalPos = position;
        moved = true;
        for (int i = 0; i < travelTime * 100; i++)
        {
            Vector2 tempPos = Vector2.Lerp(originalPos, targetPos, i / (travelTime * 100f));
            //transform.localPosition = tempPos;
            rb.MovePosition(tempPos);
            yield return new WaitForSeconds(1f / 100f);
        }
    }
    #endregion

    #region Sprite Functions
    private void FacePlayer(Vector2 dir)
    {
        if (dir.x == 0 && dir.y == tileWidth)
        {
            directionStr = "up";
        } else if (dir.x == 0 && dir.y == -1 * tileWidth)
        {
            directionStr = "down";
        } else if (dir.x == tileWidth && dir.y == 0)
        {
            directionStr = "right";
        } else if (dir.x == -1 * tileWidth && dir.y == 0)
        {
            directionStr = "left";
        }
    }

    private void AttackAnimation()
    {
        Debug.Log("attack " + directionStr);
        if (directionStr == "right")
        {
            sr.flipX = true;
            eAnim.ChangeAnimation("left" + "_attack");
        }
        else
        {
            sr.flipX = false;
            eAnim.ChangeAnimation(directionStr + "_attack");
        }
    }
    #endregion
}
