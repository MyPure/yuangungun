using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;

public class ConnectBtn : UnicersalBtn
{
    public InputField ip;
    public InputField port;
    public Text message;
    Socket socket;

    public void Connect()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect(ip.text, int.Parse(port.text));
        }
        catch (Exception e)
        {
            message.text = e.Message;
            return;
        }
        DestroyCanvas();
        gameManager.Connect(socket);
    }
}
