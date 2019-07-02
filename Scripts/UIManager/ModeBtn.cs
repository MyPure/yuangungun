using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeBtn : MonoBehaviour
{
    public GameObject levelPanel;
    public GameManager gameManager;

    private void Start()
    {

        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
    public void ClassicBtn()
    {
        if (gameManager.passLevel == 0)
        {
            gameManager.nowLevel = 0;
            gameManager.LoadLevel(0);
        }
        else
        {
            Instantiate(levelPanel);
        }
    }
}
