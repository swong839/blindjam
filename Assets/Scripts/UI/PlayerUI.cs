using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    //private static PlayerUI instance;

    #region Variables
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject heart;
    private List<GameObject> totalHearts = new List<GameObject>();

    // [Ice, Fire, Lightning]
    [SerializeField]
    private List<TextMeshProUGUI> orbCounts = new List<TextMeshProUGUI>();

    private PlayerInventory player;
    #endregion

    #region Initialization
    private void Awake()
    {
        //DontDestroyOnLoad(this);

        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    GameObject.Destroy(gameObject);
        //}

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }
    #endregion

    #region Health UI
    public void SetUpHealthUI(int hearts)
    {
        for (int i = 0; i < hearts; i++)
        {
            GameObject heartChild = Instantiate(heart);
            heartChild.transform.parent = healthBar.transform;
            totalHearts.Add(heartChild);
        }
    }

    public void UpdateHealthUI(int hearts)
    {
        //hearts = number of hearts to remove
        for (int i = hearts; i < totalHearts.Count; i++)
        {
            Destroy(totalHearts[0]);
            totalHearts.RemoveAt(0);
        }
    }
    #endregion

    #region Inventory UI
    public void UpdateInventoryUI()
    {
        for (int i = 0; i < 3; i++)
        {
            TextMeshProUGUI orbCount = orbCounts[i];
            orbCount.text = "x" + player.OrbCount(i).ToString();
        }
    }
    #endregion
}
