﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

enum ProtocolValue {
    SetUniqueID = 1000,
    StartGame,
    CharMove = 1010,
    ToCombatScene,
    ChangeTurn,
    EndGame
}

public class Client : MonoBehaviour
{
    Board board;
    Socket client;
    string ipaddress = "127.0.0.1";
    int portNum = 80;
    int uniqueID = -1;

    float elapsedTime = 0;

    public Text debugText;

    void Awake()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
        DontDestroyOnLoad(gameObject);
        Connect(ipaddress, portNum);
    }

    void Update()
    {
        // 연결되지 않을 시 6초마다 연결 시도
        if (client == null)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > 6)
            {
                elapsedTime = 0;
                Connect(ipaddress, portNum);
            }
        }
        else if (client != null && client.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            int recvLen = client.Receive(buffer);
            if(recvLen > 0)
            {
                string recvData = Encoding.UTF8.GetString(buffer);
                debugText.text += recvData + " / ";
                string[] strs = recvData.Split(',');
                int.TryParse(strs[0], out int protocolVal);

                switch(protocolVal)
                {
                    case (int)ProtocolValue.SetUniqueID:
                        int uniq;
                        int.TryParse(strs[1], out uniq);
                        uniqueID = uniq % 2;
                        Board.charCode = uniqueID;
                        break;
                    case (int)ProtocolValue.StartGame:
                        Board.ready = true;
                        break;
                    case (int)ProtocolValue.CharMove:
                        //int.TryParse(strs[1], out int pNum);
                        //int.TryParse(strs[2], out int val);
                        //board.PlayerMove(pNum, val);
                        break;
                    case (int)ProtocolValue.ToCombatScene:
                        StartCoroutine(FuncHelper.LoadScene("Combat"));
                        break;
                    case (int)ProtocolValue.ChangeTurn:
                        Board.turn = !Board.turn;
                        Board.turnNum++;
                        break;
                    case (int)ProtocolValue.EndGame:
                        break;
                }
            }
        }
    }

    private void OnGUI()
    {

    }

    public void CharMove(int pNum, int val)
    {
        string str = string.Format("1010,{0},{1}", pNum, val);
        SendMsg(str);
    }

    public void ToCombatScene()
    {
        string str = "1011";
        SendMsg(str);
    }

    public void ChangeTurn()
    {
        string str = "1012";
        SendMsg(str);
    }

    void SendMsg(string str)
    {
        byte[] buffer = new byte[str.Length];
        buffer = Encoding.UTF8.GetBytes(str);
        client.Send(buffer);        
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
