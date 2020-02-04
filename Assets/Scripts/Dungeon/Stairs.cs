using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stairs : MonoBehaviour
{
    private NextLevel nl;

    private void Awake()
    {
        nl = GameObject.FindGameObjectWithTag("NextLevel").GetComponent<NextLevel>();
        StartCoroutine(FadeIn());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GoToNextStage(collision.gameObject, collision.gameObject.GetComponent<Player>().Floor);

        }
    }

    private void GoToNextStage(GameObject player, int floor)
    {
        if (floor == 4)
        {
            SceneManager.LoadScene("FinalScene");
        } else
        {
            //Transition to black
            //SceneManager.LoadScene("MapGeneration");
            StartCoroutine(FadeBlack(player));
            nl.GoUpStairs();
        }
    }

    IEnumerator FadeBlack(GameObject player)
    {
        player.GetComponent<PlayerMovement>().StopMovement();
        Image panel = GameObject.FindGameObjectWithTag("BlackOut").GetComponent<Image>();
        Color panelTrans = panel.color;
        Color panelBlack = panelTrans;
        panelBlack.a = 1;
        panelTrans.a = 0;
        for (int i = 0; i < 100; i++)
        {
            panel.color = Color.Lerp(panelTrans, panelBlack, i / 100f);
            yield return new WaitForSeconds(0.007f);
        }

        ResetAll(player);

        //yield return new WaitForSeconds(0.5f);
        //Debug.Log("set position");
        //player.GetComponent<Rigidbody2D>().MovePosition(new Vector2(0.4f, -0.4f));

        //Debug.Log(panelBlack);
        //Debug.Log(panelTrans);
        //for (int i = 0; i < 100; i++)
        //{
        //    panel.color = Color.Lerp(panelBlack, panelTrans, i / 100f);
        //    Debug.Log(i);
        //    yield return null;
        //}
        StartCoroutine(FadeIn(player));
        //player.GetComponent<PlayerMovement>().StartMovement();
    }

    IEnumerator FadeIn()
    {
        Image panel = GameObject.FindGameObjectWithTag("BlackOut").GetComponent<Image>();
        Color panelTrans = panel.color;
        Color panelBlack = panelTrans;
        panelBlack.a = 1;
        panelTrans.a = 0;
        
        for (int j = 0; j < 100; j++)
        {
            panel.color = Color.Lerp(panelBlack, panelTrans, j / 100f);
            yield return new WaitForSeconds(0.005f);
        }
        PlayerMovement pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        pm.StartMovement();
    }

    IEnumerator FadeIn(GameObject player)
    {
        Image panel = GameObject.FindGameObjectWithTag("BlackOut").GetComponent<Image>();
        Color panelTrans = panel.color;
        Color panelBlack = panelTrans;
        panelBlack.a = 1;
        panelTrans.a = 0;
        
        for (int j = 0; j < 100; j++)
        {
            panel.color = Color.Lerp(panelBlack, panelTrans, j / 100f);
            yield return new WaitForSeconds(0.005f);
        }
        PlayerMovement pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        pm.StartMovement();
        player.GetComponent<PlayerMovement>().StartMovement();
    }

    private void ResetAll(GameObject player)
    {
        player.transform.position = new Vector2(0.4f, -0.4f);
        gameManager gm = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        MapManager mm = gm.m_mm;
        mm.Rooms.Clear();
        mm.Enemies.Clear();
        mm.EnemiesToRemove.Clear();
        gm.OccupiedSpaces.Clear();

        DungeonGeneration dg = GameObject.FindGameObjectWithTag("mapGenerator").GetComponent<DungeonGeneration>();
        dg.resetLists();

        dg.roomSetUp();
        player.GetComponent<Rigidbody2D>().MovePosition(new Vector2(0.4f, -0.4f));
    }
}
