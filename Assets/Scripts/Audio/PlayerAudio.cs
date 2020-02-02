using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip hit;

    [SerializeField]
    private AudioClip wallBump;

    [SerializeField]
    private AudioClip enemyBump;

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    public void PlayerWallBump()
    {
        aud.volume = 1f;
        aud.clip = wallBump;
        aud.Play();
    }

    public void PlayerEnemyBump()
    {
        aud.clip = enemyBump;
        aud.volume = 0.75f;
        aud.Play();
    }

    public void PlayerGetHit()
    {
        aud.volume = 1f;
        aud.clip = hit;
        aud.Play();
    }
}
