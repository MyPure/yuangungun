using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public void LoadLevel(int level)
    {
        gameManager.LoadLevel(level);
        gameManager.nowLevel = level;
    }
}
