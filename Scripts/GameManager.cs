using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void StartBtn_LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelBtn_LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}
