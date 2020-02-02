using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField]
    private AudioClip stairs;

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    public void GoUpStairs()
    {
        aud.clip = stairs;
        aud.Play();
    }
}
