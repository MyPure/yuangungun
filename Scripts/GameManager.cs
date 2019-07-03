using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int passLevel;
    public int nowLevel;
    public GameObject LevelPanel;
    public static bool first = true;
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
}
