using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOutScreen : MonoBehaviour
{
    private Image bko;

    private void Awake()
    {
        bko = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bko.color.a > 0f)
            {
                bko.color = new Color(bko.color.r, bko.color.g, bko.color.b, 0f);
            } else
            {
                bko.color = new Color(bko.color.r, bko.color.g, bko.color.b, 1f);
            }
        }
    }
}
