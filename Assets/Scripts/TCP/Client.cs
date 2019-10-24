using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    Socket client;
    string ipaddress = "127.0.0.1";
    int portNum = 80;

    void Start()
    {
        
    }

    void Update()
    {
        if(client != null && client.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            int recvLen = client.Receive(buffer);
            if(recvLen > 0)
            {
                string recvData = Encoding.UTF8.GetString(buffer);
                string[] tempStr = recvData.Split(',');
                int.TryParse(tempStr[0], out int protocolVal);

                switch(protocolVal)
                {
                    case 1000:
                        break;
                }
            }
        }
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,100,100), "Connect"))
        {
            Connect(ipaddress, portNum);
        }
    }

    bool Connect(string ipaddress, int portNum)
    {
        try
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipaddress, portNum);
            return true;
        }
        catch (Exception ex)
        {
            client = null;
            Debug.Log(ex);
            return false;
        }
    }

    private void OnApplicationQuit()
    {
        if (client != null)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            client = null;
        }
    }
}
