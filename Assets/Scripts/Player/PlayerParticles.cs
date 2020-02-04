using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> particles;
    private Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void PlayParticle(int i)
    {
        StartCoroutine(PlayParticleCoroutine(i));
    }

    private IEnumerator PlayParticleCoroutine(int i)
    {
        ParticleSystem ps = Instantiate(particles[i], player.transform).GetComponent<ParticleSystem>();
        ps.Play();
        yield return new WaitForSeconds(1.5f);
        ps.Stop();
        Destroy(ps.gameObject);
    }
}
