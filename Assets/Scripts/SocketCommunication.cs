using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

[System.Serializable]
class EventName
{
    public string event_name;

}

[System.Serializable]
class First_Connect
{
    public string id;
    public string player_name;
}

[System.Serializable]
class Rooms
{
    public Room[] rooms;
}

[System.Serializable]
class SimplePlayerInfo
{
    public string player_id;
    public string player_name;
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

            EventName _event = JsonUtility.FromJson<EventName>(response);

            switch (_event.event_name)
            {
                case "provide id":
                    //set player id in first connect
                    First_Connect data = JsonUtility.FromJson<First_Connect>(response);
                    Player_ID.MyPlayerID = data.id;
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(data.player_name, data.id);
                        UIManager._instance.uiMainMenu.JoinCall(0);
                    });
                    break;
                case "rooms":
                    //get available rooms
                    Rooms rooms = JsonUtility.FromJson<Rooms>(response);
                    Dispatcher.EnqueueToMainThread(() => UIManager._instance.uiOnlineLobby.InitListRoom(rooms.rooms));
                    break;
                case "new player join":
                    //other player join room
                    SimplePlayerInfo playerInfo = JsonUtility.FromJson<SimplePlayerInfo>(response);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(playerInfo.player_name, playerInfo.player_id);
                        UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.lsPlayers);
                        UIManager._instance.uiMainMenu.JoinCall(0);
                    });
                    break;
                case "joined":
                    //join a room
                    SimplePlayerInfo playerIn4 = JsonUtility.FromJson<SimplePlayerInfo>(response);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(playerIn4.player_name, playerIn4.player_id);
                        UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.lsPlayers);
                        UIManager._instance.uiOnlineLobby.OnGuessJoin();
                        UIManager._instance.uiMainMenu.JoinCall(1);
                    });
                    break;
            }
            Debug.Log(_event.event_name);
        
        }
    }

    public async void Send(string msg)
    {
        var messageBytes = Encoding.UTF8.GetBytes(msg);
        await udpClient.SendAsync(messageBytes, messageBytes.Length, address, port);
    }

}