using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

enum ProtocolValue {
    SetUniqueID = 1000,
    StartGame,
    CharMove = 1010,
    SaveStatusCommand,
    SaveStatus,
    ToCombatScene,
    MoveLock,
    ChangeTurn,
    EndGame
}

public class Client : MonoBehaviour
{
    public static Client instance = null;

    Board board;
    Socket client;
    string ipaddress = "127.0.0.1";
    int portNum = 80;
    int uniqueID = -1;

    static public int dataSync = 0;
    float elapsedTime = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
        Connect(ipaddress, portNum);
    }

    public void BoardConnect(Board newBoard)
    {
        board = newBoard;
    }

    void Update()
    {
        return;

        // 연결되지 않을 시 x초마다 연결 시도
        if (client == null)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > 5)
            {
                elapsedTime = 0;
                Connect(ipaddress, portNum);
            }
        }
        if (client.Connected && client.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            if(client.Receive(buffer) > 0)
            {
                string command = Encoding.UTF8.GetString(buffer);
                string[] str = command.Split('/');
                for (int i = 0; i < str.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(str[i]))
                        return;

                    string[] strs = str[i].Split(',');
                    int.TryParse(strs[0], out int protocolVal);

                    switch (protocolVal)
                    {
                        case (int)ProtocolValue.SetUniqueID:
                            if (Board.charCode >= 0)
                                break;
                            {
                                int uniq;
                                int.TryParse(strs[1], out uniq);
                                uniqueID = uniq % 2;
                                Board.charCode = uniqueID;
                            }
                            break;
                        case (int)ProtocolValue.StartGame:
                            Board.ready = true;
                            break;
                        case (int)ProtocolValue.CharMove:
                            {
                                int.TryParse(strs[1], out int val);
                                board.PlayerMove(val);
                            }
                            break;
                        case (int)ProtocolValue.SaveStatusCommand:
                            board.SavePlayerStatus();
                            break;
                        case (int)ProtocolValue.SaveStatus:
                            {
                                int.TryParse(strs[1], out int maxHp);
                                int.TryParse(strs[2], out int curHp);
                                int.TryParse(strs[3], out int atk);
                                int.TryParse(strs[4], out int def);
                                int.TryParse(strs[5], out int gold);
                                int.TryParse(strs[6], out int code);
                                FuncHelper.SetPlayerData(maxHp, curHp, atk, def, gold, code);
                                //board.SetString = string.Format("hp: {0}, atk: {1}, def: {2}", maxHp, atk, def);
                                dataSync++;
                            }
                            break;
                        case (int)ProtocolValue.ToCombatScene:
                            StartCoroutine(FuncHelper.LoadScene("Combat"));  
                            break;
                        case (int)ProtocolValue.MoveLock:
                            {
                                dataSync = 0;
                                int.TryParse(strs[1], out int code);
                                if (Board.charCode == code)
                                    Board.moveLocked = true;
                            }
                            break;
                        case (int)ProtocolValue.ChangeTurn:
                            Board.turn = !Board.turn;
                            Board.turnNum++;
                            board.TurnCheck();
                            break;
                        case (int)ProtocolValue.EndGame:
                            break;
                    }
                }
            }
        }
    }

    public void CharMove(int val)
    {
        string str = string.Format("{0},{1}/", (int)ProtocolValue.CharMove, val);
        SendMsg(str);
    }
    
    // 각 클라이언트로부터 스탯을 불러들이도록
    public void SaveStatus()
    {
        string str = string.Format("{0}/", (int)ProtocolValue.SaveStatusCommand);
        SendMsg(str);
    }

    public void SaveStatus(int maxHp, int curHp, int atk, int def, int gold, int code)
    {
        string str = string.Format("{0},{1},{2},{3},{4},{5},{6}/", (int)ProtocolValue.SaveStatus, maxHp, curHp, atk, def, gold, code);
        SendMsg(str);
    }

    public void ToCombatScene()
    {
        string str = string.Format("{0}/", (int)ProtocolValue.ToCombatScene);
        SendMsg(str);
    }

    public void MoveLock(int code)
    {
        string str = string.Format("{0},{1}/", (int)ProtocolValue.MoveLock, code);
        SendMsg(str);
    }

    public void ChangeTurn()
    {
        string str = string.Format("{0}/", (int)ProtocolValue.ChangeTurn);
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
