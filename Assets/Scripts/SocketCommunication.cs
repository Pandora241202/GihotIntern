using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

[System.Serializable]
class ReceivedData
{
    public string event_name;
    public string id;
    public string player_id;
    public string player_name;
    [field: SerializeField] public Vector3 position;
    [field: SerializeField] public Vector3 direction;
    public Room[] rooms;
}



[System.Serializable]
public class Room
{
    public string id;
    public string name;
    public string game_mode;
}

public class SocketCommunication
{
    private static SocketCommunication instance;
    public static SocketCommunication GetInstance()
    {
        if (instance == null) instance = new SocketCommunication();
        return instance;
    }
    UdpClient udpClient;
    public string address = "127.0.0.1";
    public int port = 9999;
    Thread receiveData;
    public string player_id;


    public SocketCommunication()
    {
        ConnectToServer();
    }

    async void ConnectToServer()
    {
        udpClient = new UdpClient();
        udpClient.Client.ReceiveBufferSize = 1024 * 64;

        string message = "{ \"_event\" : {\"event_name\" : \"first connect\"}}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        await udpClient.SendAsync(data, data.Length, address, 9999);

        Debug.Log("Connected to server");
        receiveData = new Thread(new ThreadStart(handleReceivedData));
        receiveData.IsBackground = true;
        receiveData.Start();
    }

    async void handleReceivedData()
    {
        while (true)
        {
            var received = await udpClient.ReceiveAsync();
            var response = Encoding.UTF8.GetString(received.Buffer);

            ReceivedData json = JsonUtility.FromJson<ReceivedData>(response);

            switch (json.event_name)
            {
                case "provide id":
                    //set player id
                    Player_ID.MyPlayerID = json.id;
                    Debug.Log(json.player_name);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(json.player_name, json.id);
                        UIManager._instance.uiMainMenu.JoinCall(0);
                    });
                    break;
                case "rooms":
                    //do sth
                    Dispatcher.EnqueueToMainThread(() => UIManager._instance.uiOnlineLobby.InitListRoom(json.rooms));
                    break;
                case "new player join":
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(json.player_name, json.player_id);
                        UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.lsPlayers);
                        UIManager._instance.uiMainMenu.JoinCall(0);
                    });
                    break;
                case "joined":
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(json.player_name, json.player_id);
                        UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.lsPlayers);
                        UIManager._instance.uiOnlineLobby.OnGuessJoin();
                        UIManager._instance.uiMainMenu.JoinCall(1);
                    });
                    break;
            }
            Debug.Log(json.event_name);
        
        }
    }

    public async void Send(string msg)
    {
        var messageBytes = Encoding.UTF8.GetBytes(msg);
        await udpClient.SendAsync(messageBytes, messageBytes.Length, address, port);
    }

}