using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server : MonoBehaviour
{
    Socket server;
    List<Socket> clients = new List<Socket>();

    int uniqueID = 0;

    void Awake()
    {
        CreateServer();
    }

    void Update()
    {
        if(server.Poll(0, SelectMode.SelectRead))
        {
            Socket client = server.Accept();
            clients.Add(client);

            Debug.Log("A Client is connected.");
            string str = string.Format("{0},{1}/", (int)ProtocolValue.SetUniqueID, uniqueID);
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            client.Send(buffer);
            uniqueID++;

            if (clients.Count >= 2)
                StartGame();
        }

        for(int i=0; i<clients.Count; i++)
        {
            if(clients[i].Poll(0, SelectMode.SelectRead))
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int recvLen = clients[i].Receive(buffer);
                    if(recvLen > 0)
                    {
                        for (int j = 0; j < clients.Count; j++)
                            clients[j].Send(buffer);
                    }
                    else
                    {
                        clients[i] = null;
                        clients.Remove(clients[i]);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    clients[i] = null;
                    clients.Remove(clients[i]);
                    Debug.Log(ex);
                }
            }
        }
    }

    void StartGame()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            string str = string.Format("{0}/", (int)ProtocolValue.StartGame);
            byte[] buffer = new byte[str.Length];
            buffer = Encoding.UTF8.GetBytes(str);
            clients[i].Send(buffer);
        }
    }

    void CreateServer()
    {
        try
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, 80));
            server.Listen(1);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i] != null)
            {
                clients[i].Shutdown(SocketShutdown.Both);
                clients[i].Close();
                clients.Remove(clients[i]);
                clients[i] = null;
            }
        }
        if (server != null)
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            server = null;
        }
    }
}
