using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
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
        Debug.Log(name);
        anim.Play(name);
    }
    #endregion
}
