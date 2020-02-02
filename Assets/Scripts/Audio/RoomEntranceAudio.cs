using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntranceAudio : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> cues = new List<AudioClip>();
    [SerializeField]
    private AudioClip stairs;
    [SerializeField]
    private AudioClip enemyGrowl;

    private AudioSource aud;
    private Player p;
    private gameManager gm;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gm = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (p.RoomNumber != -1)
            {
                StartCuesCoroutine(gm.m_mm.Rooms[p.RoomNumber]);
            }
        }
    }

    public void StartCuesCoroutine(Room rm)
    {
        StartCoroutine(StartCues(rm));
    }

    IEnumerator StartCues(Room rm)
    {
        aud.volume = 1f;
        if (rm.Neighbors["north"] != null)
        {
            aud.clip = cues[0];
            aud.Play();
            yield return new WaitForSeconds(cues[0].length + 0.1f);
        }
        if (rm.Neighbors["east"] != null)
        {
            aud.clip = cues[1];
            aud.Play();
            yield return new WaitForSeconds(cues[1].length + 0.1f);
        }
        if (rm.Neighbors["south"] != null)
        {
            aud.clip = cues[2];
            aud.Play();
            yield return new WaitForSeconds(cues[2].length + 0.1f);
        }
        if (rm.Neighbors["west"] != null)
        {
            aud.clip = cues[3];
            aud.Play();
            yield return new WaitForSeconds(cues[3].length + 0.1f);
        }

        if (rm.IsExit)
        {
            aud.clip = stairs;
            aud.Play();
            yield return new WaitForSeconds(stairs.length + 0.1f);
        }

        if (rm.Enemies.Count != 0)
        {
            aud.clip = enemyGrowl;
            aud.volume = 0.6f;
            aud.Play();
        }
    }
}
