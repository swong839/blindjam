using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    #region Variables
    private Animator anim;
    #endregion

    #region Initialization
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    #endregion

    #region Animation Functions
    public void ChangeAnimation(string name)
    {
        anim.Play(name);
    }
    #endregion
}
