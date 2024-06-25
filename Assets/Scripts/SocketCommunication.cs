using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
public class SocketCommunication
{
    private static SocketCommunication instance;
    public static SocketCommunication GetInstance()
    {
        if(instance == null) instance = new SocketCommunication();
        return instance;
    }
    Socket socket;
    public string address = "192.168.43.174";
    public int port = 9999;
    Thread receiveData;

    public SocketCommunication()
    {
        Debug.Log("SocketCommunication");
        ConnectToServer();
    }

    async void ConnectToServer()
    {
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        await socket.ConnectAsync(address, port);

        Debug.Log("Connected to server");
        receiveData = new Thread(new ThreadStart(handleReceivedData));
        receiveData.Start();
    }

    async void handleReceivedData()
    {
        while (true)
        {
            var buffer = new byte[1_024];
            var received = await socket.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            Debug.Log(response);
        }
    }

    public async void Send(string msg)
    {
        var messageBytes = Encoding.UTF8.GetBytes(msg);
        await socket.SendAsync(messageBytes, SocketFlags.None);
    }

}