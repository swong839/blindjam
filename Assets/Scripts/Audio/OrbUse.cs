using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbUse : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> cues = new List<AudioClip>();

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    public void PlayCue(int i)
    {
        aud.clip = cues[i];
        aud.Play();
    }
}
