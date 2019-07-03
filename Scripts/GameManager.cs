using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int passLevel;
    public int nowLevel;
    public GameObject LevelPanel;
    public GameObject PausePanel;
    public static bool first = true;
    bool pause = false;
    private void Start()
    {
        if (first)
        {
            DontDestroyOnLoad(gameObject);
            first = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(!pause && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("StartScene") && Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause();
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
    }
    public void BackToChooseLevel()
    {
        SceneManager.LoadScene("StartScene");
        StartCoroutine(InstantiateLevelPanel());
    }
    IEnumerator InstantiateLevelPanel()
    {
        yield return null;
        if (passLevel == 0)
        {
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            Instantiate(LevelPanel);
        }
    }
    public void TryAgain()
    {
        LoadLevel(nowLevel);
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        Instantiate(PausePanel);
        pause = true;
    }
    public void Continue()
    {
        Time.timeScale = 1;
        
        pause = false;
    }
}
