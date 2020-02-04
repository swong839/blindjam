using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float stepTimer;

    [SerializeField]
    private float travelTime;

    private Rigidbody2D rb;
    private float tileWidth;
    private Vector2 targetPos;
    private Vector2 directionVec;
    private float timer;
    private string direction;
    public string Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private PlayerAudio pAud;
    private RoomEntranceAudio reAud;

    private Player player;
    private gameManager gm;
    private SpriteRenderer sr;
    private PlayerAnimation pAnim;

    private bool canMove = false;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private Vector2 prevPos;
    private bool isPaused = false;
    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    #endregion

    #region Initialization
    private void Awake()
    {
        LoadObjects();
    }

    private void LoadObjects()
    {
        timer = 0f;
        rb = GetComponent<Rigidbody2D>();
        tileWidth = GameObject.FindGameObjectWithTag("mapGenerator").GetComponent<DungeonGeneration>().TileWidth;
        canMove = true;

        player = GetComponent<Player>();
        gm = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        sr = GetComponent<SpriteRenderer>();
        pAnim = GetComponent<PlayerAnimation>();

        pAud = GameObject.FindGameObjectWithTag("PlayerAudio").GetComponent<PlayerAudio>();
        reAud = GameObject.FindGameObjectWithTag("RoomEntrance").GetComponent<RoomEntranceAudio>();

        prevPos = rb.position;
    }
    #endregion

    #region Update Methods

    private void FixedUpdate()
    {
        Move();
        prevPos = rb.position;
    }
    #endregion

    #region Move Functions
    public void StopMovement()
    {
        isPaused = true;
    }

    public void StartMovement()
    {
        isPaused = false;
    }

    private void Move()
    {
        if (isPaused)
        {
            return;
        }

        if (canMove)
        {
            targetPos = rb.position;
            directionVec = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                targetPos.y += tileWidth;
                directionVec.y += tileWidth;
                direction = "up";
            }

            else if (Input.GetKey(KeyCode.S))
            {
                targetPos.y -= tileWidth;
                directionVec.y -= tileWidth;
                direction = "down";
            }

            if (Input.GetKey(KeyCode.D))
            {
                targetPos.x += tileWidth;
                directionVec.x += tileWidth;
                direction = "right";
            }

            else if (Input.GetKey(KeyCode.A))
            {
                targetPos.x -= tileWidth;
                directionVec.x -= tileWidth;
                direction = "left";
            }

            if (Input.GetKey(KeyCode.Tab))
            {

                Vector2 roundPos = new Vector2((float)System.Math.Round(targetPos.x, 1), (float)System.Math.Round(targetPos.y, 1));
                StartCoroutine(PlayerSpriteMove(roundPos, travelTime));
                gm.OccupiedSpaces.Clear();
                gm.OccupiedSpaces.Add(roundPos);
                gm.PlayerNextPos = roundPos;
                gm.EnemyMove();

                canMove = false;
                timer += Time.fixedDeltaTime;
                return;

            }

            if (targetPos == rb.position)
            {
                return;
            }
            
            RaycastHit2D[] rays = Physics2D.RaycastAll(rb.position, directionVec, tileWidth);
            
            foreach(RaycastHit2D ray in rays)
            {
                if (ray.collider.CompareTag("wall") || ray.collider.CompareTag("enemy"))
                {
                    if (ray.collider.CompareTag("wall"))
                    {
                        pAud.PlayerWallBump();
                        PlayAnimation(direction);
                    } else if (ray.collider.CompareTag("enemy"))
                    {
                        PlayAnimation(direction);
                        pAud.PlayerEnemyBump();
                    }
                    return;
                }
            }

            Vector2 roundedPos = new Vector2((float)System.Math.Round(targetPos.x, 1), (float)System.Math.Round(targetPos.y, 1));
            PlayAnimation(direction);
            StartCoroutine(PlayerSpriteMove(roundedPos, travelTime));
            gm.OccupiedSpaces.Clear();
            gm.OccupiedSpaces.Add(roundedPos);
            gm.PlayerNextPos = roundedPos;
            gm.EnemyMove();

            canMove = false;
            timer += Time.fixedDeltaTime;
        }
    }

    private void PlayAnimation(string direction)
    {
        if (direction == "right")
        {
            sr.flipX = true;
            pAnim.ChangeAnimation("left");
        }
        else
        {
            sr.flipX = false;
            pAnim.ChangeAnimation(direction);
        }
    }

    IEnumerator PlayerSpriteMove(Vector2 targetPos, float travelTime)
    {
        Vector2 originalPos = rb.position;
        for (int i = 0; i < travelTime * 100; i++)
        {
            Vector2 tempPos = Vector2.Lerp(originalPos, targetPos, i / (travelTime * 100f));
            rb.MovePosition(tempPos);
            yield return new WaitForSeconds(1f / 100f);
        }
        rb.position = targetPos;
        //yield return new WaitForSeconds(0.05f);
        canMove = true;
        if (!isDead)
        {
            PlayStandingAnimation(direction);
        }
        //if (travelTime > stepTimer)
        //{
        //    //gm.EnemyMove();
        //    //yield return new WaitForSeconds(0.05f);
        //    canMove = true;
        //}
        //else
        //{
        //    yield return new WaitForSeconds(stepTimer - travelTime);
        //    //rb.position = targetPos;
        //    canMove = true;
        //}
    }

    private void PlayStandingAnimation(string direction)
    {
        if (direction == "right")
        {
            sr.flipX = true;
            pAnim.ChangeAnimation("standing_left");
        }
        else
        {
            sr.flipX = false;
            pAnim.ChangeAnimation("standing_" + direction);
        }
    }
    #endregion

    #region Collision Functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tunnelEntrance"))
        {
            player.PrevRoom = player.RoomNumber;
            player.RoomNumber = collision.GetComponent<Tile>().RoomNumber;
            if (player.PrevRoom == -1)
            {
                reAud.StartCuesCoroutine(gm.m_mm.Rooms[player.RoomNumber]);
            }
        } 
       
        if (collision.CompareTag("tunnel"))
        {
            player.PrevRoom = player.RoomNumber;
            player.RoomNumber = -1;
        }
    }
    #endregion
}
