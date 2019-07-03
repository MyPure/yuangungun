using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelBtn : MonoBehaviour
{
    public GameManager gameManager;
    public int level;
    private void Start()
    {
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (gameManager.passLevel >= level)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
    public void LoadLevel()
    {
        gameManager.LoadLevel(level);
        gameManager.nowLevel = level;
    }
}
