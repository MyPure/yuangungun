using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicersalBtn : MonoBehaviour
{
    public GameManager gameManager;
    private void Start()
    {
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
    public void BackToChooseLevel()
    {
        gameManager.BackToChooseLevel();
    }
    public void NextLevel()
    {
        gameManager.LoadLevel(++gameManager.nowLevel);
    }
    public void TryAgain()
    {
        gameManager.TryAgain();
    }

    public void Continue()
    {
        gameManager.Continue();
    }
    public void DestrouCanvas()
    {
        Transform g = gameObject.transform;
        while (g.GetComponent<Canvas>() == null)
        {
            g = g.transform.parent;
        }
        Destroy(g.gameObject);
    }
}
