using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

public class NetManager
{
    Socket socket;
    const int BUFFER_SIZE = 1024;
    public byte[] readBuff = new byte[BUFFER_SIZE];
    //玩家列表
    Dictionary<string, Player> players = new Dictionary<string, Player>();
    //消息列表
    List<string> msgList = new List<string>();
    //Player预设
    //IP和端口
    string id;
    int buffCount = 0;
    byte[] lenBytes = new byte[sizeof(uint)];
    int msgLength = 0;

    GameManager gameManager;

    public NetManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public void Connect(Socket socket)
    {
        this.socket = socket;
        id = socket.LocalEndPoint.ToString();

        AddPlayer(id, new Vector3(0, 4, 0));
        players[id].connectType = ConnectType.ThisConnecting;
        gameManager.BindCamera(players[id].gameObject);
        gameManager.multiple = true;
        socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
    }

    public void AddPlayer(string id, Vector3 pos)
    {
        Debug.Log("123");
        Player player = gameManager.AddPlayer(pos);
        player.connectType = ConnectType.OtherConnecting;
        player.GetComponentInChildren<TextMesh>().text = id;
        players.Add(id, player);
    }

    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            buffCount += count;
            socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            socket.Close();

            gameManager.BackToMainMenu();
        }
    }

    public void ProcessData()
    {
        if (buffCount < sizeof(int))
        {
            return;
        }
        Array.Copy(readBuff, lenBytes, sizeof(int));
        msgLength = BitConverter.ToInt32(lenBytes, 0);
        if (buffCount < msgLength + sizeof(int))
        {
            return;
        }
        string str = Encoding.UTF8.GetString(readBuff, sizeof(int), msgLength);
        HandleMessage(str);
        int count = buffCount - msgLength - sizeof(int);
        Array.Copy(readBuff, sizeof(int) + msgLength, readBuff, 0, count);
        buffCount = count;
        if (buffCount > 0)
        {
            ProcessData();
        }
    }

    

    public void SendPos()
    {
        Vector3 pos = players[id].transform.position;
        string str = $"POS {id} {pos.x.ToString()} {pos.y.ToString()} {pos.z.ToString()} {players[id].currentState.ToString()}";
        byte[] bytes = Encoding.Default.GetBytes(str);
        byte[] length = BitConverter.GetBytes(bytes.Length);
        byte[] sendbuff = length.Concat(bytes).ToArray();
        socket.Send(sendbuff);
    }

    public void SendLeave()
    {
        string str = $"LEAVE {id}";
        byte[] bytes = Encoding.Default.GetBytes(str);
        byte[] length = BitConverter.GetBytes(bytes.Length);
        byte[] sendbuff = length.Concat(bytes).ToArray();
        socket.Send(sendbuff);
    }

    private void HandleMessage(string str)
    {
        Console.WriteLine(str);
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
        StateType stateType = PlayerState.Prase(state);
        if (players.ContainsKey(id))
        {
            Vector3 destination = new Vector3(x, y, z);
            if (destination.x > players[id].transform.position.x)
            {
                players[id].flip = false;
            }
            else if (destination.x < players[id].transform.position.x)
            {
                players[id].flip = true;
            }

            if (stateType == StateType.Stand)
            {
                if (destination.x == players[id].transform.position.x)
                {
                    players[id].animator.Play("Idle");
                }
                else
                {
                    players[id].animator.Play("Walk");
                }
            }

            players[id].transform.position = destination;
            if (players[id].currentState.stateType != stateType)
            {
                players[id].currentState.ChangeStateTo(stateType);

            }
        }
        else
        {
            Debug.Log(id);
            AddPlayer(id, new Vector3(0, 4, 0));
        }
    }

    public void OnReceiveLeave(string id)
    {
        if (players.ContainsKey(id))
        {
            UnityEngine.Object.Destroy(players[id].gameObject);
            players.Remove(id);
        }
    }
}
