using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCount : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject text;
    public int level;
    int picked;
    private void Start()
    {
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        picked = 0;
        for(int i = 0; i < gameManager.coin[level].Count; i++)
        {
            if (gameManager.coin[level][0])
            {
                text.GetComponent<Text>().text = "尚未探索";
                return;
            }
            else
            {
                if(i > 0)
                {
                    if (gameManager.coin[level][i])
                    {
                        picked++;
                    }
                }
            }
        }
        text.GetComponent<Text>().text = $"{picked} / {gameManager.coin[level].Count-1}";
    }
}
