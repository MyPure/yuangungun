using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartBtn : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject helpUI;
    public GameManager gameManager;

    private void Start()
    {

        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
    public void GameStart()
    {
        if (gameManager.passLevel == 0)
        {
            Instantiate(helpUI);
        }
        else
        {
            Instantiate(levelPanel);
        }
    }
}
