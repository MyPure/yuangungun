using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

public enum ConnectType
{
    NotConnecting,
    ThisConnecting,
    OtherConnecting
}

public class GameManager : MonoBehaviour
{
    public int passLevel;
    public int nowLevel;
    public GameObject LevelPanel;
    public GameObject PausePanel;
    public static bool first = true;
    bool pause = false;
    public bool savePoint;
    public Vector3 savePointPosition;
    public List<bool>[] coin;
    public List<Coin.PickedType> tempCoin;
    public int followCoinsCount;
    public int deathNumber;

    public bool multiple;
    private void Awake()
    {
        Screen.fullScreen = false;
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
        tempCoin = new List<Coin.PickedType>();
    }

    private void Update()
    {
        if(!pause && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("StartScene") && Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause();
        }

        if (netManager != null)
        {
            netManager.ProcessData();
        }
    }

    public void NextLevel()
    {
        savePoint = false;
        LoadLevel(nowLevel + 1);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
        nowLevel = level;          
        StartCoroutine(CheckCoin(level));
    }

    IEnumerator CheckCoin(int level)
    {
        yield return null;
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        if (coin[level][0])
        {
            for (int i = 0; i < coins.Length; i++)
            {
                coin[level].Add(false);
                tempCoin.Add(Coin.PickedType.unPicked);
            }
            coin[level][0] = false;
        }
        else
        {
            if (savePoint)
            {
                for (int i = 0; i < coins.Length; i++)
                {
                    if (tempCoin[i] == Coin.PickedType.tempPicked)
                    {
                        coins[i].GetComponent<SpriteRenderer>().enabled = false;
                        coins[i].GetComponent<BoxCollider2D>().enabled = false;
                        coins[i].GetComponent<Coin>().pickedType = Coin.PickedType.tempPicked;
                    }
                    else if (tempCoin[i] == Coin.PickedType.picked)
                    {
                        coins[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        coins[i].GetComponent<Coin>().pickedType = Coin.PickedType.picked;
                    }
                }
                GameObject.Find("FollowCoins").GetComponent<FollowCoins>().followCoinsCount = followCoinsCount;
                StartCoroutine(GameObject.Find("FollowCoins").GetComponent<FollowCoins>().GiveFollowCoins(savePointPosition));
            }
            else
            {
                for (int i = 0; i < coins.Length; i++)
                {
                    if (coin[level][i + 1])
                    {
                        coins[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        coins[i].GetComponent<Coin>().pickedType = Coin.PickedType.picked;

                    }
                }
                //foreach (List<bool> bl in coin)
                //{
                //    foreach (bool b in bl)
                //    {
                //        Debug.Log(b);
                //    }
                //}
            }
        }
    }


    public void SaveCoin()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        for (int i = 0; i < coins.Length; i++)
        {
            if (coins[i].GetComponent<Coin>().pickedType == Coin.PickedType.tempPicked || coins[i].GetComponent<Coin>().pickedType == Coin.PickedType.picked)
            {
                coin[nowLevel][i + 1] = true;
            }
        }
        //foreach (List<bool> bl in coin)
        //{
        //    foreach (bool b in bl)
        //    {
        //        Debug.Log(b);
        //    }
        //}
    }

    public void SaveTempCoin()
    {
        tempCoin.Clear();
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        for (int i = 0; i < coins.Length; i++)
        {
            tempCoin.Add(coins[i].GetComponent<Coin>().pickedType);
        }
        //foreach (Coin.PickedType b in tempCoin)
        //{
        //    Debug.Log(b);
        //}
    }

    public void SaveFollowCoinCount()
    {
        FollowCoins f = GameObject.Find("FollowCoins").GetComponent<FollowCoins>();
        followCoinsCount = Mathf.Max(f.followCoinsCount,f.followCoins.Count);
    }
    public void BackToChooseLevel()
    {
        SceneManager.LoadScene("StartScene");
        StartCoroutine(InstantiateLevelPanel());
        savePoint = false;
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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void TryAgain()
    {
        LoadLevel(nowLevel);
        if (savePoint)
        {
            StartCoroutine(ToSavePoint());
        }
    }
    IEnumerator ToSavePoint()
    {
        yield return null;
        GameObject camera = GameObject.Find("Main Camera");
        GameObject.Find("Player").transform.position = savePointPosition;
        camera.transform.position = savePointPosition + new Vector3(0,0,-10) + (Vector3)camera.GetComponent<CameraFollow>().preset;
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
    public void QuitGame()
    {
        Application.Quit();
    }


    #region 局域网联机
    NetManager netManager;
    public GameObject prefab;

    public void BindCamera(GameObject gameObject)
    {
        CameraFollow cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        cameraFollow.target = gameObject;
        cameraFollow.player = gameObject.GetComponent<Player>();
        cameraFollow.preset = cameraFollow.transform.position - gameObject.transform.position;
    }

    public IEnumerator WConnect(Socket socket)
    {
        yield return null;
        netManager = new NetManager(this);
        netManager.Connect(socket);
    }
    public void Connect(Socket socket)
    {
        SceneManager.LoadScene("Multiple");
        StartCoroutine(WConnect(socket));
    }

    public Player AddPlayer(Vector3 pos)
    {
        Player player = Instantiate(prefab, pos, Quaternion.identity).GetComponent<Player>();
        return player;
    }

    public void SendPos()
    {
        netManager.SendPos();
    }

    public void SendLeave()
    {
        netManager.SendLeave();
    }
    #endregion

    //存档功能
    Save CreateSave()
    {
        Save save = new Save();
        save.passLevel = passLevel;
        save.coin = coin;
        save.deathNumber = deathNumber;
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
        coin = save.coin;
        deathNumber = save.deathNumber;
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
