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
    public void BackToMainMenu()
    {
        gameManager.BackToMainMenu();
    }
    public void NextLevel()
    {
        gameManager.NextLevel();
    }
    public void TryAgain()
    {
        gameManager.TryAgain();
    }

    public void Continue()
    {
        gameManager.Continue();
    }
    public void DestroyCanvas()
    {
        Transform g = gameObject.transform;
        while (g.GetComponent<Canvas>() == null)
        {
            g = g.transform.parent;
        }
        Destroy(g.gameObject);
    }
    public void CheckSave(GameObject SaveUI)
    {
        Instantiate(SaveUI);
    }
    public void ClearSave()
    {
        gameManager.ClearSave();
    }
    public void InstantiateGameObject(GameObject obj)
    {
        Instantiate(obj);
    }
}
