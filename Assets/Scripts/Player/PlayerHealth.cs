using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    private SpriteRenderer sr;
    private Player player;
    private PlayerUI pUI;
    private PlayerAnimation pAnim;
    private PlayerMovement pMove;


    private PlayerAudio pAud;
    #endregion

    #region Initialization
    private void Awake()
    {
        player = GetComponent<Player>();
        player.Health = maxHealth;
        pUI = GameObject.FindGameObjectWithTag("uiManager").GetComponent<PlayerUI>();
        pUI.SetUpHealthUI(player.Health);

        sr = GetComponent<SpriteRenderer>();
        pAnim = GetComponent<PlayerAnimation>();
        pMove = GetComponent<PlayerMovement>();

        pAud = GameObject.FindGameObjectWithTag("PlayerAudio").GetComponent<PlayerAudio>();
    }
    #endregion

    #region Health Functions
    public void increaseHealth(int amount)
    {
        player.Health += amount;
        if (player.Health > maxHealth)
        {
            player.Health = maxHealth;
        }
        UpdateUI();
    }

    public void decreaseHealth(int amount)
    {
        player.Health -= amount;
        Debug.Log(amount);
        pAud.PlayerGetHit();
        if (player.Health <= 0)
        {
            player.Health = 0;
            UpdateUI();
            PlayerDeath();
        }
        else
        {
            PlayHurtAnim();
            UpdateUI();
        }
    }

    private void PlayHurtAnim()
    {
        string direction = pMove.Direction;
        if (direction == "right")
        {
            sr.flipX = true;
            pAnim.ChangeAnimation("hurt_left");
        }
        else
        {
            sr.flipX = false;
            pAnim.ChangeAnimation("hurt_" + direction);
        }
    }
    #endregion

    #region Death Functions
    private void PlayerDeath()
    {
        pMove.IsDead = true;
        //Play animation
        pAnim.ChangeAnimation("die");
        //Play sound
        StartCoroutine(ReduceSound());
        StartCoroutine(BlackScreen());
    }

    IEnumerator BlackScreen()
    {
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

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ReduceSound()
    {
        AudioSource bgm = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();
        float startVol = bgm.volume;
        for (int i = 0; i < 100; i++)
        {
            bgm.volume = Mathf.Lerp(startVol, 0, i / 100f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion

    #region UI Functions
    private void UpdateUI()
    {
        pUI.UpdateHealthUI(player.Health);
    }
    #endregion
}
