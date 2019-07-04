using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class GameManager : MonoBehaviour
{
    public int passLevel;
    public int nowLevel;
    public GameObject LevelPanel;
    public GameObject PausePanel;
    public static bool first = true;
    bool pause = false;
    private void Awake()
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

        //读档
        if (File.Exists(Application.persistentDataPath + "/save/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save/save.dat", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            LoadSave(save);
        }
        else
        {
            Save save = new Save(true);
            BinaryFormatter bf = new BinaryFormatter();
            if (!Directory.Exists(Application.persistentDataPath + "/save"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/save");
            }
            FileStream file = File.Create(Application.persistentDataPath + "/save/save.dat");
            bf.Serialize(file, save);
            file.Close();

            LoadSave(save);
            Debug.Log("未找到存档文件，已创建空存档");
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

    //存档功能
    Save CreateSave()
    {
        Save save = new Save();
        save.passLevel = passLevel;
        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSave();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save/save.dat");
        bf.Serialize(file, save);
        file.Close();
    }

    void LoadSave(Save save)
    {
        passLevel = save.passLevel;
    }

    public void ClearSave()
    {
        Save save = new Save(true);//生成空存档类
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save/save.dat");
        bf.Serialize(file, save);
        file.Close();

        LoadSave(save);
    }
}
