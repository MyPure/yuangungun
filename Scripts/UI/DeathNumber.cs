using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathNumber : MonoBehaviour
{
    public GameManager gameManager;
    void Start()
    {
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        GetComponent<Text>().text = "死亡次数：" + gameManager.deathNumber;
    }

}
