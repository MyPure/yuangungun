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

        #region 局域网联机
        for (int i = 0; i < msgList.Count; i++)
        {
            HandleMessage();
        }
        #endregion
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
    Socket socket;
    const int BUFFER_SIZE = 1024;
    public byte[] readBuff = new byte[BUFFER_SIZE];
    //玩家列表
    Dictionary<string, Player> players = new Dictionary<string, Player>();
    //消息列表
    List<string> msgList = new List<string>();
    //Player预设
    public GameObject prefab;
    //IP和端口
    string id;

    public void Connect(Socket socket)
    {
        SceneManager.LoadScene("Multiple");
        this.socket = socket;
        id = socket.LocalEndPoint.ToString();
        
        StartCoroutine( StartConnect() );
    }

    IEnumerator StartConnect()
    {
        yield return null;
        Addplayer(id, new Vector3(0, 4, 0));
        players[id].connectType = ConnectType.ThisConnecting;
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().target = players[id].gameObject;
        socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
    }

    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            string str = Encoding.UTF8.GetString(readBuff, 0, count);
            msgList.Add(str);
            socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            socket.Close();
        }
    }

    private void Addplayer(string id, Vector3 pos)
    {
        Player player = Instantiate(prefab, pos, Quaternion.identity).GetComponent<Player>();
        Debug.Log(id);
        player.connectType = ConnectType.OtherConnecting;
        player.GetComponentInChildren<TextMesh>().text = id;
        players.Add(id, player);
    }
    public void SendPos()
    {
        Vector3 pos = players[id].transform.position;
        string str = $"POS {id} {pos.x.ToString()} {pos.y.ToString()} {pos.z.ToString()} {players[id].currentState.ToString()}";
        byte[] bytes = Encoding.Default.GetBytes(str);
        socket.Send(bytes);
    }

    private void SendLeave()
    {
        string str = $"LEAVE {id}";
        byte[] bytes = Encoding.Default.GetBytes(str);
        socket.Send(bytes);
    }

    public void Leave()
    {
        SendLeave();
    }

    

    private void HandleMessage()
    {
        if (msgList.Count == 0)
        {
            return;
        }
        string str = msgList[0];
        msgList.RemoveAt(0);
        string[] args = str.Split(' ');
        if (args[0] == "POS")
        {
            OnReceivePos(args[1], args[2], args[3], args[4], args[5]);
        }
        else if (args[0] == "LEAVE")
        {
            OnReceiveLeave(args[1]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="xStr"></param>
    /// <param name="yStr"></param>
    /// <param name="zStr"></param>
    public void OnReceivePos(string id, string xStr, string yStr, string zStr, string state)
    {
        if (id == this.id)
        {
            return;//不对自己操作
        }

        float x = float.Parse(xStr);
        float y = float.Parse(yStr);
        float z = float.Parse(zStr);
        StateType StateType = PlayerState.Prase(state);
        if (players.ContainsKey(id))
        {
            players[id].transform.position = new Vector3(x, y, z);
            Debug.Log(id);
            if (players[id].currentState.stateType != StateType)
            {
                players[id].currentState.ChangeStateTo(StateType);
            }
        }
        else
        {
            Debug.Log(id);
            Addplayer(id, new Vector3(0, 4, 0));
        }
    }

    public void OnReceiveLeave(string id)
    {
        if (players.ContainsKey(id))
        {
            Leave();
        }
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
