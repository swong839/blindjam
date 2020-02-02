using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private AudioClip beginning;

    [SerializeField]
    private AudioClip loop;

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        StartCoroutine(StartMusic());
    }

    IEnumerator StartMusic()
    {
        aud.clip = beginning;
        aud.loop = false;
        aud.Play();

        yield return new WaitForSeconds(beginning.length);

        aud.clip = loop;
        aud.loop = true;
        aud.Play();
    }
}
