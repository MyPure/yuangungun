using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int passLevel;
    public int nowLevel;
    public GameObject LevelPanel;
    private void Start()
    {
            DontDestroyOnLoad(gameObject);
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
    }
    public void BackToChooseLevel()
    {
        SceneManager.LoadScene("StartScene");
        Instantiate(LevelPanel);
    }
}
