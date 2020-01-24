using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float stepTimer;

    [SerializeField]
    private float travelTime;

    private Rigidbody2D rb;
    private float tileWidth;
    private Vector2 targetPos;
    private Vector2 directionVec;
    private float timer;

    private bool canMove = false;
    public bool MyProperty
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private void Awake()
    {
        timer = 0f;
        rb = GetComponent<Rigidbody2D>();
        tileWidth = GameObject.FindGameObjectWithTag("mapGenerator").GetComponent<DungeonGeneration>().TileWidth;
        canMove = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (canMove)
        {

            targetPos = rb.position;
            directionVec = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                targetPos.y += tileWidth;
                directionVec.y += tileWidth;
            }

            else if (Input.GetKey(KeyCode.S))
            {
                targetPos.y -= tileWidth;
                directionVec.y -= tileWidth;
            }

            if (Input.GetKey(KeyCode.D))
            {
                targetPos.x += tileWidth;
                directionVec.x += tileWidth;
            }

            else if (Input.GetKey(KeyCode.A))
            {
                targetPos.x -= tileWidth;
                directionVec.x -= tileWidth;
            }

            if (targetPos == rb.position)
            {
                return;
            }
            
            RaycastHit2D ray = Physics2D.Raycast(rb.position, directionVec, tileWidth);

            if (ray.collider != null)
            {
                Debug.Log(ray.transform);
                return;
            }

            StartCoroutine(PlayerSpriteMove(targetPos, travelTime));

            canMove = false;
            timer += Time.fixedDeltaTime;
        }
    }

    IEnumerator PlayerSpriteMove(Vector2 targetPos, float travelTime)
    {
        Vector2 originalPos = rb.position;
        Debug.Log(originalPos);
        Debug.Log(targetPos);
        for (int i = 0; i < travelTime * 100; i++)
        {
            Vector2 tempPos = Vector2.Lerp(originalPos, targetPos, i / (travelTime * 100f));
            rb.MovePosition(tempPos);
            yield return new WaitForSeconds(1f / 100f);
        }
        if (travelTime > stepTimer)
        {
            canMove = true;
        }
        else
        {
            yield return new WaitForSeconds(stepTimer - travelTime);
            canMove = true;
        }
    }
}
